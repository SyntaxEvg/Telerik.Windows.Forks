using System;
using System.Text.RegularExpressions;

namespace CsQuery.Implementation
{
	class DomDocumentType : DomObject<DomDocumentType>, IDomDocumentType, IDomSpecialElement, IDomObject, IDomNode, ICloneable, IComparable<IDomObject>
	{
		public DomDocumentType()
		{
		}

		public DomDocumentType(DocType docType)
		{
			this.SetDocType(docType);
		}

		public DomDocumentType(string type, string access, string FPI, string URI)
		{
			this.SetDocType(type, access, FPI, URI);
		}

		string DocTypeName { get; set; }

		string Access { get; set; }

		string FPI { get; set; }

		string URI { get; set; }

		public override NodeType NodeType
		{
			get
			{
				return NodeType.DOCUMENT_TYPE_NODE;
			}
		}

		public override string NodeName
		{
			get
			{
				return "DOCTYPE";
			}
		}

		public DocType DocType
		{
			get
			{
				if (this._DocType == DocType.Default)
				{
					throw new InvalidOperationException("The doc type has not been set.");
				}
				return this._DocType;
			}
			protected set
			{
				this._DocType = value;
			}
		}

		public string NonAttributeData
		{
			get
			{
				return this.DocTypeName + ((!string.IsNullOrEmpty(this.Access)) ? (" " + this.Access) : "") + ((!string.IsNullOrEmpty(this.FPI)) ? (" \"" + this.FPI + "\"") : "") + ((!string.IsNullOrEmpty(this.URI)) ? (" \"" + this.URI + "\"") : "");
			}
			set
			{
				string type = "";
				string fpi = "";
				string access = "";
				string uri = "";
				MatchCollection matchCollection = DomDocumentType.DocTypeRegex.Matches(value);
				if (matchCollection.Count > 0)
				{
					type = matchCollection[0].Groups[1].Value;
					if (matchCollection[0].Groups.Count == 4)
					{
						Group group = matchCollection[0].Groups[3];
						access = group.Captures[0].Value;
						if (group.Captures.Count > 1)
						{
							fpi = group.Captures[1].Value;
							uri = group.Captures[2].Value;
						}
					}
				}
				this.SetDocType(type, access, fpi, uri);
			}
		}

		void SetDocType(string type, string access, string fpi, string uri)
		{
			this.DocTypeName = type.ToLower();
			this.Access = ((access == null) ? "" : access.ToUpper());
			this.FPI = fpi ?? "";
			this.URI = uri ?? "";
			if (this.DocTypeName == null || this.DocTypeName != "html")
			{
				this.DocType = DocType.Unknown;
				return;
			}
			if (fpi == "" && uri == "")
			{
				this.Access = "";
				this.DocType = DocType.HTML5;
				return;
			}
			if (this.FPI.IndexOf("html 4", StringComparison.CurrentCultureIgnoreCase) >= 0)
			{
				if (this.FPI.IndexOf("strict", StringComparison.CurrentCultureIgnoreCase) >= 0)
				{
					this.DocType = DocType.HTML4Strict;
					return;
				}
				this.DocType = DocType.HTML4;
				return;
			}
			else
			{
				if (this.FPI.IndexOf("xhtml", StringComparison.CurrentCultureIgnoreCase) < 0)
				{
					this.DocType = DocType.Unknown;
					return;
				}
				if (this.FPI.IndexOf("strict", StringComparison.CurrentCultureIgnoreCase) >= 0)
				{
					this.DocType = DocType.XHTMLStrict;
					return;
				}
				this.DocType = DocType.XHTML;
				return;
			}
		}

		void SetDocType(DocType type)
		{
			this._DocType = type;
			switch (type)
			{
			case DocType.HTML5:
			case DocType.Unknown:
				this.DocTypeName = "html";
				this.Access = null;
				this.FPI = null;
				this.URI = null;
				return;
			case DocType.HTML4:
				this.DocTypeName = "html";
				this.Access = "PUBLIC";
				this.FPI = "-//W3C//DTD HTML 4.01 Frameset//EN";
				this.URI = "http://www.w3.org/TR/html4/frameset.dtd";
				return;
			case DocType.XHTML:
				this.DocTypeName = "html";
				this.Access = "PUBLIC";
				this.FPI = "-//W3C//DTD XHTML 1.0 Frameset//EN";
				this.URI = "http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd";
				return;
			case DocType.HTML4Strict:
				this.DocTypeName = "html";
				this.Access = "PUBLIC";
				this.FPI = "-//W3C//DTD HTML 4.01//EN";
				this.URI = "http://www.w3.org/TR/html4/strict.dtd";
				return;
			case DocType.XHTMLStrict:
				this.DocTypeName = "html";
				this.Access = "PUBLIC";
				this.FPI = "-//W3C//DTD XHTML 1.0 Strict//EN";
				this.URI = "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd";
				return;
			default:
				throw new NotImplementedException("Unimplemented doctype");
			}
		}

		public override bool InnerHtmlAllowed
		{
			get
			{
				return false;
			}
		}

		public override bool HasChildren
		{
			get
			{
				return false;
			}
		}

		public override DomDocumentType Clone()
		{
			return new DomDocumentType
			{
				FPI = this.FPI,
				Access = this.Access,
				URI = this.URI,
				DocTypeName = this.DocTypeName,
				DocType = this.DocType
			};
		}

		IDomNode IDomNode.Clone()
		{
			return this.Clone();
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		static Regex DocTypeRegex = new Regex("^\\s*([a-zA-Z0-9]+)\\s+[a-zA-Z]+(\\s+\"(.*?)\")*\\s*$", RegexOptions.IgnoreCase);

		DocType _DocType;

		string _NonAttributeData = string.Empty;
	}
}
