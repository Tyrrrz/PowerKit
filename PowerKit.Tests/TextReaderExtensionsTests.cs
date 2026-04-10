using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class TextReaderExtensionsTests
{
    [Fact]
    public async Task ReadLinesAsync_Test()
    {
        // Act & assert
        (await new StringReader("line1\nline2\nline3").ReadLinesAsync().ToListAsync())
            .Should()
            .Equal("line1", "line2", "line3");

        (await new StringReader("").ReadLinesAsync().ToListAsync())
            .Should()
            .BeEmpty();

        (await new StringReader("hello").ReadLinesAsync().ToListAsync())
            .Should()
            .Equal("hello");
    }
}
