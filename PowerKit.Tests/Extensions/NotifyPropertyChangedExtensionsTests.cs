using System;
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

    public string? StringValue
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public int IntValue
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
    public void WatchProperty_FiresOnChange_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged { StringValue = "initial" };
        var received = new List<string?>();

        // Act
        using var _ = obj.WatchProperty(x => x.StringValue, v => received.Add(v));
        obj.StringValue = "hello";
        obj.StringValue = "world";

        // Assert
        received.Should().Equal("hello", "world");
    }

    [Fact]
    public void WatchProperty_WatchInitialValue_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged { StringValue = "initial" };
        var received = new List<string?>();

        // Act
        using var _ = obj.WatchProperty(
            x => x.StringValue,
            v => received.Add(v),
            watchInitialValue: true
        );
        obj.StringValue = "hello";

        // Assert
        received.Should().Equal("initial", "hello");
    }

    [Fact]
    public void WatchProperty_DoesNotFireForOtherProperties_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged();
        var received = new List<string?>();

        // Act
        using var _ = obj.WatchProperty(x => x.StringValue, v => received.Add(v));
        obj.IntValue = 42;

        // Assert
        received.Should().BeEmpty();
    }

    [Fact]
    public void WatchProperty_FiresOnAllPropertiesChanged_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged { StringValue = "initial" };
        var received = new List<string?>();

        // Act
        using var _ = obj.WatchProperty(x => x.StringValue, v => received.Add(v));
        obj.RaiseAllPropertiesChanged();

        // Assert
        received.Should().Equal("initial");
    }

    [Fact]
    public void WatchProperty_Unsubscribes_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged();
        var received = new List<string?>();

        // Act
        var subscription = obj.WatchProperty(x => x.StringValue, v => received.Add(v));
        obj.StringValue = "hello";
        subscription.Dispose();
        obj.StringValue = "world";

        // Assert
        received.Should().Equal("hello");
    }

    [Fact]
    public void WatchProperty_NonPropertyExpression_Throws_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged();

        // Act & assert
        var act = () => obj.WatchProperty(x => x.StringValue!.ToUpper(), _ => { });
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void WatchProperties_FiresOnMatchingChange_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged();
        var callCount = 0;

        // Act
        using var _ = obj.WatchProperties(
            [x => x.StringValue, x => (object?)x.IntValue],
            () => callCount++
        );
        obj.StringValue = "hello";
        obj.IntValue = 42;

        // Assert
        callCount.Should().Be(2);
    }

    [Fact]
    public void WatchProperties_WatchInitialValue_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged();
        var callCount = 0;

        // Act
        using var _ = obj.WatchProperties(
            [x => x.StringValue, x => (object?)x.IntValue],
            () => callCount++,
            watchInitialValue: true
        );

        // Assert
        callCount.Should().Be(1);
    }

    [Fact]
    public void WatchProperties_FiresOnAllPropertiesChanged_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged();
        var callCount = 0;

        // Act
        using var _ = obj.WatchProperties([x => x.StringValue], () => callCount++);
        obj.RaiseAllPropertiesChanged();

        // Assert
        callCount.Should().Be(1);
    }

    [Fact]
    public void WatchProperties_Unsubscribes_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged();
        var callCount = 0;

        // Act
        var subscription = obj.WatchProperties([x => x.StringValue], () => callCount++);
        obj.StringValue = "hello";
        subscription.Dispose();
        obj.StringValue = "world";

        // Assert
        callCount.Should().Be(1);
    }

    [Fact]
    public void WatchAllProperties_FiresOnAnyChange_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged();
        var callCount = 0;

        // Act
        using var _ = obj.WatchAllProperties(() => callCount++);
        obj.StringValue = "hello";
        obj.IntValue = 42;

        // Assert
        callCount.Should().Be(2);
    }

    [Fact]
    public void WatchAllProperties_WatchInitialValue_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged();
        var callCount = 0;

        // Act
        using var _ = obj.WatchAllProperties(() => callCount++, watchInitialValue: true);

        // Assert
        callCount.Should().Be(1);
    }

    [Fact]
    public void WatchAllProperties_Unsubscribes_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged();
        var callCount = 0;

        // Act
        var subscription = obj.WatchAllProperties(() => callCount++);
        obj.StringValue = "hello";
        subscription.Dispose();
        obj.StringValue = "world";

        // Assert
        callCount.Should().Be(1);
    }
}
