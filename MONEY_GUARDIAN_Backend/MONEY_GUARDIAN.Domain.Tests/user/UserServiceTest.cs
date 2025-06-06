using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Exceptions;
using MONEY_GUARDIAN.Domain.Ports;
using MONEY_GUARDIAN.Domain.Services.monetaryFund;
using MONEY_GUARDIAN.Domain.Services.user;
using MONEY_GUARDIAN.Domain.Tests.DataBuilder;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace MONEY_GUARDIAN.Domain.Tests.user
{
    [TestClass]
    public class UserServiceTest
    {
        private UserService Service { get; set; } = default!;
        private IGenericRepository<User> Repository { get; set; } = default!;
        private UserBuilder UserBuilder { get; set; } = default!;

        [TestInitialize]
        public void Initialize()
        {
            Repository = Substitute.For<IGenericRepository<User>>();

            Service = new(Repository);

            UserBuilder = new();
        }


        [TestMethod]
        public async Task CreateUserAsync_Ok()
        {
            //Arrange
            int id = 1;
            string userName = "userName";
            string name = "name";
            string password = "12345";

            User user = UserBuilder
                .WithId(id)
                .WithName(name)
                .WithUsername(userName)
                .WithPassword(password)
                .WithIsUserGoogle(false)
                .Build();

            Repository
               .AddAsync(user)
               .ReturnsForAnyArgs(user);

            //Act
            var result = await Service.CreateUserAsync(
                name,
                userName,
                password
            );

            //Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(userName, result.Username);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(password, result.Password);
            Assert.IsFalse(result.IsUserGoogle);

            await Repository
               .ReceivedWithAnyArgs(1)
               .AddAsync(Arg.Any<User>());
        }
        
        [TestMethod]
        public async Task RegisterUserGoogleAsync_Ok()
        {
            //Arrange
            int id = 1;
            string userName = "userName";
            string name = "name";
            string password = "12345";

            User user = UserBuilder
                .WithId(id)
                .WithName(name)
                .WithUsername(userName)
                .WithPassword(password)
                .WithIsUserGoogle(true)
                .Build();

            Repository
               .AddAsync(user)
               .ReturnsForAnyArgs(user);

            //Act
            var result = await Service.RegisterUserGoogleAsync(
                userName,
                true
            );

            //Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(userName, result.Username);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(password, result.Password);
            Assert.IsTrue(result.IsUserGoogle);

            await Repository
               .ReceivedWithAnyArgs(1)
               .AddAsync(Arg.Any<User>());
        }

        [TestMethod]
        public async Task UpdateUserAsync_Ok()
        {
            //Arrange
            int id = 1;
            string userName = "userName";
            string name = "name";
            string oldPassword = "oldpass";
            string newPassword = "newpass";

            User user = UserBuilder
                .WithId(id)
                .WithName(name)
                .WithUsername(userName)
                .WithPassword(oldPassword)
                .Build();

            Repository
               .GetByIdAsync(id)
               .ReturnsForAnyArgs(user);

            Repository
                .UpdateAsync(user)
                .ReturnsForAnyArgs(user);

            //Act
            var result = await Service.UpdateUserAsync(id, name, userName, newPassword);

            //Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(userName, result.Username);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(newPassword, result.Password);

            await Repository
               .ReceivedWithAnyArgs(1)
               .GetByIdAsync(Arg.Any<int>());
            await Repository
               .ReceivedWithAnyArgs(1)
               .UpdateAsync(Arg.Any<User>());
        }
        
        [TestMethod]
        public async Task UpdateUserAsync_OkWithPasswordNull()
        {
            //Arrange
            int id = 1;
            string userName = "userName";
            string name = "name";
            string originalPassword = "original123";
            string? newPassword = null;

            User user = UserBuilder
                .WithId(id)
                .WithName(name)
                .WithUsername(userName)
                .WithPassword(originalPassword)
                .Build();

            Repository
               .GetByIdAsync(id)
               .ReturnsForAnyArgs(user);

            Repository
                .UpdateAsync(user)
                .ReturnsForAnyArgs(user);

            //Act
            var result = await Service.UpdateUserAsync(id, name, userName, newPassword);

            //Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(userName, result.Username);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(originalPassword, result.Password);
            //Assert.IsNull(result.Password);

            await Repository
               .ReceivedWithAnyArgs(1)
               .GetByIdAsync(Arg.Any<int>());
            await Repository
               .ReceivedWithAnyArgs(1)
               .UpdateAsync(Arg.Any<User>());
        }

        [TestMethod]
        public async Task GetUserByIdAsync_Ok()
        {
            //Arrange
            int id = 1;
            string userName = "userName";
            string name = "name";
            string password = "12345";

            User user = UserBuilder
                .WithId(id)
                .WithName(name)
                .WithUsername(userName)
                .WithPassword(password)
                .Build();

            Repository
               .GetByIdAsync(id)
               .ReturnsForAnyArgs(user);

            //Act
            var result = await Service.GetUserById(id);

            //Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(userName, result.Username);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(password, result.Password);

            await Repository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
        }
        
        [TestMethod]
        public async Task GetUserByIdAsync_Failed()
        {
            //Arrange
            int id = 1;
                        
            Repository
               .GetByIdAsync(id)
               .ReturnsNull();

            //Act            
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.GetUserById(id);
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.NotExistUser, ex.Message);

            await Repository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
        }

        [TestMethod]
        public async Task ValidLoginUserAsync_Ok()
        {
            //Arrange
            int id = 1;
            string userName = "userName";
            string name = "name";
            string password = "12345";

            User user = UserBuilder
                .WithId(id)
                .WithName(name)
                .WithUsername(userName)
                .WithPassword(password)
                .Build();

            Repository
               .GetAsync()
               .ReturnsForAnyArgs([user]);

            //Act
            var result = await Service.ValidLoginUserAsync(userName, password);

            //Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(userName, result.Username);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(password, result.Password);

            await Repository
                .ReceivedWithAnyArgs(1)
                .GetAsync(Arg.Any<Expression<Func<User, bool>>>());
        }
        
        [TestMethod]
        public async Task ValidLoginUserAsync_Failed()
        {
            //Arrange
            string password = "12345";
            string userName = "userName";


            Repository
               .GetAsync()
               .ReturnsNull();

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.ValidLoginUserAsync(userName, password);
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.InvalidCredential, ex.Message);

            await Repository
                .ReceivedWithAnyArgs(1)
                .GetAsync(Arg.Any<Expression<Func<User, bool>>>());
        }
    }
}
