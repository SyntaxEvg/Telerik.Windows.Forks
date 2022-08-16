using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Parser
{
	abstract class RtfParserBase
	{
		protected RtfParserBase()
		{
		}

		protected RtfParserBase(params RtfParserListenerBase[] listeners)
		{
			if (listeners != null)
			{
				foreach (RtfParserListenerBase listener in listeners)
				{
					this.AddParserListener(listener);
				}
			}
		}

		public bool IgnoreContentAfterRootGroup { get; set; }

		public void AddParserListener(RtfParserListenerBase listener)
		{
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			if (this.listeners == null)
			{
				this.listeners = new List<RtfParserListenerBase>();
			}
			if (!this.listeners.Contains(listener))
			{
				this.listeners.Add(listener);
			}
		}

		public void RemoveParserListener(RtfParserListenerBase listener)
		{
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			if (this.listeners != null)
			{
				if (this.listeners.Contains(listener))
				{
					this.listeners.Remove(listener);
				}
				if (this.listeners.Count == 0)
				{
					this.listeners = null;
				}
			}
		}

		public void Parse(RtfSource rtfTextSource)
		{
			if (rtfTextSource == null)
			{
				throw new ArgumentNullException("rtfTextSource");
			}
			this.DoParse(rtfTextSource);
		}

		protected abstract void DoParse(RtfSource rtfTextSource);

		protected void NotifyParseBegin()
		{
			if (this.listeners != null)
			{
				foreach (RtfParserListenerBase rtfParserListenerBase in this.listeners)
				{
					rtfParserListenerBase.ParseBegin();
				}
			}
		}

		protected void NotifyGroupBegin()
		{
			if (this.listeners != null)
			{
				foreach (RtfParserListenerBase rtfParserListenerBase in this.listeners)
				{
					rtfParserListenerBase.GroupBegin();
				}
			}
		}

		protected void NotifyTagFound(RtfTag tag)
		{
			if (this.listeners != null)
			{
				foreach (RtfParserListenerBase rtfParserListenerBase in this.listeners)
				{
					rtfParserListenerBase.TagFound(tag);
				}
			}
		}

		protected void NotifyBinaryFound(RtfBinary bin)
		{
			if (this.listeners != null)
			{
				foreach (RtfParserListenerBase rtfParserListenerBase in this.listeners)
				{
					rtfParserListenerBase.BinaryFound(bin);
				}
			}
		}

		protected void NotifyTextFound(RtfText text)
		{
			if (this.listeners != null)
			{
				foreach (RtfParserListenerBase rtfParserListenerBase in this.listeners)
				{
					rtfParserListenerBase.TextFound(text);
				}
			}
		}

		protected void NotifyGroupEnd()
		{
			if (this.listeners != null)
			{
				foreach (RtfParserListenerBase rtfParserListenerBase in this.listeners)
				{
					rtfParserListenerBase.GroupEnd();
				}
			}
		}

		protected void NotifyParseSuccess()
		{
			if (this.listeners != null)
			{
				foreach (RtfParserListenerBase rtfParserListenerBase in this.listeners)
				{
					rtfParserListenerBase.ParseSuccess();
				}
			}
		}

		protected void NotifyParseFail(RtfException reason)
		{
			if (this.listeners != null)
			{
				foreach (RtfParserListenerBase rtfParserListenerBase in this.listeners)
				{
					rtfParserListenerBase.ParseFail(reason);
				}
			}
		}

		protected void NotifyParseEnd()
		{
			if (this.listeners != null)
			{
				foreach (RtfParserListenerBase rtfParserListenerBase in this.listeners)
				{
					rtfParserListenerBase.ParseEnd();
				}
			}
		}

		List<RtfParserListenerBase> listeners;
	}
}
