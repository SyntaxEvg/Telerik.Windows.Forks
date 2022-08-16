using System;
using Telerik.Windows.Documents.Core.Data;

namespace Telerik.Windows.Documents.Core.Shapes
{
	abstract class PathSegment
	{
		public abstract PathSegment Clone();

		public abstract void Transform(Matrix transformMatrix);
	}
}
