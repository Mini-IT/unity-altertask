#if UNITY_WEBGL
#define FORCE_UNITASK
#endif

#if FORCE_UNITASK

using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;

namespace MiniIT.Threading.Tasks
{
	public struct AlterUniTaskVoidMethodBuilder
	{
		private UniTaskCompletionSource _completionSource;

		public static AlterUniTaskVoidMethodBuilder Create() => new AlterUniTaskVoidMethodBuilder();

		public AlterTaskVoid Task => new AlterTaskVoid();

		public void SetResult() => _completionSource.TrySetResult();

		public void SetException(Exception exception) => _completionSource.TrySetException(exception);

		public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
		{
			if (_completionSource == null)
			{
				_completionSource = new UniTaskCompletionSource();
			}
			stateMachine.MoveNext();
		}

		public void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			// No state machine tracking needed
		}

		public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter : INotifyCompletion
			where TStateMachine : IAsyncStateMachine
		{
			awaiter.OnCompleted(stateMachine.MoveNext);
		}

		public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter : ICriticalNotifyCompletion
			where TStateMachine : IAsyncStateMachine
		{
			awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
		}
	}

}

#endif
