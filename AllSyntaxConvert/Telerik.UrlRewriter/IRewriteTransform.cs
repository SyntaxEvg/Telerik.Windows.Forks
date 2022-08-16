using System;

namespace Telerik.UrlRewriter
{
	public interface IRewriteTransform
	{
		string ApplyTransform(string input);

		string Name { get; }
	}
}
