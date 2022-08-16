using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Model.Types
{
	class ScatterStyleToStringConverter : IStringConverter<ScatterStyle>
	{
		public ScatterStyle ConvertFromString(string value)
		{
			switch (value)
			{
			case "line":
				return ScatterStyle.Line;
			case "lineMarker":
				return ScatterStyle.LineMarker;
			case "marker":
				return ScatterStyle.Marker;
			case "none":
				return ScatterStyle.None;
			case "smooth":
				return ScatterStyle.Smooth;
			case "smoothMarker":
				return ScatterStyle.SmoothMarker;
			}
			throw new NotSupportedException();
		}

		public string ConvertToString(ScatterStyle value)
		{
			switch (value)
			{
			case ScatterStyle.None:
				return "none";
			case ScatterStyle.Line:
				return "line";
			case ScatterStyle.LineMarker:
				return "lineMarker";
			case ScatterStyle.Marker:
				return "marker";
			case ScatterStyle.Smooth:
				return "smooth";
			case ScatterStyle.SmoothMarker:
				return "smoothMarker";
			default:
				throw new NotSupportedException();
			}
		}
	}
}
