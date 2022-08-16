using System;

namespace Telerik.UrlRewriter.Logging
{
	public interface IRewriteLogger
	{
		void Debug(object message);

		void Info(object message);

		void Warn(object message);

		void Error(object message);

		void Error(object message, Exception exception);

		void Fatal(object message, Exception exception);
	}
}
