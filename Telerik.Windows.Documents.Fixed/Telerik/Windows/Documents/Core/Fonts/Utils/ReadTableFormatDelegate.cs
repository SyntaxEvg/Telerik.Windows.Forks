using System;
using Telerik.Windows.Documents.Core.Fonts.OpenType;

namespace Telerik.Windows.Documents.Core.Fonts.Utils
{
	delegate T ReadTableFormatDelegate<T>(OpenTypeFontSourceBase fontSource, OpenTypeFontReader reader);
}
