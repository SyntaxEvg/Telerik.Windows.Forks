using System;

namespace CsQuery.Promises
{
	interface IPromise<T> : IPromise
	{
		IPromise Then(PromiseAction<T> success, PromiseAction<T> failure = null);

		IPromise Then(PromiseFunction<T> success, PromiseFunction<T> failure = null);
	}
}
