using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    /// <summary>
    /// Provides endpoints for retrieving event data from the server.
    /// </summary>
    /// <remarks>This controller handles HTTP requests related to event data. It retrieves events from an
    /// internal  event queue and returns them to the caller. The controller is designed to be used in an API context 
    /// and follows RESTful conventions.</remarks>
    [ApiController]
    [Route("api/[controller]")]
    sealed public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance used to log diagnostic and operational messages for the <see cref="EventsController"/>.</param>
        public EventsController(ILogger<EventsController> logger)
        {
            _logger = logger;
            _logger.LogTrace("EventsController initialized.");
        }

        /// <summary>
        /// Retrieves all available events from the event queue.
        /// </summary>
        /// <remarks>This method dequeues all events currently in the event queue managed by the <see
        /// cref="EventReaderSingleton"/> instance. The returned collection reflects the state of the queue at the time
        /// of the method call.</remarks>
        /// <returns>An <see cref="IEnumerable{T}"/> of strings, where each string represents a serialized event in JSON format.
        /// If no events are available, returns an empty collection.</returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogTrace("Get() called.");
            var events = new List<string>();
            while (EventReaderSingleton.Instance.EventQueue.TryDequeue(out var json))
            {
                events.Add(json);
            }
            return events;
        }
    }
}