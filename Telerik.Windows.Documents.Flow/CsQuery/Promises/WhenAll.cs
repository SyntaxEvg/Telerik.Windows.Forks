using System;
using System.Collections.Generic;

namespace CsQuery.Promises
{
	class WhenAll : IPromise
	{
		public WhenAll(params IPromise[] promises)
		{
			this.Configure(promises);
		}

		public WhenAll(int timeoutMilliseconds, params IPromise[] promises)
		{
			this.Configure(promises);
			this.timeout = new Timeout(timeoutMilliseconds);
			this.timeout.Then(null, new Action(this.TimedOut));
		}

		int ResolvedCount
		{
			get
			{
				return this._ResolvedCount;
			}
			set
			{
				lock (this._locker)
				{
					this._ResolvedCount = value;
					if (this._ResolvedCount == this.TotalCount)
					{
						this.CompletePromise();
					}
				}
			}
		}

		public IPromise Then(Delegate success, Delegate failure = null)
		{
			return this.Deferred.Then(success, failure);
		}

		public IPromise Then(Action success, Action failure = null)
		{
			return this.Deferred.Then(success, failure);
		}

		public IPromise Then(PromiseAction<object> success, PromiseAction<object> failure = null)
		{
			return this.Deferred.Then(success, failure);
		}

		public IPromise Then(Func<IPromise> success, Func<IPromise> failure = null)
		{
			return this.Deferred.Then(success, failure);
		}

		public IPromise Then(PromiseFunction<object> success, PromiseFunction<object> failure = null)
		{
			return this.Deferred.Then(success, failure);
		}

		void Configure(IEnumerable<IPromise> promises)
		{
			lock (this._locker)
			{
				this.Success = true;
				this.Deferred = new Deferred();
				int num = 0;
				foreach (IPromise promise in promises)
				{
					num++;
					promise.Then(new Action(this.PromiseResolve), new Action(this.PromiseReject));
				}
				this.TotalCount = num;
			}
		}

		void PromiseResolve()
		{
			lock (this._locker)
			{
				this.ResolvedCount++;
			}
		}

		void PromiseReject()
		{
			lock (this._locker)
			{
				this.Success = false;
				this.ResolvedCount++;
			}
		}

		void TimedOut()
		{
			lock (this._locker)
			{
				this.Success = false;
				this.CompletePromise();
			}
		}

		void CompletePromise()
		{
			lock (this._locker)
			{
				if (!this.Complete)
				{
					this.Complete = true;
					if (this.timeout != null)
					{
						this.timeout.Stop(true);
					}
					if (this.Success)
					{
						this.Deferred.Resolve(null);
					}
					else
					{
						this.Deferred.Reject(null);
					}
				}
			}
		}

		Deferred Deferred;

		int TotalCount;

		int _ResolvedCount;

		Timeout timeout;

		bool Complete;

		object _locker = new object();

		bool Success;
	}
}
