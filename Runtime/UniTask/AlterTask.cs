#if UNITY_WEBGL
#define FORCE_UNITASK
#endif

#if FORCE_UNITASK

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using YieldAwaitable = Cysharp.Threading.Tasks.YieldAwaitable;

namespace MiniIT.Threading.Tasks
{
	[AsyncMethodBuilder(typeof(AlterUniTaskGenericMethodBuilder<>))]
	public struct AlterTask<T>
	{
		public readonly UniTask<T> Task => _task;

		private readonly UniTask<T> _task;

		public AlterTask(UniTask<T> task)
		{
			_task = task;
		}

		public UniTask<T>.Awaiter GetAwaiter() => _task.GetAwaiter();

		public void Forget() => _task.Forget();

		public static bool operator ==(AlterTask<T> a, AlterTask<T> b)
			=> Equals(a.Task, b.Task);
		public static bool operator ==(AlterTask<T> alterTask, UniTask<T> task)
			=> Equals(alterTask.Task, task);
		public static bool operator ==(UniTask<T> task, AlterTask<T> alterTask)
			=> Equals(alterTask.Task, task);
		public static bool operator !=(AlterTask<T> a, AlterTask<T> b)
			=> !Equals(a.Task, b.Task);
		public static bool operator !=(AlterTask<T> alterTask, UniTask<T> task)
			=> !Equals(alterTask.Task, task);
		public static bool operator !=(UniTask<T> task, AlterTask<T> alterTask)
			=> !Equals(alterTask.Task, task);

		public override bool Equals(object obj) => obj is AlterTask<T> task && EqualityComparer<UniTask<T>>.Default.Equals(_task, task._task);
		public override int GetHashCode() => HashCode.Combine(_task);
	}

	[AsyncMethodBuilder(typeof(AlterUniTaskMethodBuilder))]
	public struct AlterTask
	{
		public readonly UniTask Task => _task;

		private readonly UniTask _task;

		public AlterTask(UniTask task)
		{
			_task = task;
		}

		public UniTask.Awaiter GetAwaiter() => _task.GetAwaiter();

		public void Forget() => _task.Forget();

		public static AlterTask Run(Action action)
			=> new AlterTask(RunTask(action, CancellationToken.None));
		public static AlterTask Run(Action action, CancellationToken cancellationToken)
			=> new AlterTask(RunTask(action, cancellationToken));
		public static AlterTask<T> Run<T>(Func<T> action)
			=> new AlterTask<T>(RunTask(action, CancellationToken.None));
		public static AlterTask<T> Run<T>(Func<T> action, CancellationToken cancellationToken)
			=> new AlterTask<T>(RunTask(action, cancellationToken));

		public static AlterTask CompletedTask
			=> new AlterTask(UniTask.CompletedTask);

		public static AlterTask Delay(TimeSpan delay) =>
			new AlterTask(UniTask.Delay(delay));
		public static AlterTask Delay(TimeSpan delay, CancellationToken cancellationToken)
			=> new AlterTask(UniTask.Delay(delay, cancellationToken: cancellationToken));
		public static AlterTask Delay(int milliseconds)
			=> new AlterTask(UniTask.Delay(milliseconds));
		public static AlterTask Delay(int milliseconds, CancellationToken cancellationToken)
			=> new AlterTask(UniTask.Delay(milliseconds, cancellationToken: cancellationToken));

		public static YieldAwaitable Yield() => UniTask.Yield();

		private static async UniTask RunTask(Action action, CancellationToken cancellationToken)
		{
			var taskCompletionSource = new UniTaskCompletionSource();
			InternalRunActionAsync(action, taskCompletionSource).Forget();
			await taskCompletionSource.Task.AttachExternalCancellation(cancellationToken);
		}

		private static async UniTaskVoid InternalRunActionAsync(Action action, UniTaskCompletionSource taskCompletionSource)
		{
			await UniTask.CompletedTask; // Yield ????
			action.Invoke();
			taskCompletionSource.TrySetResult();
		}

		private static async UniTask<T> RunTask<T>(Func<T> action, CancellationToken cancellationToken)
		{
			var taskCompletionSource = new UniTaskCompletionSource<T>();
			InternalRunActionAsync(action, taskCompletionSource).Forget();
			return await taskCompletionSource.Task.AttachExternalCancellation(cancellationToken);
		}

		private static async UniTask<T> InternalRunActionAsync<T>(Func<T> action, UniTaskCompletionSource<T> taskCompletionSource)
		{
			await UniTask.CompletedTask; // Yield ????
			T result = action.Invoke();
			taskCompletionSource.TrySetResult(result);
			return result;
		}

		#region WhenAll

		public static AlterTask WhenAll(params AlterTask[] tasks)
		{
			var internalTasks = new UniTask[tasks.Length];
			for (int i = 0; i < tasks.Length; i++)
			{
				internalTasks[i] = tasks[i].Task;
			}
			return WhenAll(internalTasks);
		}

		public static AlterTask WhenAll(IEnumerable<AlterTask> tasks)
		{
			var internalTasks = new List<UniTask>();
			foreach (AlterTask task in tasks)
			{
				internalTasks.Add(task.Task);
			}
			return WhenAll(internalTasks);
		}

		public static AlterTask<TResult[]> WhenAll<TResult>(IEnumerable<AlterTask<TResult>> tasks)
		{
			var internalTasks = new List<UniTask<TResult>>();
			foreach (var task in tasks)
			{
				internalTasks.Add(task.Task);
			}
			return WhenAll(internalTasks);
		}

		public static AlterTask<TResult[]> WhenAll<TResult>(params AlterTask<TResult>[] tasks)
		{
			var internalTasks = new UniTask<TResult>[tasks.Length];
			for (int i = 0; i < tasks.Length; i++)
			{
				internalTasks[i] = tasks[i].Task;
			}
			return WhenAll(internalTasks);
		}

		public static AlterTask WhenAll(params UniTask[] tasks)
			=> new AlterTask(UniTask.WhenAll(tasks));
		public static AlterTask WhenAll(IEnumerable<UniTask> tasks)
			=> new AlterTask(UniTask.WhenAll(tasks));

		public static AlterTask<TResult[]> WhenAll<TResult>(IEnumerable<UniTask<TResult>> tasks)
			=> new AlterTask<TResult[]>(UniTask.WhenAll(tasks));
		public static AlterTask<TResult[]> WhenAll<TResult>(params UniTask<TResult>[] tasks)
			=> new AlterTask<TResult[]>(UniTask.WhenAll(tasks));

		#endregion WhenAll

		#region WhenAny

		public static AlterTask<AlterTask> WhenAny(IEnumerable<AlterTask> tasks)
		{
			var internalTasks = new List<UniTask>();
			foreach (var task in tasks)
			{
				internalTasks.Add(task.Task);
			}
			return WhenAny(internalTasks);
		}

		public static AlterTask<AlterTask> WhenAny(params AlterTask[] tasks)
		{
			var internalTasks = new UniTask[tasks.Length];
			for (int i = 0; i < tasks.Length; i++)
			{
				internalTasks[i] = tasks[i].Task;
			}
			return WhenAny(internalTasks);
		}

		public static AlterTask<AlterTask<TResult>> WhenAny<TResult>(IEnumerable<AlterTask<TResult>> tasks)
		{
			var internalTasks = new List<UniTask<TResult>>();
			foreach (var task in tasks)
			{
				internalTasks.Add(task.Task);
			}
			return WhenAny(internalTasks);
		}

		public static AlterTask<AlterTask<TResult>> WhenAny<TResult>(params AlterTask<TResult>[] tasks)
		{
			var internalTasks = new UniTask<TResult>[tasks.Length];
			for (int i = 0; i < tasks.Length; i++)
			{
				internalTasks[i] = tasks[i].Task;
			}
			return WhenAny(internalTasks);
		}

		public static async AlterTask<AlterTask> WhenAny(IEnumerable<UniTask> tasks)
		{
			int index = await UniTask.WhenAny(tasks);
			int i = 0;
			foreach (var task in tasks)
			{
				if (index == i++)
				{
					return new AlterTask(task);
				}
			}
			return default;
		}

		public static async AlterTask<AlterTask> WhenAny(params UniTask[] tasks)
		{
			int index = await UniTask.WhenAny(tasks);
			return new AlterTask(tasks[index]);
		}

		public static async AlterTask<AlterTask<TResult>> WhenAny<TResult>(IEnumerable<UniTask<TResult>> tasks)
		{
			var result = await UniTask.WhenAny(tasks);
			return new AlterTask<TResult>(UniTask.FromResult(result.result));
		}
		public static async AlterTask<AlterTask<TResult>> WhenAny<TResult>(params UniTask<TResult>[] tasks)
		{
			var result = await UniTask.WhenAny(tasks);
			return new AlterTask<TResult>(tasks[result.winArgumentIndex]);
		}

		#endregion WhenAny

		public static bool operator ==(AlterTask a, AlterTask b)
			=> Equals(a.Task, b.Task);
		public static bool operator ==(AlterTask alterTask, UniTask task)
			=> Equals(alterTask.Task, task);
		public static bool operator ==(UniTask task, AlterTask alterTask)
			=> Equals(alterTask.Task, task);
		public static bool operator !=(AlterTask a, AlterTask b)
			=> !Equals(a.Task, b.Task);
		public static bool operator !=(AlterTask alterTask, UniTask task)
			=> !Equals(alterTask.Task, task);
		public static bool operator !=(UniTask task, AlterTask alterTask)
			=> !Equals(alterTask.Task, task);

		public override bool Equals(object obj) => obj is AlterTask task && EqualityComparer<UniTask>.Default.Equals(_task, task._task);
		public override int GetHashCode() => HashCode.Combine(_task);
	}

	[AsyncMethodBuilder(typeof(AlterUniTaskVoidMethodBuilder))]
	public struct AlterTaskVoid
	{
		// UniTaskVoid doesn't need a GetAwaiter, just serves as a marker
	}
}

#endif
