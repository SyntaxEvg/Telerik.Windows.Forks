using System;
using CsQuery.Promises;

namespace CsQuery
{
	static class When
	{
		public static bool Debug { get; set; }

		public static Deferred Deferred()
		{
			return new Deferred();
		}

		public static Deferred<T> Deferred<T>()
		{
			return new Deferred<T>();
		}

		public static IPromise All(params IPromise[] promises)
		{
			return new WhenAll(promises);
		}

		public static IPromise All(int timeoutMilliseconds, params IPromise[] promises)
		{
			return new WhenAll(timeoutMilliseconds, promises);
		}

		public static IPromise Timeout(int timeoutMilliseconds)
		{
			return new Timeout(timeoutMilliseconds);
		}

		public static IPromise Timer(int timerMilliseconds)
		{
			return new Timeout(timerMilliseconds, true);
		}
	}
}
