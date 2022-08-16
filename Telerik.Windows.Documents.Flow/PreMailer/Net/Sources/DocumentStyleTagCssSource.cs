using System;
using CsQuery;

namespace PreMailer.Net.Sources
{
	class DocumentStyleTagCssSource : ICssSource
	{
		public DocumentStyleTagCssSource(IDomObject node)
		{
			this._node = node;
		}

		public string GetCss()
		{
			return this._node.InnerHTML;
		}

		readonly IDomObject _node;
	}
}
