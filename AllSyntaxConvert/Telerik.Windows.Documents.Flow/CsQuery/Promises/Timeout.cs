using System;
using System.Timers;

namespace CsQuery.Promises
{
	class Timeout<T> : IPromise<T>, IPromise
	{
		public Timeout(int timeoutMilliseconds)
		{
			this.ConfigureTimeout(timeoutMilliseconds, default(T), false);
		}

		public Timeout(int timeoutMilliseconds, T parameterValue)
		{
			this.useParameter = true;
			this.ConfigureTimeout(timeoutMilliseconds, parameterValue, false);
		}

		public Timeout(int timeoutMilliseconds, bool resolveOnTimeout)
		{
			this.ConfigureTimeout(timeoutMilliseconds, default(T), resolveOnTimeout);
		}

		public Timeout(int timeoutMilliseconds, T parameterValue, bool resolveOnTimeout)
		{
			this.useParameter = true;
			this.ConfigureTimeout(timeoutMilliseconds, parameterValue, resolveOnTimeout);
		}

		void ConfigureTimeout(int timeoutMilliseconds, T parameterValue, bool succeedOnTimeout)
		{
			this.TimeoutMilliseconds = timeoutMilliseconds;
			this.ResolveOnTimeout = succeedOnTimeout;
			this.ParameterValue = parameterValue;
			this.deferred = new Deferred<T>();
			this.Timer = new Timer((double)timeoutMilliseconds);
			this.Timer.Elapsed += this.Timer_Elapsed;
			this.Timer.Start();
		}

		public void Stop(bool resolve)
		{
			this.CompletePromise(resolve);
		}

		public void Stop()
		{
			this.CompletePromise(this.ResolveOnTimeout);
		}

		protected void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.CompletePromise(this.ResolveOnTimeout);
		}

		protected void CompletePromise(bool resolve)
		{
			this.Timer.Stop();
			if (resolve)
			{
				if (this.useParameter)
				{
					this.deferred.Resolve(this.ParameterValue);
					return;
				}
				this.deferred.Resolve(null);
				return;
			}
			else
			{
				if (this.useParameter)
				{
					this.deferred.Reject(this.ParameterValue);
					return;
				}
				this.deferred.Reject(null);
				return;
			}
		}

		public IPromise Then(PromiseAction<T> success, PromiseAction<T> failure = null)
		{
			return this.deferred.Then(success, failure);
		}

		public IPromise Then(PromiseFunction<T> success, PromiseFunction<T> failure = null)
		{
			return this.deferred.Then(success, failure);
		}

		public IPromise Then(Delegate success, Delegate failure = null)
		{
			return this.deferred.Then(success, failure);
		}

		public IPromise Then(Action success, Action failure = null)
		{
			return this.deferred.Then(success, failure);
		}

		public IPromise Then(Func<IPromise> success, Func<IPromise> failure = null)
		{
			return this.deferred.Then(success, failure);
		}

		IPromise IPromise.Then(PromiseAction<object> success, PromiseAction<object> failure)
		{
			return this.deferred.Then(success, failure);
		}

		IPromise IPromise.Then(PromiseFunction<object> success, PromiseFunction<object> failure)
		{
			return this.deferred.Then(success, failure);
		}

		Timer Timer;

		int TimeoutMilliseconds;

		bool ResolveOnTimeout;

		T ParameterValue;

		bool useParameter;

		Deferred<T> deferred;
	}
}
