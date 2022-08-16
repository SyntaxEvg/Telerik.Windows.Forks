using System;

namespace CsQuery.Promises
{
	class Timeout : Timeout<object>, IPromise
	{
		public Timeout(int timeoutMilliseconds)
			: base(timeoutMilliseconds)
		{
		}

		public Timeout(int timeoutMilliseconds, object parameterValue)
			: base(timeoutMilliseconds, parameterValue)
		{
		}

		public Timeout(int timeoutMilliseconds, bool succeedOnTimeout)
			: base(timeoutMilliseconds, succeedOnTimeout)
		{
		}

		public Timeout(int timeoutMilliseconds, object parameterValue, bool succeedOnTimeout)
			: base(timeoutMilliseconds, parameterValue, succeedOnTimeout)
		{
		}
	}
}
