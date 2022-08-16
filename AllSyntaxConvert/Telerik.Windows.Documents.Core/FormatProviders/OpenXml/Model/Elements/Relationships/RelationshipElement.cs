using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Contexts;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Relationships
{
	class RelationshipElement : RelationshipElementBase
	{
		public RelationshipElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.idAttribute = base.RegisterAttribute<string>("Id", true);
			this.typeAttribute = base.RegisterAttribute<string>("Type", true);
			this.targetAttribute = base.RegisterAttribute<string>("Target", true);
			this.targetModeAttribute = base.RegisterAttribute<string>("TargetMode", string.Empty, false);
		}

		public string Target
		{
			get
			{
				return this.targetAttribute.Value;
			}
			set
			{
				this.targetAttribute.Value = value;
			}
		}

		public string Type
		{
			get
			{
				return this.typeAttribute.Value;
			}
			set
			{
				this.typeAttribute.Value = value;
			}
		}

		public string Id
		{
			get
			{
				return this.idAttribute.Value;
			}
			set
			{
				this.idAttribute.Value = value;
			}
		}

		public string TargetMode
		{
			get
			{
				return this.targetModeAttribute.Value;
			}
			set
			{
				this.targetModeAttribute.Value = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "Relationship";
			}
		}

		public void SetRelationshipInfo(RelationshipInfo relationshipInfo)
		{
			Guard.ThrowExceptionIfNull<RelationshipInfo>(relationshipInfo, "relationshipInfo");
			if (!string.IsNullOrEmpty(relationshipInfo.Target))
			{
				if (base.Part.Level == 1 && relationshipInfo.TargetMode != "External")
				{
					string relationshipRelativeTarget = OpenXmlPartsManager.GetRelationshipRelativeTarget(base.Part.Name, relationshipInfo.Target);
					this.Target = relationshipRelativeTarget;
				}
				else
				{
					this.Target = relationshipInfo.Target;
				}
			}
			if (!string.IsNullOrEmpty(relationshipInfo.TargetMode))
			{
				this.TargetMode = relationshipInfo.TargetMode;
			}
			this.Id = relationshipInfo.Id;
			this.Type = relationshipInfo.Type;
		}

		public RelationshipInfo CreateRelationshipInfo()
		{
			return new RelationshipInfo
			{
				Target = this.Target,
				Id = this.Id,
				TargetMode = this.TargetMode,
				Type = this.Type
			};
		}

		readonly OpenXmlAttribute<string> targetAttribute;

		readonly OpenXmlAttribute<string> typeAttribute;

		readonly OpenXmlAttribute<string> idAttribute;

		readonly OpenXmlAttribute<string> targetModeAttribute;
	}
}
