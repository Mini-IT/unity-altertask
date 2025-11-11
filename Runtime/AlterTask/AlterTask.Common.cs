using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace MiniIT.Threading
{
	public static partial class AlterTask
	{
		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static void RunAndForget(Func<UniTask> action, CancellationToken cancellationToken = default)
		{
			RunAndForget(() => action.Invoke().Forget(), cancellationToken);
		}
	}
}
