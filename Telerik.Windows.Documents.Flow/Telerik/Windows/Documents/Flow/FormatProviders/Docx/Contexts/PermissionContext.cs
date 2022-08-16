using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Protection;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Contexts
{
	class PermissionContext : AnnotationContextBase<PermissionRange>
	{
		internal PermissionContext(RadFlowDocument document)
			: base(document)
		{
			this.hangingPermissionStarts = new List<PermissionRangeStart>();
			this.hangingPermissionEndIds = new List<int>();
		}

		internal static PermissionRange CreatePermission(RadFlowDocument document, PermissionStartElement permissionStartElement)
		{
			PermissionRangeCredentials permissionRangeCredentials;
			if (!string.IsNullOrEmpty(permissionStartElement.Editor))
			{
				permissionRangeCredentials = new PermissionRangeCredentials(permissionStartElement.Editor);
			}
			else
			{
				if (permissionStartElement.EditingGroup == null)
				{
					return null;
				}
				permissionRangeCredentials = new PermissionRangeCredentials(permissionStartElement.EditingGroup.Value);
			}
			PermissionRange result;
			if (permissionStartElement.ColFirst != permissionStartElement.ColLast)
			{
				result = new PermissionRange(document, permissionRangeCredentials, new int?(permissionStartElement.ColFirst), new int?(permissionStartElement.ColLast));
			}
			else
			{
				result = new PermissionRange(document, permissionRangeCredentials);
			}
			return result;
		}

		internal void AddHangingPermissionStart(PermissionRangeStart annotationStart)
		{
			this.hangingPermissionStarts.Add(annotationStart);
		}

		internal void AddHangingPermissionEndId(int id)
		{
			this.hangingPermissionEndIds.Add(id);
		}

		internal List<PermissionRangeStart> GetHangingPermissionStarts()
		{
			return this.hangingPermissionStarts;
		}

		internal List<int> GetHangingPermissionEndIds()
		{
			return this.hangingPermissionEndIds;
		}

		internal void ClearHangingPermissionStarts()
		{
			this.hangingPermissionStarts.Clear();
		}

		internal void ClearHangingPermissionEnds()
		{
			this.hangingPermissionEndIds.Clear();
		}

		readonly List<PermissionRangeStart> hangingPermissionStarts;

		readonly List<int> hangingPermissionEndIds;
	}
}
