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
    public void WatchProperty_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged { StringValue = "initial" };
        var received = new List<string?>();

        // Act
        var sub = obj.WatchProperty(
            x => x.StringValue,
            v => received.Add(v),
            watchInitialValue: true
        );
        obj.StringValue = "hello"; // matching change
        obj.IntValue = 42; // unrelated — should not fire
        obj.RaiseAllPropertiesChanged(); // blank name — should fire
        sub.Dispose();
        obj.StringValue = "world"; // after dispose — should not fire

        // Assert: initial value + "hello" + "hello" (re-read on blank event)
        received.Should().Equal("initial", "hello", "hello");
    }

    [Fact]
    public void WatchProperty_InvalidExpression_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged();

        // Act & assert
        var act = () => obj.WatchProperty(x => x.StringValue!.ToUpper(), _ => { });
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void WatchProperties_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged();
        var callCount = 0;

        // Act
        var sub = obj.WatchProperties(
            [x => x.StringValue, x => (object?)x.IntValue],
            () => callCount++,
            watchInitialValue: true
        );
        obj.StringValue = "hello"; // matching
        obj.IntValue = 42; // matching
        obj.RaiseAllPropertiesChanged(); // blank name — should fire
        sub.Dispose();
        obj.StringValue = "world"; // after dispose — should not fire

        // Assert: initial + StringValue + IntValue + blank
        callCount.Should().Be(4);
    }

    [Fact]
    public void WatchAllProperties_Test()
    {
        // Arrange
        var obj = new FakeNotifyPropertyChanged();
        var callCount = 0;

        // Act
        var sub = obj.WatchAllProperties(() => callCount++, watchInitialValue: true);
        obj.StringValue = "hello";
        obj.IntValue = 42;
        sub.Dispose();
        obj.StringValue = "world"; // after dispose — should not fire

        // Assert: initial + StringValue + IntValue
        callCount.Should().Be(3);
    }
}
