using System;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	sealed class FunctionalLazy<T>
	{
		public FunctionalLazy(Func<T> function)
		{
			this.function = function;
		}

		public FunctionalLazy(T value)
		{
			this.value = value;
		}

		public Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		public bool HasException
		{
			get
			{
				return this.exception != null;
			}
		}

		public bool IsForced
		{
			get
			{
				return this.forced;
			}
		}

		public T Value
		{
			get
			{
				return this.Force();
			}
		}

		public T Force()
		{
			T result;
			lock (this.forceLock)
			{
				result = this.UnsynchronizedForce();
			}
			return result;
		}

		public T UnsynchronizedForce()
		{
			if (this.exception != null)
			{
				throw this.exception;
			}
			if (this.function != null && !this.forced)
			{
				try
				{
					this.value = this.function();
					this.forced = true;
				}
				catch (Exception ex)
				{
					this.exception = ex;
					throw;
				}
			}
			return this.value;
		}

		readonly object forceLock = new object();

		readonly Func<T> function;

		Exception exception;

		bool forced;

		T value;
	}
}
