using System;
using System.Net;

namespace Telerik.UrlRewriter.Actions
{
	public sealed class NotFoundAction : SetStatusAction
	{
		public NotFoundAction()
			: base(HttpStatusCode.NotFound)
		{
		}
	}
}
