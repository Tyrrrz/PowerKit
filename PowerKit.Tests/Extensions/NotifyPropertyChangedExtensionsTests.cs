using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FluentAssertions;
using PowerKit.Extensions;
using Xunit;

namespace PowerKit.Tests.Extensions;

file class FakeNotifyPropertyChanged : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string? StringProperty
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public int IntProperty
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public void RaiseAllPropertiesChanged() =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

public class NotifyPropertyChangedExtensionsTests
{
    [Fact]
    public void WatchProperty_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged { StringProperty = "initial" };
        var values = new List<string?>();

        // Act
        var sub = obj.WatchProperty(x => x.StringProperty, v => values.Add(v), true);

        obj.StringProperty = "hello";
        obj.IntProperty = 42;
        obj.RaiseAllPropertiesChanged();
        sub.Dispose();
        obj.StringProperty = "world";

        // Assert
        values.Should().Equal("initial", "hello", "hello");
    }

    [Fact]
    public void WatchProperties_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged();
        var callCount = 0;

        // Act
        var sub = obj.WatchProperties(
            [x => x.StringProperty, x => x.IntProperty],
            () => callCount++,
            true
        );

        obj.StringProperty = "hello";
        obj.IntProperty = 42;
        obj.RaiseAllPropertiesChanged();
        sub.Dispose();
        obj.StringProperty = "world";

        // Assert
        callCount.Should().Be(4);
    }

    [Fact]
    public void WatchAllProperties_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged();
        var callCount = 0;

        // Act
        var sub = obj.WatchAllProperties(() => callCount++, true);
        obj.StringProperty = "hello";
        obj.IntProperty = 42;
        sub.Dispose();
        obj.StringProperty = "world";

        // Assert
        callCount.Should().Be(3);
    }
}
