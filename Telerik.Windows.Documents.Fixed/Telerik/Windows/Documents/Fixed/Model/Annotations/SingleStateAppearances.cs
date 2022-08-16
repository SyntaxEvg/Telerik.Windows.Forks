using System;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	class SingleStateAppearances : AnnotationAppearances
	{
		public SingleStateAppearances()
		{
		}

		public SingleStateAppearances(AnnotationContentSource widgetContent)
		{
			this.NormalAppearance = widgetContent.NormalContentSource;
			this.MouseDownAppearance = widgetContent.MouseDownContentSource;
			this.MouseOverAppearance = widgetContent.MouseOverContentSource;
		}

		public sealed override AnnotationAppearancesType AppearancesType
		{
			get
			{
				return AnnotationAppearancesType.SingleStateAppearances;
			}
		}

		public FormSource NormalAppearance { get; set; }

		public FormSource MouseDownAppearance { get; set; }

		public FormSource MouseOverAppearance { get; set; }

		public override AnnotationAppearances Clone(RadFixedDocumentCloneContext cloneContext)
		{
			return new SingleStateAppearances
			{
				NormalAppearance = this.NormalAppearance,
				MouseDownAppearance = this.MouseDownAppearance,
				MouseOverAppearance = this.MouseOverAppearance
			};
		}
	}
}
