using System;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	public class AnnotationContentSource
	{
		public FormSource NormalContentSource { get; set; }

		public FormSource MouseDownContentSource { get; set; }

		public FormSource MouseOverContentSource { get; set; }

		internal void Initialize(SingleStateAppearances singleStateAppearances)
		{
			this.NormalContentSource = singleStateAppearances.NormalAppearance;
			this.MouseDownContentSource = singleStateAppearances.MouseDownAppearance;
			this.MouseOverContentSource = singleStateAppearances.MouseOverAppearance;
		}

		internal void Initialize(AnnotationContentSource other)
		{
			this.NormalContentSource = other.NormalContentSource;
			this.MouseDownContentSource = other.MouseDownContentSource;
			this.MouseOverContentSource = other.MouseOverContentSource;
		}
	}
}
