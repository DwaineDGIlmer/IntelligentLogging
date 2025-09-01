using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace WebApp.Pages
{
    /// <summary>
    /// Represents the model for handling error pages in the application.
    /// </summary>
    /// <remarks>This model is used to display error information, including the request ID,  which can help in
    /// diagnosing issues. It is designed to work with Razor Pages  and includes functionality to log error
    /// details.</remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ErrorModel"/> class.
    /// </remarks>
    /// <param name="logger">The logger instance used to log error-related information.  This parameter cannot be <see langword="null"/>.</param>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    sealed public class ErrorModel(ILogger<ErrorModel> logger) : PageModel
    {
        /// <summary>       
        /// The request ID associated with the error.
        ///</summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether the request ID should be displayed.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        /// <summary>
        /// Represents a logger instance used to log messages related to the <see cref="ErrorModel"/> class.
        /// </summary>
        /// <remarks>This logger is intended for internal use within the <see cref="ErrorModel"/> class to
        /// record error-related information.</remarks>
        private readonly ILogger<ErrorModel> _logger = logger;

        /// <summary>
        /// Handles GET requests and initializes the request identifier.
        /// </summary>
        /// <remarks>This method sets the <see cref="RequestId"/> property to the current activity's ID if
        /// available,  or to the HTTP context's trace identifier as a fallback.</remarks>
        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
