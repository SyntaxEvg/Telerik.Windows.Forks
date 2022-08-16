using System;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Protection
{
	public class PermissionRangeStart : AnnotationRangeStartBase
	{
		internal PermissionRangeStart(RadFlowDocument document, PermissionRange permission)
			: base(document)
		{
			this.permission = permission;
		}

		public PermissionRange Permission
		{
			get
			{
				return this.permission;
			}
		}

		internal override DocumentElementType Type
		{
			get
			{
				return DocumentElementType.PermissionRangeStart;
			}
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			PermissionRangeStart start = new PermissionRange(cloneContext.Document, this.Permission.Credentials.Clone(), this.Permission.FromColumn, this.Permission.ToColumn).Start;
			cloneContext.PermissionContext.AddHangingAnnotationStart(start);
			return start;
		}

		readonly PermissionRange permission;
	}
}
