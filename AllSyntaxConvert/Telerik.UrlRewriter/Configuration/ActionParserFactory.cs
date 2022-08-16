using System;
using System.Collections;
using Telerik.UrlRewriter.Utilities;

namespace Telerik.UrlRewriter.Configuration
{
	public class ActionParserFactory
	{
		public void AddParser(string parserType)
		{
			this.AddParser((IRewriteActionParser)TypeHelper.Activate(parserType, null));
		}

		public void AddParser(IRewriteActionParser parser)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			ArrayList arrayList;
			if (this._parsers.ContainsKey(parser.Name))
			{
				arrayList = (ArrayList)this._parsers[parser.Name];
			}
			else
			{
				arrayList = new ArrayList();
				this._parsers.Add(parser.Name, arrayList);
			}
			arrayList.Add(parser);
		}

		public IList GetParsers(string verb)
		{
			if (this._parsers.ContainsKey(verb))
			{
				return (ArrayList)this._parsers[verb];
			}
			return null;
		}

		Hashtable _parsers = new Hashtable();
	}
}
