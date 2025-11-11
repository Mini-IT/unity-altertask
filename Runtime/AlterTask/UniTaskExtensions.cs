#if UNITY_WEBGL
#define SINGLE_THREAD
#endif

#if SINGLE_THREAD

using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;

namespace MiniIT.Threading
{
	public static class UniTaskExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UniTask ConfigureAwait(this UniTask task, bool _)
		{
			return task;
		}
	}
}

#endif
