using System;
using System.IO;

namespace Telerik.UrlRewriter.Conditions
{
	public class ExistsCondition : IRewriteCondition
	{
		public ExistsCondition(string location)
		{
			if (location == null)
			{
				throw new ArgumentNullException("location");
			}
			this._location = location;
		}

		public bool IsMatch(RewriteContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			string path = context.MapPath(context.Expand(this._location));
			return File.Exists(path) || Directory.Exists(path);
		}

		string _location;
	}
}
