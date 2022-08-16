using System;

namespace Telerik.Windows.Documents.Fixed.Model.Internal.Annotations
{
	abstract class AnnotationAppearancesOld
	{
		public abstract Container GetNormalAppearance();

		public abstract Container GetMouseDownAppearance();

		public abstract Container GetMouseOverAppearance();
	}
}
