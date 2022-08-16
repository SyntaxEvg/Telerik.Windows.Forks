using System;
using System.Collections.Generic;
using CsQuery.Implementation;

namespace CsQuery
{
	interface IDomObject : IDomNode, ICloneable, IComparable<IDomObject>
	{
		IDomDocument Document { get; }

		IDomContainer ParentNode { get; }

		IEnumerable<IDomContainer> GetAncestors();

		IEnumerable<IDomObject> GetDescendents();

		IEnumerable<IDomElement> GetDescendentElements();

		IDomObject this[int index] { get; }

		string this[string attribute] { get; set; }

		string Id { get; set; }

		IAttributeCollection Attributes { get; }

		CSSStyleDeclaration Style { get; set; }

		string ClassName { get; set; }

		IEnumerable<string> Classes { get; }

		string Value { get; set; }

		string DefaultValue { get; set; }

		string InnerHTML { get; set; }

		string OuterHTML { get; set; }

		string InnerText { get; set; }

		string TextContent { get; set; }

		void AppendChild(IDomObject element);

		void RemoveChild(IDomObject element);

		void InsertBefore(IDomObject newNode, IDomObject referenceNode);

		void InsertAfter(IDomObject newNode, IDomObject referenceNode);

		IDomObject FirstChild { get; }

		IDomElement FirstElementChild { get; }

		IDomObject LastChild { get; }

		IDomElement LastElementChild { get; }

		IDomObject NextSibling { get; }

		IDomObject PreviousSibling { get; }

		IDomElement NextElementSibling { get; }

		IDomElement PreviousElementSibling { get; }

		void SetAttribute(string name);

		void SetAttribute(string name, string value);

		string GetAttribute(string name);

		string GetAttribute(string name, string defaultValue);

		bool TryGetAttribute(string name, out string value);

		bool HasAttribute(string name);

		bool RemoveAttribute(string name);

		bool HasClass(string className);

		bool AddClass(string className);

		bool RemoveClass(string className);

		bool HasStyle(string styleName);

		void AddStyle(string styleString);

		void AddStyle(string style, bool strict);

		bool RemoveStyle(string name);

		bool HasAttributes { get; }

		bool HasClasses { get; }

		bool HasStyles { get; }

		bool Selected { get; set; }

		bool Checked { get; set; }

		bool Disabled { get; set; }

		bool ReadOnly { get; set; }

		string Type { get; set; }

		string Name { get; set; }

		bool InnerHtmlAllowed { get; }

		bool InnerTextAllowed { get; }

		bool ChildrenAllowed { get; }

		int DescendantCount();

		int Depth { get; }

		[Obsolete]
		char PathID { get; }

		[Obsolete]
		string Path { get; }

		ushort NodePathID { get; }

		ushort[] NodePath { get; }

		CQ Cq();

		IDomObject Clone();

		ushort NodeNameID { get; }
	}
}
