#if UNITY_WEBGL
#define SINGLE_THREAD
#endif

#if !SINGLE_THREAD

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

using YieldAwaitable = System.Runtime.CompilerServices.YieldAwaitable;

namespace MiniIT.Threading
{
	public static partial class AlterTask
	{
		public static Task Run(Action action)
		{
			return Task.Run(action);
		}

		public static Task<T> Run<T>(Func<T> func)
		{
			return Task.Run(func);
		}

		public static void RunAndForget(Action action, CancellationToken cancellationToken = default)
		{
			UniTask.RunOnThreadPool(action, false, cancellationToken).Forget();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task Delay(TimeSpan delay)
			=> Task.Delay(delay);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task Delay(TimeSpan delay, CancellationToken cancellationToken)
			=> Task.Delay(delay, cancellationToken: cancellationToken);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task Delay(int milliseconds)
			=> Task.Delay(milliseconds);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task Delay(int milliseconds, CancellationToken cancellationToken)
			=> Task.Delay(milliseconds, cancellationToken: cancellationToken);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static YieldAwaitable Yield() => Task.Yield();
	}
}

#endif
