using Aurochses.Xunit.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Aurochses.AspNetCore.Mvc.Tests
{
    public class ControllerHelpersTests
    {
        private readonly Mock<Controller> _mockController;

        public ControllerHelpersTests()
        {
            _mockController = new Mock<Controller>(MockBehavior.Strict);
        }

        [Theory]
        [InlineData(null, false, null, null)]
        [InlineData("/SignIn", true, null, null)]
        [InlineData("http://www.example.com", false, "TestAction", "TestController")]
        public void RedirectToLocal_Success(string returnUrl, bool isLocalUrl, string actionName, string controllerName)
        {
            // Arrange
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(x => x.IsLocalUrl(returnUrl))
                .Returns(isLocalUrl)
                .Verifiable();

            _mockController.Object.Url = mockUrlHelper.Object;

            if (_mockController.Object.Url.IsLocalUrl(returnUrl))
            {
                _mockController
                    .Setup(x => x.Redirect(returnUrl))
                    .Returns(() => new RedirectResult(returnUrl))
                    .Verifiable();
            }
            else
            {
                _mockController
                    .Setup(x => x.RedirectToAction(actionName, controllerName))
                    .Returns(() => new RedirectToActionResult(actionName, controllerName, null))
                    .Verifiable();
            }


            // Act
            var actionResult = _mockController.Object.RedirectToLocal(returnUrl, actionName, controllerName);

            // Assert
            mockUrlHelper.Verify();
            _mockController.Verify();

            if (_mockController.Object.Url.IsLocalUrl(returnUrl))
            {
                MvcAssert.RedirectResult(actionResult, returnUrl);
            }
            else
            {
                MvcAssert.RedirectToActionResult(actionResult, actionName, controllerName);
            }
        }
    }
}