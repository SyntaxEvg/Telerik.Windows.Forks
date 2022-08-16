using System;
using System.IO;

namespace CsQuery.Output
{
	interface IOutputFormatter
	{
		void Render(IDomObject node, TextWriter writer);

		string Render(IDomObject node);
	}
}
