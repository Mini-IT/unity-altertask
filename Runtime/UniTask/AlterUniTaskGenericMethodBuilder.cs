#if UNITY_WEBGL
//#define FORCE_UNITASK
#endif

#if FORCE_UNITASK

using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;

namespace MiniIT.Threading.Tasks
{
	public struct AlterUniTaskGenericMethodBuilder<T>
	{
		private UniTaskCompletionSource<T> _completionSource;

		public static AlterUniTaskGenericMethodBuilder<T> Create() => new AlterUniTaskGenericMethodBuilder<T>();

		public AlterTask<T> Task
		{
			get
			{
				if (_completionSource == null)
				{
					_completionSource = new UniTaskCompletionSource<T>();
				}
				return new AlterTask<T>(_completionSource.Task);
			}
		}

		public void SetResult(T result) => _completionSource.TrySetResult(result);

		public void SetException(Exception exception) => _completionSource.TrySetException(exception);

		public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
		{
			if (_completionSource == null)
			{
				_completionSource = new UniTaskCompletionSource<T>();
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
