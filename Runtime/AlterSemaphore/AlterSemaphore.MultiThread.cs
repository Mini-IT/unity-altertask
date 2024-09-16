#if UNITY_WEBGL
#define SINGLE_THREAD
#endif

#if !SINGLE_THREAD

using System.Threading;

namespace MiniIT.Threading
{
	public class AlterSemaphore : SemaphoreSlim
	{
		public AlterSemaphore(int initialCount) : base(initialCount)
		{
		}

		public AlterSemaphore(int initialCount, int maxCount) : base(initialCount, maxCount)
		{
		}
	}
}

#endif
