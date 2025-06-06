using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Exceptions;
using MONEY_GUARDIAN.Domain.Ports;
using MONEY_GUARDIAN.Domain.Services.expenseType;
using MONEY_GUARDIAN.Domain.Tests.DataBuilder;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace MONEY_GUARDIAN.Domain.Tests.expenseType
{
    [TestClass]
    public class ExpenseTypeServiceTest
    {
        private ExpenseTypeService Service { get; set; } = default!;
        private IGenericRepository<ExpenseType> Repository { get; set; } = default!;
        private ExpenseTypeBuilder ExpenseTypeBuilder { get; set; } = default!;

        [TestInitialize]
        public void Initialize()
        {
            Repository = Substitute.For<IGenericRepository<ExpenseType>>();

            Service = new(Repository);

            ExpenseTypeBuilder = new();
        }

        [TestMethod]
        public async Task CreateExpenseTypeAsync_Ok()
        {
            //Arrange
            int id = 1;
            string code = "codeName";
            string name = "name";

            Repository
              .GetAsync()
              .ReturnsForAnyArgs([]);

            ExpenseType expenseType = ExpenseTypeBuilder
                .WithId(id)
                .WithCode(code)
                .WithName(name)
                .Build();

            Repository
               .AddAsync(expenseType)
               .ReturnsForAnyArgs(expenseType);

            Repository
               .UpdateAsync(expenseType)
               .ReturnsForAnyArgs(expenseType);

            //Act
            var result = await Service.CreateExpenseTypeAsync(
                name,
                ""
            );

            //Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual($"{id}_{name}_{id}", result.Code);
            Assert.AreEqual(name, result.Name);

            await Repository
               .ReceivedWithAnyArgs(1)
               .GetAsync(Arg.Any<Expression<Func<ExpenseType, bool>>>());
            await Repository
               .ReceivedWithAnyArgs(1)
               .AddAsync(Arg.Any<ExpenseType>());
            await Repository
               .ReceivedWithAnyArgs(1)
               .UpdateAsync(Arg.Any<ExpenseType>());
        }
        
        [TestMethod]
        public async Task CreateExpenseTypeAsync_Failed()
        {
            //Arrange
            int id = 1;
            string code = "codeName";
            string name = "name";

            ExpenseType expenseType = ExpenseTypeBuilder
                .WithId(id)
                .WithCode(code)
                .WithName(name)
                .Build();

            Repository
              .GetAsync()
              .ReturnsForAnyArgs([expenseType]);            

            //Act
            
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.CreateExpenseTypeAsync(
                    name,
                    ""
                );
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.ExistExpenseType, ex.Message);

            await Repository
               .ReceivedWithAnyArgs(1)
               .GetAsync(Arg.Any<Expression<Func<ExpenseType, bool>>>());            
        }

        [TestMethod]
        public async Task UpdateExpenseTypeAsync_Ok()
        {
            //Arrange
            int id = 1;
            string code = "codeName";
            string name = "name";

            Repository
              .GetAsync()
              .ReturnsForAnyArgs([]);

            ExpenseType expenseType = ExpenseTypeBuilder
               .WithId(id)
               .WithCode(code)
               .WithName(name)
               .Build();

            Repository
               .GetByIdAsync(id)
               .ReturnsForAnyArgs(expenseType);

            Repository
                .UpdateAsync(expenseType)
                .ReturnsForAnyArgs(expenseType);

            //Act
            var result = await Service.UpdateExpenseTypeAsync(id, name);

            //Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual($"{id}_{name}_{id}", result.Code);
            Assert.AreEqual(name, result.Name);

            await Repository
               .ReceivedWithAnyArgs(1)
               .GetAsync(Arg.Any<Expression<Func<ExpenseType, bool>>>());
            await Repository
               .ReceivedWithAnyArgs(1)
               .GetByIdAsync(Arg.Any<int>());
            await Repository
               .ReceivedWithAnyArgs(1)
               .UpdateAsync(Arg.Any<ExpenseType>());
        }
        
        [TestMethod]
        public async Task UpdateExpenseTypeAsync_Failed()
        {
            //Arrange
            int id = 1;
            string code = "codeName";
            string name = "name";

            ExpenseType expenseType = ExpenseTypeBuilder
               .WithId(id)
               .WithCode(code)
               .WithName(name)
               .Build();

            Repository
              .GetAsync()
              .ReturnsForAnyArgs([expenseType]);

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.UpdateExpenseTypeAsync(id, name);
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.ExistExpenseType, ex.Message);

            await Repository
               .ReceivedWithAnyArgs(1)
               .GetAsync(Arg.Any<Expression<Func<ExpenseType, bool>>>());            
        }

        [TestMethod]
        public async Task GetExpenseTypeByIdAsync_Ok()
        {
            //Arrange
            int id = 1;
            string code = "codeName";
            string name = "name";

            ExpenseType expenseType = ExpenseTypeBuilder
                .WithId(id)
                .WithCode(code)
                .WithName(name)
                .Build();

            Repository
               .GetByIdAsync(id)
               .ReturnsForAnyArgs(expenseType);

            //Act
            var result = await Service.GetExpenseTypeById(id);

            //Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(code, result.Code);
            Assert.AreEqual(name, result.Name);

            await Repository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
        }
        
        [TestMethod]
        public async Task GetExpenseTypeByIdAsync_Failed()
        {
            //Arrange
            int id = 1;

            Repository
               .GetByIdAsync(id)
               .ReturnsNull();

            //Act            
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.GetExpenseTypeById(id);
            });

            //Assert
            Assert.AreEqual(MessagesExceptions.NotExistExpenseType, ex.Message);

            await Repository
                .ReceivedWithAnyArgs(1)
                .GetByIdAsync(Arg.Any<int>());
        }
    }
}
