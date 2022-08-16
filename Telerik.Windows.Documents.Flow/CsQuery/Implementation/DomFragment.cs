using System;
using System.IO;
using System.Text;
using CsQuery.Engine;
using CsQuery.HtmlParser;

namespace CsQuery.Implementation
{
	class DomFragment : DomDocument, IDomFragment, IDomDocument, IDomContainer, IDomObject, IDomNode, ICloneable, IComparable<IDomObject>
	{
		public static IDomDocument Create(string html, string context = null, DocType docType = DocType.Default)
		{
			ElementFactory elementFactory = new ElementFactory();
			elementFactory.FragmentContext = context;
			elementFactory.HtmlParsingMode = HtmlParsingMode.Fragment;
			elementFactory.HtmlParsingOptions = HtmlParsingOptions.AllowSelfClosingTags;
			elementFactory.DocType = docType;
			Encoding utf = Encoding.UTF8;
			IDomDocument result;
			using (MemoryStream memoryStream = new MemoryStream(utf.GetBytes(html)))
			{
				result = elementFactory.Parse(memoryStream, utf);
			}
			return result;
		}

		public DomFragment()
		{
		}

		public DomFragment(IDomIndex domIndex)
			: base(domIndex)
		{
		}

		public override NodeType NodeType
		{
			get
			{
				return NodeType.DOCUMENT_FRAGMENT_NODE;
			}
		}

		public override bool IsIndexed
		{
			get
			{
				return true;
			}
		}

		public override bool IsFragment
		{
			get
			{
				return true;
			}
		}

		public override IDomDocument CreateNew()
		{
			return base.CreateNew<IDomFragment>();
		}
	}
}
