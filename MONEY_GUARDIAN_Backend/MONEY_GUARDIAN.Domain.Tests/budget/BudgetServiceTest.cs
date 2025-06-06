using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Exceptions;
using MONEY_GUARDIAN.Domain.Ports;
using MONEY_GUARDIAN.Domain.Services.budget;
using MONEY_GUARDIAN.Domain.Services.expenseType;
using MONEY_GUARDIAN.Domain.Services.user;
using MONEY_GUARDIAN.Domain.Tests.DataBuilder;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MONEY_GUARDIAN.Domain.Tests.budget
{
    [TestClass]
    public class BudgetServiceTest
    {
        private BudgetService Service { get; set; } = default!;
        private ExpenseTypeService ExpenseTypeService { get; set; } = default!;
        private UserService UserService { get; set; } = default!;
        private IGenericRepository<Deposit> DepositRepository { get; set; } = default!;
        private IGenericRepository<ExpenseType> ExpenseTypeRepository { get; set; } = default!;
        private IGenericRepository<Budget> Repository { get; set; } = default!;
        private IGenericRepository<User> UserRepository { get; set; } = default!;

        private BudgetBuilder BudgetBuilder { get; set; } = default!;
        private ExpenseTypeBuilder ExpenseTypeBuilder { get; set; } = default!;
        private UserBuilder UserBuilder { get; set; } = default!;
        private DepositBuilder DepositBuilder { get; set; } = default!;

        [TestInitialize]
        public void Initialize()
        {
            DepositRepository = Substitute.For<IGenericRepository<Deposit>>();
            ExpenseTypeRepository = Substitute.For<IGenericRepository<ExpenseType>>();
            Repository = Substitute.For<IGenericRepository<Budget>>();
            UserRepository = Substitute.For<IGenericRepository<User>>();

            ExpenseTypeService = new(ExpenseTypeRepository);

            UserService = new(UserRepository);

            Service = new(
                Repository,
                DepositRepository,
                ExpenseTypeService,
                UserService
            );

            BudgetBuilder = new();
            ExpenseTypeBuilder = new();
            UserBuilder = new();
            DepositBuilder = new();
        }

        [TestMethod]
        public async Task CreateBudgetAsync_Ok()
        {
            //Arrange
            int expenseTypeId = 1;
            int userId = 1;
            int year = 2020;
            int month = 1;
            decimal amount = 0;

            ExpenseType expenseType = ExpenseTypeBuilder
                .WithId(expenseTypeId)
                .Build();

            User user = UserBuilder
                .WithId(userId)
                .Build();

            Budget budget = BudgetBuilder
                .WithExpenseTypeId(expenseTypeId)
                .WithUserId(userId)
                .WithYear(year)
                .WithMonth(month)
                .WithAmount(amount)
                .Build();

            Deposit deposit = DepositBuilder
                .WithAmount(30000)
                .Build();

            ExpenseTypeRepository.GetByIdAsync(expenseTypeId)
                .ReturnsForAnyArgs(expenseType);

            UserRepository
                .GetByIdAsync(userId)
                .ReturnsForAnyArgs(user);

            Repository.GetAsync()
                .ReturnsForAnyArgs([budget]);

            DepositRepository
                .GetAsync()
               .ReturnsForAnyArgs([deposit]);

            Repository
                .GetAsync(Arg.Any<Expression<Func<Budget, bool>>>())
                .Returns([]);

            Repository.AddAsync(budget)
               .ReturnsForAnyArgs(budget);

            //Act
            var result = await Service.CreateBudgetAsync(
                expenseTypeId,
                userId,
                year,
                month,
                amount
            );

            //Assert
            Assert.AreEqual(expenseTypeId, result.ExpenseTypeId);
            Assert.AreEqual(userId, result.UserId);
            Assert.AreEqual(year, result.Year);
            Assert.AreEqual(month, result.Month);
            Assert.AreEqual(amount, result.Amount);

            await ExpenseTypeRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await UserRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await DepositRepository
                .ReceivedWithAnyArgs(1)
                .GetAsync(
                    Arg.Any<Expression<Func<Deposit, bool>>>()
                );
            await Repository
                .ReceivedWithAnyArgs(2)
                .GetAsync(
                    Arg.Any<Expression<Func<Budget, bool>>>()
                );
            await Repository.ReceivedWithAnyArgs(1)
                .AddAsync(
                    Arg.Any<Budget>()
                );
        }

        [TestMethod]
        public async Task CreateBudgetAsync_FailedBudgetExceedsDeposits()
        {
            //Arrange
            int expenseTypeId = 1;
            int userId = 1;
            int year = 2020;
            int month = 1;
            decimal amount = 1000;

            ExpenseType expenseType = ExpenseTypeBuilder
                .WithId(expenseTypeId)
                .Build();

            ExpenseTypeRepository.GetByIdAsync(expenseTypeId)
                .ReturnsForAnyArgs(expenseType);

            User user = UserBuilder
                .WithId(userId)
                .Build();

            UserRepository
                .GetByIdAsync(userId)
                .ReturnsForAnyArgs(user);

            Budget budget = BudgetBuilder
                .WithExpenseTypeId(expenseTypeId)
                .WithUserId(userId)
                .WithYear(year)
                .WithMonth(month)
                .WithAmount(amount)
                .Build();

            Repository.GetAsync()
                .ReturnsForAnyArgs([budget]);

            Deposit deposit = DepositBuilder
                .WithAmount(1000)
                .Build();

            DepositRepository
                .GetAsync()
               .ReturnsForAnyArgs([deposit]);

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.CreateBudgetAsync(
                    expenseTypeId,
                    userId,
                    year,
                    month,
                    amount
                );
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.BudgetExceedsDeposits, ex.Message);

            await ExpenseTypeRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await UserRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await DepositRepository
                .ReceivedWithAnyArgs(1)
                .GetAsync(
                    Arg.Any<Expression<Func<Deposit, bool>>>()
                );
            await Repository
                .ReceivedWithAnyArgs(1)
                .GetAsync(
                    Arg.Any<Expression<Func<Budget, bool>>>()
                );
        }

        [TestMethod]
        public async Task CreateBudgetAsync_FailedUniqueMonthlyBudget()
        {
            //Arrange
            int expenseTypeId = 1;
            int userId = 1;
            int year = 2020;
            int month = 1;
            decimal amount = 1000;

            ExpenseType expenseType = ExpenseTypeBuilder
                .WithId(expenseTypeId)
                .Build();

            ExpenseTypeRepository.GetByIdAsync(expenseTypeId)
                .ReturnsForAnyArgs(expenseType);

            User user = UserBuilder
                .WithId(userId)
                .Build();

            UserRepository
                .GetByIdAsync(userId)
                .ReturnsForAnyArgs(user);

            Budget budget = BudgetBuilder
                .WithExpenseTypeId(expenseTypeId)
                .WithUserId(userId)
                .WithYear(year)
                .WithMonth(month)
                .WithAmount(amount)
                .Build();

            Repository.GetAsync()
                .ReturnsForAnyArgs([budget]);

            Deposit deposit = DepositBuilder
                .WithAmount(2000)
                .Build();

            DepositRepository
                .GetAsync()
               .ReturnsForAnyArgs([deposit]);

            Repository
               .GetAsync(Arg.Any<Expression<Func<Budget, bool>>>())
               .Returns([budget]);

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.CreateBudgetAsync(
                    expenseTypeId,
                    userId,
                    year,
                    month,
                    amount
                );
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.UniqueMonthlyBudget, ex.Message);

            await ExpenseTypeRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await UserRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await DepositRepository
                .ReceivedWithAnyArgs(1)
                .GetAsync(
                    Arg.Any<Expression<Func<Deposit, bool>>>()
                );
            await Repository
                .ReceivedWithAnyArgs(2)
                .GetAsync(
                    Arg.Any<Expression<Func<Budget, bool>>>()
                );
        }

        [TestMethod]
        public async Task CreateBudgetAsync_FailedYearInvalid()
        {
            //Arrange
            int expenseTypeId = 1;
            int userId = 1;
            int year = 2019;
            int month = 12;
            decimal amount = 0;

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.CreateBudgetAsync(
                    expenseTypeId,
                    userId,
                    year,
                    month,
                    amount
                );
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.YearInvalid, ex.Message);
        }

        [TestMethod]
        public async Task CreateBudgetAsync_FailedMonthInvalid()
        {
            //Arrange
            int expenseTypeId = 1;
            int userId = 1;
            int year = 2020;
            int month = 13;
            decimal amount = 0;

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.CreateBudgetAsync(
                    expenseTypeId,
                    userId,
                    year,
                    month,
                    amount
                );
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.MonthInvalid, ex.Message);
        }

        [TestMethod]
        public async Task CreateBudgetAsync_FailedAmountInvalid()
        {
            //Arrange
            int expenseTypeId = 1;
            int userId = 1;
            int year = 2020;
            int month = 12;
            decimal amount = -1;

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.CreateBudgetAsync(
                    expenseTypeId,
                    userId,
                    year,
                    month,
                    amount
                );
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.AmountGreatherThanZero, ex.Message);
        }

        [TestMethod]
        public async Task UpdateBudgetAsync_Ok()
        {
            //Arrange
            int id = 1;
            int expenseTypeId = 1;
            int userId = 1;
            int year = 2023;
            int month = 1;
            decimal amount = 10000;

            ExpenseType expenseType = ExpenseTypeBuilder
                .WithId(expenseTypeId)
                .Build();

            ExpenseTypeRepository.GetByIdAsync(expenseTypeId)
               .ReturnsForAnyArgs(expenseType);


            User user = UserBuilder
                .WithId(userId)
                .Build();


            UserRepository
                .GetByIdAsync(userId)
                .ReturnsForAnyArgs(user);

            Budget budget = BudgetBuilder
                .WithExpenseTypeId(expenseTypeId)
                .WithUserId(userId)
                .WithYear(year)
                .WithMonth(month)
                .WithAmount(amount)
                .Build();

            Repository
             .GetAsync(Arg.Is<Expression<Func<Budget, bool>>>(
                 expr =>
                     expr.Compile().Invoke(new Budget { Id = 99 }) &&
                     !expr.Compile().Invoke(new Budget { Id = id })
             ))
             .Returns([budget]);

            Deposit deposit = DepositBuilder
              .WithAmount(30000)
              .Build();


            DepositRepository
               .GetAsync()
              .ReturnsForAnyArgs([deposit]);

            Repository
                .GetAsync(Arg.Is<Expression<Func<Budget, bool>>>(expr =>
                    expr.Compile().Invoke(new Budget
                    {
                        Id = 2,
                        UserId = userId,
                        ExpenseTypeId = expenseTypeId,
                        Month = month,
                        Year = year
                    }) &&
                    !expr.Compile().Invoke(new Budget
                    {
                        Id = id,
                        UserId = userId,
                        ExpenseTypeId = expenseTypeId,
                        Month = month,
                        Year = year
                    })
                ))
                .Returns([]);

            Repository
                .GetByIdAsync(id)
                .ReturnsForAnyArgs(budget);

            Repository
                .UpdateAsync(budget)
                .ReturnsForAnyArgs(budget);

            //Act
            var result = await Service.UpdateBudgetAsync(
                id,
                expenseTypeId,
                userId,
                year,
                month,
                amount
            );

            //Assert
            Assert.AreEqual(expenseTypeId, result.ExpenseTypeId);
            Assert.AreEqual(userId, result.UserId);
            Assert.AreEqual(year, result.Year);
            Assert.AreEqual(month, result.Month);
            Assert.AreEqual(amount, result.Amount);

            await ExpenseTypeRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await UserRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await DepositRepository
                .ReceivedWithAnyArgs(1)
                .GetAsync(
                    Arg.Any<Expression<Func<Deposit, bool>>>()
                );
            await Repository
                .ReceivedWithAnyArgs(2)
                .GetAsync(
                    Arg.Any<Expression<Func<Budget, bool>>>()
                );
            await Repository
                .ReceivedWithAnyArgs(1)
               .GetByIdAsync(Arg.Any<int>());
            await Repository
                .ReceivedWithAnyArgs(1)
                .UpdateAsync(
                    Arg.Any<Budget>()
                );
        }

        [TestMethod]
        public async Task UpdateBudgetAsync_UniqueMonthlyBudget()
        {
            //Arrange
            int id = 1;
            int expenseTypeId = 1;
            int userId = 1;
            int year = 2023;
            int month = 1;
            decimal amount = 10000;

            ExpenseType expenseType = ExpenseTypeBuilder
                .WithId(expenseTypeId)
                .Build();

            ExpenseTypeRepository.GetByIdAsync(expenseTypeId)
               .ReturnsForAnyArgs(expenseType);


            User user = UserBuilder
                .WithId(userId)
                .Build();


            UserRepository
                .GetByIdAsync(userId)
                .ReturnsForAnyArgs(user);

            Budget budget = BudgetBuilder
                .WithExpenseTypeId(expenseTypeId)
                .WithUserId(userId)
                .WithYear(year)
                .WithMonth(month)
                .WithAmount(amount)
                .Build();

            Repository
             .GetAsync(Arg.Is<Expression<Func<Budget, bool>>>(
                 expr =>
                     expr.Compile().Invoke(new Budget { Id = 99 }) &&
                     !expr.Compile().Invoke(new Budget { Id = id })
             ))
             .Returns([budget]);

            Deposit deposit = DepositBuilder
              .WithAmount(30000)
              .Build();


            DepositRepository
               .GetAsync()
              .ReturnsForAnyArgs([deposit]);

            Repository
                .GetAsync(Arg.Is<Expression<Func<Budget, bool>>>(expr =>
                    expr.Compile().Invoke(new Budget
                    {
                        Id = 2,
                        UserId = userId,
                        ExpenseTypeId = expenseTypeId,
                        Month = month,
                        Year = year
                    }) &&
                    !expr.Compile().Invoke(new Budget
                    {
                        Id = id,
                        UserId = userId,
                        ExpenseTypeId = expenseTypeId,
                        Month = month,
                        Year = year
                    })
                ))
                .Returns([budget]);

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.UpdateBudgetAsync(
                    id,
                    expenseTypeId,
                    userId,
                    year,
                    month,
                    amount
                );
            });
            //Assert
            Assert.AreEqual(MessagesExceptions.UniqueMonthlyBudget, ex.Message);

            await ExpenseTypeRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await UserRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await DepositRepository
                .ReceivedWithAnyArgs(1)
                .GetAsync(
                    Arg.Any<Expression<Func<Deposit, bool>>>()
                );
            await Repository
                .ReceivedWithAnyArgs(2)
                .GetAsync(
                    Arg.Any<Expression<Func<Budget, bool>>>()
                );
        }

        [TestMethod]
        public async Task GetBudgetByIdAsync_Ok()
        {
            //Arrange
            int id = 1;
            int expenseTypeId = 1;
            int userId = 1;
            int year = 2023;
            int month = 1;
            decimal amount = 10000;

            Budget budget = BudgetBuilder
                .WithExpenseTypeId(expenseTypeId)
                .WithUserId(userId)
                .WithYear(year)
                .WithMonth(month)
                .WithAmount(amount)
                .Build();

            Repository
                .GetByIdAsync(id)
                .ReturnsForAnyArgs(budget);

            //Act

            var result = await Service.GetBudgetById(id);

            //Assert
            Assert.AreEqual(expenseTypeId, result.ExpenseTypeId);
            Assert.AreEqual(userId, result.UserId);
            Assert.AreEqual(year, result.Year);
            Assert.AreEqual(month, result.Month);
            Assert.AreEqual(amount, result.Amount);

            await Repository
                .ReceivedWithAnyArgs(1)
               .GetByIdAsync(Arg.Any<int>());
        }

        [TestMethod]
        public async Task GetBudgetByIdAsync_Failed()
        {
            //Arrange
            int id = 1;

            Repository
                .GetByIdAsync(id)
                .ReturnsNull();

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.GetBudgetById(id);
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.NotExistBudget, ex.Message);

            await Repository
                .ReceivedWithAnyArgs(1)
               .GetByIdAsync(Arg.Any<int>());
        }
    }
}
