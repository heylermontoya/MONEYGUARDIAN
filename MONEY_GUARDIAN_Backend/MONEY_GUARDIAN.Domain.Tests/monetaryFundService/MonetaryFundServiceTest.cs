using Castle.Core.Resource;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Exceptions;
using MONEY_GUARDIAN.Domain.Ports;
using MONEY_GUARDIAN.Domain.Services.monetaryFund;
using MONEY_GUARDIAN.Domain.Tests.DataBuilder;
using NSubstitute;
using System.Linq.Expressions;

namespace MONEY_GUARDIAN.Domain.Tests.monetaryFundService
{
    [TestClass]
    public class MonetaryFundServiceTest
    {
        private MonetaryFundService Service { get; set; } = default!;
        private IGenericRepository<MonetaryFund> Repository { get; set; } = default!;
        private MonetaryFundBuilder MonetaryFundBuilder { get; set; } = default!;

        [TestInitialize]
        public void Initialize()
        {
            Repository = Substitute.For<IGenericRepository<MonetaryFund>>();

            Service = new(Repository);

            MonetaryFundBuilder = new();
        }


        [TestMethod]
        public async Task CreateMonetaryFundAsync_Ok()
        {
            //Arrange
            int id = 1;
            string type = "typeName";
            string name = "name";

            Repository
              .GetAsync()
              .ReturnsForAnyArgs([]);

            MonetaryFund monetaryFund = MonetaryFundBuilder
                .WithId(id)
                .WithName(name)
                .WithType(type)
                .Build();

            Repository
               .AddAsync(monetaryFund)
               .ReturnsForAnyArgs(monetaryFund);            

            //Act
            var result = await Service.CreateMonetaryFundAsync(
                name,
                type
            );

            //Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(type, result.Type);
            Assert.AreEqual(name, result.Name);

            await Repository
               .ReceivedWithAnyArgs(1)
               .GetAsync(Arg.Any<Expression<Func<MonetaryFund, bool>>>());
            await Repository
               .ReceivedWithAnyArgs(1)
               .AddAsync(Arg.Any<MonetaryFund>());
        }
        
        [TestMethod]
        public async Task CreateMonetaryFundAsync_Failed()
        {
            //Arrange
            int id = 1;
            string type = "typeName";
            string name = "name";

            MonetaryFund monetaryFund = MonetaryFundBuilder
                .WithId(id)
                .WithName(name)
                .WithType(type)
                .Build();

            Repository
              .GetAsync()
              .ReturnsForAnyArgs([monetaryFund]);                      

            //Act            
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.CreateMonetaryFundAsync(
                    name,
                    type
                );
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.ExistMonetaryFund, ex.Message);

            await Repository
               .ReceivedWithAnyArgs(1)
               .GetAsync(Arg.Any<Expression<Func<MonetaryFund, bool>>>());
        }
        
        [TestMethod]
        public async Task UpdateMonetaryFundAsync_Ok()
        {
            //Arrange
            int id = 1;
            string type = "typeName";
            string name = "name";

            Repository
              .GetAsync()
              .ReturnsForAnyArgs([]);

            MonetaryFund monetaryFund = MonetaryFundBuilder
                .WithId(id)
                .WithName(name)
                .WithType(type)
                .Build();

            Repository
               .GetByIdAsync(id)
               .ReturnsForAnyArgs(monetaryFund);

            Repository
                .UpdateAsync(monetaryFund)
                .ReturnsForAnyArgs(monetaryFund);

            //Act
            var result = await Service.UpdateMonetaryFundAsync(id, name,type);

            //Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(type, result.Type);
            Assert.AreEqual(name, result.Name);

            await Repository
               .ReceivedWithAnyArgs(1)
               .GetAsync(Arg.Any<Expression<Func<MonetaryFund, bool>>>());
            await Repository
               .ReceivedWithAnyArgs(1)
               .GetByIdAsync(Arg.Any<int>());
            await Repository
               .ReceivedWithAnyArgs(1)
               .UpdateAsync(Arg.Any<MonetaryFund>());
        }
        
        [TestMethod]
        public async Task UpdateMonetaryFundAsync_Failed()
        {
            //Arrange
            int id = 1;
            string type = "typeName";
            string name = "name";

            MonetaryFund monetaryFund = MonetaryFundBuilder
                .WithId(id)
                .WithName(name)
                .WithType(type)
                .Build();

            Repository
              .GetAsync()
              .ReturnsForAnyArgs([monetaryFund]);            

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.UpdateMonetaryFundAsync(id, name, type);
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.ExistMonetaryFund, ex.Message);

            await Repository
               .ReceivedWithAnyArgs(1)
               .GetAsync(Arg.Any<Expression<Func<MonetaryFund, bool>>>());            
        }

        [TestMethod]
        public async Task GetMonetaryFundByIdAsync_Ok()
        {
            //Arrange
            int id = 1;
            string type = "typeName";
            string name = "name";

            MonetaryFund monetaryFund = MonetaryFundBuilder
                .WithId(id)
                .WithName(name)
                .WithType(type)
                .Build();

            Repository
               .GetByIdAsync(id)
               .ReturnsForAnyArgs(monetaryFund);

            //Act
            var result = await Service.GetMonetaryFundById(id);

            //Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(type, result.Type);
            Assert.AreEqual(name, result.Name);

            await Repository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
        }
    }
}
