using System;

namespace Telerik.Windows.Documents.Flow.Model.Styles.Core
{
	static class GlobalPropertyIndexGenerator
	{
		public static int GetNext(StylePropertyType stylePropertyType)
		{
			int num = 0;
			switch (stylePropertyType)
			{
			case StylePropertyType.Character:
				num = 0;
				break;
			case StylePropertyType.Paragraph:
				num = 1;
				break;
			case StylePropertyType.Table:
				num = 2;
				break;
			case StylePropertyType.TableRow:
				num = 3;
				break;
			case StylePropertyType.TableCell:
				num = 4;
				break;
			case StylePropertyType.Section:
				num = 5;
				break;
			case StylePropertyType.Document:
				num = 6;
				break;
			case StylePropertyType.DocumentElement:
				return 0;
			}
			int result;
			lock (GlobalPropertyIndexGenerator.lockObject)
			{
				result = ++GlobalPropertyIndexGenerator.counters[num];
			}
			return result;
		}

		static readonly int[] counters = new int[7];

		static readonly object lockObject = new object();
	}
}
