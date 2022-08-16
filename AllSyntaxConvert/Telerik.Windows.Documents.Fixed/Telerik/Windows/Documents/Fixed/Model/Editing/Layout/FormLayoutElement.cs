using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Objects;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class FormLayoutElement : GraphicBasedLayoutElementBase<Form>
	{
		public FormLayoutElement(Form form, TextProperties textProperties)
			: base(form, new Size(form.Width, form.Height), textProperties)
		{
		}
	}
}
