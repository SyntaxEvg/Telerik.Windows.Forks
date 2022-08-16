using System;
using Telerik.Windows.Documents.Fixed.Model.Annotations;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	interface IWidgetCreator<T> where T : Widget
	{
		T CreateWidget();
	}
}
