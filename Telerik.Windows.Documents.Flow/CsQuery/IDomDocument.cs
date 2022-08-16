using System;
using System.Collections.Generic;
using CsQuery.Engine;

namespace CsQuery
{
	interface IDomDocument : IDomContainer, IDomObject, IDomNode, ICloneable, IComparable<IDomObject>
	{
		IDomIndex DocumentIndex { get; }

		IDomDocumentType DocTypeNode { get; set; }

		DocType DocType { get; }

		IList<ICSSStyleSheet> StyleSheets { get; }

		IDomElement GetElementById(string id);

		T GetElementById<T>(string id) where T : IDomElement;

		IDomElement CreateElement(string nodeName);

		IDomText CreateTextNode(string text);

		IDomComment CreateComment(string comment);

		IDomDocumentType CreateDocumentType(string type, string access, string fpi, string uri);

		IDomDocumentType CreateDocumentType(DocType docType);

		IDomElement QuerySelector(string selector);

		IList<IDomElement> QuerySelectorAll(string selector);

		INodeList<IDomElement> GetElementsByTagName(string tagName);

		IDomElement Body { get; }

		IDomDocument CreateNew<T>() where T : IDomDocument;

		IDomDocument CreateNew();

		IDictionary<string, object> Data { get; set; }
	}
}
