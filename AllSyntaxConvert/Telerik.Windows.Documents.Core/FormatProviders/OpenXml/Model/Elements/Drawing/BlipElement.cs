using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing
{
	class BlipElement : DrawingElementBase
	{
		public BlipElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.relationshipId = base.RegisterAttribute<string>("embed", OpenXmlNamespaces.OfficeDocumentRelationshipsNamespace, true);
		}

		public override string ElementName
		{
			get
			{
				return "blip";
			}
		}

		public override IEnumerable<OpenXmlNamespace> Namespaces
		{
			get
			{
				yield return OpenXmlNamespaces.OfficeDocumentRelationshipsNamespace;
				yield break;
			}
		}

		public string RelationshipId
		{
			get
			{
				return this.relationshipId.Value;
			}
			set
			{
				this.relationshipId.Value = value;
			}
		}

		readonly OpenXmlAttribute<string> relationshipId;
	}
}
