﻿using System.Diagnostics;

namespace R3;

public static class ObservableSubscribeExtensions
{
    [DebuggerStepThrough]
    public static IDisposable Subscribe<T>(this Observable<T> source)
    {
        return source.Subscribe(new NopObserver<T>());
    }

    [DebuggerStepThrough]
    public static IDisposable Subscribe<T>(this Observable<T> source, Action<T> onNext)
    {
        return source.Subscribe(new AnonymousObserver<T>(onNext, ObservableSystem.GetUnhandledExceptionHandler(), Stubs.HandleResult));
    }

    [DebuggerStepThrough]
    public static IDisposable Subscribe<T>(this Observable<T> source, Action<T> onNext, Action<Result> onComplete)
    {
        return source.Subscribe(new AnonymousObserver<T>(onNext, ObservableSystem.GetUnhandledExceptionHandler(), onComplete));
    }

    [DebuggerStepThrough]
    public static IDisposable Subscribe<T>(this Observable<T> source, Action<T> onNext, Action<Exception> onErrorResume, Action<Result> onComplete)
    {
        return source.Subscribe(new AnonymousObserver<T>(onNext, onErrorResume, onComplete));
    }

    // with state

    [DebuggerStepThrough]
    public static IDisposable Subscribe<T, TState>(this Observable<T> source, TState state, Action<T, TState> onNext)
    {
        return source.Subscribe(new AnonymousObserver<T, TState>(onNext, Stubs<TState>.HandleException, Stubs<TState>.HandleResult, state));
    }

    [DebuggerStepThrough]
    public static IDisposable Subscribe<T, TState>(this Observable<T> source, TState state, Action<T, TState> onNext, Action<Result, TState> onComplete)
    {
        return source.Subscribe(new AnonymousObserver<T, TState>(onNext, Stubs<TState>.HandleException, onComplete, state));
    }

    [DebuggerStepThrough]
    public static IDisposable Subscribe<T, TState>(this Observable<T> source, TState state, Action<T, TState> onNext, Action<Exception, TState> onErrorResume, Action<Result, TState> onComplete)
    {
        return source.Subscribe(new AnonymousObserver<T, TState>(onNext, onErrorResume, onComplete, state));
    }
}

[DebuggerStepThrough]
internal sealed class NopObserver<T> : Observer<T>
{
    public NopObserver()
    {
    }

    [DebuggerStepThrough]
    protected override void OnNextCore(T value)
    {
    }

    [DebuggerStepThrough]
    protected override void OnErrorResumeCore(Exception error)
    {
        ObservableSystem.GetUnhandledExceptionHandler().Invoke(error);
    }

    [DebuggerStepThrough]
    protected override void OnCompletedCore(Result result)
    {
        if (result.IsFailure)
        {
            ObservableSystem.GetUnhandledExceptionHandler().Invoke(result.Exception);
        }
    }
}

[DebuggerStepThrough]
internal sealed class AnonymousRObserver<T>(Action<T> onNext, Action<Exception> onErrorResume) : Observer<T>
{
    [DebuggerStepThrough]
    protected override void OnNextCore(T value)
    {
        onNext(value);
    }

    [DebuggerStepThrough]
    protected override void OnErrorResumeCore(Exception error)
    {
        onErrorResume(error);
    }

    [DebuggerStepThrough]
    protected override void OnCompletedCore(Result result)
    {
        if (result.IsFailure)
        {
            ObservableSystem.GetUnhandledExceptionHandler().Invoke(result.Exception);
        }
    }
}

[DebuggerStepThrough]
internal sealed class AnonymousObserver<T>(Action<T> onNext, Action<Exception> onErrorResume, Action<Result> onComplete) : Observer<T>
{
    [DebuggerStepThrough]
    protected override void OnNextCore(T value)
    {
        onNext(value);
    }

    [DebuggerStepThrough]
    protected override void OnErrorResumeCore(Exception error)
    {
        onErrorResume(error);
    }

    [DebuggerStepThrough]
    protected override void OnCompletedCore(Result complete)
    {
        onComplete(complete);
    }
}

[DebuggerStepThrough]
internal sealed class AnonymousObserver<T, TState>(Action<T, TState> onNext, Action<Exception, TState> onErrorResume, Action<Result, TState> onComplete, TState state) : Observer<T>
{
    [DebuggerStepThrough]
    protected override void OnNextCore(T value)
    {
        onNext(value, state);
    }

    [DebuggerStepThrough]
    protected override void OnErrorResumeCore(Exception error)
    {
        onErrorResume(error, state);
    }

    [DebuggerStepThrough]
    protected override void OnCompletedCore(Result complete)
    {
        onComplete(complete, state);
    }
}
