using System;
using System.Threading;

namespace Telerik.UrlRewriter.Transforms
{
	public sealed class LowerTransform : IRewriteTransform
	{
		public string ApplyTransform(string input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return input.ToLower(Thread.CurrentThread.CurrentCulture);
		}

		public string Name
		{
			get
			{
				return "lower";
			}
		}
	}
}
