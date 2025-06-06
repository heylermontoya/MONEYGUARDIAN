using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Exceptions;
using MONEY_GUARDIAN.Domain.Ports;
using MONEY_GUARDIAN.Domain.Services.deposit;
using MONEY_GUARDIAN.Domain.Services.monetaryFund;
using MONEY_GUARDIAN.Domain.Services.user;
using MONEY_GUARDIAN.Domain.Tests.DataBuilder;
using NSubstitute;

namespace MONEY_GUARDIAN.Domain.Tests.deposit
{
    [TestClass]
    public class DepositServiceTest
    {
        private DepositService Service { get; set; } = default!;
        private MonetaryFundService MonetaryFundService { get; set; } = default!;
        private UserService UserService { get; set; } = default!;
        private IGenericRepository<Deposit> Repository { get; set; } = default!;
        private IGenericRepository<User> UserRepository { get; set; } = default!;
        private IGenericRepository<MonetaryFund> MonetaryFundRepository { get; set; } = default!;

        private MonetaryFundBuilder MonetaryFundBuilder { get; set; } = default!;
        private UserBuilder UserBuilder { get; set; } = default!;
        private DepositBuilder DepositBuilder { get; set; } = default!;

        [TestInitialize]
        public void Initialize()
        {
            Repository = Substitute.For<IGenericRepository<Deposit>>();
            UserRepository = Substitute.For<IGenericRepository<User>>();
            MonetaryFundRepository = Substitute.For<IGenericRepository<MonetaryFund>>();

            UserService = new(UserRepository);
            MonetaryFundService = new(MonetaryFundRepository);

            Service = new(
                Repository,
                MonetaryFundService,
                UserService
            );

            MonetaryFundBuilder = new();
            UserBuilder = new();
            DepositBuilder = new();
        }

        [TestMethod]
        public async Task CreateDepositAsync_Ok()
        {
            //Arrange
            DateTime date = new(2020, 6, 5, 14, 30, 0, DateTimeKind.Utc);
            int monetaryFundId = 1;
            decimal amount = 0;
            int userId = 1;

            MonetaryFund monetaryFund = MonetaryFundBuilder
                .WithId(monetaryFundId)
                .Build();

            MonetaryFundRepository
                .GetByIdAsync(monetaryFundId)
                .ReturnsForAnyArgs(monetaryFund);

            User user = UserBuilder
                .WithId(userId)
                .Build();


            UserRepository
                .GetByIdAsync(userId)
                .ReturnsForAnyArgs(user);

            Deposit deposit = DepositBuilder
                .WithDate(date)
                .WithMonetaryFundId(monetaryFundId)
                .WithAmount(amount)
                .WithUserId(userId)
                .Build();

            Repository
                .AddAsync(deposit)
                .ReturnsForAnyArgs(deposit);

            //Act

            var result = await Service.CreateDepositAsync(
                date,
                monetaryFundId,
                amount,
                userId
            );

            //Assert
            Assert.AreEqual(date, result.Date);
            Assert.AreEqual(monetaryFundId, result.MonetaryFundId);
            Assert.AreEqual(amount, result.Amount);
            Assert.AreEqual(userId, result.UserId);

            await MonetaryFundRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await UserRepository
                 .ReceivedWithAnyArgs(1)
                 .GetByIdAsync(Arg.Any<int>());
            await Repository
                .ReceivedWithAnyArgs(1)
                .AddAsync(Arg.Any<Deposit>());
        }

        [TestMethod]
        public async Task CreateDepositAsync_FailedYearInvalid()
        {
            //Arrange
            DateTime date = DateTime.Now.AddYears(-100);
            int monetaryFundId = 1;
            decimal amount = 0;
            int userId = 1;

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.CreateDepositAsync(
                    date,
                    monetaryFundId,
                    amount,
                    userId
                );
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.YearInvalid, ex.Message);
        }

        [TestMethod]
        public async Task CreateDepositAsync_FailedAmountInvalid()
        {
            //Arrange
            DateTime date = DateTime.Now;
            int monetaryFundId = 1;
            decimal amount = -1;
            int userId = 1;

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.CreateDepositAsync(
                    date,
                    monetaryFundId,
                    amount,
                    userId
                );
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.AmountGreatherThanZero, ex.Message);
        }

        [TestMethod]
        public async Task UpdateDepositAsync_Ok()
        {
            //Arrange
            int id = 1;
            DateTime date = DateTime.Now;
            int monetaryFundId = 1;
            decimal amount = 10000;
            int userId = 1;

            MonetaryFund monetaryFund = MonetaryFundBuilder
                .WithId(monetaryFundId)
                .Build();

            MonetaryFundRepository
                .GetByIdAsync(monetaryFundId)
                .ReturnsForAnyArgs(monetaryFund);


            Deposit deposit = DepositBuilder
                .WithDate(date)
                .WithMonetaryFundId(monetaryFundId)
                .WithAmount(amount)
                .WithUserId(userId)
                .Build();

            Repository
                .GetByIdAsync(id)
                .ReturnsForAnyArgs(deposit);

            Repository
                .UpdateAsync(deposit)
                .ReturnsForAnyArgs(deposit);

            //Act

            var result = await Service.UpdateDepositAsync(
                id,
                date,
                monetaryFundId,
                amount
            );

            //Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(date, result.Date);
            Assert.AreEqual(monetaryFundId, result.MonetaryFundId);
            Assert.AreEqual(amount, result.Amount);
            Assert.AreEqual(userId, result.UserId);

            await MonetaryFundRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await Repository
                 .ReceivedWithAnyArgs(1)
                 .GetByIdAsync(Arg.Any<int>());
            await Repository
                .ReceivedWithAnyArgs(1)
                .UpdateAsync(Arg.Any<Deposit>());
        }

        [TestMethod]
        public async Task GetDepositByIdAsync_Ok()
        {
            //Arrange
            int id = 1;
            DateTime date = DateTime.Now;
            int monetaryFundId = 1;
            decimal amount = 10000;
            int userId = 1;

            Deposit deposit = DepositBuilder
               .WithDate(date)
               .WithMonetaryFundId(monetaryFundId)
               .WithAmount(amount)
               .WithUserId(userId)
               .Build();

            Repository
                .GetByIdAsync(id)
                .ReturnsForAnyArgs(deposit);

            //Act

            var result = await Service.GetDepositById(id);

            //Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(date, result.Date);
            Assert.AreEqual(monetaryFundId, result.MonetaryFundId);
            Assert.AreEqual(amount, result.Amount);
            Assert.AreEqual(userId, result.UserId);

            await Repository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
        }
    }
}
