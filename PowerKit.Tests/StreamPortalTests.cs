using System.IO;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests;

public class StreamPortalTests
{
    [Fact]
    public void CreatePortal_AtCurrentPosition_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);
        stream.Seek(2, SeekOrigin.Begin);

        // Act
        var portal = stream.CreatePortal();

        // Assert
        portal.Position.Should().Be(2);
    }

    [Fact]
    public void CreatePortal_AtSpecifiedPosition_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);

        // Act
        var portal = stream.CreatePortal(3);

        // Assert
        portal.Position.Should().Be(3);
    }

    [Fact]
    public void Jump_SeeksToPortalPosition_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);
        var portal = stream.CreatePortal(3);
        stream.Seek(0, SeekOrigin.Begin);

        // Act
        using (portal.Jump())
        {
            // Assert
            stream.Position.Should().Be(3);
        }
    }

    [Fact]
    public void Jump_RestoresPositionOnDispose_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);
        stream.Seek(1, SeekOrigin.Begin);
        var portal = stream.CreatePortal(4);

        // Act
        var jump = portal.Jump();
        stream.Position.Should().Be(4);
        jump.Dispose();

        // Assert
        stream.Position.Should().Be(1);
    }
}
