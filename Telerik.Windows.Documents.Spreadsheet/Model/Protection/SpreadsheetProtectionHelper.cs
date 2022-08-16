using System;
using Telerik.Windows.Documents.Common.Model.Protection;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Protection
{
	class SpreadsheetProtectionHelper : ProtectionHelperBase
	{
		internal static SpreadsheetProtectionHelper Instance
		{
			get
			{
				return SpreadsheetProtectionHelper.instance;
			}
		}

		public string GeneratePasswordLegacyScheme(string password)
		{
			Guard.ThrowExceptionIfNullOrEmpty(password, "password");
			int length = password.Length;
			int num = 0;
			if (length == 0)
			{
				return num.ToString("X");
			}
			for (int i = length - 1; i >= 0; i--)
			{
				num = ((num >> 14) & 1) | ((num << 1) & 32767);
				num ^= (int)password[i];
			}
			num = ((num >> 14) & 1) | ((num << 1) & 32767);
			num ^= (int)('耀' | ((int)SpreadsheetProtectionHelper.nConstant << 8) | SpreadsheetProtectionHelper.kConstant);
			return (num ^ length).ToString("X");
		}

		static readonly char nConstant = 'N';

		static readonly char kConstant = 'K';

		static readonly SpreadsheetProtectionHelper instance = new SpreadsheetProtectionHelper();
	}
}
