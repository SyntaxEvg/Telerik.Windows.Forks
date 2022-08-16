using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Comments
{
	class CommentElement : BlockLevelElementsContainerElementBase<BlockContainerBase>
	{
		public CommentElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.idAttribute = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("id", OpenXmlNamespaces.WordprocessingMLNamespace, false));
			this.authorAttribute = base.RegisterAttribute<string>("author", OpenXmlNamespaces.WordprocessingMLNamespace, false);
			this.initialsAttribute = base.RegisterAttribute<string>("initials", OpenXmlNamespaces.WordprocessingMLNamespace, false);
			this.dateAttribute = base.RegisterAttribute<string>("date", OpenXmlNamespaces.WordprocessingMLNamespace, false);
		}

		public override string ElementName
		{
			get
			{
				return "comment";
			}
		}

		public int Id
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

		public string Author
		{
			get
			{
				return this.authorAttribute.Value;
			}
			set
			{
				this.authorAttribute.Value = value;
			}
		}

		public string Initials
		{
			get
			{
				return this.initialsAttribute.Value;
			}
			set
			{
				this.initialsAttribute.Value = value;
			}
		}

		public string Date
		{
			get
			{
				return this.dateAttribute.Value;
			}
			set
			{
				this.dateAttribute.Value = value;
			}
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			Comment comment = (Comment)base.BlockContainer;
			comment.Author = this.Author;
			comment.Initials = this.Initials;
			DateTime date;
			if (DateTime.TryParse(this.Date, out date))
			{
				comment.Date = date;
			}
		}

		readonly IntOpenXmlAttribute idAttribute;

		readonly OpenXmlAttribute<string> authorAttribute;

		readonly OpenXmlAttribute<string> initialsAttribute;

		readonly OpenXmlAttribute<string> dateAttribute;
	}
}
