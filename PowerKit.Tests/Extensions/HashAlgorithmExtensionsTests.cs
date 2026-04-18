using System;
using System.Security.Cryptography;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

public class HashAlgorithmExtensionsTests
{
    [Fact]
    public void ComputeHash_Test()
    {
        // Arrange
        var data = "hello"u8.ToArray();

        // Act
        var hash = HashAlgorithm.ComputeHash(SHA256.Create(), data);

        // Assert
        hash.Should()
            .Equal(
                Convert.FromHexString(
                    "2cf24dba5fb0a30e26e83b2ac5b9e29e1b161e5c1fa7425e73043362938b9824"
                )
            );
    }
}
