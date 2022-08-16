using System;

namespace Telerik.Windows.Documents.Utilities
{
	class IdGenerator
	{
		public IdGenerator(int startFromIndex)
		{
			this.counter = startFromIndex;
		}

		public IdGenerator()
			: this(0)
		{
		}

		public int GetNext()
		{
			int result;
			lock (IdGenerator.lockObject)
			{
				result = this.counter++;
			}
			return result;
		}

		static readonly object lockObject = new object();

		int counter;
	}
}
