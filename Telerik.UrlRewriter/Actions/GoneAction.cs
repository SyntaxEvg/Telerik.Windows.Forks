using System;
using System.Net;

namespace Telerik.UrlRewriter.Actions
{
	public sealed class GoneAction : SetStatusAction
	{
		public GoneAction()
			: base(HttpStatusCode.Gone)
		{
		}
	}
}
