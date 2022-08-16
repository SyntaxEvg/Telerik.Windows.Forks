using System;
using System.Web;

namespace Telerik.UrlRewriter.Transforms
{
	public sealed class EncodeTransform : IRewriteTransform
	{
		public string ApplyTransform(string input)
		{
			return HttpUtility.UrlEncode(input);
		}

		public string Name
		{
			get
			{
				return "encode";
			}
		}
	}
}
