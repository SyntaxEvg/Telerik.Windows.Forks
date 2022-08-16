using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Core
{
	class RadWeakEventListener<TInstance, TSource, TEventArgs> where TInstance : class
	{
		public Action<TInstance, object, TEventArgs> OnEventAction
		{
			get
			{
				return this.onEventAction;
			}
			set
			{
				if (value != null && !value.Method.IsStatic)
				{
					throw new ArgumentException("OnEventAction method must be static otherwise the event WeakEventListner class does not prevent memory leaks.");
				}
				this.onEventAction = value;
			}
		}

		internal Action<RadWeakEventListener<TInstance, TSource, TEventArgs>, TSource> OnDetachAction
		{
			get
			{
				return this.onDetachAction;
			}
			set
			{
				if (value != null && !value.Method.IsStatic)
				{
					throw new ArgumentException("OnDetachAction method must be static otherwise the event WeakEventListner cannot guarantee to unregister the handler.");
				}
				this.onDetachAction = value;
			}
		}

		public RadWeakEventListener(TInstance instance, TSource source)
		{
			Guard.ThrowExceptionIfNull<TInstance>(instance, "instance");
			Guard.ThrowExceptionIfNull<TSource>(source, "source");
			this.weakInstance = new WeakReference(instance);
			this.weakSource = new WeakReference(source);
		}

		public void OnEvent(object source, TEventArgs eventArgs)
		{
			TInstance tinstance = (TInstance)((object)this.weakInstance.Target);
			if (tinstance != null)
			{
				if (this.OnEventAction != null)
				{
					this.OnEventAction(tinstance, source, eventArgs);
					return;
				}
			}
			else
			{
				this.Detach();
			}
		}

		public void Detach()
		{
			TSource tsource = (TSource)((object)this.weakSource.Target);
			if (this.OnDetachAction != null && tsource != null)
			{
				this.OnDetachAction(this, tsource);
				this.OnDetachAction = null;
			}
		}

		readonly WeakReference weakInstance;

		readonly WeakReference weakSource;

		Action<TInstance, object, TEventArgs> onEventAction;

		Action<RadWeakEventListener<TInstance, TSource, TEventArgs>, TSource> onDetachAction;
	}
}
