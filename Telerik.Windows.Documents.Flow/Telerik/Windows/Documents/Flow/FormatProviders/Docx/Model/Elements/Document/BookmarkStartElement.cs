using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Common;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class BookmarkStartElement : AnnotationStartEndElementBase
	{
		public BookmarkStartElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.nameAttribute = base.RegisterAttribute<string>("name", OpenXmlNamespaces.WordprocessingMLNamespace, false);
			this.colFirstAttribute = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("colFirst", OpenXmlNamespaces.WordprocessingMLNamespace, false));
			this.colLastAttribute = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("colLast", OpenXmlNamespaces.WordprocessingMLNamespace, false));
		}

		public override string ElementName
		{
			get
			{
				return "bookmarkStart";
			}
		}

		public string Name
		{
			get
			{
				return this.nameAttribute.Value;
			}
			set
			{
				this.nameAttribute.Value = value;
			}
		}

		public int ColFirst
		{
			get
			{
				return this.colFirstAttribute.Value;
			}
			set
			{
				this.colFirstAttribute.Value = value;
			}
		}

		public int ColLast
		{
			get
			{
				return this.colLastAttribute.Value;
			}
			set
			{
				this.colLastAttribute.Value = value;
			}
		}

		internal void CopyPropertiesFrom(Bookmark bookmark)
		{
			base.Id = AnnotationIdGenerator.GetNext();
			this.Name = bookmark.Name;
			if (bookmark.FromColumn != bookmark.ToColumn)
			{
				this.ColFirst = bookmark.FromColumn.Value;
				this.ColLast = bookmark.ToColumn.Value;
			}
		}

		readonly OpenXmlAttribute<string> nameAttribute;

		readonly IntOpenXmlAttribute colFirstAttribute;

		readonly IntOpenXmlAttribute colLastAttribute;
	}
}
