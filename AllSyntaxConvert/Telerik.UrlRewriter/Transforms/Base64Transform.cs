using System;
using System.Text;

namespace Telerik.UrlRewriter.Transforms
{
	public sealed class Base64Transform : IRewriteTransform
	{
		public string ApplyTransform(string input)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
		}

		public string Name
		{
			get
			{
				return "base64decode";
			}
		}
	}
}
