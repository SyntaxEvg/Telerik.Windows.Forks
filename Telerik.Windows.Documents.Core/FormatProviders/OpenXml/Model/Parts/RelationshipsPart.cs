using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Relationships;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts
{
	class RelationshipsPart : OpenXmlPartBase
	{
		public RelationshipsPart(OpenXmlPartsManager partsManager, string partName)
			: base(partsManager, partName)
		{
			this.relationships = new RelationshipsElement(base.PartsManager, this);
		}

		public override string ContentType
		{
			get
			{
				throw new InvalidOperationException("This part don't have content type.");
			}
		}

		public override bool OverrideDefaultContentType
		{
			get
			{
				return false;
			}
		}

		public override int Level
		{
			get
			{
				return 1;
			}
		}

		public override OpenXmlElementBase RootElement
		{
			get
			{
				return this.relationships;
			}
		}

		public string CreateRelationship(string target, string type, string targetMode = null)
		{
			return this.relationships.CreateRelationship(target, type, targetMode);
		}

		public string GetRelationshipId(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			return this.relationships.GetRelationshipId(name);
		}

		public string GetRelationshipTarget(string id)
		{
			Guard.ThrowExceptionIfNull<string>(id, "id");
			return this.relationships.GetRelationshipTarget(id);
		}

		readonly RelationshipsElement relationships;
	}
}
