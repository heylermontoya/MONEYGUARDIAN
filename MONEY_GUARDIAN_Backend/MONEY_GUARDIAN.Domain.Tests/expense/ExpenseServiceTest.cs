using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Exceptions;
using MONEY_GUARDIAN.Domain.Ports;
using MONEY_GUARDIAN.Domain.Services.expense;
using MONEY_GUARDIAN.Domain.Services.monetaryFund;
using MONEY_GUARDIAN.Domain.Services.user;
using MONEY_GUARDIAN.Domain.Tests.DataBuilder;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace MONEY_GUARDIAN.Domain.Tests.expense
{
    [TestClass]
    public class ExpenseServiceTest
    {
        private ExpenseService Service { get; set; } = default!;
        private UserService UserService { get; set; } = default!;
        private MonetaryFundService MonetaryFundService { get; set; } = default!;
        private IGenericRepository<ExpenseDetail> ExpenseDetailRepository { get; set; } = default!;
        private IGenericRepository<ExpenseHeader> ExpenseHeaderRepository { get; set; } = default!;
        private IGenericRepository<Budget> BudgetRepository { get; set; } = default!;
        private IGenericRepository<MonetaryFund> MonetaryFundRepository { get; set; } = default!;
        private IGenericRepository<User> UserRepository { get; set; } = default!;
        private ExpenseDetailBuilder ExpenseDetailBuilder { get; set; } = default!;
        private ExpenseHeaderBuilder ExpenseHeaderBuilder { get; set; } = default!;
        private MonetaryFundBuilder MonetaryFundBuilder { get; set; } = default!;
        private UserBuilder UserBuilder { get; set; } = default!;
        private BudgetBuilder BudgetBuilder { get; set; } = default!;

        [TestInitialize]
        public void Initialize()
        {
            ExpenseDetailRepository = Substitute.For<IGenericRepository<ExpenseDetail>>();
            ExpenseHeaderRepository = Substitute.For<IGenericRepository<ExpenseHeader>>();
            BudgetRepository = Substitute.For<IGenericRepository<Budget>>();
            MonetaryFundRepository = Substitute.For<IGenericRepository<MonetaryFund>>();
            UserRepository = Substitute.For<IGenericRepository<User>>();

            MonetaryFundService = new(MonetaryFundRepository);
            UserService = new(UserRepository);

            Service = new(
                ExpenseDetailRepository,
                ExpenseHeaderRepository,
                BudgetRepository,
                MonetaryFundService,
                UserService
            );

            ExpenseDetailBuilder = new();
            ExpenseHeaderBuilder = new();
            MonetaryFundBuilder = new();
            UserBuilder = new();
            BudgetBuilder = new();
        }

        [TestMethod]
        public async Task CreateExpenseAsync_Ok()
        {
            //Arrange
            int expenseHeaderId = 1;

            ExpenseDetail expenseDetail = ExpenseDetailBuilder
                    .WithExpenseHeaderId(expenseHeaderId)
                    .Build();

            List<ExpenseDetail> listExpenseDetail = [
                expenseDetail
            ];
            DateTime date = new(2020, 6, 5, 14, 30, 0, DateTimeKind.Utc);
            int monetaryFundId = 1;
            int userId = 1;
            string observation = "observation";
            string merchant = "merchant";
            string documentType = "documentType";

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

            Budget budget = BudgetBuilder.Build();

            BudgetRepository
                .GetAsync()
                .ReturnsForAnyArgs([budget]);

            ExpenseHeader expenseHeader = ExpenseHeaderBuilder
                .WithId(expenseHeaderId)
                .WithDate(date)
                .WithMonetaryFundId(monetaryFundId)
                .WithUserId(userId)
                .WithObservations(observation)
                .WithMerchant(merchant)
                .WithDocumentType(documentType)
                .Build();

            ExpenseHeaderRepository
                .AddAsync(expenseHeader)
                .ReturnsForAnyArgs(expenseHeader);

            // for para add los detalles

            ExpenseDetailRepository
                .AddAsync(expenseDetail)
                .ReturnsForAnyArgs(expenseDetail);

            //

            //Act
            var result = await Service.CreateExpenseAsync(
                listExpenseDetail,
                date,
                monetaryFundId,
                userId,
                observation,
                merchant,
                documentType
            );

            //Assert
            Assert.AreEqual(date, result.Date);
            Assert.AreEqual(monetaryFundId, result.MonetaryFundId);
            Assert.AreEqual(userId, result.UserId);
            Assert.AreEqual(observation, result.Observations);
            Assert.AreEqual(merchant, result.Merchant);
            Assert.AreEqual(documentType, result.DocumentType);

            await MonetaryFundRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await UserRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await BudgetRepository
                .ReceivedWithAnyArgs(1)
                .GetAsync(
                    Arg.Any<Expression<Func<Budget, bool>>>()
                );
            await ExpenseHeaderRepository
                .ReceivedWithAnyArgs(1)
                .AddAsync(Arg.Any<ExpenseHeader>());
            await ExpenseDetailRepository
                .ReceivedWithAnyArgs(1)
                .AddAsync(Arg.Any<ExpenseDetail>());
        }
        
        [TestMethod]
        public async Task CreateExpenseAsync_FailedYearInvalid()
        {
            //Arrange
            int expenseHeaderId = 1;
            ExpenseDetail expenseDetail = ExpenseDetailBuilder
                    .WithExpenseHeaderId(expenseHeaderId)
                    .Build();

            List<ExpenseDetail> listExpenseDetail = [
                expenseDetail
            ];

            DateTime date = DateTime.Now.AddYears(-100);
            int monetaryFundId = 1;
            int userId = 1;
            string observation = "observation";
            string merchant = "merchant";
            string documentType = "documentType";
           
            //Act            
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.CreateExpenseAsync(
                    listExpenseDetail,
                    date,
                    monetaryFundId,
                    userId,
                    observation,
                    merchant,
                    documentType
                );
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.YearInvalid, ex.Message);
        }
        
        [TestMethod]
        public async Task CreateExpenseAsync_FailedWithoutBudget()
        {
            //Arrange
            int expenseHeaderId = 1;

            ExpenseDetail expenseDetail = ExpenseDetailBuilder
                    .WithExpenseHeaderId(expenseHeaderId)
                    .Build();

            List<ExpenseDetail> listExpenseDetail = [
                expenseDetail
            ];
            DateTime date = DateTime.Now;
            int monetaryFundId = 1;
            int userId = 1;
            string observation = "observation";
            string merchant = "merchant";
            string documentType = "documentType";

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


            BudgetRepository
                .GetAsync()
                .ReturnsNull();

            //Act            
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.CreateExpenseAsync(
                    listExpenseDetail,
                    date,
                    monetaryFundId,
                    userId,
                    observation,
                    merchant,
                    documentType
                );
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.NotExistBudget, ex.Message);

            await MonetaryFundRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await UserRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await BudgetRepository
                .ReceivedWithAnyArgs(1)
                .GetAsync(
                    Arg.Any<Expression<Func<Budget, bool>>>()
                );           
        }
        
        [TestMethod]
        public async Task CreateExpenseAsync_FailedAmountExpenseGreaterThanBudget()
        {
            //Arrange
            int expenseHeaderId = 1;

            ExpenseDetail expenseDetail = ExpenseDetailBuilder
                    .WithExpenseHeaderId(expenseHeaderId)
                    .Build();

            List<ExpenseDetail> listExpenseDetail = [
                expenseDetail
            ];
            DateTime date = DateTime.Now;
            int monetaryFundId = 1;
            int userId = 1;
            string observation = "observation";
            string merchant = "merchant";
            string documentType = "documentType";

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


            Budget budget = BudgetBuilder.WithAmount(1).Build();

            BudgetRepository
                .GetAsync()
                .ReturnsForAnyArgs([budget]);

            //Act            
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.CreateExpenseAsync(
                    listExpenseDetail,
                    date,
                    monetaryFundId,
                    userId,
                    observation,
                    merchant,
                    documentType
                );
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.ExpenseExceedsBudget, ex.Message);

            await MonetaryFundRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await UserRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await BudgetRepository
                .ReceivedWithAnyArgs(1)
                .GetAsync(
                    Arg.Any<Expression<Func<Budget, bool>>>()
                );           
        }

        [TestMethod]
        public async Task UpdateExpenseAsync_Ok()
        {
            //Arrange
            int expenseHeaderId = 1;
            int expenseDetailId = 1;

            ExpenseDetail expenseDetail = ExpenseDetailBuilder
                    .WithId(expenseDetailId)
                    .WithExpenseHeaderId(expenseHeaderId)
                    .Build();

            List<ExpenseDetail> listExpenseDetail = [
                expenseDetail,
                ExpenseDetailBuilder.WithId(0).Build()
            ];

            DateTime date = DateTime.Now;
            int monetaryFundId = 1;
            int userId = 1;
            string observation = "observation";
            string merchant = "merchant";
            string documentType = "documentType";

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

            Budget budget = BudgetBuilder.WithAmount(2000).Build();

            BudgetRepository
                .GetAsync()
                .ReturnsForAnyArgs([budget]);

            ExpenseDetailRepository
                .DeleteAsync(
                    Arg.Any<Expression<Func<ExpenseDetail, bool>>>()
                )
                .ReturnsForAnyArgs(Task.CompletedTask);

            ExpenseDetailRepository
                .AddAsync(expenseDetail)
                .ReturnsForAnyArgs(expenseDetail);

            ExpenseDetailRepository
                .GetByIdAsync(expenseDetailId)
                .ReturnsForAnyArgs(expenseDetail);

            ExpenseDetailRepository
                .UpdateAsync(expenseDetail)
                .ReturnsForAnyArgs(expenseDetail);

            ExpenseHeader expenseHeader = ExpenseHeaderBuilder
                .WithId(expenseHeaderId)
                .WithDate(date)
                .WithMonetaryFundId(monetaryFundId)
                .WithUserId(userId)
                .WithObservations(observation)
                .WithMerchant(merchant)
                .WithDocumentType(documentType)
                .Build();

            ExpenseHeaderRepository
                .GetByIdAsync(expenseHeaderId)
                .ReturnsForAnyArgs(expenseHeader);

            ExpenseHeaderRepository
                .UpdateAsync(expenseHeader)
                .ReturnsForAnyArgs(expenseHeader);

            //Act
            var result = await Service.UpdateExpenseAsync(
                expenseHeaderId,
                listExpenseDetail,
                date,
                monetaryFundId,
                userId,
                observation,
                merchant,
                documentType
            );

            //Assert
            Assert.AreEqual(expenseHeaderId, result.Id);
            Assert.AreEqual(date, result.Date);
            Assert.AreEqual(monetaryFundId, result.MonetaryFundId);
            Assert.AreEqual(userId, result.UserId);
            Assert.AreEqual(observation, result.Observations);
            Assert.AreEqual(merchant, result.Merchant);
            Assert.AreEqual(documentType, result.DocumentType);

            await MonetaryFundRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await UserRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await BudgetRepository
                .ReceivedWithAnyArgs(2)
                .GetAsync(
                    Arg.Any<Expression<Func<Budget, bool>>>()
                );
            await ExpenseDetailRepository
                .ReceivedWithAnyArgs(1)
                .DeleteAsync(
                   Arg.Any<Expression<Func<ExpenseDetail, bool>>>()
                );
            await ExpenseDetailRepository
                .ReceivedWithAnyArgs(1)
                .AddAsync(
                    Arg.Any<ExpenseDetail>()
                );
            await ExpenseDetailRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await ExpenseDetailRepository
                .ReceivedWithAnyArgs(1)
                .UpdateAsync(Arg.Any<ExpenseDetail>());
            await ExpenseHeaderRepository
               .ReceivedWithAnyArgs(1)
               .GetByIdAsync(Arg.Any<int>());
            await ExpenseHeaderRepository
                .ReceivedWithAnyArgs(1)
                .UpdateAsync(Arg.Any<ExpenseHeader>());
        }
        
        [TestMethod]
        public async Task UpdateExpenseAsync_FailedWithoutBudget()
        {
            //Arrange
            int expenseHeaderId = 1;
            int expenseDetailId = 1;

            ExpenseDetail expenseDetail = ExpenseDetailBuilder
                    .WithId(expenseDetailId)
                    .WithExpenseHeaderId(expenseHeaderId)
                    .Build();

            List<ExpenseDetail> listExpenseDetail = [
                expenseDetail,
                ExpenseDetailBuilder.WithId(0).Build()
            ];

            DateTime date = DateTime.Now;
            int monetaryFundId = 1;
            int userId = 1;
            string observation = "observation";
            string merchant = "merchant";
            string documentType = "documentType";

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


            BudgetRepository
                .GetAsync()
                .ReturnsNull();

            //Act

            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.UpdateExpenseAsync(
                    expenseHeaderId,
                    listExpenseDetail,
                    date,
                    monetaryFundId,
                    userId,
                    observation,
                    merchant,
                    documentType
                );
            });
            //Assert
            Assert.AreEqual(MessagesExceptions.NotExistBudget, ex.Message);

            await MonetaryFundRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await UserRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await BudgetRepository
                .ReceivedWithAnyArgs(1)
                .GetAsync(
                    Arg.Any<Expression<Func<Budget, bool>>>()
                );           
        }
        
        [TestMethod]
        public async Task UpdateExpenseAsync_FailedAmountExpenseGreaterThanBueget()
        {
            //Arrange
            int expenseHeaderId = 1;
            int expenseDetailId = 1;

            ExpenseDetail expenseDetail = ExpenseDetailBuilder
                    .WithId(expenseDetailId)
                    .WithExpenseHeaderId(expenseHeaderId)
                    .Build();

            List<ExpenseDetail> listExpenseDetail = [
                expenseDetail,
                ExpenseDetailBuilder.WithId(0).Build()
            ];

            DateTime date = DateTime.Now;
            int monetaryFundId = 1;
            int userId = 1;
            string observation = "observation";
            string merchant = "merchant";
            string documentType = "documentType";

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

            Budget budget = BudgetBuilder.WithAmount(1).Build();

            BudgetRepository
                .GetAsync()
                .ReturnsForAnyArgs([budget]);

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.UpdateExpenseAsync(
                    expenseHeaderId,
                    listExpenseDetail,
                    date,
                    monetaryFundId,
                    userId,
                    observation,
                    merchant,
                    documentType
                );
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.ExpenseExceedsBudget, ex.Message);

            await MonetaryFundRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await UserRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
            await BudgetRepository
                .ReceivedWithAnyArgs(1)
                .GetAsync(
                    Arg.Any<Expression<Func<Budget, bool>>>()
                );           
        }

        [TestMethod]
        public async Task GetExpenseDetailByIdAsync_Ok()
        {
            //Arrange
            int expenseHeaderId = 1;
            int expenseDetailId = 1;

            ExpenseDetail expenseDetail = ExpenseDetailBuilder
                    .WithId(expenseDetailId)
                    .WithExpenseHeaderId(expenseHeaderId)
                    .Build();

            ExpenseDetailRepository
               .GetByIdAsync(expenseDetailId)
               .ReturnsForAnyArgs(expenseDetail);

            //Act
            var result = await Service.GetExpenseDetailById(expenseDetailId);

            //Assert
            Assert.AreEqual(expenseDetailId, result.Id);
            Assert.AreEqual(expenseHeaderId, result.ExpenseHeaderId);
            Assert.AreEqual(expenseDetail.ExpenseTypeId, result.ExpenseTypeId);

            await ExpenseDetailRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
        }
        
        [TestMethod]
        public async Task GetExpenseDetailByIdAsync_Failed()
        {
            //Arrange
            int expenseDetailId = 1;

            ExpenseDetailRepository
               .GetByIdAsync(expenseDetailId)
               .ReturnsNull();

            //Act            
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.GetExpenseDetailById(expenseDetailId);
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.NotExistExpenseDetail, ex.Message);

            await ExpenseDetailRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
        }

        [TestMethod]
        public async Task GetExpenseHeaderByIdAsync_Ok()
        {
            //Arrange
            int expenseHeaderId = 1;

            ExpenseHeader expenseHeader = ExpenseHeaderBuilder
                    .WithId(expenseHeaderId)
                    .Build();

            ExpenseHeaderRepository
               .GetByIdAsync(expenseHeaderId)
               .ReturnsForAnyArgs(expenseHeader);

            //Act
            var result = await Service.GetExpenseHeaderById(expenseHeaderId);

            //Assert
            Assert.AreEqual(expenseHeaderId, result.Id);
            Assert.AreEqual(expenseHeader.Date, result.Date);
            Assert.AreEqual(expenseHeader.MonetaryFundId, result.MonetaryFundId);
            Assert.AreEqual(expenseHeader.UserId, result.UserId);
            Assert.AreEqual(expenseHeader.Observations, result.Observations);
            Assert.AreEqual(expenseHeader.Merchant, result.Merchant);
            Assert.AreEqual(expenseHeader.DocumentType, result.DocumentType);

            await ExpenseHeaderRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
        }
        
        [TestMethod]
        public async Task GetExpenseHeaderByIdAsync_Failed()
        {
            //Arrange
            int expenseHeaderId = 1;

            ExpenseHeaderRepository
               .GetByIdAsync(expenseHeaderId)
                .ReturnsNull();

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.GetExpenseHeaderById(expenseHeaderId);
            });
            //Assert
            Assert.AreEqual(MessagesExceptions.NotExistExpenseHeader, ex.Message);

            await ExpenseHeaderRepository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
        }
    }
}
