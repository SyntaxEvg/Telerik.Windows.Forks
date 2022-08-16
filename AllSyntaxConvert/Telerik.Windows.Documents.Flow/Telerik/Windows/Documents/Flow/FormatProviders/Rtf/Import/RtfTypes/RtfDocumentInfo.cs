using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes
{
	sealed class RtfDocumentInfo : RtfElementIteratorBase
	{
		public int? Id { get; set; }

		public int? Version { get; set; }

		public int? Revision { get; set; }

		public string Title { get; set; }

		public string Subject { get; set; }

		public string Author { get; set; }

		public string Manager { get; set; }

		public string Company { get; set; }

		public string Operator { get; set; }

		public string Category { get; set; }

		public string Keywords { get; set; }

		public string Comment { get; set; }

		public string DocumentComment { get; set; }

		public string HyperLinkbase { get; set; }

		public DateTime? CreationTime { get; set; }

		public DateTime? RevisionTime { get; set; }

		public DateTime? PrintTime { get; set; }

		public DateTime? BackupTime { get; set; }

		public int? NumberOfPages { get; set; }

		public int? NumberOfWords { get; set; }

		public int? NumberOfCharacters { get; set; }

		public int? EditingTimeInMinutes { get; set; }

		public void FillDocumentInfo(RtfGroup group)
		{
			Util.EnsureGroupDestination(group, "info");
			base.VisitGroupChildren(group, false);
		}

		protected override void DoVisitGroup(RtfGroup group)
		{
			string destination;
			switch (destination = group.Destination)
			{
			case "title":
				this.Title = RtfHelper.GetGroupText(group, true);
				return;
			case "subject":
				this.Subject = RtfHelper.GetGroupText(group, true);
				return;
			case "author":
				this.Author = RtfHelper.GetGroupText(group, true);
				return;
			case "manager":
				this.Manager = RtfHelper.GetGroupText(group, true);
				return;
			case "company":
				this.Company = RtfHelper.GetGroupText(group, true);
				return;
			case "operator":
				this.Operator = RtfHelper.GetGroupText(group, true);
				return;
			case "category":
				this.Category = RtfHelper.GetGroupText(group, true);
				return;
			case "keywords":
				this.Keywords = RtfHelper.GetGroupText(group, true);
				return;
			case "comment":
				this.Comment = RtfHelper.GetGroupText(group, true);
				return;
			case "doccomm":
				this.DocumentComment = RtfHelper.GetGroupText(group, true);
				return;
			case "hlinkbase":
				this.HyperLinkbase = RtfHelper.GetGroupText(group, true);
				return;
			case "creatim":
				this.CreationTime = new DateTime?(this.ExtractTimestamp(group));
				return;
			case "revtim":
				this.RevisionTime = new DateTime?(this.ExtractTimestamp(group));
				return;
			case "printim":
				this.PrintTime = new DateTime?(this.ExtractTimestamp(group));
				return;
			case "buptim":
				this.BackupTime = new DateTime?(this.ExtractTimestamp(group));
				return;
			}
			base.VisitGroupChildren(group, false);
		}

		protected override void DoVisitTag(RtfTag tag)
		{
			string name;
			switch (name = tag.Name)
			{
			case "version":
				this.Version = new int?(tag.ValueAsNumber);
				return;
			case "vern":
				this.Revision = new int?(tag.ValueAsNumber);
				return;
			case "nofpages":
				this.NumberOfPages = new int?(tag.ValueAsNumber);
				return;
			case "nofwords":
				this.NumberOfWords = new int?(tag.ValueAsNumber);
				return;
			case "nofchars":
				this.NumberOfCharacters = new int?(tag.ValueAsNumber);
				return;
			case "id":
				this.Id = new int?(tag.ValueAsNumber);
				return;
			case "edmins":
				this.EditingTimeInMinutes = new int?(tag.ValueAsNumber);
				break;

				return;
			}
		}

		DateTime ExtractTimestamp(RtfGroup group)
		{
			return this.timestampBuilder.CreateTimestamp(group);
		}

		readonly RtfTimestampBuilder timestampBuilder = new RtfTimestampBuilder();
	}
}
