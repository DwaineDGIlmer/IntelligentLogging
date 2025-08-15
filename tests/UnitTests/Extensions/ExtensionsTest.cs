using Microsoft.Extensions.Logging;
using WebApp.Extensions;

namespace UnitTests.Extensions;

public class ExtensionsTest
{
    [Fact]
    public void FromOtelJson_ParsesValidJson_ReturnsOtelLogEvents()
    {
        // Arrange
        var json = @"{
                ""timestamp"": 1710000000000000000,
                ""body"": ""Test log message"",
                ""trace_id"": ""abc123"",
                ""span_id"": ""def456"",
                ""severity_number"": 2,
                ""attributes"": {
                    ""source"": ""UnitTest"",
                    ""correlation_id"": ""corr789""
                }
            }";

        // Act
        var result = json.FromOtelJson();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1710000000000000000 / 1_000_000), result.Timestamp);
        Assert.Equal("Test log message", result.Body);
        Assert.Equal("abc123", result.TraceId);
        Assert.Equal("def456", result.SpanId);
        Assert.Equal((LogLevel)2, result.Level);
        Assert.Equal("UnitTest", result.Source);
        Assert.Equal("corr789", result.CorrelationId);
    }

    [Fact]
    public void FromOtelJson_MissingOptionalFields_HandlesGracefully()
    {
        // Arrange
        var json = @"{
                ""timestamp"": 1710000000000000000,
                ""body"": ""Test"",
                ""severity_number"": 1
            }";

        // Act
        var result = json.FromOtelJson();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1710000000000000000 / 1_000_000), result.Timestamp);
        Assert.Equal("Test", result.Body);
        Assert.Equal("", result.TraceId);
        Assert.Equal("", result.SpanId);
        Assert.Equal((LogLevel)1, result.Level);
        Assert.Empty(result.Source);
        Assert.Null(result.CorrelationId);
    }

    [Theory]
    [InlineData("null")]
    [InlineData("")]
    [InlineData("   ")]
    public void IsJson_NullOrWhitespace_ReturnsFalse(string input)
    {
        if (input == null)
        {
            string? nullValue = null;
            Assert.False(nullValue!.IsJson());
        }
        else
        {
            Assert.False(input.IsJson());
        }
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("[]")]
    [InlineData("{\"a\":1}")]
    [InlineData("[1,2,3]")]
    public void IsJson_ValidJson_ReturnsTrue(string input)
    {
        Assert.True(input.IsJson());
    }

    [Theory]
    [InlineData("{]")]
    [InlineData("[}")]
    [InlineData("{invalid}")]
    [InlineData("notjson")]
    public void IsJson_InvalidJson_ReturnsFalse(string input)
    {
        Assert.False(input.IsJson());
    }
}