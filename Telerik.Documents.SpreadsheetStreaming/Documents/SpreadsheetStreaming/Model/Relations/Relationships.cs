using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;

namespace Telerik.Documents.SpreadsheetStreaming.Model.Relations
{
	class Relationships : IEnumerable<Relationship>, IEnumerable
	{
		internal Relationships(bool import = false)
		{
			this.relationships = new Dictionary<string, Relationship>();
			if (!import)
			{
				this.AddTheme("theme1");
				this.AddStyles("styles");
			}
		}

		public IEnumerable<Relationship> GetRelationships(string type)
		{
			return from relationship in this
				where relationship.Type == type
				select relationship;
		}

		public void AddRelationship(Relationship relationship)
		{
			Relationship relationship2;
			if (this.relationships.TryGetValue(relationship.RelationshipId, out relationship2))
			{
				relationship2.MergeWith(relationship);
				return;
			}
			this.relationships.Add(relationship.RelationshipId, relationship);
		}

		public int AddWorksheet(string targetName)
		{
			string worksheetRelationshipType = XlsxRelationshipTypes.WorksheetRelationshipType;
			int nextTypeId = this.GetNextTypeId(worksheetRelationshipType);
			string target = string.Format(Relationships.WorksheetTargetFormat, nextTypeId);
			this.AddRelationship(targetName, worksheetRelationshipType, target);
			return nextTypeId;
		}

		public bool ContainsWorksheetName(string sheetName)
		{
			return this.Any((Relationship relationship) => relationship.Type == XlsxRelationshipTypes.WorksheetRelationshipType && relationship.Name == sheetName);
		}

		public IEnumerator<Relationship> GetEnumerator()
		{
			return this.relationships.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		void AddTheme(string targetName)
		{
			string target = string.Format(Relationships.ThemeTargetFormat, targetName);
			this.AddRelationship(targetName, XlsxRelationshipTypes.ThemeRelationshipType, target);
		}

		void AddStyles(string targetName)
		{
			string target = string.Format(Relationships.StylesTargetFormat, targetName);
			this.AddRelationship(targetName, XlsxRelationshipTypes.StylesRelationshipType, target);
		}

		void AddRelationship(string targetName, string type, string target)
		{
			int nextRelationshipId = this.GetNextRelationshipId();
			int nextTypeId = this.GetNextTypeId(type);
			Relationship relationship = new Relationship(targetName, nextTypeId, nextRelationshipId, type, target);
			if (relationship.Type == XlsxRelationshipTypes.WorksheetRelationshipType)
			{
				relationship.Index = nextTypeId;
			}
			this.relationships.Add(nextRelationshipId.ToString(), relationship);
		}

		int GetNextTypeId(string type)
		{
			return this.GetRelationships(type).Count<Relationship>() + 1;
		}

		int GetNextRelationshipId()
		{
			return this.relationships.Count + 1;
		}

		static readonly string ThemeTargetFormat = "theme/{0}.xml";

		static readonly string WorksheetTargetFormat = "worksheets/sheet{0}.xml";

		static readonly string StylesTargetFormat = "{0}.xml";

		readonly Dictionary<string, Relationship> relationships;
	}
}
