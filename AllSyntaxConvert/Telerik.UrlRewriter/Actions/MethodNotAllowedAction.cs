using System;
using System.Net;

namespace Telerik.UrlRewriter.Actions
{
	public sealed class MethodNotAllowedAction : SetStatusAction
	{
		public MethodNotAllowedAction()
			: base(HttpStatusCode.MethodNotAllowed)
		{
		}
	}
}
