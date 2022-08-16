using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Common
{
	static class UndefinedStyleIdGenerator
	{
		public static string GetNext()
		{
			return "UndefinedStyleId" + UndefinedStyleIdGenerator.idGenerator.GetNext();
		}

		const string UndefinedStyleIdName = "UndefinedStyleId";

		static readonly IdGenerator idGenerator = new IdGenerator();
	}
}
