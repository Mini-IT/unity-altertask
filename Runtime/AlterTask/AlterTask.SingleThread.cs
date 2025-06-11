#if UNITY_WEBGL
#define SINGLE_THREAD
#endif

#if SINGLE_THREAD

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

using YieldAwaitable = Cysharp.Threading.Tasks.YieldAwaitable;

namespace MiniIT.Threading
{
	public static class AlterTask
	{
		public static UniTask Run(Action action)
		{
			action.Invoke();
			return UniTask.CompletedTask;
		}

		public static UniTask<T> Run<T>(Func<T> func)
		{
			T result = func.Invoke();
			return UniTask.FromResult(result);
		}

		public static void RunAndForget(Action action, CancellationToken cancellationToken = default)
		{
			Wrap(action).Forget();
		}

		public static UniTask Wrap(Action action)
		{
			action.Invoke();
			return UniTask.CompletedTask;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UniTask Delay(TimeSpan delay)
			=> UniTask.Delay(delay);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UniTask Delay(TimeSpan delay, CancellationToken cancellationToken)
			=> UniTask.Delay(delay, cancellationToken: cancellationToken);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UniTask Delay(int milliseconds)
			=> UniTask.Delay(milliseconds);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UniTask Delay(int milliseconds, CancellationToken cancellationToken)
			=> UniTask.Delay(milliseconds, cancellationToken: cancellationToken);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static YieldAwaitable Yield()
			=> UniTask.Yield();
	}
}

#endif
