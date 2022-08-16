using System;

namespace Telerik.Windows.Documents.Fixed.Model.Internal.Annotations
{
	class SingleStateAppearancesOld : AnnotationAppearancesOld
	{
		public SingleStateAppearancesOld(Container normalAppearance, Container mouseDownAppearance, Container mouseOverAppearance)
		{
			this.normalAppearance = normalAppearance;
			this.mouseDownAppearance = mouseDownAppearance;
			this.mouseOverAppearance = mouseOverAppearance;
		}

		public override Container GetNormalAppearance()
		{
			return this.normalAppearance;
		}

		public override Container GetMouseDownAppearance()
		{
			return this.mouseDownAppearance;
		}

		public override Container GetMouseOverAppearance()
		{
			return this.mouseOverAppearance;
		}

		readonly Container normalAppearance;

		readonly Container mouseDownAppearance;

		readonly Container mouseOverAppearance;
	}
}
