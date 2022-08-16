using System;
using System.Linq;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities
{
	static class Util
	{
		public static void EnsureGroupDestination(RtfGroup group, string expected)
		{
			if (expected != group.Destination)
			{
				throw new RtfUnexpectedElementException(expected, group.Destination);
			}
		}

		public static void EnsureTagName(RtfTag tag, string expected)
		{
			if (expected != tag.Name)
			{
				throw new RtfUnexpectedElementException(expected, tag.Name);
			}
		}

		public static void EnsureTagName(RtfTag tag, params string[] expected)
		{
			if (!expected.Any((string str) => str == tag.Name))
			{
				throw new RtfUnexpectedElementException(string.Concat(from str in expected
					select "[" + str + "]"), tag.Name);
			}
		}
	}
}
