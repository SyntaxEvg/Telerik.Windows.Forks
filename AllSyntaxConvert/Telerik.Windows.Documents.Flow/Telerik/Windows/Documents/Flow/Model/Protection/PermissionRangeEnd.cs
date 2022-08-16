using System;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Protection
{
	public class PermissionRangeEnd : AnnotationRangeEndBase
	{
		internal PermissionRangeEnd(RadFlowDocument document, PermissionRange permission)
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
				return DocumentElementType.PermissionRangeEnd;
			}
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			PermissionRangeStart permissionRangeStart = cloneContext.PermissionContext.PopHangingAnnotationStart();
			if (permissionRangeStart != null)
			{
				return permissionRangeStart.Permission.End;
			}
			return null;
		}

		readonly PermissionRange permission;
	}
}
