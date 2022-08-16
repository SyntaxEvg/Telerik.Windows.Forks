using System;
using System.Net;

namespace Telerik.UrlRewriter.Actions
{
	public sealed class ForbiddenAction : SetStatusAction
	{
		public ForbiddenAction()
			: base(HttpStatusCode.Forbidden)
		{
		}
	}
}
