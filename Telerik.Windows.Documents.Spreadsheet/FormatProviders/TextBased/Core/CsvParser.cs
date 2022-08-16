using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Core
{
	class CsvParser
	{
		public CsvParser(TextReader reader, CsvSettings settings = null)
		{
			this.tokenizer = new CsvTokenizer(reader, settings);
		}

		public IEnumerable<CsvRecord> Parse()
		{
			while (!this.tokenizer.IsAtEndOfFile)
			{
				IEnumerable<CsvToken> recordTokens = this.ReadRecordTokens();
				yield return new CsvRecord(CsvParser.ParseRecordTokens(recordTokens));
			}
			yield break;
		}

		IEnumerable<CsvToken> ReadRecordTokens()
		{
			bool isEndOfLineValid = true;
			CsvToken token = this.tokenizer.Read();
			while (token != null && (token.TokenType != CsvTokenType.EndOfLine || !isEndOfLineValid))
			{
				if (token.TokenType == CsvTokenType.Quote)
				{
					isEndOfLineValid = !isEndOfLineValid;
				}
				yield return token;
				token = this.tokenizer.Read();
			}
			yield break;
		}

		static IEnumerable<string> ParseRecordTokens(IEnumerable<CsvToken> recordTokens)
		{
			bool isSpecialCharValid = true;
			List<CsvToken> value = new List<CsvToken>();
			foreach (CsvToken token in recordTokens)
			{
				if (token.TokenType == CsvTokenType.Quote)
				{
					isSpecialCharValid = !isSpecialCharValid;
				}
				if (token.TokenType == CsvTokenType.Delimiter && isSpecialCharValid)
				{
					yield return CsvParser.ParseValueTokens(value);
					value.Clear();
				}
				else
				{
					value.Add(token);
				}
			}
			yield return CsvParser.ParseValueTokens(value);
			yield break;
		}

		static string ParseValueTokens(List<CsvToken> valueTokens)
		{
			bool flag = false;
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (CsvToken csvToken in valueTokens)
			{
				if (csvToken.TokenType != CsvTokenType.Quote || (num > 1 && flag))
				{
					stringBuilder.Append(csvToken.Value);
				}
				if (csvToken.TokenType == CsvTokenType.Quote)
				{
					num++;
				}
				flag = (!flag || num <= 2) && csvToken.TokenType == CsvTokenType.Quote;
			}
			return stringBuilder.ToString();
		}

		readonly CsvTokenizer tokenizer;
	}
}
