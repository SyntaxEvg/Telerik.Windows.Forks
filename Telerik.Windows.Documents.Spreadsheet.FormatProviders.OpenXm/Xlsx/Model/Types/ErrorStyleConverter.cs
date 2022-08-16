using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	class ErrorStyleConverter : IStringConverter<ErrorStyle>
	{
		static ErrorStyleConverter()
		{
			ErrorStyleConverter.AddId(ErrorStyleConverter.Stop, ErrorStyle.Stop);
			ErrorStyleConverter.AddId(ErrorStyleConverter.Warning, ErrorStyle.Warning);
			ErrorStyleConverter.AddId(ErrorStyleConverter.Information, ErrorStyle.Information);
			ErrorStyleConverter.defaultId = ErrorStyleConverter.errorStyleToId[ErrorStyle.Stop];
		}

		public ErrorStyle ConvertFromString(string value)
		{
			ErrorStyle result;
			if (ErrorStyleConverter.typeIdToErrorStyle.TryGetValue(value, out result))
			{
				return result;
			}
			return ErrorStyle.Stop;
		}

		public string ConvertToString(ErrorStyle value)
		{
			string result;
			if (ErrorStyleConverter.errorStyleToId.TryGetValue(value, out result))
			{
				return result;
			}
			return ErrorStyleConverter.defaultId;
		}

		static void AddId(string id, ErrorStyle errorStyle)
		{
			ErrorStyleConverter.typeIdToErrorStyle[id] = errorStyle;
			if (!ErrorStyleConverter.errorStyleToId.ContainsKey(errorStyle))
			{
				ErrorStyleConverter.errorStyleToId[errorStyle] = id;
			}
		}

		public const ErrorStyle DefaultExportErrorStyle = ErrorStyle.Stop;

		static readonly string Stop = "stop";

		static readonly string Warning = "warning";

		static readonly string Information = "information";

		static readonly Dictionary<ErrorStyle, string> errorStyleToId = new Dictionary<ErrorStyle, string>();

		static readonly Dictionary<string, ErrorStyle> typeIdToErrorStyle = new Dictionary<string, ErrorStyle>();

		static readonly string defaultId;
	}
}
