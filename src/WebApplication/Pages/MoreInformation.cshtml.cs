using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    /// <summary>
    /// Represents the model for the "More Information" page in an ASP.NET Core Razor Pages application.
    /// </summary>
    /// <remarks>This class is used to handle the data and logic for the "More Information" page.  It inherits
    /// from <see cref="PageModel"/>, which provides the base functionality for Razor Pages models.</remarks>
    sealed public class MoreInformationModel : PageModel
    {
        /// <summary>
        /// Handles GET requests for the page.
        /// </summary>
        /// <remarks>This method is invoked when the page is accessed via an HTTP GET request.  Override
        /// this method to include any logic needed to initialize the page's state or data.</remarks>
        public void OnGet()
        {
        }
    }
}
