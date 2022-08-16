using System;
using System.Web;

namespace Telerik.UrlRewriter.Transforms
{
	public sealed class DecodeTransform : IRewriteTransform
	{
		public string ApplyTransform(string input)
		{
			return HttpUtility.UrlDecode(input);
		}

		public string Name
		{
			get
			{
				return "decode";
			}
		}
	}
}
