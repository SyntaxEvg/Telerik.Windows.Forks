using System;
using Telerik.Windows.Documents.Flow.Model.Cloning;

namespace Telerik.Windows.Documents.Flow.Model
{
	public class Break : InlineBase
	{
		public Break(RadFlowDocument document)
			: base(document)
		{
		}

		public BreakType BreakType
		{
			get
			{
				return this.breakType;
			}
			set
			{
				this.breakType = value;
			}
		}

		public TextWrappingRestartLocation TextWrappingRestartLocation
		{
			get
			{
				return this.textWrappingRestartLocation;
			}
			set
			{
				this.textWrappingRestartLocation = value;
			}
		}

		internal override DocumentElementType Type
		{
			get
			{
				return DocumentElementType.Break;
			}
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			return new Break(cloneContext.Document)
			{
				BreakType = this.BreakType,
				TextWrappingRestartLocation = this.TextWrappingRestartLocation
			};
		}

		BreakType breakType;

		TextWrappingRestartLocation textWrappingRestartLocation;
	}
}
