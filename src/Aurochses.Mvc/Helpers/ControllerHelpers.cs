using Microsoft.AspNetCore.Mvc;

namespace Aurochses.Mvc.Helpers
{
    /// <summary>
    /// Class ControllerHelpers.
    /// </summary>
    public static class ControllerHelpers
    {
        /// <summary>
        /// Redirects to local.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns>IActionResult.</returns>
        public static IActionResult RedirectToLocal(this Controller controller, string returnUrl, string actionName = "Index", string controllerName = "Home")
        {
            if (controller.Url.IsLocalUrl(returnUrl))
            {
                return controller.Redirect(returnUrl);
            }

            return controller.RedirectToAction(actionName, controllerName);
        }
    }
}