using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Parser
{
	class RtfParserListenerBase
	{
		public int Level { get; set; }

		public void ParseBegin()
		{
			this.Level = 0;
			this.DoParseBegin();
		}

		public void GroupBegin()
		{
			this.DoGroupBegin();
			this.Level++;
		}

		public void TagFound(RtfTag tag)
		{
			if (tag != null)
			{
				this.DoTagFound(tag);
			}
		}

		public void TextFound(RtfText text)
		{
			if (text != null)
			{
				this.DoTextFound(text);
			}
		}

		public void BinaryFound(RtfBinary bin)
		{
			if (bin != null)
			{
				this.DoBinaryFound(bin);
			}
		}

		public void GroupEnd()
		{
			this.Level--;
			this.DoGroupEnd();
		}

		public void ParseSuccess()
		{
			this.DoParseSuccess();
		}

		public void ParseFail(RtfException reason)
		{
			this.DoParseFail(reason);
		}

		public void ParseEnd()
		{
			this.DoParseEnd();
		}

		protected virtual void DoParseBegin()
		{
		}

		protected virtual void DoGroupBegin()
		{
		}

		protected virtual void DoTagFound(RtfTag tag)
		{
		}

		protected virtual void DoTextFound(RtfText text)
		{
		}

		protected virtual void DoBinaryFound(RtfBinary bin)
		{
		}

		protected virtual void DoGroupEnd()
		{
		}

		protected virtual void DoParseSuccess()
		{
		}

		protected virtual void DoParseFail(RtfException reason)
		{
		}

		protected virtual void DoParseEnd()
		{
		}
	}
}
