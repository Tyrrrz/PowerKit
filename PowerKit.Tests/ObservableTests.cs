using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using PowerKit;
using Xunit;

namespace PowerKit.Tests;

file class DelegateObserver<T>(
    Action<T>? onNext = null,
    Action<Exception>? onError = null,
    Action? onCompleted = null
) : IObserver<T>
{
    public void OnNext(T value) => onNext?.Invoke(value);

    public void OnError(Exception error) => onError?.Invoke(error);

    public void OnCompleted() => onCompleted?.Invoke();
}

public class ObservableTests
{
    [Fact]
    public void Observable_Create_Subscribe_Test()
    {
        // Arrange
        var subscribed = false;
        var observable = Observable.Create<int>(_ =>
        {
            subscribed = true;
            return Disposable.Null;
        });

        // Act
        subscribed.Should().BeFalse();
        observable.Subscribe(new DelegateObserver<int>());

        // Assert
        subscribed.Should().BeTrue();
    }

    [Fact]
    public void Observable_Create_OnNext_Test()
    {
        // Arrange
        var received = new List<int>();
        var observable = Observable.Create<int>(observer =>
        {
            observer.OnNext(1);
            observer.OnNext(2);
            observer.OnNext(3);
            observer.OnCompleted();
            return Disposable.Null;
        });

        // Act
        observable.Subscribe(new DelegateObserver<int>(onNext: received.Add));

        // Assert
        received.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void Observable_Create_OnError_Test()
    {
        // Arrange
        Exception? receivedError = null;
        var observable = Observable.Create<int>(observer =>
        {
            observer.OnError(new InvalidOperationException("test error"));
            return Disposable.Null;
        });

        // Act
        observable.Subscribe(new DelegateObserver<int>(onError: ex => receivedError = ex));

        // Assert
        receivedError.Should().BeOfType<InvalidOperationException>();
        receivedError!.Message.Should().Be("test error");
    }

    [Fact]
    public void Observable_Create_OnCompleted_Test()
    {
        // Arrange
        var completed = false;
        var observable = Observable.Create<int>(observer =>
        {
            observer.OnCompleted();
            return Disposable.Null;
        });

        // Act
        observable.Subscribe(new DelegateObserver<int>(onCompleted: () => completed = true));

        // Assert
        completed.Should().BeTrue();
    }

    [Fact]
    public void Observable_Create_Dispose_Test()
    {
        // Arrange
        var disposed = false;
        var observable = Observable.Create<int>(_ => Disposable.Create(() => disposed = true));

        // Act
        disposed.Should().BeFalse();
        var subscription = observable.Subscribe(new DelegateObserver<int>());
        subscription.Dispose();

        // Assert
        disposed.Should().BeTrue();
    }

    [Fact]
    public void Observable_CreateSynchronized_OnNext_Test()
    {
        // Arrange
        var received = new List<int>();
        var observable = Observable.CreateSynchronized<int>(observer =>
        {
            observer.OnNext(1);
            observer.OnNext(2);
            observer.OnNext(3);
            observer.OnCompleted();
            return Disposable.Null;
        });

        // Act
        observable.Subscribe(new DelegateObserver<int>(onNext: received.Add));

        // Assert
        received.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void Observable_CreateSynchronized_ThreadSafe_Test()
    {
        // Arrange
        const int threadCount = 10;
        const int valuesPerThread = 100;
        var received = new List<int>();
        var observable = Observable.CreateSynchronized<int>(observer =>
        {
            var threads = new List<Thread>();

            for (var i = 0; i < threadCount; i++)
            {
                var thread = new Thread(() =>
                {
                    for (var j = 0; j < valuesPerThread; j++)
                    {
                        observer.OnNext(j);
                    }
                });
                threads.Add(thread);
            }

            foreach (var t in threads)
                t.Start();
            foreach (var t in threads)
                t.Join();

            observer.OnCompleted();
            return Disposable.Null;
        });

        // Act
        observable.Subscribe(new DelegateObserver<int>(onNext: v => received.Add(v)));

        // Assert
        received.Should().HaveCount(threadCount * valuesPerThread);
    }

    [Fact]
    public void SynchronizedObserver_OnNext_Test()
    {
        // Arrange
        var received = new List<int>();
        var synchronized = new SynchronizedObserver<int>(
            new DelegateObserver<int>(onNext: received.Add)
        );

        // Act
        synchronized.OnNext(1);
        synchronized.OnNext(2);
        synchronized.OnNext(3);

        // Assert
        received.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void SynchronizedObserver_OnError_Test()
    {
        // Arrange
        Exception? receivedError = null;
        var synchronized = new SynchronizedObserver<int>(
            new DelegateObserver<int>(onError: ex => receivedError = ex)
        );
        var error = new InvalidOperationException("test");

        // Act
        synchronized.OnError(error);

        // Assert
        receivedError.Should().BeSameAs(error);
    }

    [Fact]
    public void SynchronizedObserver_OnCompleted_Test()
    {
        // Arrange
        var completed = false;
        var synchronized = new SynchronizedObserver<int>(
            new DelegateObserver<int>(onCompleted: () => completed = true)
        );

        // Act
        synchronized.OnCompleted();

        // Assert
        completed.Should().BeTrue();
    }
}
