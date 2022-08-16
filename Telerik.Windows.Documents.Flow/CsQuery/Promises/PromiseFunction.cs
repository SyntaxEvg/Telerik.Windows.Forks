using System;

namespace CsQuery.Promises
{
	delegate IPromise PromiseFunction<T>(T parameter);
}
