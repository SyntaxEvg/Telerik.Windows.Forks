using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Common
{
	static class AnnotationIdGenerator
	{
		public static int GetNext()
		{
			int result;
			lock (AnnotationIdGenerator.LockObject)
			{
				AnnotationIdGenerator.lastId++;
				result = AnnotationIdGenerator.lastId;
			}
			return result;
		}

		static readonly object LockObject = new object();

		static int lastId = -1;
	}
}
