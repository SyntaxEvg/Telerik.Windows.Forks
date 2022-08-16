using System;

namespace Telerik.Windows.Documents.Core.Sevices
{
	class SystemDateTimeProvider : IDateTimeProvider
	{
		public DateTime GetDateTime()
		{
			return DateTime.Now;
		}
	}
}
