using Core.Models;

namespace WebApp.Models
{
    /// <summary>
    /// Represents a message with an associated timestamp.
    /// </summary>
    /// <remarks>The <see cref="DemoMessage"/> class extends the base <see cref="Message"/> class to include a
    /// timestamp indicating when the message was created or processed. This can be useful for tracking or logging
    /// purposes.</remarks>
    public class DemoMessage : Message
    {
        /// <summary>
        /// Used for testing.
        /// </summary>
        public DemoMessage() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DemoMessage"/> class using the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="message">The <see cref="Message"/> instance whose role and content will be used to initialize the new <see
        /// cref="DemoMessage"/>.</param>
        public DemoMessage(Message message)
        {
            Role = message.Role;
            Content = message.Content;
        }

        /// <summary>
        /// Gets or sets the timestamp indicating when the event occurred.
        /// </summary>
        public string Timestamp { get; } = DateTime.UtcNow.ToString("o").Replace("T", Environment.NewLine);
    }
}
