using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CsQuery.Output;

namespace CsQuery
{
	interface IDomNode : ICloneable
	{
		NodeType NodeType { get; }

		string NodeName { get; }

		string NodeValue { get; set; }

		bool HasChildren { get; }

		int Index { get; }

		INodeList ChildNodes { get; }

		IEnumerable<IDomElement> ChildElements { get; }

		string Render();

		string Render(DomRenderingOptions options);

		string Render(IOutputFormatter formatter);

		void Render(IOutputFormatter formatter, TextWriter writer);

		[Obsolete]
		void Render(StringBuilder sb);

		[Obsolete]
		void Render(StringBuilder sb, DomRenderingOptions options = DomRenderingOptions.Default);

		void Remove();

		bool IsIndexed { get; }

		bool IsDisconnected { get; }

		bool IsFragment { get; }

		IDomNode Clone();
	}
}
