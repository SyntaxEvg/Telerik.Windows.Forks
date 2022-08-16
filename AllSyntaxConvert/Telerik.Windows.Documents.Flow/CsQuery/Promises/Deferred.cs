using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace CsQuery.Promises
{
	class Deferred : IPromise
	{
		public Deferred()
		{
			if (When.Debug)
			{
				this.FailOnResolutionExceptions = true;
			}
		}

		protected Func<object, IPromise> Success
		{
			get
			{
				return this._Success;
			}
			set
			{
				if (this._Success != null)
				{
					throw new InvalidOperationException("This promise has already been assigned a success delegate.");
				}
				this._Success = value;
				if (this.Resolved == true)
				{
					this.ResolveImpl();
				}
			}
		}

		protected Func<object, IPromise> Failure
		{
			get
			{
				return this._Failure;
			}
			set
			{
				if (this._Failure != null)
				{
					throw new InvalidOperationException("This promise has already been assigned a failure delegate.");
				}
				this._Failure = value;
				if (this.Resolved == false)
				{
					this.RejectImpl();
				}
			}
		}

		public bool FailOnResolutionExceptions { get; set; }

		public void Resolve(object parm = null)
		{
			this.Parameter = parm;
			this.Resolved = new bool?(true);
			this.ResolveImpl();
		}

		public void Reject(object parm = null)
		{
			this.Parameter = parm;
			this.Resolved = new bool?(false);
			this.RejectImpl();
		}

		public IPromise Then(Delegate success, Delegate failure = null)
		{
			object locker =new object();
			bool flag = false;
			IPromise result;
			try
			{
				
				Monitor.Enter(locker = this.Locker, ref flag);
				Deferred nextDeferred = this.GetNextDeferred();
				MethodInfo methodInfo = ((success != null) ? success.Method : failure.Method);
				Type[] array = (from item in methodInfo.GetParameters()
					select item.ParameterType).ToArray<Type>();
				bool useParms = array.Length > 0;
				this.Success = delegate(object parm)
				{
					object obj = success.DynamicInvoke(this.GetParameters(useParms));
					return obj as IPromise;
				};
				this.Failure = delegate(object parm)
				{
					object obj = failure.DynamicInvoke(this.GetParameters(useParms));
					return obj as IPromise;
				};
				result = nextDeferred;
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(locker);
				}
			}
			return result;
		}

		public IPromise Then(PromiseAction<object> success, PromiseAction<object> failure = null)
		{
			IPromise result;
			lock (this.Locker)
			{
				Deferred nextDeferred = this.GetNextDeferred();
				this.Success = delegate(object parm)
				{
					success(parm);
					return null;
				};
				if (failure != null)
				{
					this.Failure = delegate(object parm)
					{
						failure(parm);
						return null;
					};
				}
				result = nextDeferred;
			}
			return result;
		}

		public IPromise Then(PromiseFunction<object> success, PromiseFunction<object> failure = null)
		{
			IPromise result;
			lock (this.Locker)
			{
				Deferred nextDeferred = this.GetNextDeferred();
				this.Success = (object parm) => success(this.Parameter);
				if (failure != null)
				{
					this.Failure = (object parm) => success(this.Parameter);
				}
				result = nextDeferred;
			}
			return result;
		}

		public IPromise Then(Action success, Action failure = null)
		{
			IPromise result;
			lock (this.Locker)
			{
				Deferred nextDeferred = this.GetNextDeferred();
				this.Success = delegate(object parm)
				{
					success();
					return null;
				};
				if (failure != null)
				{
					this.Failure = delegate(object parm)
					{
						failure();
						return null;
					};
				}
				result = nextDeferred;
			}
			return result;
		}

		public IPromise Then(Func<IPromise> success, Func<IPromise> failure = null)
		{
			IPromise result;
			lock (this.Locker)
			{
				Deferred nextDeferred = this.GetNextDeferred();
				this.Success = (object parm) => success();
				if (failure != null)
				{
					this.Failure = (object parm) => failure();
				}
				result = nextDeferred;
			}
			return result;
		}

		protected object[] GetParameters(bool useParms)
		{
			object[] result = null;
			if (useParms)
			{
				result = new object[] { this.Parameter };
			}
			return result;
		}

		protected void ResolveImpl()
		{
			lock (this.Locker)
			{
				if (this.Success != null)
				{
					if (!this.FailOnResolutionExceptions)
					{
						try
						{
							this.Success(this.Parameter);
							goto IL_52;
						}
						catch
						{
							this.RejectImpl();
							return;
						}
					}
					this.Success(this.Parameter);
				}
				IL_52:
				if (this.NextDeferred != null)
				{
					this.NextDeferred.ForEach(delegate(Deferred item)
					{
						item.Resolve(this.Parameter);
					});
				}
			}
		}

		protected void RejectImpl()
		{
			if (this.Failure != null)
			{
				if (!this.FailOnResolutionExceptions)
				{
					try
					{
						this.Failure(this.Parameter);
						goto IL_3B;
					}
					catch
					{
						goto IL_3B;
					}
				}
				this.Failure(this.Parameter);
			}
			IL_3B:
			if (this.NextDeferred != null)
			{
				this.NextDeferred.ForEach(delegate(Deferred item)
				{
					item.Reject(this.Parameter);
				});
			}
		}

		Deferred GetNextDeferred()
		{
			Deferred deferred = new Deferred();
			if (this.NextDeferred == null)
			{
				this.NextDeferred = new List<Deferred>();
			}
			this.NextDeferred.Add(deferred);
			return deferred;
		}

		internal object Locker = new object();

		Func<object, IPromise> _Success;

		Func<object, IPromise> _Failure;

		protected List<Deferred> NextDeferred;

		protected bool? Resolved;

		protected object Parameter;
	}
}
