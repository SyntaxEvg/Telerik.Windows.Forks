using System;
using System.Collections.Specialized;

namespace Telerik.UrlRewriter.Transforms
{
	public sealed class StaticMappingTransform : IRewriteTransform
	{
		public StaticMappingTransform(string name, StringDictionary map)
		{
			this._name = name;
			this._map = map;
		}

		public string ApplyTransform(string input)
		{
			return this._map[input];
		}

		public string Name
		{
			get
			{
				return this._name;
			}
		}

		string _name;

		StringDictionary _map;
	}
}
