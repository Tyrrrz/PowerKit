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
        var observable = Observable.Create<int>(_ => Disposable.Create(() => disposed = true));

        // Act
        disposed.Should().BeFalse();
        var subscription = observable.Subscribe(Observer.Create<int>());
        subscription.Dispose();

        // Assert
        disposed.Should().BeTrue();
    }

    [Fact]
    public void Observable_Create_Dispose_StopsEvents_Test()
    {
        // Arrange: capture the observer, subscribe, then dispose the subscription.
        // Subsequent OnNext calls must be silently ignored (no callback invocations).
        IObserver<int>? capturedObserver = null;
        var received = new List<int>();
        var observable = Observable.Create<int>(observer =>
        {
            capturedObserver = observer;
            return Disposable.Null;
        });
        var subscription = observable.Subscribe(Observer.Create<int>(onNext: received.Add));

        // Act
        subscription.Dispose();
        capturedObserver!.OnNext(1);

        // Assert
        received.Should().BeEmpty();
    }

    [Fact]
    public void Observable_Create_Dispose_AfterOnCompleted_NoDoubleDispose_Test()
    {
        // Arrange: OnCompleted fires and disposes the source. A subsequent external Dispose
        // must be a no-op — the source disposable must only run once.
        IObserver<int>? capturedObserver = null;
        var disposeCount = 0;
        var observable = Observable.Create<int>(observer =>
        {
            capturedObserver = observer;
            return Disposable.Create(() => disposeCount++);
        });
        var subscription = observable.Subscribe(Observer.Create<int>());
        capturedObserver!.OnCompleted();

        // Act
        subscription.Dispose();

        // Assert
        disposeCount.Should().Be(1);
    }

    [Fact]
    public void Observable_Create_Dispose_OnCompleted_NoEventAfterDispose_Test()
    {
        // Arrange: dispose the subscription first, then fire OnCompleted.
        // The observer's OnCompleted callback must not be invoked.
        IObserver<int>? capturedObserver = null;
        var completedCalled = false;
        var observable = Observable.Create<int>(observer =>
        {
            capturedObserver = observer;
            return Disposable.Null;
        });
        var subscription = observable.Subscribe(
            Observer.Create<int>(onCompleted: () => completedCalled = true)
        );

        // Act
        subscription.Dispose();
        capturedObserver!.OnCompleted();

        // Assert
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
        // Arrange: the subscribe callback emits events synchronously; the observer throws
        // on the third item. The source disposable must be disposed once subscribe returns.
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
            })
        );

        // Assert
        disposed.Should().BeTrue();
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
