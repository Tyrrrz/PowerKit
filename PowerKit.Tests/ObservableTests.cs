using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using Xunit;

namespace PowerKit.Tests;

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
        observable.Subscribe(Observer.Create<int>());

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
        observable.Subscribe(Observer.Create<int>(onNext: received.Add));

        // Assert
        received.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void Observable_Create_OnError_Test()
    {
        // Arrange
        var receivedError = default(Exception);
        var observable = Observable.Create<int>(observer =>
        {
            observer.OnError(new InvalidOperationException("test error"));
            return Disposable.Null;
        });

        // Act
        observable.Subscribe(Observer.Create<int>(onError: ex => receivedError = ex));

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
        observable.Subscribe(Observer.Create<int>(onCompleted: () => completed = true));

        // Assert
        completed.Should().BeTrue();
    }

    [Fact]
    public void Observable_Create_Dispose_Test()
    {
        // Arrange
        var disposed = false;
        IObserver<int>? producer = null;
        var received = new List<int>();

        var observable = Observable.Create<int>(observer =>
        {
            producer = observer;
            return Disposable.Create(() => disposed = true);
        });

        var subscription = observable.Subscribe(Observer.Create<int>(received.Add));

        producer!.OnNext(1);
        producer.OnNext(2);
        producer.OnNext(3);
        subscription.Dispose();
        producer.OnNext(4);
        producer.OnNext(5);

        // Assert
        disposed.Should().BeTrue();
        received.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void Observable_Create_Dispose_AfterOnCompleted_NoDoubleDispose_Test()
    {
        // Arrange
        var disposeCount = 0;
        IObserver<int>? producer = null;
        var received = new List<int>();

        var observable = Observable.Create<int>(observer =>
        {
            producer = observer;
            return Disposable.Create(() => disposeCount++);
        });

        var subscription = observable.Subscribe(Observer.Create<int>(received.Add));

        producer!.OnNext(1);
        producer.OnNext(2);
        producer.OnNext(3);
        producer.OnCompleted();
        subscription.Dispose();

        // Assert
        received.Should().Equal(1, 2, 3);
        disposeCount.Should().Be(1);
    }

    [Fact]
    public void Observable_Create_Dispose_NoEventAfterDispose_Test()
    {
        // Arrange
        var completedCalled = false;
        IObserver<int>? producer = null;
        var received = new List<int>();

        var observable = Observable.Create<int>(observer =>
        {
            producer = observer;
            return Disposable.Null;
        });

        var subscription = observable.Subscribe(
            Observer.Create<int>(onNext: received.Add, onCompleted: () => completedCalled = true)
        );

        producer!.OnNext(1);
        producer.OnNext(2);
        producer.OnNext(3);
        subscription.Dispose();
        producer.OnNext(4);
        producer.OnNext(5);
        producer.OnCompleted();

        // Assert
        received.Should().Equal(1, 2, 3);
        completedCalled.Should().BeFalse();
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
        observable.Subscribe(Observer.Create<int>(onNext: received.Add));

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
        observable.Subscribe(Observer.Create<int>(onNext: v => received.Add(v)));

        // Assert
        received.Should().HaveCount(threadCount * valuesPerThread);
    }

    [Fact]
    public void Observable_Create_AutoDetach_OnNext_Throw_DisposesSource_Test()
    {
        // Arrange: the subscribe callback emits 5 items synchronously; the observer
        // collects them and throws on the third. Only 2 items are received and the
        // source disposable must be disposed once subscribe returns.
        var received = new List<int>();
        var disposed = false;
        var observable = Observable.Create<int>(observer =>
        {
            for (var i = 1; i <= 5; i++)
            {
                try
                {
                    observer.OnNext(i);
                }
                catch
                {
                    break;
                }
            }
            return Disposable.Create(() => disposed = true);
        });

        // Act
        observable.Subscribe(
            Observer.Create<int>(onNext: v =>
            {
                if (v == 3)
                    throw new InvalidOperationException();
                received.Add(v);
            })
        );

        // Assert
        disposed.Should().BeTrue();
        received.Should().Equal(1, 2);
    }

    [Fact]
    public void Observable_Create_AutoDetach_OnError_Throw_DisposesSource_Test()
    {
        // Arrange
        IObserver<int>? capturedObserver = null;
        var disposed = false;
        var observable = Observable.Create<int>(observer =>
        {
            capturedObserver = observer;
            return Disposable.Create(() => disposed = true);
        });
        observable.Subscribe(
            Observer.Create<int>(onError: _ => throw new InvalidOperationException("boom"))
        );

        // Act & Assert
        var act = () => capturedObserver!.OnError(new Exception("source error"));
        act.Should().Throw<InvalidOperationException>().WithMessage("boom");
        disposed.Should().BeTrue();
    }

    [Fact]
    public void Observable_Create_AutoDetach_OnCompleted_Throw_DisposesSource_Test()
    {
        // Arrange
        IObserver<int>? capturedObserver = null;
        var disposed = false;
        var observable = Observable.Create<int>(observer =>
        {
            capturedObserver = observer;
            return Disposable.Create(() => disposed = true);
        });
        observable.Subscribe(
            Observer.Create<int>(onCompleted: () => throw new InvalidOperationException("boom"))
        );

        // Act & Assert
        var act = () => capturedObserver!.OnCompleted();
        act.Should().Throw<InvalidOperationException>().WithMessage("boom");
        disposed.Should().BeTrue();
    }

    [Fact]
    public void Observable_Create_AutoDetach_SuccessfulOnError_DisposesSource_Test()
    {
        // Arrange
        var disposed = false;
        var observable = Observable.Create<int>(observer =>
        {
            observer.OnError(new Exception("source error"));
            return Disposable.Create(() => disposed = true);
        });

        // Act
        observable.Subscribe(Observer.Create<int>(onError: _ => { }));

        // Assert
        disposed.Should().BeTrue();
    }

    [Fact]
    public void Observable_Create_AutoDetach_SuccessfulOnCompleted_DisposesSource_Test()
    {
        // Arrange
        var disposed = false;
        var observable = Observable.Create<int>(observer =>
        {
            observer.OnCompleted();
            return Disposable.Create(() => disposed = true);
        });

        // Act
        observable.Subscribe(Observer.Create<int>());

        // Assert
        disposed.Should().BeTrue();
    }
}
