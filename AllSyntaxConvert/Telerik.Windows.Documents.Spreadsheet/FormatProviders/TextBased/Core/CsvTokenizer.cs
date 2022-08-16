using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Core
{
	class CsvTokenizer
	{
		public bool IsAtEndOfFile
		{
			get
			{
				return this.textReader.Peek() == -1;
			}
		}

		public CsvTokenizer(TextReader textReader, CsvSettings settings)
		{
			Guard.ThrowExceptionIfNull<TextReader>(textReader, "textReader");
			this.textReader = textReader;
			this.settings = settings ?? new CsvSettings();
			this.tokenReaders = new List<Func<CsvToken>>();
			this.InitTokenHadlers();
		}

		void InitTokenHadlers()
		{
			this.tokenReaders.Add(() => this.ReadSingleCharToken(CsvTokenType.Delimiter, new char?(this.settings.Delimiter)));
			this.tokenReaders.Add(() => this.ReadSingleCharToken(CsvTokenType.Quote, new char?(this.settings.Quote)));
			this.tokenReaders.Add(() => this.ReadEndOfLineToken());
			this.tokenReaders.Add(() => this.ReadSingleCharToken(CsvTokenType.Symbol, null));
		}

		public CsvToken Read()
		{
			CsvToken csvToken = null;
			foreach (Func<CsvToken> func in this.tokenReaders)
			{
				csvToken = func();
				if (csvToken != null)
				{
					break;
				}
			}
			return csvToken;
		}

		CsvToken ReadSingleCharToken(CsvTokenType csvTokenType, char? tokenChar = null)
		{
			int num = this.textReader.Peek();
			if (num != -1)
			{
				char c = (char)num;
				if (tokenChar == null || c == tokenChar)
				{
					this.textReader.Read();
					return new CsvToken(csvTokenType, c.ToString());
				}
			}
			return null;
		}

		CsvToken ReadEndOfLineToken()
		{
			char c = (char)this.textReader.Peek();
			if (c == '\r' || c == '\n')
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(c);
				this.textReader.Read();
				c = (char)this.textReader.Peek();
				if ((c == '\r' && stringBuilder[0] == '\n') || (c == '\n' && stringBuilder[0] == '\r'))
				{
					stringBuilder.Append(c);
					this.textReader.Read();
				}
				return new CsvToken(CsvTokenType.EndOfLine, stringBuilder.ToString());
			}
			return null;
		}

		readonly List<Func<CsvToken>> tokenReaders;

		readonly TextReader textReader;

		readonly CsvSettings settings;
	}
}
