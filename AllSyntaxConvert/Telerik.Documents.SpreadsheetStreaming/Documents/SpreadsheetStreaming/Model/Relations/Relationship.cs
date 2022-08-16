using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;

namespace Telerik.Documents.SpreadsheetStreaming.Model.Relations
{
	class Relationship
	{
		internal Relationship(string relationshipId, string type, string target)
			: this(-1, type, relationshipId)
		{
			this.Target = target;
		}

		internal Relationship(string name, int id, string relationshipId, string type)
			: this(id, type, relationshipId)
		{
			this.Name = name;
		}

		internal Relationship(string name, int id, int relationshipId, string type, string target)
			: this(id, type, OpenXmlHelper.CreateRelationshipId(relationshipId))
		{
			this.Name = name;
			this.Target = target;
		}

		Relationship(int id, string type, string relationshipId)
		{
			this.Id = id;
			this.Type = type;
			this.RelationshipId = relationshipId;
			this.Index = -1;
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public string RelationshipId { get; set; }

		public string Type { get; set; }

		public string Target { get; set; }

		public int Index { get; internal set; }

		public void MergeWith(Relationship relationship)
		{
			if (string.IsNullOrEmpty(this.Name))
			{
				this.Name = relationship.Name;
			}
			if (string.IsNullOrEmpty(this.RelationshipId))
			{
				this.RelationshipId = relationship.RelationshipId;
			}
			if (string.IsNullOrEmpty(this.Target))
			{
				this.Target = relationship.Target;
			}
			if (this.Id < 0)
			{
				this.Id = relationship.Id;
			}
			if (this.Index < 0)
			{
				this.Index = relationship.Index;
			}
		}
	}
}
