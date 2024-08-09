#if UNITY_WEBGL
#define FORCE_UNITASK
#endif

#if !FORCE_UNITASK


using System;
using System.Runtime.CompilerServices;

namespace MiniIT.Threading.Tasks
{
	public class AlterTaskMethodBuilder<T>
	{
		private AsyncTaskMethodBuilder<T> _builder;

		public static AlterTaskMethodBuilder<T> Create() => new AlterTaskMethodBuilder<T>();

		public AlterTask<T> Task => new AlterTask<T>(_builder.Task);

		public void SetResult(T result) => _builder.SetResult(result);

		public void SetException(Exception exception) => _builder.SetException(exception);

		public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
		{
			_builder.Start(ref stateMachine);
		}

		public void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_builder.SetStateMachine(stateMachine);
		}

		public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter : INotifyCompletion
			where TStateMachine : IAsyncStateMachine
		{
			_builder.AwaitOnCompleted(ref awaiter, ref stateMachine);
		}

		public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter : ICriticalNotifyCompletion
			where TStateMachine : IAsyncStateMachine
		{
			_builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
		}
	}
}

#endif
