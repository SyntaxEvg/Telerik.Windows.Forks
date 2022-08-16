using System;

namespace CsQuery.Promises
{
	class Deferred<T> : Deferred, IPromise<T>, IPromise
	{
		public IPromise Then(PromiseAction<T> success, PromiseAction<T> failure = null)
		{
			return base.Then(success, failure);
		}

		public IPromise Then(PromiseFunction<T> success, PromiseFunction<T> failure = null)
		{
			return base.Then(success, failure);
		}
	}
}
