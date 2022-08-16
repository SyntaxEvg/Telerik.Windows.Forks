using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Parser
{
	class RtfCodePageDecoder
	{
		static RtfCodePageDecoder()
		{
			foreach (int num in RtfCodePageDecoder.CodePages)
			{
				RtfCodePageDecoder.CodePagesData[num] = RtfCodePageDecoder.ReadCodePage(num);
			}
		}

		public RtfCodePageDecoder(int codePage)
		{
			this.CurrentCodePage = codePage;
		}

		public int CurrentCodePage
		{
			get
			{
				return this.currentCodePage;
			}
			set
			{
				this.currentCodePage = value;
				if (!RtfCodePageDecoder.CodePagesData.TryGetValue(this.currentCodePage, out this.codePageData))
				{
					this.codePageData = RtfCodePageDecoder.CodePagesData[CharSetHelper.AnsiCodePage];
				}
			}
		}

		public char Convert(int b)
		{
			if (this.codePageData.Length <= b)
			{
				return '?';
			}
			return this.codePageData[b];
		}

		static string ReadCodePage(int codePage)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string result;
			using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(typeof(RtfCodePageDecoder), string.Format("{0}.{1}{2}", "CodePages", codePage, ".bin")))
			{
				using (StreamReader streamReader = new StreamReader(manifestResourceStream))
				{
					result = streamReader.ReadToEnd();
				}
			}
			return result;
		}

		const string MetafilesRelativePath = "CodePages";

		const string MetafilesExtension = ".bin";

		static readonly int[] CodePages = new int[]
		{
			437, 708, 720, 850, 852, 860, 862, 863, 864, 865,
			866, 874, 932, 936, 949, 950, 1250, 1251, 1252, 1253,
			1254, 1255, 1256, 1257, 1258, 1361, 10000, 10001, 10004, 10005,
			10006, 10007, 10029, 10081, 57002, 57003, 57004, 57005, 57006, 57007,
			57008, 57009, 57010, 57011
		};

		static readonly Dictionary<int, string> CodePagesData = new Dictionary<int, string>();

		string codePageData;

		int currentCodePage;
	}
}
