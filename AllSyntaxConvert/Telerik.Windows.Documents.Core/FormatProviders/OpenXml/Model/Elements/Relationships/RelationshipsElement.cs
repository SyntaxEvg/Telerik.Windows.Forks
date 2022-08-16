using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Utilities;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Contexts;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Relationships
{
	class RelationshipsElement : OpenXmlPartRootElementBase
	{
		public RelationshipsElement(OpenXmlPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager, part)
		{
			this.relationships = new Dictionary<string, RelationshipInfo>();
		}

		public override string ElementName
		{
			get
			{
				return "Relationships";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.RelationshipsNamespace;
			}
		}

		public string CreateRelationship(string target, string type, string targetMode)
		{
			Guard.ThrowExceptionIfNull<string>(target, "target");
			Guard.ThrowExceptionIfNullOrEmpty(type, "type");
			string text = OpenXmlHelper.CreateRelationshipId(this.relationships.Count + 1);
			RelationshipInfo relationshipInfo = new RelationshipInfo();
			relationshipInfo.Id = text;
			relationshipInfo.Target = target;
			relationshipInfo.TargetMode = targetMode;
			relationshipInfo.Type = type;
			this.relationships[text] = relationshipInfo;
			return text;
		}

		public string GetRelationshipId(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			return this.relationships.Values.FirstOrDefault((RelationshipInfo r) => name.EndsWith(PathHelper.NormalizePath(r.Target))).Id;
		}

		public string GetRelationshipTarget(string id)
		{
			Guard.ThrowExceptionIfNull<string>(id, "id");
			RelationshipInfo relationshipInfo;
			if (this.relationships.TryGetValue(id, out relationshipInfo))
			{
				return relationshipInfo.Target;
			}
			return null;
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IOpenXmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			foreach (RelationshipInfo relationshipInfo in this.relationships.Values)
			{
				RelationshipElement relationshipElement = base.CreateElement<RelationshipElement>("Relationship");
				relationshipElement.SetRelationshipInfo(relationshipInfo);
				yield return relationshipElement;
			}
			yield break;
		}

		protected override void OnAfterReadChildElement(IOpenXmlImportContext context, OpenXmlElementBase element)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(element, "element");
			RelationshipInfo relationshipInfo = ((RelationshipElement)element).CreateRelationshipInfo();
			this.relationships[relationshipInfo.Id] = relationshipInfo;
		}

		protected override void ClearOverride()
		{
			this.relationships.Clear();
		}

		readonly Dictionary<string, RelationshipInfo> relationships;
	}
}
