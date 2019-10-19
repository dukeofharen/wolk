using System;
using System.Threading.Tasks;
using Ducode.Wolk.Common;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Persistence.SaveChanges.Impl;
using Ducode.Wolk.TestUtilities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ducode.Wolk.Persistence.Tests.SaveChanges
{
    [TestClass]
    public class ChangeDateSaveChangesHandlerTests
    {
        private static DateTime _now = new DateTime(2019, 7, 26, 20, 50, 55);
        private readonly Mock<IDateTime> _dateTimeMock = new Mock<IDateTime>();
        private WolkDbContext _context;
        private ChangeDateSaveChangesHandler _handler;

        [TestInitialize]
        public void Initialize()
        {
            _dateTimeMock
                .Setup(m => m.Now)
                .Returns(_now);

            _handler = new ChangeDateSaveChangesHandler(_dateTimeMock.Object);
            _context = InMemoryDbContextFactory.Create(new[] {_handler});
        }

        [TestMethod]
        public async Task ChangeDateSaveChangesHandler_HandleSaveChangesAsync_StateIsCreated_ShouldNotSetUpdated()
        {
            // Arrange
            var notebook = new Notebook {Name = "Notebook 1"};
            _context.Notebooks.Add(notebook);

            // Act
            await _context.SaveChangesAsync();

            // Assert
            Assert.IsNull(notebook.Changed);
        }

        [TestMethod]
        public async Task ChangeDateSaveChangesHandler_HandleSaveChangesAsync_StateIsUpdated_ShouldSetUpdated()
        {
            // Arrange
            var notebook = new Notebook {Name = "Notebook 1"};
            _context.Notebooks.Add(notebook);

            // Act
            await _context.SaveChangesAsync();

            // Arrange
            notebook = await _context.Notebooks.SingleAsync(n => n.Id == notebook.Id);
            notebook.Name = "Changed title";

            // Act
            await _context.SaveChangesAsync();

            // Assert
            Assert.AreEqual(_now, notebook.Changed);
        }

        [TestMethod]
        public async Task ChangeDateSaveChangesHandler_HandleSaveChangesAsync_UpdatedSetManually_ShouldNotOverrideUpdated()
        {
            // Arrange
            var notebook = new Notebook {Name = "Notebook 1"};
            _context.Notebooks.Add(notebook);

            // Act
            await _context.SaveChangesAsync();

            // Arrange
            notebook.Name = "Changed title";

            var updated = new DateTime(2019, 6, 1, 21, 0, 0);
            notebook.Changed = updated;

            // Act
            await _context.SaveChangesAsync();

            // Assert
            Assert.AreEqual(updated, notebook.Changed);
        }
    }
}
