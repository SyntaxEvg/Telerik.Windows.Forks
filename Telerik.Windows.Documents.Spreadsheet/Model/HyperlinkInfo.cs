using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class HyperlinkInfo
	{
		public string Address
		{
			get
			{
				return this.address;
			}
		}

		public string EmailSubject
		{
			get
			{
				return this.emailSubject;
			}
		}

		public string ScreenTip
		{
			get
			{
				return this.screenTip;
			}
		}

		public string SubAddress
		{
			get
			{
				return this.subAddress;
			}
		}

		public HyperlinkType Type
		{
			get
			{
				return this.type;
			}
		}

		HyperlinkInfo(string address, string subAddress, string emailSubject, HyperlinkType type, string screenTip = null)
		{
			this.address = address;
			this.subAddress = subAddress;
			this.emailSubject = emailSubject;
			this.screenTip = screenTip;
			this.type = type;
		}

		public static HyperlinkInfo CreateHyperlink(string address, string screenTip = null)
		{
			return new HyperlinkInfo(address, string.Empty, string.Empty, HyperlinkType.Url, screenTip);
		}

		public static HyperlinkInfo CreateMailtoHyperlink(string emailAddress)
		{
			return new HyperlinkInfo(emailAddress, string.Empty, string.Empty, HyperlinkType.Mailto, null);
		}

		public static HyperlinkInfo CreateMailtoHyperlink(string emailAddress, string emailSubject)
		{
			return new HyperlinkInfo(emailAddress, string.Empty, emailSubject, HyperlinkType.Mailto, null);
		}

		public static HyperlinkInfo CreateMailtoHyperlink(string emailAddress, string emailSubject, string screenTip)
		{
			return new HyperlinkInfo(emailAddress, string.Empty, emailSubject, HyperlinkType.Mailto, screenTip);
		}

		public static HyperlinkInfo CreateInDocumentHyperlink(string subAddress, string screenTip = null)
		{
			return new HyperlinkInfo(string.Empty, subAddress, string.Empty, HyperlinkType.InDocument, screenTip);
		}

		internal string GetHyperlinkAsString()
		{
			switch (this.type)
			{
			case HyperlinkType.Url:
				return this.Address;
			case HyperlinkType.Mailto:
				return this.GetEmailAddress();
			case HyperlinkType.InDocument:
				return this.SubAddress;
			default:
				throw new InvalidOperationException();
			}
		}

		internal string GetEmailAddress()
		{
			return HyperlinkInfo.GetEmailAddress(this.Address, this.emailSubject);
		}

		internal static string GetEmailAddress(string email, string emailSubject)
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(email))
			{
				text += string.Format("mailto:{0}", email);
				if (!string.IsNullOrEmpty(emailSubject))
				{
					text += string.Format("?Subject={0}", emailSubject);
				}
			}
			return text;
		}

		public override bool Equals(object obj)
		{
			HyperlinkInfo hyperlinkInfo = obj as HyperlinkInfo;
			return hyperlinkInfo != null && (TelerikHelper.EqualsOfT<string>(this.Address, hyperlinkInfo.Address) && TelerikHelper.EqualsOfT<string>(this.EmailSubject, hyperlinkInfo.EmailSubject) && TelerikHelper.EqualsOfT<string>(this.ScreenTip, hyperlinkInfo.ScreenTip) && TelerikHelper.EqualsOfT<string>(this.SubAddress, hyperlinkInfo.SubAddress)) && this.Type.Equals(hyperlinkInfo.Type);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.Address.GetHashCodeOrZero(), this.EmailSubject.GetHashCodeOrZero(), this.ScreenTip.GetHashCodeOrZero(), this.SubAddress.GetHashCodeOrZero(), this.Type.GetHashCodeOrZero());
		}

		readonly string address;

		readonly string emailSubject;

		readonly string screenTip;

		readonly string subAddress;

		readonly HyperlinkType type;
	}
}
