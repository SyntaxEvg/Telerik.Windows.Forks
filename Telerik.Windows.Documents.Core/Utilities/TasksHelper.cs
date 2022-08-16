using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Telerik.Windows.Documents.Utilities
{
	static class TasksHelper
	{
		public static void DoAsync(IEnumerable<Action> actions)
		{
			TasksHelper.DoAsync(actions.ToArray<Action>());
		}

		public static void DoAsync(params Action[] actions)
		{
			Task[] array = new Task[actions.Length];
			for (int i = 0; i < actions.Length; i++)
			{
				array[i] = new Task(actions[i]);
			}
			for (int j = 0; j < actions.Length; j++)
			{
				array[j].Start(TaskScheduler.Default);
			}
			Task.WaitAll(array);
		}
	}
}
