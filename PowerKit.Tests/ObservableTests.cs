using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using Xunit;

namespace PowerKit.Tests;

file class FakeObserver<T>(
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
        observable.Subscribe(new FakeObserver<int>());

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
        observable.Subscribe(new FakeObserver<int>(received.Add));

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
        observable.Subscribe(new FakeObserver<int>(onError: ex => receivedError = ex));

        // Assert
        receivedError.Should().BeOfType<InvalidOperationException>();
        receivedError.Message.Should().Be("test error");
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
        observable.Subscribe(new FakeObserver<int>(onCompleted: () => completed = true));

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
        var subscription = observable.Subscribe(new FakeObserver<int>());
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
        observable.Subscribe(new FakeObserver<int>(received.Add));

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
        observable.Subscribe(new FakeObserver<int>(v => received.Add(v)));

        // Assert
        received.Should().HaveCount(threadCount * valuesPerThread);
    }

    [Fact]
    public void Observable_Create_AutoDetach_OnNext_Throw_DisposesSource_Test()
    {
        // Arrange: subscribe captures the observer and returns the disposable without emitting.
        // OnNext is fired after the subscription is fully established, so the source disposable
        // is already assigned when the callback throws.
        IObserver<int>? capturedObserver = null;
        var disposed = false;
        var observable = Observable.Create<int>(observer =>
        {
            capturedObserver = observer;
            return Disposable.Create(() => disposed = true);
        });
        observable.Subscribe(
            new FakeObserver<int>(onNext: _ => throw new InvalidOperationException("boom"))
        );

        // Act & Assert
        var act = () => capturedObserver!.OnNext(1);
        act.Should().Throw<InvalidOperationException>().WithMessage("boom");
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
            new FakeObserver<int>(onError: _ => throw new InvalidOperationException("boom"))
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
            new FakeObserver<int>(onCompleted: () => throw new InvalidOperationException("boom"))
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
        observable.Subscribe(new FakeObserver<int>(onError: _ => { }));

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
        observable.Subscribe(new FakeObserver<int>());

        // Assert
        disposed.Should().BeTrue();
    }

    [Fact]
    public void Observable_Create_AutoDetach_SynchronousOnNext_Throw_DisposesReturnedDisposable_Test()
    {
        // Arrange
        // The subscribe callback invokes OnNext synchronously (before returning its IDisposable).
        // If OnNext throws, the returned disposable must be disposed immediately once assigned.
        var disposed = false;
        var observable = Observable.Create<int>(observer =>
        {
            // This fires OnNext before we return the disposable.
            // The throw will propagate out of subscribe, so we catch it here to still return.
            try
            {
                observer.OnNext(1);
            }
            catch
            {
                // swallow so we can still return the disposable
            }

            return Disposable.Create(() => disposed = true);
        });

        // The observer throws on OnNext.
        observable.Subscribe(
            new FakeObserver<int>(onNext: _ => throw new InvalidOperationException("boom"))
        );

        // Assert: once the disposable is returned from subscribe and assigned, it is disposed immediately.
        disposed.Should().BeTrue();
    }

    [Fact]
    public void Observable_CreateSynchronized_AutoDetach_OnNext_Throw_DisposesSource_Test()
    {
        // Arrange
        IObserver<int>? capturedObserver = null;
        var disposed = false;
        var observable = Observable.CreateSynchronized<int>(observer =>
        {
            capturedObserver = observer;
            return Disposable.Create(() => disposed = true);
        });
        observable.Subscribe(
            new FakeObserver<int>(onNext: _ => throw new InvalidOperationException("boom"))
        );

        // Act & Assert
        var act = () => capturedObserver!.OnNext(1);
        act.Should().Throw<InvalidOperationException>().WithMessage("boom");
        disposed.Should().BeTrue();
    }

    [Fact]
    public void Observable_CreateSynchronized_AutoDetach_SuccessfulOnCompleted_DisposesSource_Test()
    {
        // Arrange
        var disposed = false;
        var observable = Observable.CreateSynchronized<int>(observer =>
        {
            observer.OnCompleted();
            return Disposable.Create(() => disposed = true);
        });

        // Act
        observable.Subscribe(new FakeObserver<int>());

        // Assert
        disposed.Should().BeTrue();
    }
}
