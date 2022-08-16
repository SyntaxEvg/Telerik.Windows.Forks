using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core
{
	abstract class ConsecutiveElementBase : ElementBase
	{
		public ConsecutiveElementBase()
		{
			this.childElementsManager = new ElementChildManager();
		}

		public void EnsureWritingStarted()
		{
			if (!base.IsWritingStarted)
			{
				this.BeginWriteElement();
			}
		}

		public void EnsureWritingEnded()
		{
			if (base.IsWritingStarted && !base.IsWritingEnded)
			{
				ConsecutiveElementBase consecutiveElementBase = null;
				if (this.childElementsManager.TryGetLastChild<ConsecutiveElementBase>(out consecutiveElementBase))
				{
					consecutiveElementBase.EnsureWritingEnded();
				}
				this.EndWriteElement();
			}
		}

		public void BeginWriteElement()
		{
			if (base.IsWritingStarted)
			{
				throw new InvalidOperationException("Can not start already started element.");
			}
			base.BeginWrite();
		}

		public void EndWriteElement()
		{
			if (!base.IsWritingStarted)
			{
				throw new InvalidOperationException("Can not end an element before starting it.");
			}
			if (base.IsWritingEnded)
			{
				throw new InvalidOperationException("Can not end already ended element.");
			}
			base.EndWrite();
		}

		public void BeginReadElement()
		{
			if (base.IsReadingStarted)
			{
				throw new InvalidOperationException("Can not start already started element.");
			}
			base.BeginRead();
		}

		public void EndReadElement()
		{
			if (!base.IsReadingStarted)
			{
				throw new InvalidOperationException("Can not end an element before starting it.");
			}
			if (base.IsReadingEnded)
			{
				throw new InvalidOperationException("Can not end already ended element.");
			}
			base.EndRead();
		}

		protected bool ReadToElement<T>()
		{
			if (base.Reader.IsEndOfElement())
			{
				base.Reader.Read();
			}
			Type typeFromHandle = typeof(T);
			int registeredChildElementIndex = this.GetRegisteredChildElementIndex(typeFromHandle);
			int i;
			for (i = this.GetCurrentElementIndex(); i < registeredChildElementIndex; i = this.GetCurrentElementIndex())
			{
				base.Reader.SkipElement();
				if (base.Reader.IsEndOfElement())
				{
					base.Reader.Read();
				}
			}
			return i == registeredChildElementIndex;
		}

		protected TResult GetRegisteredChildElement<TResult>() where TResult : ConsecutiveElementBase, new()
		{
			return this.childElementsManager.GetRegisteredChild<TResult>();
		}

		protected void RegisterChildElement<TResult>() where TResult : ElementBase, new()
		{
			this.RegisterChildElement<TResult>(null);
		}

		protected void RegisterChildElement<TResult>(Type dependOnType) where TResult : ElementBase, new()
		{
			this.childElementsManager.RegisterChild<TResult>(delegate()
			{
				ElementContext context = new ElementContext(base.Writer, base.Reader, base.Theme);
				TResult tresult = Activator.CreateInstance<TResult>();
				tresult.SetContext(context);
				return tresult;
			}, dependOnType);
		}

		protected TResult CreateChildElement<TResult>() where TResult : ElementBase, new()
		{
			ElementContext context = new ElementContext(base.Writer, base.Reader, base.Theme);
			TResult result = Activator.CreateInstance<TResult>();
			result.SetContext(context);
			return result;
		}

		int GetCurrentElementIndex()
		{
			int result = int.MaxValue;
			string elementName = null;
			if (base.Reader.GetElementName(out elementName))
			{
				Type elementType = ElementsFactory.GetElementType(elementName);
				result = this.GetRegisteredChildElementIndex(elementType);
			}
			return result;
		}

		int GetRegisteredChildElementIndex(Type type)
		{
			return this.childElementsManager.GetRegisteredChildIndex(type);
		}

		ElementChildManager childElementsManager;
	}
}
