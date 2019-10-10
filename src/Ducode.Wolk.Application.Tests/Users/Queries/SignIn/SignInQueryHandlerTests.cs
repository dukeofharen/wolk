using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces.Identity;
using Ducode.Wolk.Application.Users.Queries.SignIn;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ducode.Wolk.Application.Tests.Users.Queries.SignIn
{
    [TestClass]
    public class SignInQueryHandlerTests
    {
        private readonly Mock<IJwtManager> _mockJwtManager = new Mock<IJwtManager>();
        private readonly Mock<ISignInManager> _mockSignInManager = new Mock<ISignInManager>();
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private SignInQueryHandler _handler;

        [TestInitialize]
        public void Setup() => _handler = new SignInQueryHandler(
            _mockJwtManager.Object,
            _mockSignInManager.Object,
            _wolkDbContext);

        [TestCleanup]
        public void Initialize()
        {
            _mockJwtManager.VerifyAll();
            _mockSignInManager.VerifyAll();
            InMemoryDbContextFactory.Destroy(_wolkDbContext);
        }

        [TestMethod]
        public async Task Handle_CredentialsIncorrect_ShouldThrowUnauthorizedException()
        {
            // Arrange
            var request = new SignInQuery {Email = "test@gmail.com", Password = "password"};

            _mockSignInManager
                .Setup(m => m.CheckCredentialsAsync(request.Email, request.Password))
                .ReturnsAsync(false);

            // Act / Assert
            await Assert.ThrowsExceptionAsync<UnauthorizedException>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_CredentialsCorrect_ShouldReturnLoggedInData()
        {
            // Arrange
            var user = await _wolkDbContext.CreateAndSaveUser();
            var request = new SignInQuery {Email = user.Email, Password = "password"};

            _mockSignInManager
                .Setup(m => m.CheckCredentialsAsync(request.Email, request.Password))
                .ReturnsAsync(true);

            var jwt = "dsfad.jwt.rwsdf";
            _mockJwtManager
                .Setup(m => m.CreateJwt(user))
                .Returns(jwt);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.AreEqual(user.Email, result.Email);
            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual(jwt, result.Token);
        }
    }
}
