using System.Text;
using PowerKit.Extensions;

namespace PowerKit.Tests;

public class StringBuilderExtensionsTests
{
    [Fact]
    public void AppendIfNotEmpty_EmptyBuilder_DoesNotAppend()
    {
        var builder = new StringBuilder();
        builder.AppendIfNotEmpty(',');
        Assert.Equal("", builder.ToString());
    }

    [Fact]
    public void AppendIfNotEmpty_NonEmptyBuilder_Appends()
    {
        var builder = new StringBuilder("hello");
        builder.AppendIfNotEmpty(',');
        Assert.Equal("hello,", builder.ToString());
    }

    [Fact]
    public void AppendIfNotEmpty_ReturnsBuilder_AllowsChaining()
    {
        var builder = new StringBuilder("a");
        builder.AppendIfNotEmpty(',').Append("b");
        Assert.Equal("a,b", builder.ToString());
    }
}
