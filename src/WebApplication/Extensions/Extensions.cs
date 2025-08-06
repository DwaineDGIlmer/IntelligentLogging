using Loggers.Models;
using System.Text.Json;

namespace WebApp.Extensions;

/// <summary>
/// Extensions for parsing OpenTelemetry log events from JSON strings and validating JSON format.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Parses a JSON string representing OpenTelemetry log events and converts it into an <see cref="OtelLogEvents"/>
    /// object.
    /// </summary>
    /// <remarks>This method extracts key properties such as <c>timestamp</c>, <c>body</c>, <c>trace_id</c>,
    /// <c>span_id</c>, <c>severity_number</c>, and attributes like <c>source</c> and <c>correlation_id</c> from the
    /// JSON structure. The <c>timestamp</c> is converted from Unix time in nanoseconds to a <see
    /// cref="DateTimeOffset"/>.</remarks>
    /// <param name="json">A JSON string containing OpenTelemetry log event data. Must not be <see langword="null"/> or empty.</param>
    /// <returns>An <see cref="OtelLogEvents"/> object populated with the data extracted from the provided JSON string.</returns>
    public static OtelLogEvents FromOtelJson(this string json)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var otel = new OtelLogEvents();

        if (root.TryGetProperty("timestamp", out var ts))
            otel.Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(ts.GetInt64() / 1_000_000);

        if (root.TryGetProperty("body", out var body))
            otel.Body = body.GetString() ?? "";

        if (root.TryGetProperty("trace_id", out var traceId) && traceId.ValueKind != JsonValueKind.Null)
            otel.TraceId = traceId.GetString() ?? "";

        if (root.TryGetProperty("span_id", out var spanId) && spanId.ValueKind != JsonValueKind.Null)
            otel.SpanId = spanId.GetString() ?? "";

        if (root.TryGetProperty("severity_number", out var sevNum))
            otel.Level = (LogLevel)sevNum.GetInt32();

        if (root.TryGetProperty("attributes", out var attrs))
        {
            if (attrs.TryGetProperty("source", out var source))
                otel.Source = source.GetString() ?? "";
            if (attrs.TryGetProperty("correlation_id", out var corr))
                otel.CorrelationId = corr.GetString();
        }

        return otel;
    }

    /// <summary>
    /// Determines whether the specified string is a valid JSON object or array.
    /// </summary>
    /// <remarks>The method checks if the input string is non-empty, properly formatted as JSON (starting and
    /// ending with curly braces for objects or square brackets for arrays), and can be successfully parsed as
    /// JSON.</remarks>
    /// <param name="input">The string to evaluate. This can be any text input.</param>
    /// <returns><see langword="true"/> if the input is a valid JSON object or array; otherwise, <see langword="false"/>.</returns>
    public static bool IsJson(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        input = input.Trim();
        if (!(input.StartsWith('{') && input.EndsWith('}')) &&
            !(input.StartsWith('[') && input.EndsWith(']')))
            return false;

        try
        {
            using (JsonDocument.Parse(input)) { }
            return true;
        }
        catch
        {
            return false;
        }
    }
}
