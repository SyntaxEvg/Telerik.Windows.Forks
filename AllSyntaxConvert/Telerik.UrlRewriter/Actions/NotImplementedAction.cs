using System;
using System.Net;

namespace Telerik.UrlRewriter.Actions
{
	public sealed class NotImplementedAction : SetStatusAction
	{
		public NotImplementedAction()
			: base(HttpStatusCode.NotImplemented)
		{
		}
	}
}
