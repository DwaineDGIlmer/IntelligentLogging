using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using WebApp.Models;

namespace WebApp.Controllers
{   
    /// <summary>
    /// Provides endpoints for retrieving ChatGPT messages.
    /// </summary>
    /// <remarks>This controller exposes an API endpoint to retrieve messages from the ChatGPT message queue.
    /// The messages are returned as a list of objects containing the role and content of each message.</remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class ChatGptMessagesController : ControllerBase
    {
        /// <summary>
        /// Gets the queue that holds responses from the ChatGPT API.
        /// </summary>
        /// <remarks>This property provides a thread-safe mechanism for accessing ChatGPT responses in
        /// scenarios where multiple threads may be producing or consuming responses.</remarks>
        public ConcurrentQueue<DemoMessage> ChatGptMessageQueue { get; set; } = EventReaderSingleton.Instance.ChatGptMessageQueue;

        /// <summary>
        /// Retrieves a list of chat messages from the message queue.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing a collection of chat messages,  where each message includes the
        /// role and content. Returns an empty collection if no messages are available.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var messages = ChatGptMessageQueue
                .Select(m => new { timestamp = m.Timestamp, role = m.Role, content = m.Content })
                .ToList();
            return Ok(messages);
        }
    }
}
