using System;
using System.Text;

namespace Telerik.Windows.Documents.Utilities
{
	static class TextHelper
	{
		public static string SanitizeNewLines(string input)
		{
			StringBuilder stringBuilder = new StringBuilder(input.Length);
			bool flag = false;
			int i = 0;
			int length = input.Length;
			while (i < length)
			{
				if (input[i] == TextHelper.CarrigeReturn)
				{
					if (flag)
					{
						stringBuilder.Append(Environment.NewLine);
					}
					flag = true;
				}
				else if (input[i] == TextHelper.LineFeed)
				{
					stringBuilder.Append(Environment.NewLine);
					flag = false;
				}
				else
				{
					if (flag)
					{
						stringBuilder.Append(Environment.NewLine);
						flag = false;
					}
					stringBuilder.Append(input[i]);
				}
				i++;
			}
			if (flag)
			{
				stringBuilder.Append(Environment.NewLine);
			}
			return stringBuilder.ToString();
		}

		public static string RemoveNewLines(string input)
		{
			StringBuilder stringBuilder = new StringBuilder(input.Length);
			int i = 0;
			int length = input.Length;
			while (i < length)
			{
				if (input[i] != TextHelper.CarrigeReturn && input[i] != TextHelper.LineFeed)
				{
					stringBuilder.Append(input[i]);
				}
				i++;
			}
			return stringBuilder.ToString();
		}

		public static string[] GetLines(string input)
		{
			input = TextHelper.SanitizeNewLines(input);
			return input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
		}

		public static string AddSlashes(string source)
		{
			if (source == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(source.Length);
			int i = 0;
			int length = source.Length;
			while (i < length)
			{
				if (source[i] == '\\' || source[i] == '"')
				{
					stringBuilder.Append('\\');
				}
				stringBuilder.Append(source[i]);
				i++;
			}
			return stringBuilder.ToString();
		}

		static readonly char LineFeed = '\n';

		static readonly char CarrigeReturn = '\r';
	}
}
