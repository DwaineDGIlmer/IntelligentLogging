namespace WebApp.Models;

/// <summary>
/// Represents a message with an associated timestamp.
/// </summary>
/// <remarks>The <see cref="DemoMessage"/> class extends the base <see cref="OpenAiMessage"/> class to include a
/// timestamp indicating when the message was created or processed. This can be useful for tracking or logging
/// purposes.</remarks>
public class DemoMessage
{
    /// <summary>
    /// The content of the message.
    /// </summary>
    public string Message { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp indicating when the event occurred.
    /// </summary>
    public string Timestamp { get; } = DateTime.UtcNow.ToString("o").Replace("T", Environment.NewLine);

    /// <summary>
    /// Used for testing.
    /// </summary>
    public DemoMessage() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DemoMessage"/> class with the specified message content.
    /// </summary>
    /// <param name="message">The message content to associate with this instance. Cannot be <see langword="null"/>.</param>
    public DemoMessage(string message)
    {
        Message = message;
    }
}
