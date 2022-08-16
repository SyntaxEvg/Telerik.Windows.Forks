using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Text;

namespace Telerik.Windows.Documents.Fixed.Utilities.Rendering
{
	class GlyphRenderingContext : ContentRenderingContext
	{
		internal GlyphRenderingContext(TextFragment textFragment, GlyphOutlinesCollection outlines, Matrix outlinesTransformation, Rect currentContainerBounds)
			: base(textFragment, currentContainerBounds)
		{
			this.outlines = outlines;
			this.outlinesTransformation = outlinesTransformation;
			this.properties = textFragment.TextProperties.GeometryProperties;
		}

		public GlyphOutlinesCollection Outlines
		{
			get
			{
				return this.outlines;
			}
		}

		public Matrix OutlinesTransformation
		{
			get
			{
				return this.outlinesTransformation;
			}
		}

		public GeometryPropertiesOwner Properties
		{
			get
			{
				return this.properties;
			}
		}

		readonly GlyphOutlinesCollection outlines;

		readonly Matrix outlinesTransformation;

		readonly GeometryPropertiesOwner properties;
	}
}
