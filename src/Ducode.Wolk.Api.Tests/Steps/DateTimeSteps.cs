using System;

namespace Ducode.Wolk.Api.Tests
{
    public abstract partial class IntegrationTestBase
    {
        protected void SetLocalDateTime(DateTime now) =>
            MockDateTime
                .Setup(m => m.Now)
                .Returns(now);

        protected void SetUtcDateTime(DateTime now) =>
            MockDateTime
                .Setup(m => m.UtcNow)
                .Returns(now);
    }
}
