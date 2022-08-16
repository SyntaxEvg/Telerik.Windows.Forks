using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class HyperlinkFunction : FunctionWithArguments
	{
		public override string Name
		{
			get
			{
				return HyperlinkFunction.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return HyperlinkFunction.Info;
			}
		}

		internal HyperlinkInfo HyperlinkInfo
		{
			get
			{
				return this.hyperlinkInfo;
			}
		}

		static HyperlinkFunction()
		{
			string description = "Creates a shortcut or jump that opens a document stored on a network server, an intranet, or the Internet.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Hyperlink_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Link Location", "is the path and file name to the document to be opened. Link Location can refer to a place in a document — such as a specific cell or named range in a worksheet or workbook. The path can be to a file that is stored on a hard disk drive.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_LinkLocation", "Spreadsheet_Functions_Hyperlink_LinkLocationInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Friendly Name", "specifies the jump text or numeric value that is displayed in the cell. Friendly Name is displayed in blue and is underlined. If Friendly Name is omitted, the cell displays the Link Location as the jump text.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_FriendlyName", "Spreadsheet_Functions_Hyperlink_FriendlyNameInfo")
			};
			HyperlinkFunction.Info = new FunctionInfo(HyperlinkFunction.FunctionName, FunctionCategory.LookupReference, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			string address;
			string text;
			string text2;
			string emailSubject;
			SpreadsheetHelper.TryParseHyperlinkAddress(context.Arguments[0].ToString(), out address, out text, out text2, out emailSubject);
			string screenTip = string.Format("{0} - Click once to follow. Click and hold to select this cell.", context.Arguments[0].ToString());
			HyperlinkInfo hyperlinkInfo;
			if (!string.IsNullOrEmpty(text2))
			{
				hyperlinkInfo = HyperlinkInfo.CreateMailtoHyperlink(text2, emailSubject, screenTip);
			}
			else
			{
				hyperlinkInfo = HyperlinkInfo.CreateHyperlink(address, screenTip);
			}
			this.hyperlinkInfo = hyperlinkInfo;
			RadExpression result;
			if (context.Arguments.Length > 1)
			{
				result = (RadExpression)context.Arguments[1];
			}
			else
			{
				result = new StringExpression(context.Arguments[0].ToString());
			}
			return result;
		}

		public static readonly string FunctionName = "HYPERLINK";

		static readonly FunctionInfo Info;

		HyperlinkInfo hyperlinkInfo;
	}
}
