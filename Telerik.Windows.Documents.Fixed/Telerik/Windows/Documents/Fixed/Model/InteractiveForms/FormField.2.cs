using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.Annotations;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public abstract class FormField<T> : FormField, IWidgetCreator<T> where T : Widget
	{
		internal FormField(string fieldName)
			: base(fieldName)
		{
		}

		public new WidgetCollection<T> Widgets
		{
			get
			{
				if (this.widgets == null)
				{
					this.widgets = this.CreateWidgetsCollection();
				}
				return this.widgets;
			}
		}

		internal sealed override IEnumerable<Widget> GetWidgets()
		{
			foreach (T widget in this.Widgets)
			{
				yield return widget;
			}
			yield break;
		}

		internal sealed override Widget AddWidget()
		{
			return this.Widgets.AddEmptyWidget();
		}

		internal sealed override void AddClonedWidget(Widget clonedWidget)
		{
			this.Widgets.AddClonedWidget(clonedWidget);
		}

		internal WidgetCollection<T> CreateWidgetsCollection()
		{
			return new WidgetCollection<T>(this, new Action<T>(this.InitializeWidgetAppearanceProperties));
		}

		internal abstract void InitializeWidgetAppearanceProperties(T widget);

		internal abstract T CreateEmptyWidget();

		T IWidgetCreator<T>.CreateWidget()
		{
			return this.CreateEmptyWidget();
		}

		WidgetCollection<T> widgets;
	}
}
