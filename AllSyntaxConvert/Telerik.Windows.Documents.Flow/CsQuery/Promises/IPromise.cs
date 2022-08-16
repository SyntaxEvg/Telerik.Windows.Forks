using System;

namespace CsQuery.Promises
{
	interface IPromise
	{
		IPromise Then(Delegate success, Delegate failure = null);

		IPromise Then(Action success, Action failure = null);

		IPromise Then(Func<IPromise> success, Func<IPromise> failure = null);

		IPromise Then(PromiseAction<object> success, PromiseAction<object> failure = null);

		IPromise Then(PromiseFunction<object> success, PromiseFunction<object> failure = null);
	}
}
