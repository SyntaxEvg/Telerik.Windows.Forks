using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Protection
{
	public class PermissionRange
	{
		public PermissionRange(RadFlowDocument document, PermissionRangeCredentials permissionRangeCredentials)
			: this(document, permissionRangeCredentials, null, null)
		{
		}

		public PermissionRange(RadFlowDocument document, PermissionRangeCredentials permissionRangeCredentials, int? fromColumn, int? toColumn)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			Guard.ThrowExceptionIfNull<PermissionRangeCredentials>(permissionRangeCredentials, "permissionRangeCredentials");
			this.document = document;
			this.permissionCredentials = permissionRangeCredentials;
			this.fromColumn = fromColumn;
			this.toColumn = toColumn;
			this.permissionRangeStart = new PermissionRangeStart(this.Document, this);
			this.permissionRangeEnd = new PermissionRangeEnd(this.Document, this);
		}

		public RadFlowDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public PermissionRangeCredentials Credentials
		{
			get
			{
				return this.permissionCredentials;
			}
		}

		public PermissionRangeStart Start
		{
			get
			{
				return this.permissionRangeStart;
			}
		}

		public PermissionRangeEnd End
		{
			get
			{
				return this.permissionRangeEnd;
			}
		}

		public int? FromColumn
		{
			get
			{
				return this.fromColumn;
			}
		}

		public int? ToColumn
		{
			get
			{
				return this.toColumn;
			}
		}

		readonly RadFlowDocument document;

		readonly PermissionRangeStart permissionRangeStart;

		readonly PermissionRangeEnd permissionRangeEnd;

		readonly PermissionRangeCredentials permissionCredentials;

		readonly int? fromColumn;

		readonly int? toColumn;
	}
}
