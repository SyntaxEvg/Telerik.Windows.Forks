using System;
using Telerik.Windows.Documents.Fixed.Model.Actions;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class ActionHyperlinkStartLayoutElement : HyperlinkStartLayoutElement
	{
		public ActionHyperlinkStartLayoutElement(Telerik.Windows.Documents.Fixed.Model.Actions.Action action, FontBase font, double fontSize)
			: base(font, fontSize)
		{
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Fixed.Model.Actions.Action>(action, "action");
			this.action = action;
		}

		public Telerik.Windows.Documents.Fixed.Model.Actions.Action Action
		{
			get
			{
				return this.action;
			}
		}

		public override Annotation CreateAnnotation()
		{
			return new Link(this.action);
		}

		public override StartMarkerLayoutElement Clone()
		{
			return new ActionHyperlinkStartLayoutElement(this.action, base.Font, base.FontSize);
		}

		readonly Telerik.Windows.Documents.Fixed.Model.Actions.Action action;
	}
}
