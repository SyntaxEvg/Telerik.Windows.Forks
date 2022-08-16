using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Sevices
{
	static class DateTimeService
	{
		internal static IDateTimeProvider DateTimeProvider { get; set; } = new SystemDateTimeProvider();
		//{
		//	get
		//	{
		//		return DateTimeService.dateTimeProvider;
		//	}
		//	set
		//	{
		//		Guard.ThrowExceptionIfNull<IDateTimeProvider>(value, "value");
		//		DateTimeService.dateTimeProvider = value;
		//	}
		//} = new SystemDateTimeProvider();

		internal static DateTime GetDateTime()
		{
			return DateTimeService.DateTimeProvider.GetDateTime();
		}

		static IDateTimeProvider dateTimeProvider;
	}
}
