using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Shapes;

namespace Telerik.Windows.Documents.Core.Fonts.Glyphs
{
	class GlyphOutlinesCollection : List<PathFigure>
	{
		public GlyphOutlinesCollection Clone()
		{
			GlyphOutlinesCollection glyphOutlinesCollection = new GlyphOutlinesCollection();
			foreach (PathFigure pathFigure in this)
			{
				glyphOutlinesCollection.Add(pathFigure.Clone());
			}
			return glyphOutlinesCollection;
		}

		public void Transform(Matrix transformMatrix)
		{
			foreach (PathFigure pathFigure in this)
			{
				pathFigure.Transform(transformMatrix);
			}
		}
	}
}
