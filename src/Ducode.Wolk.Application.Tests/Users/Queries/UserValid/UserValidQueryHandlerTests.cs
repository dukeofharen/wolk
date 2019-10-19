using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Users.Queries.UserValid;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Application.Tests.Users.Queries.UserValid
{
    [TestClass]
    public class UserValidQueryHandlerTests
    {
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private UserValidQueryHandler _handler;

        [TestInitialize]
        public void Setup() => _handler = new UserValidQueryHandler(_wolkDbContext);

        [TestCleanup]
        public void Cleanup() => _wolkDbContext.Destroy();

        [TestMethod]
        public async Task Handle_UserNotFound_ShouldReturnFalse()
        {
            // Arrange
            var user = await _wolkDbContext.CreateAndSaveUser();
            var request = new UserValidQuery {SecurityStamp = user.SecurityStamp, UserId = user.Id + 1};

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Handle_UserFound_SecurityStampInvalid_ShouldReturnFalse()
        {
            // Arrange
            var user = await _wolkDbContext.CreateAndSaveUser();
            var request = new UserValidQuery {SecurityStamp = user.SecurityStamp + "X", UserId = user.Id};

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Handle_UserFound_SecurityStampValid_ShouldReturnTrue()
        {
            // Arrange
            var user = await _wolkDbContext.CreateAndSaveUser();
            var request = new UserValidQuery {SecurityStamp = user.SecurityStamp, UserId = user.Id};

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
