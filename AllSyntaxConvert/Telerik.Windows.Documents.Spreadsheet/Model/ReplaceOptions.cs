using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class ReplaceOptions : FindOptions
	{
		public string ReplaceWith
		{
			get
			{
				if (this.replaceWith == null)
				{
					this.replaceWith = string.Empty;
				}
				return this.replaceWith;
			}
			set
			{
				if (this.replaceWith != value)
				{
					this.replaceWith = value;
				}
			}
		}

		public void CopyPropertiesFrom(ReplaceOptions options)
		{
			base.CopyPropertiesFrom(options);
			this.ReplaceWith = options.ReplaceWith;
		}

		public override bool Equals(object obj)
		{
			ReplaceOptions replaceOptions = obj as ReplaceOptions;
			return replaceOptions != null && base.Equals(obj) && this.ReplaceWith.Equals(replaceOptions.ReplaceWith);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(base.GetHashCode(), this.ReplaceWith.GetHashCodeOrZero());
		}

		string replaceWith;
	}
}
