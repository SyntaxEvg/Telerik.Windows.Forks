using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using CsQuery.Engine;
using CsQuery.ExtensionMethods;
using CsQuery.ExtensionMethods.Internal;
using CsQuery.HtmlParser;
using CsQuery.Implementation;
using CsQuery.Output;
using CsQuery.StringScanner;
using CsQuery.Utility;

namespace CsQuery
{
	class CQ : IEnumerable<IDomObject>, IEnumerable
	{
		public int Length
		{
			get
			{
				return this.SelectionSet.Count;
			}
		}

		public IDomDocument Document
		{
			get
			{
				if (this._Document == null)
				{
					this.CreateNewFragment();
				}
				return this._Document;
			}
			protected set
			{
				this._Document = value;
			}
		}

		public Selector Selector
		{
			get
			{
				return this._Selector;
			}
			protected set
			{
				this._Selector = value;
			}
		}

		public IEnumerable<IDomObject> Selection
		{
			get
			{
				return this.SelectionSet;
			}
		}

		public IEnumerable<IDomElement> Elements
		{
			get
			{
				return CQ.OnlyElements(this.SelectionSet);
			}
		}

		public SelectionSetOrder Order
		{
			get
			{
				return this.SelectionSet.OutputOrder;
			}
			set
			{
				this.SelectionSet.OutputOrder = value;
			}
		}

		public override string ToString()
		{
			return this.SelectionHtml();
		}

		public IEnumerator<IDomObject> GetEnumerator()
		{
			return this.SelectionSet.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.SelectionSet.GetEnumerator();
		}

		protected CQ CsQueryParent
		{
			get
			{
				return this._CsQueryParent;
			}
			set
			{
				this._CsQueryParent = value;
				if (value != null && this._Document == null)
				{
					this.Document = value.Document;
				}
			}
		}

		protected SelectionSet<IDomObject> SelectionSet
		{
			get
			{
				if (this._Selection == null)
				{
					this._Selection = new SelectionSet<IDomObject>(SelectionSetOrder.OrderAdded);
				}
				return this._Selection;
			}
			set
			{
				this._Selection = value;
			}
		}

		protected void Clear()
		{
			this.CsQueryParent = null;
			this.Document = null;
			this.ClearSelections();
		}

		protected void ClearSelections()
		{
			this.SelectionSet.Clear();
		}

		protected CQ SetSelection(IEnumerable<IDomObject> selectionSet, SelectionSetOrder inputOrder = SelectionSetOrder.Ascending, SelectionSetOrder outputOrder = (SelectionSetOrder)0)
		{
			this.SelectionSet = new SelectionSet<IDomObject>(selectionSet, inputOrder, outputOrder);
			return this;
		}

		protected CQ SetSelection(IDomObject element, SelectionSetOrder outputOrder = SelectionSetOrder.Ascending)
		{
			this.SelectionSet = new SelectionSet<IDomObject>(Objects.Enumerate<IDomObject>(element), outputOrder, outputOrder);
			return this;
		}

		protected HashSet<string> MapMultipleValues(object value)
		{
			HashSet<string> hashSet = new HashSet<string>();
			if (value is string)
			{
				hashSet.AddRange(value.ToString().Split(new char[] { ',' }));
			}
			if (value is IEnumerable)
			{
				foreach (object obj in ((IEnumerable)value))
				{
					hashSet.Add(obj.ToString());
				}
			}
			if (hashSet.Count == 0 && value != null)
			{
				hashSet.Add(value.ToString());
			}
			return hashSet;
		}

		protected void SetOptionSelected(IEnumerable<IDomElement> elements, object value, bool multiple)
		{
			HashSet<string> values = this.MapMultipleValues(value);
			this.SetOptionSelected(elements, values, multiple);
		}

		protected void SetOptionSelected(IEnumerable<IDomElement> elements, HashSet<string> values, bool multiple)
		{
			bool flag = false;
			foreach (IDomElement domElement in elements)
			{
				string text = string.Empty;
				ushort nodeNameID = domElement.NodeNameID;
				switch (nodeNameID)
				{
				case 9:
				{
					string a;
					if ((a = domElement["type"]) != null && (a == "checkbox" || a == "radio"))
					{
						text = "checked";
					}
					break;
				}
				case 10:
					break;
				case 11:
					text = "selected";
					break;
				default:
					if (nodeNameID == 24)
					{
						this.SetOptionSelected(domElement.ChildElements, values, multiple);
					}
					break;
				}
				if (text != string.Empty && !flag && values.Contains(domElement["value"]))
				{
					domElement.SetAttribute(text);
					if (!multiple)
					{
						flag = true;
					}
				}
				else
				{
					domElement.RemoveAttribute(text);
				}
			}
		}

		protected bool AddSelection(IDomObject element)
		{
			return this.SelectionSet.Add(element);
		}

		protected bool AddSelection(IEnumerable<IDomObject> elements)
		{
			bool result = false;
			foreach (IDomObject element in elements)
			{
				result = true;
				this.AddSelection(element);
			}
			return result;
		}

		protected CQ MapRangeToNewCQ(IEnumerable<IDomObject> source, Func<IDomObject, IEnumerable<IDomObject>> del)
		{
			CQ cq = this.NewCqInDomain();
			foreach (IDomObject arg in source)
			{
				cq.SelectionSet.AddRange(del(arg));
			}
			return cq;
		}

		protected IEnumerable<IDomObject> MergeSelections(IEnumerable<string> selectors)
		{
			SelectionSet<IDomObject> allContent = new SelectionSet<IDomObject>(SelectionSetOrder.Ascending);
			CQ.Each<string>(selectors, delegate(string item)
			{
				allContent.AddRange(this.Select(item));
			});
			return allContent;
		}

		protected IEnumerable<IDomObject> MergeContent(IEnumerable<string> content)
		{
			List<IDomObject> list = new List<IDomObject>();
			foreach (string html in content)
			{
				list.AddRange(CQ.Create(html));
			}
			return list;
		}

		protected static IEnumerable<IDomElement> OnlyElements(IEnumerable<IDomObject> objects)
		{
			foreach (IDomObject item in objects)
			{
				IDomElement el = item as IDomElement;
				if (el != null)
				{
					yield return el;
				}
			}
			yield break;
		}

		protected CQ FilterIfSelector(string selector, IEnumerable<IDomObject> list)
		{
			return this.FilterIfSelector(selector, list, SelectionSetOrder.OrderAdded);
		}

		protected CQ FilterIfSelector(string selector, IEnumerable<IDomObject> list, SelectionSetOrder order)
		{
			CQ cq;
			if (string.IsNullOrEmpty(selector))
			{
				cq = this.NewInstance(list, this);
			}
			else
			{
				cq = this.NewInstance(this.FilterElements(list, selector), this);
			}
			cq.Order = order;
			return cq;
		}

		protected IEnumerable<IDomObject> FilterElements(IEnumerable<IDomObject> elements, string selector)
		{
			return this.FilterElementsIgnoreNull(elements, selector ?? "");
		}

		protected IEnumerable<IDomObject> FilterElementsIgnoreNull(IEnumerable<IDomObject> elements, string selector)
		{
			if (selector == "")
			{
				return Objects.EmptyEnumerable<IDomObject>();
			}
			if (selector == null)
			{
				return elements;
			}
			Selector selector2 = new Selector(selector).ToFilterSelector();
			return selector2.Filter(this.Document, elements);
		}

		public CQ AttrReplace(string name, string replaceWhat, string replaceWith)
		{
			foreach (IDomObject domObject in this.SelectionSet)
			{
				IDomElement domElement = (IDomElement)domObject;
				string text = domElement[name];
				if (text != null)
				{
					domElement[name] = text.Replace(replaceWhat, replaceWith);
				}
			}
			return this;
		}

		public static CQ Create()
		{
			return new CQ();
		}

		public static CQ Create(string html)
		{
			return new CQ(html, HtmlParsingMode.Auto, HtmlParsingOptions.Default, DocType.Default);
		}

		public static CQ Create(char[] html)
		{
			return new CQ(html.AsString(), HtmlParsingMode.Auto, HtmlParsingOptions.Default, DocType.Default);
		}

		public static CQ Create(IDomObject element)
		{
			CQ cq = new CQ();
			if (element is IDomDocument)
			{
				cq.Document = (IDomDocument)element;
				cq.AddSelection(cq.Document.ChildNodes);
			}
			else
			{
				cq.CreateNewFragment(Objects.Enumerate<IDomObject>(element));
			}
			return cq;
		}

		public static CQ Create(string html, HtmlParsingMode parsingMode = HtmlParsingMode.Auto, HtmlParsingOptions parsingOptions = HtmlParsingOptions.Default, DocType docType = DocType.Default)
		{
			return new CQ(html, parsingMode, parsingOptions, docType);
		}

		public static CQ Create(string html, object quickSet)
		{
			CQ cq = CQ.Create(html);
			return cq.AttrSet(quickSet, true);
		}

		public static CQ Create(IEnumerable<IDomObject> elements)
		{
			CQ cq = new CQ();
			cq.CreateNewFragment(elements);
			return cq;
		}

		public static CQ Create(Stream html)
		{
			return new CQ(html, null, HtmlParsingMode.Auto, HtmlParsingOptions.Default, DocType.Default);
		}

		public static CQ Create(Stream html, Encoding encoding)
		{
			return new CQ(html, encoding, HtmlParsingMode.Auto, HtmlParsingOptions.Default, DocType.Default);
		}

		public static CQ Create(TextReader html)
		{
			return new CQ(html, HtmlParsingMode.Auto, HtmlParsingOptions.Default, DocType.Default);
		}

		public static CQ Create(Stream html, Encoding encoding = null, HtmlParsingMode parsingMode = HtmlParsingMode.Auto, HtmlParsingOptions parsingOptions = HtmlParsingOptions.Default, DocType docType = DocType.Default)
		{
			return new CQ(html, encoding, parsingMode, parsingOptions, docType);
		}

		public static CQ Create(TextReader html, HtmlParsingMode parsingMode = HtmlParsingMode.Auto, HtmlParsingOptions parsingOptions = HtmlParsingOptions.Default, DocType docType = DocType.Default)
		{
			return new CQ(html, parsingMode, parsingOptions, docType);
		}

		public static CQ CreateFragment(string html)
		{
			return new CQ(html, HtmlParsingMode.Fragment, HtmlParsingOptions.AllowSelfClosingTags, DocType.Default);
		}

		public static CQ CreateFragment(string html, string context)
		{
			CQ cq = new CQ();
			cq.CreateNewFragment(cq, html, context, DocType.Default);
			return cq;
		}

		public static CQ CreateFragment(IEnumerable<IDomObject> elements)
		{
			return CQ.Create(elements);
		}

		public static CQ CreateDocument(string html)
		{
			return new CQ(html, HtmlParsingMode.Document, HtmlParsingOptions.Default, DocType.Default);
		}

		public static CQ CreateDocument(Stream html)
		{
			return new CQ(html, null, HtmlParsingMode.Document, HtmlParsingOptions.Default, DocType.Default);
		}

		public static CQ CreateDocument(Stream html, Encoding encoding)
		{
			return new CQ(html, encoding, HtmlParsingMode.Document, HtmlParsingOptions.Default, DocType.Default);
		}

		public static CQ CreateDocument(TextReader html)
		{
			return new CQ(html, HtmlParsingMode.Document, HtmlParsingOptions.Default, DocType.Default);
		}

		public static CQ CreateDocumentFromFile(string htmlFile)
		{
			CQ result;
			using (Stream fileStream = Support.GetFileStream(htmlFile))
			{
				result = CQ.CreateDocument(fileStream);
			}
			return result;
		}

		public static CQ CreateFromFile(string htmlFile)
		{
			CQ result;
			using (Stream fileStream = Support.GetFileStream(htmlFile))
			{
				result = CQ.Create(fileStream);
			}
			return result;
		}

		public CQ EnsureCsQuery(IEnumerable<IDomObject> elements)
		{
			if (elements is CQ)
			{
				return (CQ)elements;
			}
			CQ cq = this.NewCqUnbound();
			this.ConfigureNewInstance(cq, elements);
			return cq;
		}

		public IDomElement FirstElement()
		{
			IDomElement result;
			using (IEnumerator<IDomElement> enumerator = this.Elements.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					result = enumerator.Current;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		public CQ GetTableColumn()
		{
			CQ cq = this.Filter("th,td");
			CQ cq2 = this.NewCqInDomain();
			foreach (IDomObject domObject in cq)
			{
				CQ cq3 = domObject.Cq();
				int column = cq3.Index();
				cq2.AddSelection(cq3.Closest("table").GetTableColumn(column));
			}
			return cq2;
		}

		public CQ GetTableColumn(int column)
		{
			CQ cq = this.NewCqInDomain();
			foreach (IDomObject domObject in this.FilterElements(this, "table"))
			{
				cq.AddSelection(domObject.Cq().Find(string.Format("tr>th:eq({0}), tr>td:eq({0})", column)));
			}
			return cq;
		}

		public bool HasAttr(string name)
		{
			return this.Length > 0 && !string.IsNullOrEmpty(name) && this[0].HasAttribute(name);
		}

		public CQ IncludeWhen(bool include)
		{
			if (!include)
			{
				this.Remove(null);
			}
			return this;
		}

		public CQ KeepOne(bool which, string trueSelector, string falseSelector)
		{
			return this.KeepOne(which ? 0 : 1, new string[] { trueSelector, falseSelector });
		}

		public CQ KeepOne(bool which, CQ trueContent, CQ falseContent)
		{
			return this.KeepOne(which ? 0 : 1, new CQ[] { trueContent, falseContent });
		}

		public CQ KeepOne(int which, params string[] content)
		{
			CQ[] array = new CQ[content.Length];
			for (int i = 0; i < content.Length; i++)
			{
				array[i] = this.Select(content[i]);
			}
			return this.KeepOne(which, array);
		}

		public CQ KeepOne(int which, params CQ[] content)
		{
			for (int i = 0; i < content.Length; i++)
			{
				if (i == which)
				{
					content[i].Show();
				}
				else
				{
					content[i].Remove(null);
				}
			}
			return this;
		}

		public CQ MakeRoot()
		{
			this.Document.ChildNodes.Clear();
			this.Document.ChildNodes.AddRange(this.Elements);
			return this;
		}

		public CQ MakeRoot(string selector)
		{
			return this.Select(selector).MakeRoot();
		}

		public CQ NewCqInDomain()
		{
			CQ cq = this.NewCqUnbound();
			cq.CsQueryParent = this;
			return cq;
		}

		protected virtual CQ NewCqUnbound()
		{
			return new CQ();
		}

		public string RenderSelection()
		{
			return this.RenderSelection(OutputFormatters.Default);
		}

		public string RenderSelection(IOutputFormatter outputFormatter)
		{
			StringWriter stringWriter = new StringWriter();
			this.RenderSelection(outputFormatter, stringWriter);
			return stringWriter.ToString();
		}

		public void RenderSelection(IOutputFormatter outputFormatter, StringWriter writer)
		{
			foreach (IDomObject domObject in this)
			{
				domObject.Render(outputFormatter, writer);
			}
		}

		public string Render()
		{
			return this.Document.Render();
		}

		public string Render(DomRenderingOptions options)
		{
			return this.Document.Render(options);
		}

		public string Render(IOutputFormatter formatter)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringWriter writer = new StringWriter(stringBuilder);
			this.Render(formatter, writer);
			return stringBuilder.ToString();
		}

		public void Render(IOutputFormatter formatter, TextWriter writer)
		{
			foreach (IDomObject node in this.Document.ChildNodes)
			{
				formatter.Render(node, writer);
			}
		}

		[Obsolete]
		public void Render(StringBuilder sb, DomRenderingOptions options = DomRenderingOptions.Default)
		{
			this.Document.Render(sb, options);
		}

		public void Save(string fileName, DomRenderingOptions renderingOptions = DomRenderingOptions.Default)
		{
			File.WriteAllText(fileName, this.Render(renderingOptions));
		}

		public string SelectionHtml()
		{
			return this.SelectionHtml(false);
		}

		public string SelectionHtml(bool includeInner)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (IDomObject domObject in this)
			{
				stringBuilder.Append((stringBuilder.Length == 0) ? string.Empty : ", ");
				stringBuilder.Append(includeInner ? domObject.Render() : domObject.ToString());
			}
			return stringBuilder.ToString();
		}

		public CQ SetSelected(string groupName, IConvertible value)
		{
			CQ cq = this.Find("input[name='" + groupName + "']");
			CQ cq2 = cq.Filter("[value='" + value + "']");
			if (cq.Length == 0)
			{
				cq2 = this.Find("#" + groupName);
			}
			if (cq2.Length > 0)
			{
				ushort nodeNameID = cq[0].NodeNameID;
				string a = cq[0]["type"].ToUpper();
				if (nodeNameID == 11)
				{
					bool flag = cq.Closest("select").Prop("multiple");
					if (Objects.IsTruthy(flag))
					{
						cq2.Prop("selected", true);
					}
					else
					{
						cq.Prop("selected", false);
						cq2.Prop("selected", true);
					}
				}
				else if (nodeNameID == 9 && (a == "radio" || a == "checkbox"))
				{
					if (a == "radio")
					{
						cq.Prop("checked", false);
					}
					cq2.Prop("checked", true);
				}
			}
			return this;
		}

		public static string Version()
		{
			return typeof(CQ).Assembly.GetName().Version.ToString();
		}

		public CQ()
		{
		}

		public CQ(string html, HtmlParsingMode parsingMode = HtmlParsingMode.Auto, HtmlParsingOptions parsingOptions = HtmlParsingOptions.Default, DocType docType = DocType.Default)
		{
			UTF8Encoding encoding = new UTF8Encoding(false);
			using (Stream encodedStream = Support.GetEncodedStream(html ?? "", encoding))
			{
				this.CreateNew(this, encodedStream, encoding, parsingMode, parsingOptions, docType);
			}
		}

		public CQ(Stream html, Encoding encoding, HtmlParsingMode parsingMode = HtmlParsingMode.Auto, HtmlParsingOptions parsingOptions = HtmlParsingOptions.Default, DocType docType = DocType.Default)
		{
			this.CreateNew(this, html, encoding, parsingMode, parsingOptions, docType);
		}

		public CQ(TextReader html, HtmlParsingMode parsingMode = HtmlParsingMode.Auto, HtmlParsingOptions parsingOptions = HtmlParsingOptions.Default, DocType docType = DocType.Default)
		{
			Encoding utf = Encoding.UTF8;
			MemoryStream html2 = new MemoryStream(utf.GetBytes(html.ReadToEnd()));
			this.CreateNew(this, html2, utf, parsingMode, parsingOptions, docType);
		}

		public CQ(IDomObject element)
		{
			this.Document = element.Document;
			this.AddSelection(element);
		}

		public CQ(IEnumerable<IDomObject> elements)
		{
			this.ConfigureNewInstance(this, elements);
		}

		public CQ(IDomObject element, CQ context)
		{
			this.ConfigureNewInstance(this, element, context);
		}

		public CQ(string selector, CQ context)
		{
			this.ConfigureNewInstance(selector, context);
		}

		public CQ(string selector, string cssJson, CQ context)
		{
			this.ConfigureNewInstance(selector, context);
			this.AttrSet(cssJson);
		}

		public CQ(string selector, object css, CQ context)
		{
			this.ConfigureNewInstance(selector, context);
			this.AttrSet(css);
		}

		public CQ(IEnumerable<IDomObject> elements, CQ context)
		{
			this.ConfigureNewInstance(this, elements, context);
		}

		public static implicit operator CQ(string html)
		{
			return CQ.Create(html);
		}

		protected void CreateNewDocument()
		{
			this.Document = new DomDocument();
		}

		protected void CreateNewFragment()
		{
			this.Document = new DomFragment();
		}

		protected void CreateNewFragment(IEnumerable<IDomObject> elements)
		{
			this.Document = DomDocument.Create(elements.Clone(), HtmlParsingMode.Fragment, DocType.Default);
			this.AddSelection(this.Document.ChildNodes);
		}

		protected void CreateNew(CQ target, Stream html, Encoding encoding, HtmlParsingMode parsingMode, HtmlParsingOptions parsingOptions, DocType docType)
		{
			target.Document = DomDocument.Create(html, encoding, parsingMode, parsingOptions, docType);
			target.SetSelection(this.Document.ChildNodes.ToList<IDomObject>(), SelectionSetOrder.Ascending, (SelectionSetOrder)0);
		}

		protected void CreateNewFragment(CQ target, string html, string context, DocType docType)
		{
			target.Document = DomFragment.Create(html, context, docType);
			target.SetSelection(this.Document.ChildNodes.ToList<IDomObject>(), SelectionSetOrder.Ascending, (SelectionSetOrder)0);
		}

		CQ NewInstance(string html)
		{
			CQ cq = this.NewCqUnbound();
			Encoding utf = Encoding.UTF8;
			MemoryStream html2 = new MemoryStream(utf.GetBytes(html));
			this.CreateNew(cq, html2, utf, HtmlParsingMode.Auto, HtmlParsingOptions.Default, DocType.Default);
			return cq;
		}

		CQ NewInstance(IEnumerable<IDomObject> elements, CQ context)
		{
			CQ cq = this.NewCqUnbound();
			this.ConfigureNewInstance(cq, elements, context);
			return cq;
		}

		void ConfigureNewInstance(CQ dom, IEnumerable<IDomObject> elements, CQ context)
		{
			dom.CsQueryParent = context;
			dom.AddSelection(elements);
		}

		CQ NewInstance(IEnumerable<IDomObject> elements)
		{
			CQ cq = this.NewCqUnbound();
			this.ConfigureNewInstance(cq, elements);
			return cq;
		}

		void ConfigureNewInstance(CQ dom, IEnumerable<IDomObject> elements)
		{
			List<IDomObject> list = elements.ToList<IDomObject>();
			if (elements is CQ)
			{
				CQ cq = (CQ)elements;
				dom.CsQueryParent = cq;
				dom.Document = cq.Document;
			}
			else
			{
				IDomObject domObject = list.FirstOrDefault<IDomObject>();
				if (domObject != null)
				{
					dom.Document = domObject.Document;
				}
			}
			dom.SetSelection(list, SelectionSetOrder.OrderAdded, (SelectionSetOrder)0);
		}

		void ConfigureNewInstance(string selector, CQ context)
		{
			this.CsQueryParent = context;
			if (!string.IsNullOrEmpty(selector))
			{
				this.Selector = new Selector(selector);
				this.SetSelection(this.Selector.ToContextSelector().Select(this.Document, context), this.Selector.IsHtml ? SelectionSetOrder.OrderAdded : SelectionSetOrder.Ascending, (SelectionSetOrder)0);
			}
		}

		CQ NewInstance(IDomObject element, CQ context)
		{
			CQ cq = this.NewCqUnbound();
			this.ConfigureNewInstance(cq, element, context);
			return cq;
		}

		void ConfigureNewInstance(CQ dom, IDomObject element, CQ context)
		{
			dom.CsQueryParent = context;
			dom.SetSelection(element, SelectionSetOrder.OrderAdded);
		}

		[Obsolete]
		public static DomRenderingOptions DefaultDomRenderingOptions
		{
			get
			{
				return Config.DomRenderingOptions;
			}
			set
			{
				Config.DomRenderingOptions = value;
			}
		}

		[Obsolete]
		public static DocType DefaultDocType
		{
			get
			{
				return Config.DocType;
			}
			set
			{
				Config.DocType = value;
			}
		}

		public static JsObject ToExpando(object obj)
		{
			if (obj is IDictionary<string, object>)
			{
				return Objects.Dict2Dynamic<JsObject>((IDictionary<string, object>)obj);
			}
			return Objects.ToExpando(obj);
		}

		public static T ToDynamic<T>(object obj) where T : IDynamicMetaObjectProvider, IDictionary<string, object>, new()
		{
			if (obj is IDictionary<string, object>)
			{
				return Objects.Dict2Dynamic<T>((IDictionary<string, object>)obj);
			}
			return Objects.ToExpando<T>(obj);
		}

		public CQ Add(string selector)
		{
			return this.Add(this.Select(selector));
		}

		public CQ Add(IDomObject element)
		{
			return this.Add(Objects.Enumerate<IDomObject>(element));
		}

		public CQ Add(IEnumerable<IDomObject> elements)
		{
			CQ cq = this.NewInstance(this);
			cq.AddSelection(elements);
			return cq;
		}

		public CQ Add(string selector, IEnumerable<IDomObject> context)
		{
			return this.Add(this.Select(selector, context));
		}

		public CQ Add(string selector, IDomObject context)
		{
			return this.Add(this.Select(selector, context));
		}

		public CQ After(string selector)
		{
			return this.After(this.Select(selector));
		}

		public CQ After(IDomObject element)
		{
			return this.After(Objects.Enumerate<IDomObject>(element));
		}

		public CQ After(IEnumerable<IDomObject> elements)
		{
			return this.EnsureCsQuery(elements).InsertAtOffset(this, 1);
		}

		protected CQ InsertAtOffset(IEnumerable<IDomObject> target, int offset)
		{
			CQ cq;
			return this.InsertAtOffset(this.EnsureCsQuery(target), offset, out cq);
		}

		protected CQ InsertAtOffset(CQ target, int offset, out CQ insertedElements)
		{
			List<IDomObject> list = new List<IDomObject>(target);
			bool flag;
			if (target.CsQueryParent == null || target.Document == target.CsQueryParent.Document)
			{
				flag = list.Any((IDomObject item) => item.ParentNode == null);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			bool flag3 = true;
			insertedElements = this.NewCqUnbound();
			this.Document = target.Document;
			if (flag2)
			{
				CQ cq = this.NewCqInDomain();
				cq.CsQueryParent = this.CsQueryParent;
				if (offset == 0)
				{
					cq.AddSelection(this.Selection);
				}
				cq.AddSelection(target);
				if (offset == 1)
				{
					cq.AddSelection(this.Selection);
				}
				cq.SelectionSet.OutputOrder = SelectionSetOrder.OrderAdded;
				insertedElements.AddSelection(this.Selection);
				return cq;
			}
			foreach (IDomObject target2 in list)
			{
				if (flag3)
				{
					insertedElements.AddSelection(this.SelectionSet);
					this.InsertAtOffset(target2, offset);
					flag3 = false;
				}
				else
				{
					CQ cq2 = this.Clone();
					cq2.InsertAtOffset(target2, offset);
					insertedElements.AddSelection(cq2);
				}
			}
			return target;
		}

		public CQ AndSelf()
		{
			CQ cq = this.NewInstance(this);
			cq.Order = SelectionSetOrder.Ascending;
			if (this.CsQueryParent == null)
			{
				return cq;
			}
			cq.SelectionSet.AddRange(this.CsQueryParent.SelectionSet);
			return cq;
		}

		public CQ Append(params string[] content)
		{
			return this.Append(this.MergeContent(content));
		}

		public CQ Append(IDomObject element)
		{
			return this.Append(Objects.Enumerate<IDomObject>(element));
		}

		public CQ Append(IEnumerable<IDomObject> elements)
		{
			CQ cq;
			return this.Append(elements, out cq);
		}

		public CQ Append(Func<int, string, string> func)
		{
			int num = 0;
			foreach (IDomElement domElement in this.Elements)
			{
				DomElement domElement2 = (DomElement)domElement;
				string text = func(num, domElement2.InnerHTML);
				domElement2.Cq().Append(new string[] { text });
				num++;
			}
			return this;
		}

		public CQ Append(Func<int, string, IDomElement> func)
		{
			int num = 0;
			foreach (IDomElement domElement in this.Elements)
			{
				IDomElement element = func(num, domElement.InnerHTML);
				domElement.Cq().Append(element);
				num++;
			}
			return this;
		}

		public CQ Append(Func<int, string, IEnumerable<IDomElement>> func)
		{
			int num = 0;
			foreach (IDomElement domElement in this.Elements)
			{
				IEnumerable<IDomElement> elements = func(num, domElement.InnerHTML);
				domElement.Cq().Append(elements);
				num++;
			}
			return this;
		}

		CQ Append(IEnumerable<IDomObject> elements, out CQ insertedElements)
		{
			insertedElements = this.NewCqInDomain();
			bool flag = true;
			List<IDomObject> list = new List<IDomObject>(elements);
			foreach (IDomElement target in this.Elements)
			{
				IDomElement trueTarget = this.GetTrueTarget(target);
				foreach (IDomObject domObject in list)
				{
					IDomObject domObject2 = (flag ? domObject : domObject.Clone());
					trueTarget.AppendChild(domObject2);
					insertedElements.SelectionSet.Add(domObject2);
				}
				flag = false;
			}
			return this;
		}

		IDomElement GetTrueTarget(IDomElement target)
		{
			IDomElement domElement = target;
			if (target.NodeNameID == 23)
			{
				bool flag = false;
				if (target.HasChildren)
				{
					IDomElement domElement2 = target.ChildElements.FirstOrDefault((IDomElement item) => item.NodeNameID == 27);
					if (domElement2 != null)
					{
						domElement = domElement2;
					}
					else if (target.FirstElementChild == null)
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					domElement = this.Document.CreateElement("tbody");
					target.AppendChild(domElement);
				}
			}
			return domElement;
		}

		public CQ AppendTo(params string[] target)
		{
			CQ result;
			this.NewInstance(this.MergeSelections(target)).Append(this.SelectionSet, out result);
			return result;
		}

		public CQ AppendTo(IDomObject target)
		{
			return this.AppendTo(Objects.Enumerate<IDomObject>(target));
		}

		public CQ AppendTo(IEnumerable<IDomObject> targets)
		{
			CQ result;
			this.EnsureCsQuery(targets).Append(this.SelectionSet, out result);
			return result;
		}

		public string Attr(string name)
		{
			if (this.Length > 0 && !string.IsNullOrEmpty(name))
			{
				name = name.ToLower();
				IDomObject domObject = this[0];
				string a;
				if ((a = name) != null)
				{
					if (a == "class")
					{
						return domObject.ClassName;
					}
					if (a == "style")
					{
						return domObject.Style.ToString();
					}
				}
				string result;
				if (domObject.TryGetAttribute(name, out result))
				{
					if (HtmlData.IsBoolean(name))
					{
						return name;
					}
					return result;
				}
				else
				{
					if (name == "value" && (domObject.NodeName == "INPUT" || domObject.NodeName == "SELECT" || domObject.NodeName == "OPTION"))
					{
						return this.Val();
					}
					if (name == "value" && domObject.NodeName == "TEXTAREA")
					{
						return domObject.TextContent;
					}
				}
			}
			return null;
		}

		public T Attr<T>(string name)
		{
			string value;
			if (this.Length > 0 && this[0].TryGetAttribute(name, out value))
			{
				return (T)((object)Convert.ChangeType(value, typeof(T)));
			}
			return default(T);
		}

		public CQ Attr(string name, IConvertible value)
		{
			if (Objects.IsJson(name) && value.GetType() == typeof(bool))
			{
				return this.AttrSet(name, (bool)value);
			}
			bool flag = HtmlData.IsBoolean(name);
			if (flag)
			{
				if (value is string && (string)value == string.Empty)
				{
					value = true;
				}
				this.SetProp(name, value);
				return this;
			}
			string value2;
			if (value is bool)
			{
				value2 = value.ToString().ToLower();
			}
			else
			{
				value2 = this.GetValueString(value);
			}
			foreach (IDomElement domElement in this.Elements)
			{
				if ((domElement.NodeNameID == 9 || domElement.NodeNameID == 37) && name == "type" && !domElement.IsFragment)
				{
					throw new InvalidOperationException("Can't change type of \"input\" elements that have already been added to a DOM");
				}
				domElement.SetAttribute(name, value2);
			}
			return this;
		}

		public CQ AttrSet(object map)
		{
			return this.AttrSet(map, false);
		}

		public CQ AttrSet(object map, bool quickSet = false)
		{
			IDictionary<string, object> dictionary = Objects.ToExpando(map);
			foreach (IDomElement domElement in this.Elements)
			{
				foreach (KeyValuePair<string, object> keyValuePair in dictionary)
				{
					if (quickSet)
					{
						string text = keyValuePair.Key.ToLower();
						string a;
						if ((a = text) != null)
						{
							if (a == "css")
							{
								this.Select(domElement).CssSet(Objects.ToExpando(keyValuePair.Value));
								continue;
							}
							if (a == "html")
							{
								this.Select(domElement).Html(new string[] { keyValuePair.Value.ToString() });
								continue;
							}
							if (a == "height" || a == "width")
							{
								this.Select(domElement).Css(text, keyValuePair.Value.ToString());
								continue;
							}
							if (a == "text")
							{
								this.Select(domElement).Text(keyValuePair.Value.ToString());
								continue;
							}
						}
						domElement.SetAttribute(keyValuePair.Key, keyValuePair.Value.ToString());
					}
					else
					{
						domElement.SetAttribute(keyValuePair.Key, keyValuePair.Value.ToString());
					}
				}
			}
			return this;
		}

		public CQ RemoveAttr(string name)
		{
			foreach (IDomElement domElement in this.Elements)
			{
				if (name != null)
				{
					if (name == "class")
					{
						domElement.ClassName = "";
						continue;
					}
					if (name == "style")
					{
						domElement.Style = null;
						continue;
					}
				}
				domElement.RemoveAttribute(name);
			}
			return this;
		}

		public CQ RemoveProp(string name)
		{
			return this.RemoveAttr(name);
		}

		public CQ Before(string selector)
		{
			return this.Before(this.Select(selector));
		}

		public CQ Before(IDomObject element)
		{
			return this.Before(Objects.Enumerate<IDomObject>(element));
		}

		public CQ Before(IEnumerable<IDomObject> elements)
		{
			return this.EnsureCsQuery(elements).InsertAtOffset(this, 0);
		}

		public CQ Children(string filter = null)
		{
			return this.FilterIfSelector(filter, this.SelectionChildren());
		}

		protected IEnumerable<IDomObject> SelectionChildren()
		{
			foreach (IDomObject obj in this.Elements)
			{
				foreach (IDomObject child in obj.ChildElements)
				{
					yield return child;
				}
			}
			yield break;
		}

		public CQ AddClass(string className)
		{
			foreach (IDomElement domElement in this.Elements)
			{
				domElement.AddClass(className);
			}
			return this;
		}

		public CQ ToggleClass(string classes)
		{
			IEnumerable<string> enumerable = classes.SplitClean(' ');
			foreach (IDomElement domElement in this.Elements)
			{
				foreach (string className in enumerable)
				{
					if (domElement.HasClass(className))
					{
						domElement.RemoveClass(className);
					}
					else
					{
						domElement.AddClass(className);
					}
				}
			}
			return this;
		}

		public CQ ToggleClass(string classes, bool addRemoveSwitch)
		{
			IEnumerable<string> enumerable = classes.SplitClean(' ');
			foreach (IDomElement domElement in this.Elements)
			{
				foreach (string className in enumerable)
				{
					if (addRemoveSwitch)
					{
						domElement.AddClass(className);
					}
					else
					{
						domElement.RemoveClass(className);
					}
				}
			}
			return this;
		}

		public bool HasClass(string className)
		{
			IDomElement domElement = this.FirstElement();
			return domElement != null && domElement.HasClass(className);
		}

		public CQ Clone()
		{
			CQ cq = this.NewCqInDomain();
			cq.Document = this.Document.CreateNew();
			foreach (IDomObject domObject in this.SelectionSet)
			{
				cq.Document.ChildNodes.Add(domObject.Clone());
			}
			cq.SelectionSet = new SelectionSet<IDomObject>(cq.Document.ChildNodes.ToList<IDomObject>(), this.Order, this.Order);
			return cq;
		}

		public CQ Closest(string selector)
		{
			CQ elements = this.Select(selector);
			return this.Closest(elements);
		}

		public CQ Closest(IDomObject element)
		{
			return this.Closest(Objects.Enumerate<IDomObject>(element));
		}

		public CQ Closest(IEnumerable<IDomObject> elements)
		{
			HashSet<IDomObject> hashSet = new HashSet<IDomObject>(elements);
			CQ cq = this.NewCqInDomain();
			foreach (IDomObject domObject in this.SelectionSet)
			{
				IDomObject domObject2 = domObject;
				while (domObject2 != null)
				{
					if (hashSet.Contains(domObject2))
					{
						cq.AddSelection(domObject2);
						domObject2 = null;
					}
					else
					{
						domObject2 = domObject2.ParentNode;
					}
				}
			}
			return cq;
		}

		public CQ Contents()
		{
			List<IDomObject> list = new List<IDomObject>();
			foreach (IDomObject domObject in this.SelectionSet)
			{
				if (domObject is IDomContainer)
				{
					list.AddRange(domObject.ChildNodes);
				}
			}
			return this.NewInstance(list, this);
		}

		public CQ CssSet(object map)
		{
			IDictionary<string, object> dictionary = Objects.ToExpando(map);
			foreach (IDomElement domElement in this.Elements)
			{
				foreach (KeyValuePair<string, object> keyValuePair in dictionary)
				{
					domElement.Style[keyValuePair.Key] = this.StringOrNull(keyValuePair.Value);
				}
			}
			return this;
		}

		public CQ Css(string name, IConvertible value)
		{
			foreach (IDomElement domElement in this.Elements)
			{
				domElement.Style[name] = this.StringOrNull(value);
			}
			return this;
		}

		public T Css<T>(string style) where T : IConvertible
		{
			IDomElement domElement = this.FirstElement();
			if (domElement == null)
			{
				return default(T);
			}
			if (!Objects.IsNumericType(typeof(T)))
			{
				return (T)((object)Objects.ChangeType(domElement.Style[style] ?? "", typeof(T)));
			}
			IStringScanner stringScanner = Scanner.Create(domElement.Style[style] ?? "");
			T result;
			if (stringScanner.TryGetNumber<T>(out result))
			{
				return result;
			}
			return default(T);
		}

		public string Css(string style)
		{
			IDomElement domElement = this.FirstElement();
			string text = null;
			if (domElement != null)
			{
				text = domElement.Style[style];
				if (style != null)
				{
					if (!(style == "display"))
					{
						if (style == "opacity")
						{
							if (string.IsNullOrEmpty(text))
							{
								text = "1";
							}
						}
					}
					else if (string.IsNullOrEmpty(text))
					{
						text = (domElement.IsBlock ? "block" : "inline");
					}
				}
			}
			return text;
		}

		string StringOrNull(object value)
		{
			if (value == null)
			{
				return null;
			}
			string text = value.ToString();
			if (!(text == ""))
			{
				return text;
			}
			return null;
		}

		public CQ Data(string key, string data)
		{
			foreach (IDomElement domElement in this.Elements)
			{
				domElement.SetAttribute("data-" + key, data);
			}
			return this;
		}

		public global::CsQuery.CQ RemoveData()
		{
			string key = null;
			return this.RemoveData(key);
		}

		public CQ RemoveData(string key)
		{
			foreach (IDomElement domElement in this.Elements)
			{
				List<string> list = new List<string>();
				foreach (KeyValuePair<string, string> keyValuePair in domElement.Attributes)
				{
					bool flag = (string.IsNullOrEmpty(key) ? keyValuePair.Key.StartsWith("data-") : (keyValuePair.Key == "data-" + key));
					if (flag)
					{
						list.Add(keyValuePair.Key);
					}
				}
				foreach (string name in list)
				{
					domElement.RemoveAttribute(name);
				}
			}
			return this;
		}

		public CQ RemoveData(IEnumerable<string> keys)
		{
			foreach (string key in keys)
			{
				this.RemoveData(key);
			}
			return this;
		}

		public string DataRaw(string key)
		{
			return this.First().Attr("data-" + key);
		}

		public bool HasData()
		{
			foreach (IDomElement domElement in this.Elements)
			{
				foreach (KeyValuePair<string, string> keyValuePair in domElement.Attributes)
				{
					if (keyValuePair.Key.StartsWith("data-"))
					{
						return true;
					}
				}
			}
			return false;
		}

		public CQ EachUntil(Func<int, IDomObject, bool> func)
		{
			int num = 0;
			foreach (IDomObject arg in this.Selection)
			{
				if (!func(num++, arg))
				{
					break;
				}
			}
			return this;
		}

		public CQ EachUntil(Func<IDomObject, bool> func)
		{
			foreach (IDomObject arg in this.Selection)
			{
				if (!func(arg))
				{
					break;
				}
			}
			return this;
		}

		public CQ Each(Action<IDomObject> func)
		{
			foreach (IDomObject obj in this.Selection)
			{
				func(obj);
			}
			return this;
		}

		public CQ Each(Action<int, IDomObject> func)
		{
			int num = 0;
			foreach (IDomObject arg in this.Selection)
			{
				func(num++, arg);
			}
			return this;
		}

		public static void Each<T>(IEnumerable<T> list, Action<T> func)
		{
			foreach (T obj in list)
			{
				func(obj);
			}
		}

		public CQ Empty()
		{
			return this.Each(delegate(IDomObject e)
			{
				if (e.HasChildren)
				{
					e.ChildNodes.Clear();
				}
			});
		}

		public CQ End()
		{
			return this.CsQueryParent ?? this.NewCqInDomain();
		}

		public CQ Eq(int index)
		{
			if (index < 0)
			{
				index = this.Length + index - 1;
			}
			if (index >= 0 && index < this.Length)
			{
				return this.NewInstance(this.SelectionSet[index], this);
			}
			return this.NewCqInDomain();
		}

		public static object Extend(object target, params object[] sources)
		{
			return Objects.Extend(false, target, sources);
		}

		public static object Extend(bool deep, object target, params object[] sources)
		{
			return Objects.Extend(deep, target, sources);
		}

		public CQ Filter(string selector)
		{
			return this.NewInstance(this.FilterElements(this.SelectionSet, selector));
		}

		public CQ Filter(IDomObject element)
		{
			return this.Filter(Objects.Enumerate<IDomObject>(element));
		}

		public CQ Filter(IEnumerable<IDomObject> elements)
		{
			CQ cq = this.NewInstance(this);
			cq.SelectionSet.IntersectWith(elements);
			return cq;
		}

		public CQ Filter(Func<IDomObject, bool> function)
		{
			CQ cq = this.NewCqInDomain();
			List<IDomObject> list = new List<IDomObject>();
			foreach (IDomObject domObject in this.SelectionSet)
			{
				if (function(domObject))
				{
					list.Add(domObject);
				}
			}
			return cq.SetSelection(list, this.Order, (SelectionSetOrder)0);
		}

		public CQ Filter(Func<IDomObject, int, bool> function)
		{
			CQ cq = this.NewCqInDomain();
			List<IDomObject> list = new List<IDomObject>();
			int num = 0;
			foreach (IDomObject domObject in this.SelectionSet)
			{
				if (function(domObject, num++))
				{
					list.Add(domObject);
				}
			}
			return cq.SetSelection(list, this.Order, (SelectionSetOrder)0);
		}

		public CQ Find(string selector)
		{
			return this.FindImpl(new Selector(selector));
		}

		public CQ Find(IEnumerable<IDomObject> elements)
		{
			return this.FindImpl(new Selector(elements));
		}

		public CQ Find(IDomObject element)
		{
			return this.FindImpl(new Selector(element));
		}

		CQ FindImpl(Selector selector)
		{
			CQ cq = this.NewCqInDomain();
			IList<IDomObject> elements = selector.ToContextSelector().Select(this.Document, this);
			cq.AddSelection(elements);
			cq.Selector = selector;
			return cq;
		}

		public CQ First()
		{
			return this.Eq(0);
		}

		public CQ Last()
		{
			if (this.SelectionSet.Count == 0)
			{
				return this.NewCqInDomain();
			}
			return this.Eq(this.SelectionSet.Count - 1);
		}

		public IEnumerable<IDomObject> Get()
		{
			return this.SelectionSet;
		}

		public IDomObject Get(int index)
		{
			int num = ((index < 0) ? (this.SelectionSet.Count + index - 1) : index);
			if (num < 0 || num >= this.SelectionSet.Count)
			{
				return null;
			}
			return this.SelectionSet.ElementAt(num);
		}

		public CQ Has(string selector)
		{
			CQ cq = this.NewCqInDomain();
			foreach (IDomObject domObject in this.SelectionSet)
			{
				if (this.Select(domObject).Find(selector).Length > 0)
				{
					cq.SelectionSet.Add(domObject);
				}
			}
			return cq;
		}

		public CQ Has(IDomObject element)
		{
			return this.Has(Objects.Enumerate<IDomObject>(element));
		}

		public CQ Has(IEnumerable<IDomObject> elements)
		{
			CQ cq = this.NewCqInDomain();
			foreach (IDomObject domObject in this.SelectionSet)
			{
				if (domObject.Cq().Find(elements).Length > 0)
				{
					cq.SelectionSet.Add(domObject);
				}
			}
			return cq;
		}

		public string Html()
		{
			if (this.Length <= 0)
			{
				return string.Empty;
			}
			return this[0].InnerHTML;
		}

		public CQ Html(params string[] html)
		{
			CQ cq = this.EnsureCsQuery(this.MergeContent(html));
			bool flag = true;
			foreach (IDomElement domElement in CQ.OnlyElements(this.SelectionSet))
			{
				DomElement domElement2 = (DomElement)domElement;
				if (domElement2.InnerHtmlAllowed)
				{
					domElement2.ChildNodes.Clear();
					domElement2.ChildNodes.AddRange(flag ? cq : cq.Clone());
					flag = false;
				}
			}
			return this;
		}

		public int Index()
		{
			IDomObject domObject = this.SelectionSet.FirstOrDefault<IDomObject>();
			if (domObject != null)
			{
				return this.GetElementIndex(domObject);
			}
			return -1;
		}

		public int Index(string selector)
		{
			CQ cq = this.Select(selector);
			return cq.Index(this.SelectionSet);
		}

		public int Index(IDomObject element)
		{
			int result = -1;
			if (element != null)
			{
				int num = 0;
				foreach (IDomObject objA in this.SelectionSet)
				{
					if (object.ReferenceEquals(objA, element))
					{
						result = num;
						break;
					}
					num++;
				}
			}
			return result;
		}

		public int Index(IEnumerable<IDomObject> elements)
		{
			return this.Index(elements.FirstOrDefault<IDomObject>());
		}

		protected int GetElementIndex(IDomObject element)
		{
			int num = 0;
			IDomContainer parentNode = element.ParentNode;
			if (parentNode == null)
			{
				num = -1;
			}
			else
			{
				foreach (IDomElement objA in parentNode.ChildElements)
				{
					if (object.ReferenceEquals(objA, element))
					{
						break;
					}
					num++;
				}
			}
			return num;
		}

		public CQ InsertAfter(IDomObject target)
		{
			return this.InsertAtOffset(target, 1);
		}

		public CQ InsertAfter(IEnumerable<IDomObject> target)
		{
			CQ result;
			this.InsertAtOffset(this.EnsureCsQuery(target), 1, out result);
			return result;
		}

		public CQ InsertAfter(string selectorTarget)
		{
			return this.InsertAfter(this.Select(selectorTarget));
		}

		public CQ InsertBefore(string selector)
		{
			return this.InsertBefore(this.Select(selector));
		}

		public CQ InsertBefore(IDomObject target)
		{
			return this.InsertAtOffset(target, 0);
		}

		public CQ InsertBefore(IEnumerable<IDomObject> target)
		{
			CQ result;
			this.InsertAtOffset(this.EnsureCsQuery(target), 0, out result);
			return result;
		}

		CQ InsertAtOffset(IDomObject target, int offset)
		{
			int num = target.Index;
			List<IDomObject> list = this.SelectionSet.ToList<IDomObject>();
			foreach (IDomObject item in list)
			{
				target.ParentNode.ChildNodes.Insert(num + offset, item);
				num++;
			}
			return this;
		}

		public bool Is(string selector)
		{
			return this.Filter(selector).Length > 0;
		}

		public bool Is(IEnumerable<IDomObject> elements)
		{
			HashSet<IDomObject> hashSet = new HashSet<IDomObject>(elements);
			hashSet.IntersectWith(this.SelectionSet);
			return hashSet.Count > 0;
		}

		public bool Is(IDomObject element)
		{
			return this.SelectionSet.Contains(element);
		}

		public static IEnumerable<T> Map<T>(IEnumerable<IDomObject> elements, Func<IDomObject, T> function)
		{
			foreach (IDomObject element in elements)
			{
				T result = function(element);
				if (result != null)
				{
					yield return result;
				}
			}
			yield break;
		}

		public IEnumerable<T> Map<T>(Func<IDomObject, T> function)
		{
			return CQ.Map<T>(this, function);
		}

		public CQ Prev(string selector = null)
		{
			return this.nextPrevImpl(selector, false);
		}

		public CQ Next(string selector = null)
		{
			return this.nextPrevImpl(selector, true);
		}

		public CQ NextAll(string filter = null)
		{
			return this.nextPrevAllImpl(filter, true);
		}

		public CQ NextUntil(string selector = null, string filter = null)
		{
			return this.nextPrevUntilImpl(selector, filter, true);
		}

		public CQ PrevAll(string filter = null)
		{
			return this.nextPrevAllImpl(filter, false);
		}

		public CQ PrevUntil(string selector = null, string filter = null)
		{
			return this.nextPrevUntilImpl(selector, filter, false);
		}

		CQ nextPrevImpl(string selector, bool next)
		{
			IEnumerable<IDomElement> list;
			SelectionSetOrder order;
			if (next)
			{
				list = from item in this.Elements
					select item.NextElementSibling into item
					where item != null
					select item;
				order = SelectionSetOrder.Ascending;
			}
			else
			{
				list = from item in this.Elements
					select item.PreviousElementSibling into item
					where item != null
					select item;
				order = SelectionSetOrder.Descending;
			}
			return this.FilterIfSelector(selector, list, order);
		}

		CQ nextPrevAllImpl(string filter, bool next)
		{
			return this.FilterIfSelector(filter, this.MapRangeToNewCQ(this.Elements, (IDomObject input) => CQ.NextPrevAllImpl(input, next)), next ? SelectionSetOrder.Ascending : SelectionSetOrder.Descending);
		}

		CQ nextPrevUntilImpl(string selector, string filter, bool next)
		{
			if (!string.IsNullOrEmpty(selector))
			{
				HashSet<IDomElement> untilEls = new HashSet<IDomElement>(this.Select(selector).Elements);
				return this.FilterIfSelector(filter, this.MapRangeToNewCQ(this.Elements, (IDomObject input) => CQ.NextPrevUntilFilterImpl(input, untilEls, next)), next ? SelectionSetOrder.Ascending : SelectionSetOrder.Descending);
			}
			if (!next)
			{
				return this.PrevAll(filter);
			}
			return this.NextAll(filter);
		}

		static IEnumerable<IDomObject> NextPrevAllImpl(IDomObject input, bool next)
		{
			for (IDomObject item = (next ? input.NextElementSibling : input.PreviousElementSibling); item != null; item = (next ? item.NextElementSibling : item.PreviousElementSibling))
			{
				yield return item;
			}
			yield break;
		}

		static IEnumerable<IDomObject> NextPrevUntilFilterImpl(IDomObject input, HashSet<IDomElement> untilEls, bool next)
		{
			foreach (IDomObject domObject in CQ.NextPrevAllImpl(input, next))
			{
				IDomElement el = (IDomElement)domObject;
				if (untilEls.Contains(el))
				{
					break;
				}
				yield return el;
			}
			yield break;
		}

		public CQ Not(string selector)
		{
			Selector selector2 = new Selector(selector);
			IList<IDomObject> elements = selector2.ToFilterSelector().Select(this.Document, this.Selection);
			return this.Not(elements);
		}

		public CQ Not(IDomObject element)
		{
			return this.Not(Objects.Enumerate<IDomObject>(element));
		}

		public CQ Not(IEnumerable<IDomObject> elements)
		{
			CQ cq = this.NewInstance(this.SelectionSet);
			cq.SelectionSet.ExceptWith(elements);
			cq.Selector = this.Selector;
			return cq;
		}

		public CQ Parent(string selector = null)
		{
			return this.FilterIfSelector(selector, this.MapRangeToNewCQ(this.Selection, new Func<IDomObject, IEnumerable<IDomObject>>(this.ParentImpl)));
		}

		IEnumerable<IDomObject> ParentImpl(IDomObject input)
		{
			if (input.ParentNode != null && input.ParentNode.NodeType == NodeType.ELEMENT_NODE)
			{
				yield return input.ParentNode;
			}
			yield break;
		}

		public CQ Parents(string filter = null)
		{
			string selector = null;
			return this.ParentsUntil(selector, filter);
		}

		static IEnumerable<IDomElement> ParentsImpl(IEnumerable<IDomObject> source, HashSet<IDomElement> until)
		{
			HashSet<IDomElement> alreadyAdded = new HashSet<IDomElement>();
			foreach (IDomObject item in source)
			{
				IDomElement parent = item.ParentNode as IDomElement;
				while (parent != null && !until.Contains(parent) && alreadyAdded.Add(parent))
				{
					yield return parent;
					parent = parent.ParentNode as IDomElement;
				}
			}
			yield break;
		}

		public CQ ParentsUntil(string selector = null, string filter = null)
		{
			return this.ParentsUntil(this.Select(selector).Elements, filter);
		}

		public CQ ParentsUntil(IDomElement element, string filter = null)
		{
			return this.ParentsUntil(Objects.Enumerate<IDomElement>(element), filter);
		}

		public CQ ParentsUntil(IEnumerable<IDomElement> elements, string filter = null)
		{
			CQ cq = this.NewCqInDomain();
			HashSet<IDomElement> until = new HashSet<IDomElement>(elements);
			IEnumerable<IDomObject> selectionSet = this.FilterElementsIgnoreNull(CQ.ParentsImpl(this.Selection, until), filter);
			cq.SetSelection(selectionSet, SelectionSetOrder.OrderAdded, SelectionSetOrder.Descending);
			return cq;
		}

		public CQ Prepend(params IDomObject[] elements)
		{
			return this.Prepend(Objects.Enumerate<IDomObject>(elements));
		}

		public CQ Prepend(params string[] selector)
		{
			return this.Prepend(this.MergeContent(selector));
		}

		public CQ Prepend(IEnumerable<IDomObject> elements)
		{
			CQ cq;
			return this.Prepend(elements, out cq);
		}

		public CQ Prepend(IEnumerable<IDomObject> elements, out CQ insertedElements)
		{
			insertedElements = this.NewCqInDomain();
			bool flag = true;
			List<IDomObject> list = elements.ToList<IDomObject>();
			foreach (IDomElement target in this.Elements)
			{
				int num = 0;
				IDomElement trueTarget = this.GetTrueTarget(target);
				foreach (IDomObject domObject in list)
				{
					IDomObject item = (flag ? domObject : domObject.Clone());
					trueTarget.ChildNodes.Insert(num++, item);
					insertedElements.SelectionSet.Add(item);
				}
				flag = false;
			}
			return this;
		}

		public CQ PrependTo(params string[] target)
		{
			CQ result;
			this.NewInstance(this.MergeSelections(target)).Prepend(this.SelectionSet, out result);
			return result;
		}

		public CQ PrependTo(IEnumerable<IDomObject> targets)
		{
			CQ result;
			this.EnsureCsQuery(targets).Prepend(this.SelectionSet, out result);
			return result;
		}

		public CQ Prop(string name, IConvertible value)
		{
			if (HtmlData.IsBoolean(name))
			{
				this.SetProp(name, value);
			}
			else
			{
				this.Attr(name, value);
			}
			return this;
		}

		public bool Prop(string name)
		{
			name = name.ToLower();
			if (this.Length > 0 && HtmlData.IsBoolean(name))
			{
				bool flag = this[0].HasAttribute(name);
				if (name == "selected" && !flag)
				{
					CQ cq = this.First().Closest("select");
					string a = cq.Val();
					if (a == string.Empty && !cq.Prop("multiple"))
					{
						return object.ReferenceEquals(cq.Find("option").FirstOrDefault<IDomObject>(), this[0]);
					}
				}
				return flag;
			}
			return false;
		}

		protected void SetProp(string name, object value)
		{
			bool flag = Objects.IsTruthy(value);
			foreach (IDomElement domElement in this.Elements)
			{
				if (flag)
				{
					domElement.SetAttribute(name);
				}
				else
				{
					domElement.RemoveAttribute(name);
				}
			}
		}

		public CQ RemoveClass()
		{
			this.Elements.ForEach(delegate(IDomElement item)
			{
				item.ClassName = "";
			});
			return this;
		}

		public CQ RemoveClass(string className)
		{
			foreach (IDomElement domElement in this.Elements)
			{
				if (!string.IsNullOrEmpty(className))
				{
					domElement.RemoveClass(className);
				}
			}
			return this;
		}

		public CQ Remove(string selector = null)
		{
			SelectionSet<IDomObject> selectionSet = ((!string.IsNullOrEmpty(selector)) ? this.Filter(selector).SelectionSet : this.SelectionSet);
			List<IDomObject> list = selectionSet.ToList<IDomObject>();
			List<bool> list2 = (from item in selectionSet
				select item.IsDisconnected || item.Document != this.Document).ToList<bool>();
			for (int i = 0; i < list.Count; i++)
			{
				IDomObject domObject = list[i];
				if (list2[i])
				{
					selectionSet.Remove(domObject);
				}
				if (domObject.ParentNode != null)
				{
					domObject.Remove();
				}
			}
			return this;
		}

		public CQ Detach(string selector = null)
		{
			return this.Remove(selector);
		}

		public CQ ReplaceAll(string selector)
		{
			return this.ReplaceAll(this.Select(selector));
		}

		public CQ ReplaceAll(IDomObject target)
		{
			return this.ReplaceAll(Objects.Enumerate<IDomObject>(target));
		}

		public CQ ReplaceAll(IEnumerable<IDomObject> targets)
		{
			return this.EnsureCsQuery(targets).ReplaceWith(this.SelectionSet);
		}

		public CQ ReplaceWith(params string[] content)
		{
			CQ cq = this;
			if (this.Length > 0)
			{
				CQ elements = this.EnsureCsQuery(this.MergeContent(content));
				CQ cq2 = this.NewInstance(this);
				cq = this.Before(elements);
				cq.SelectionSet.ExceptWith(cq2);
				cq2.Remove(null);
			}
			return cq;
		}

		public CQ ReplaceWith(IDomObject element)
		{
			return this.ReplaceWith(Objects.Enumerate<IDomObject>(element));
		}

		public CQ ReplaceWith(IEnumerable<IDomObject> elements)
		{
			return this.Before(elements).Remove(null);
		}

		public IDomObject this[int index]
		{
			get
			{
				return this.Get(index);
			}
		}

		public CQ Select(Selector selector)
		{
			CQ cq = this.NewCqInDomain();
			cq.Selector = selector;
			IDomDocument document = ((this.CsQueryParent == null) ? this.Document : this.CsQueryParent.Document);
			cq.SetSelection(cq.Selector.Select(document), SelectionSetOrder.Ascending, (SelectionSetOrder)0);
			return cq;
		}

		public CQ Select(string selector)
		{
			Selector selector2 = new Selector(selector);
			if (selector2.IsHtml)
			{
				CQ cq = CQ.Create(selector);
				cq.CsQueryParent = this;
				return cq;
			}
			return this.Select(selector2);
		}

		public CQ this[string selector]
		{
			get
			{
				return this.Select(selector);
			}
		}

		public CQ Select(IDomObject element)
		{
			return this.NewInstance(element, this);
		}

		public CQ this[IDomObject element]
		{
			get
			{
				return this.Select(element);
			}
		}

		public CQ Select(IEnumerable<IDomObject> elements)
		{
			return this.NewInstance(elements, this);
		}

		public CQ this[IEnumerable<IDomObject> element]
		{
			get
			{
				return this.Select(element);
			}
		}

		public CQ Select(string selector, IDomObject context)
		{
			Selector selector2 = new Selector(selector);
			IList<IDomObject> elements = selector2.ToContextSelector().Select(this.Document, context);
			CQ cq = this.NewInstance(elements, this);
			cq.Selector = selector2;
			return cq;
		}

		public CQ this[string selector, IDomObject context]
		{
			get
			{
				return this.Select(selector, context);
			}
		}

		public CQ Select(string selector, IEnumerable<IDomObject> context)
		{
			Selector selector2 = new Selector(selector).ToContextSelector();
			IEnumerable<IDomObject> elements = selector2.Select(this.Document, context);
			CQ cq = this.NewInstance(elements, this);
			cq.Selector = selector2;
			return cq;
		}

		public CQ this[string selector, IEnumerable<IDomObject> context]
		{
			get
			{
				return this.Select(selector, context);
			}
		}

		public CQ Hide()
		{
			foreach (IDomElement domElement in this.Elements)
			{
				domElement.Style["display"] = "none";
			}
			return this;
		}

		public CQ Show()
		{
			foreach (IDomElement domElement in this.Elements)
			{
				if (domElement.Style["display"] == "none")
				{
					domElement.RemoveStyle("display");
				}
			}
			return this;
		}

		public CQ Toggle()
		{
			foreach (IDomElement domElement in this.Elements)
			{
				string text = domElement.Style["display"];
				bool flag = text == null || text != "none";
				domElement.Style["display"] = (flag ? "none" : null);
			}
			return this;
		}

		public CQ Toggle(bool isVisible)
		{
			if (!isVisible)
			{
				return this.Hide();
			}
			return this.Show();
		}

		public CQ Siblings(string selector = null)
		{
			return this.FilterIfSelector(selector, CQ.GetSiblings(this.SelectionSet), SelectionSetOrder.Ascending);
		}

		protected static IEnumerable<IDomObject> GetSiblings(IEnumerable<IDomObject> elements)
		{
			foreach (IDomObject item in elements)
			{
				foreach (IDomElement child in item.ParentNode.ChildElements)
				{
					if (!object.ReferenceEquals(child, item))
					{
						yield return child;
					}
				}
			}
			yield break;
		}

		public CQ Slice(int start)
		{
			return this.Slice(start, this.SelectionSet.Count);
		}

		public CQ Slice(int start, int end)
		{
			if (start < 0)
			{
				start = this.SelectionSet.Count + start;
				if (start < 0)
				{
					start = 0;
				}
			}
			if (end < 0)
			{
				end = this.SelectionSet.Count + end;
				if (end < 0)
				{
					end = 0;
				}
			}
			if (end >= this.SelectionSet.Count)
			{
				end = this.SelectionSet.Count;
			}
			CQ cq = this.NewCqInDomain();
			for (int i = start; i < end; i++)
			{
				cq.SelectionSet.Add(this.SelectionSet[i]);
			}
			return cq;
		}

		public string Text()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.AddTextToStringBuilder(stringBuilder, this.Selection);
			return stringBuilder.ToString();
		}

		public CQ Text(string value)
		{
			foreach (IDomElement el in this.Elements)
			{
				this.SetChildText(el, value);
			}
			return this;
		}

		public CQ Text(Func<int, string, object> func)
		{
			int num = 0;
			foreach (IDomElement domElement in this.Elements)
			{
				string textContent = domElement.TextContent;
				string text = func(num, textContent).ToString();
				if (text != textContent)
				{
					this.SetChildText(domElement, text);
				}
				num++;
			}
			return this;
		}

		void AddTextToStringBuilder(StringBuilder sb, IEnumerable<IDomObject> nodes)
		{
			foreach (IDomObject domObject in nodes)
			{
				NodeType nodeType = domObject.NodeType;
				switch (nodeType)
				{
				case NodeType.ELEMENT_NODE:
					sb.Append(domObject.TextContent);
					break;
				case (NodeType)2:
					break;
				case NodeType.TEXT_NODE:
					sb.Append(domObject.NodeValue);
					break;
				default:
					switch (nodeType)
					{
					case NodeType.DOCUMENT_NODE:
					case NodeType.DOCUMENT_FRAGMENT_NODE:
						this.AddTextToStringBuilder(sb, domObject.ChildNodes);
						break;
					}
					break;
				}
			}
		}

		void SetChildText(IDomElement el, string text)
		{
			if (el.ChildrenAllowed)
			{
				el.ChildNodes.Clear();
				IDomText item = new DomText(text);
				el.ChildNodes.Add(item);
			}
		}

		public T Val<T>()
		{
			string value = this.Val();
			return Objects.Convert<T>(value);
		}

		public T ValOrDefault<T>()
		{
			string value = this.Val();
			T result;
			if (Objects.TryConvert<T>(value, out result))
			{
				return result;
			}
			return (T)((object)Objects.DefaultValue(typeof(T)));
		}

		public string Val()
		{
			if (this.Length <= 0)
			{
				return null;
			}
			IDomElement domElement = this.Elements.First<IDomElement>();
			ushort nodeNameID = domElement.NodeNameID;
			switch (nodeNameID)
			{
			case 9:
			{
				string text = domElement.GetAttribute("value", string.Empty);
				string attribute;
				if ((attribute = domElement.GetAttribute("type", string.Empty)) != null && (attribute == "radio" || attribute == "checkbox") && string.IsNullOrEmpty(text))
				{
					text = "on";
				}
				return text;
			}
			case 10:
			{
				string empty = string.Empty;
				HTMLSelectElement htmlselectElement = (HTMLSelectElement)domElement;
				if (!htmlselectElement.Multiple)
				{
					return htmlselectElement.Value;
				}
				IEnumerable<IHTMLOptionElement> source = htmlselectElement.ChildElementsOfTag<IHTMLOptionElement>(11);
				return string.Join(",", from item in source
					where item.HasAttribute("selected") && !item.Disabled
					select item.Value ?? item.TextContent);
			}
			case 11:
			{
				string text = domElement.GetAttribute("value");
				return text ?? domElement.TextContent;
			}
			default:
				if (nodeNameID == 33)
				{
					return domElement.Value;
				}
				return domElement.GetAttribute("value", string.Empty);
			}
		}

		public CQ Val(object value)
		{
			bool flag = true;
			string valueString = this.GetValueString(value);
			foreach (IDomElement domElement in this.Elements)
			{
				ushort nodeNameID = domElement.NodeNameID;
				switch (nodeNameID)
				{
				case 9:
				{
					string attribute;
					if ((attribute = domElement.GetAttribute("type", string.Empty)) != null && (attribute == "checkbox" || attribute == "radio"))
					{
						if (flag)
						{
							this.SetOptionSelected(this.Elements, value, true);
						}
					}
					else
					{
						domElement.SetAttribute("value", valueString);
					}
					break;
				}
				case 10:
					if (flag)
					{
						bool multiple = domElement.HasAttribute("multiple");
						this.SetOptionSelected(domElement.ChildElements, value, multiple);
					}
					break;
				default:
					if (nodeNameID == 33)
					{
						domElement.TextContent = valueString;
					}
					else
					{
						domElement.SetAttribute("value", valueString);
					}
					break;
				}
				flag = false;
			}
			return this;
		}

		protected string GetValueString(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is string)
			{
				return (string)value;
			}
			if (!(value is IEnumerable))
			{
				return value.ToString();
			}
			return Objects.Join((IEnumerable)value);
		}

		public CQ Width(int value)
		{
			return this.Width(value.ToString() + "px");
		}

		public CQ Width(string value)
		{
			return this.Css("width", value);
		}

		public CQ Height(int value)
		{
			return this.Height(value.ToString() + "px");
		}

		public CQ Height(string value)
		{
			return this.Css("height", value);
		}

		public CQ Wrap(string wrappingSelector)
		{
			return this.Wrap(this.Select(wrappingSelector));
		}

		public CQ Wrap(IDomObject element)
		{
			return this.Wrap(Objects.Enumerate<IDomObject>(element));
		}

		public CQ Wrap(IEnumerable<IDomObject> wrapper)
		{
			return this.Wrap(wrapper, false);
		}

		public CQ WrapAll(string wrappingSelector)
		{
			return this.WrapAll(this.Select(wrappingSelector));
		}

		public CQ WrapAll(IDomObject element)
		{
			return this.WrapAll(Objects.Enumerate<IDomObject>(element));
		}

		public CQ WrapAll(IEnumerable<IDomObject> wrapper)
		{
			return this.Wrap(wrapper, true);
		}

		public CQ Unwrap()
		{
			HashSet<IDomObject> hashSet = new HashSet<IDomObject>();
			foreach (IDomObject domObject in this.SelectionSet)
			{
				if (domObject.ParentNode != null)
				{
					hashSet.Add(domObject.ParentNode);
				}
			}
			foreach (IDomObject domObject2 in hashSet)
			{
				CQ cq = domObject2.Cq();
				cq.ReplaceWith(cq.Contents());
			}
			return this;
		}

		public CQ WrapInner(string selector)
		{
			return this.WrapInner(this.Select(selector));
		}

		public CQ WrapInner(IDomObject wrapper)
		{
			return this.WrapInner(Objects.Enumerate<IDomObject>(wrapper));
		}

		public CQ WrapInner(IEnumerable<IDomObject> wrapper)
		{
			foreach (IDomElement domElement in this.Elements)
			{
				CQ cq = domElement.Cq();
				CQ cq2 = cq.Contents();
				if (cq2.Length > 0)
				{
					cq2.WrapAll(wrapper);
				}
				else
				{
					cq.Append(wrapper);
				}
			}
			return this;
		}

		CQ Wrap(IEnumerable<IDomObject> wrapper, bool keepSiblingsTogether)
		{
			CQ cq = this.EnsureCsQuery(wrapper);
			IDomElement domElement = null;
			IDomElement domElement2 = null;
			this.GetInnermostContainer(cq.Elements, out domElement, out domElement2);
			if (domElement != null)
			{
				IDomObject domObject = null;
				IDomElement domElement3 = null;
				IDomElement domElement4 = null;
				foreach (IDomObject domObject2 in this.SelectionSet)
				{
					if (domObject == null || (!object.ReferenceEquals(domObject, domObject2) && !keepSiblingsTogether))
					{
						CQ cq2 = domElement2.Cq().Clone();
						if (domObject2.ParentNode != null)
						{
							cq2.InsertBefore(domObject2);
						}
						this.GetInnermostContainer(cq2.Elements, out domElement3, out domElement4);
					}
					domObject = domObject2.NextSibling;
					domElement3.AppendChild(domObject2);
				}
			}
			return this;
		}

		protected int GetInnermostContainer(IEnumerable<IDomElement> elements, out IDomElement element, out IDomElement rootElement)
		{
			int num = 0;
			element = null;
			rootElement = null;
			foreach (IDomElement domElement in elements)
			{
				if (domElement.HasChildren)
				{
					IDomElement domElement2;
					IDomElement domElement3;
					int innermostContainer = this.GetInnermostContainer(domElement.ChildElements, out domElement2, out domElement3);
					if (innermostContainer > num)
					{
						num = innermostContainer + 1;
						element = domElement2;
						rootElement = domElement;
					}
				}
				if (num == 0)
				{
					num = 1;
					element = domElement;
					rootElement = domElement;
				}
			}
			return num;
		}

		Selector _Selector;

		IDomDocument _Document;

		CQ _CsQueryParent;

		SelectionSet<IDomObject> _Selection;
	}
}
