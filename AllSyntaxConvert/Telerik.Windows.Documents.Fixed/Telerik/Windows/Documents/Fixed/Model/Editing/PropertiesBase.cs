using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing
{
	public abstract class PropertiesBase<T> where T : PropertiesBase<T>, new()
	{
		internal PropertiesBase()
		{
			this.propertiesStack = new Stack<T>();
		}

		public IDisposable Save()
		{
			T item = Activator.CreateInstance<T>();
			item.CopyFrom((T)((object)this));
			this.propertiesStack.Push(item);
			return new DisposableObject(new Action(this.Restore));
		}

		public void Restore()
		{
			this.CopyFrom(this.propertiesStack.Pop());
		}

		public abstract void CopyFrom(T other);

		readonly Stack<T> propertiesStack;
	}
}
