using System;
using System.Collections;
using System.Collections.Generic;

namespace CsQuery
{
	interface IAttributeCollection : IEnumerable<KeyValuePair<string, string>>, IEnumerable
	{
		string GetAttribute(string name);

		void SetAttribute(string name, string value);

		string this[string attributeName] { get; set; }

		int Length { get; }
	}
}
