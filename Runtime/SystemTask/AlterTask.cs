#if UNITY_WEBGL
#define FORCE_UNITASK
#endif

#if !FORCE_UNITASK

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MiniIT.Threading.Tasks
{

	[AsyncMethodBuilder(typeof(AlterTaskMethodBuilder<>))]
	public struct AlterTask<T>
	{
		public readonly Task<T> Task => _task;

		private readonly Task<T> _task;

		public AlterTask(Task<T> task)
		{
			_task = task;
		}

		public TaskAwaiter<T> GetAwaiter() => _task.GetAwaiter();
		
		public static bool operator ==(AlterTask<T> a, AlterTask<T> b)
			=> a.Task == b.Task;
		public static bool operator ==(AlterTask<T> alterTask, Task<T> task)
			=> alterTask.Task == task;
		public static bool operator ==(Task<T> task, AlterTask<T> alterTask)
			=> alterTask.Task == task;
		public static bool operator !=(AlterTask<T> a, AlterTask<T> b)
			=> a.Task != b.Task;
		public static bool operator !=(AlterTask<T> alterTask, Task<T> task)
			=> alterTask.Task != task;
		public static bool operator !=(Task<T> task, AlterTask<T> alterTask)
			=> alterTask.Task != task;

		public override bool Equals(object obj) => obj is AlterTask<T> task && EqualityComparer<Task<T>>.Default.Equals(_task, task._task);
		public override int GetHashCode() => HashCode.Combine(_task);
	}

	[AsyncMethodBuilder(typeof(AlterTaskVoidMethodBuilder))]
	public struct AlterTask
	{
		public readonly Task Task => _task;

		private readonly Task _task;

		public AlterTask(Task task)
		{
			_task = task;
		}

		public TaskAwaiter GetAwaiter() => _task.GetAwaiter();

		public void Forget() { }

		public static AlterTask Run(Action action)
			=> new AlterTask(Task.Run(action));
		public static AlterTask Run(Action action, CancellationToken cancellationToken)
			=> new AlterTask(Task.Run(action, cancellationToken));
		public static AlterTask<T> Run<T>(Func<T> action)
			=> new AlterTask<T>(Task.Run(action));
		public static AlterTask<T> Run<T>(Func<T> action, CancellationToken cancellationToken)
			=> new AlterTask<T>(Task.Run(action, cancellationToken));

		public static AlterTask CompletedTask
			=> new AlterTask(Task.CompletedTask);

		public static AlterTask Delay(TimeSpan delay) =>
			new AlterTask(Task.Delay(delay));
		public static AlterTask Delay(TimeSpan delay, CancellationToken cancellationToken)
			=> new AlterTask(Task.Delay(delay, cancellationToken));
		public static AlterTask Delay(int milliseconds)
			=> new AlterTask(Task.Delay(milliseconds));
		public static AlterTask Delay(int milliseconds, CancellationToken cancellationToken)
			=> new AlterTask(Task.Delay(milliseconds, cancellationToken));

		public static YieldAwaitable Yield() => Task.Yield();

		#region WhenAll

		public static AlterTask WhenAll(params AlterTask[] tasks)
		{
			var internalTasks = new Task[tasks.Length];
			for (int i = 0; i < tasks.Length; i++)
			{
				internalTasks[i] = tasks[i].Task;
			}
			return WhenAll(internalTasks);
		}

		public static AlterTask WhenAll(IEnumerable<AlterTask> tasks)
		{
			var internalTasks = new List<Task>();
			foreach (AlterTask task in tasks)
			{
				internalTasks.Add(task.Task);
			}
			return WhenAll(internalTasks);
		}

		public static AlterTask<TResult[]> WhenAll<TResult>(IEnumerable<AlterTask<TResult>> tasks)
		{
			var internalTasks = new List<Task<TResult>>();
			foreach (var task in tasks)
			{
				internalTasks.Add(task.Task);
			}
			return WhenAll(internalTasks);
		}

		public static AlterTask<TResult[]> WhenAll<TResult>(params AlterTask<TResult>[] tasks)
		{
			var internalTasks = new Task<TResult>[tasks.Length];
			for (int i = 0; i < tasks.Length; i++)
			{
				internalTasks[i] = tasks[i].Task;
			}
			return WhenAll(internalTasks);
		}

		public static AlterTask WhenAll(params Task[] tasks)
			=> new AlterTask(Task.WhenAll(tasks));
		public static AlterTask WhenAll(IEnumerable<Task> tasks)
			=> new AlterTask(Task.WhenAll(tasks));

		public static AlterTask<TResult[]> WhenAll<TResult>(IEnumerable<Task<TResult>> tasks)
			=> new AlterTask<TResult[]>(Task.WhenAll(tasks));
		public static AlterTask<TResult[]> WhenAll<TResult>(params Task<TResult>[] tasks)
			=> new AlterTask<TResult[]>(Task.WhenAll(tasks));

		#endregion WhenAll

		#region WhenAny

		public static AlterTask<AlterTask> WhenAny(IEnumerable<AlterTask> tasks)
		{
			var internalTasks = new List<Task>();
			foreach (var task in tasks)
			{
				internalTasks.Add(task.Task);
			}
			return WhenAny(internalTasks);
		}

		public static AlterTask<AlterTask> WhenAny(params AlterTask[] tasks)
		{
			var internalTasks = new Task[tasks.Length];
			for (int i = 0; i < tasks.Length; i++)
			{
				internalTasks[i] = tasks[i].Task;
			}
			return WhenAny(internalTasks);
		}

		public static AlterTask<AlterTask<TResult>> WhenAny<TResult>(IEnumerable<AlterTask<TResult>> tasks)
		{
			var internalTasks = new List<Task<TResult>>();
			foreach (var task in tasks)
			{
				internalTasks.Add(task.Task);
			}
			return WhenAny(internalTasks);
		}

		public static AlterTask<AlterTask<TResult>> WhenAny<TResult>(params AlterTask<TResult>[] tasks)
		{
			var internalTasks = new Task<TResult>[tasks.Length];
			for (int i = 0; i < tasks.Length; i++)
			{
				internalTasks[i] = tasks[i].Task;
			}
			return WhenAny(internalTasks);
		}

		public static async AlterTask<AlterTask> WhenAny(IEnumerable<Task> tasks)
			=> new AlterTask(await Task.WhenAny(tasks));
		public static async AlterTask<AlterTask> WhenAny(params Task[] tasks)
			=> new AlterTask(await Task.WhenAny(tasks));
		public static async AlterTask<AlterTask<TResult>> WhenAny<TResult>(IEnumerable<Task<TResult>> tasks)
			=> new AlterTask<TResult>(await Task.WhenAny(tasks));
		public static async AlterTask<AlterTask<TResult>> WhenAny<TResult>(params Task<TResult>[] tasks)
			=> new AlterTask<TResult>(await Task.WhenAny(tasks));
		
		#endregion WhenAny

		public static bool operator ==(AlterTask a, AlterTask b)
			=> a.Task == b.Task;
		public static bool operator ==(AlterTask alterTask, Task task)
			=> alterTask.Task == task;
		public static bool operator ==(Task task, AlterTask alterTask)
			=> alterTask.Task == task;
		public static bool operator !=(AlterTask a, AlterTask b)
			=> a.Task != b.Task;
		public static bool operator !=(AlterTask alterTask, Task task)
			=> alterTask.Task != task;
		public static bool operator !=(Task task, AlterTask alterTask)
			=> alterTask.Task != task;

		public override bool Equals(object obj) => obj is AlterTask task && EqualityComparer<Task>.Default.Equals(_task, task._task);
		public override int GetHashCode() => HashCode.Combine(_task);
	}
}

#endif
