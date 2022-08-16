using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.Annotations;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public class WidgetCollectionBase<T> : IEnumerable<T>, IEnumerable where T : Widget
	{
		internal WidgetCollectionBase(IWidgetCreator<T> creator)
		{
			this.widgets = new List<T>();
			this.creator = creator;
		}

		public int Count
		{
			get
			{
				return this.widgets.Count;
			}
		}

		public void Remove(T widget)
		{
			this.widgets.Remove(widget);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.widgets.GetEnumerator();
		}

		internal T AddEmptyWidget()
		{
			T t = this.creator.CreateWidget();
			this.widgets.Add(t);
			return t;
		}

		internal void AddClonedWidget(Widget clonedWidget)
		{
			T item = (T)((object)clonedWidget);
			this.widgets.Add(item);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.widgets.GetEnumerator();
		}

		IWidgetCreator<T> creator;

		readonly List<T> widgets;
	}
}
