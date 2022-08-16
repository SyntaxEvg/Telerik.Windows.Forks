using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model
{
	abstract class RtfElement
	{
		public abstract RtfElementType Type { get; }

		public sealed override bool Equals(object obj)
		{
			return obj == this || (obj != null && !(base.GetType() != obj.GetType()) && this.IsEqual(obj));
		}

		public sealed override int GetHashCode()
		{
			return HashTool.AddHashCode(base.GetType().GetHashCode(), this.ComputeHashCode());
		}

		protected virtual bool IsEqual(object obj)
		{
			return true;
		}

		protected virtual int ComputeHashCode()
		{
			return 251705873;
		}
	}
}
