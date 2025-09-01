using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    /// <summary>
    /// Represents the model for the Index page in an ASP.NET Core Razor Pages application.
    /// </summary>
    /// <remarks>This class is used to handle requests and responses for the Index page. It provides methods
    /// and properties to manage the page's behavior and state during a request.</remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </remarks>
    /// <param name="logger">The logger instance used to log diagnostic and operational messages.</param>
    public sealed class IndexModel(ILogger<IndexModel> logger) : PageModel
    {
        /// <summary>
        /// The logger instance used for logging information, warnings, and errors.
        /// </summary>
        private readonly ILogger<IndexModel> _logger = logger;

        /// <summary>
        /// Handles GET requests for the Index page.
        /// </summary>
        /// <remarks>This method is invoked when the Index page is accessed via an HTTP GET request. It
        /// logs an informational message indicating that the page was accessed.</remarks>
        public void OnGet()
        {
            // Log an informational message indicating that the Index page was accessed
            _logger.LogInformation("Index page accessed.");
        }
    }
}
