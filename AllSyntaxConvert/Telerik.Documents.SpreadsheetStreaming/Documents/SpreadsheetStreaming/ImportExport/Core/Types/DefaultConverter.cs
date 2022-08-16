using System;
using System.Globalization;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	class DefaultConverter<T> : IStringConverter<T>
	{
		T IStringConverter<T>.ConvertFromString(string value)
		{
			return (T)((object)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture));
		}

		public string ConvertToString(T value)
		{
			if (object.ReferenceEquals(value, null))
			{
				return string.Empty;
			}
			string text = (string)Convert.ChangeType(value, typeof(string), CultureInfo.InvariantCulture);
			return IllegalXmlCharHelper.RemoveInvalidCharacters(text);
		}
	}
}
