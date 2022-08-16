using System;
using System.Globalization;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model
{
	sealed class RtfTag : RtfElement
	{
		public RtfTag(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.fullName = name;
			this.name = name;
			this.valueAsText = null;
			this.valueAsNumber = -1;
		}

		public RtfTag(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.fullName = string.Format("{0}{1}", name, value);
			this.name = name;
			this.valueAsText = value;
			int num;
			if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
			{
				this.valueAsNumber = num;
				return;
			}
			this.valueAsNumber = -1;
		}

		public override RtfElementType Type
		{
			get
			{
				return RtfElementType.Tag;
			}
		}

		public string FullName
		{
			get
			{
				return this.fullName;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public bool HasValue
		{
			get
			{
				return this.valueAsText != null;
			}
		}

		public string ValueAsText
		{
			get
			{
				return this.valueAsText;
			}
		}

		public int ValueAsNumber
		{
			get
			{
				return this.valueAsNumber;
			}
		}

		public override string ToString()
		{
			return string.Format("\\{0}", this.fullName);
		}

		protected override bool IsEqual(object obj)
		{
			RtfTag rtfTag = obj as RtfTag;
			return rtfTag != null && base.IsEqual(obj) && this.fullName.Equals(rtfTag.fullName);
		}

		protected override int ComputeHashCode()
		{
			int hash = base.ComputeHashCode();
			return HashTool.AddHashCode(hash, this.fullName);
		}

		readonly string fullName;

		readonly string name;

		readonly string valueAsText;

		readonly int valueAsNumber;
	}
}
