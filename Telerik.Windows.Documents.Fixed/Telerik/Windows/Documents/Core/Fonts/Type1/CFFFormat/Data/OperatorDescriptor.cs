using System;
using System.Text;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.Data
{
	class OperatorDescriptor
	{
		public OperatorDescriptor(byte b0)
		{
			this.value = new byte[1];
			this.value[0] = b0;
			this.CalculateHashCode();
		}

		public OperatorDescriptor(byte[] bytes)
		{
			this.value = bytes;
			this.CalculateHashCode();
		}

		public OperatorDescriptor(byte b0, object defaultValue)
			: this(b0)
		{
			this.defaultValue = defaultValue;
		}

		public OperatorDescriptor(byte[] bytes, object defaultValue)
			: this(bytes)
		{
			this.defaultValue = defaultValue;
		}

		public object DefaultValue
		{
			get
			{
				return this.defaultValue;
			}
		}

		void CalculateHashCode()
		{
			this.hashCode = 17;
			for (int i = 0; i < this.value.Length; i++)
			{
				this.hashCode = this.hashCode * 23 + (int)this.value[i];
			}
		}

		public override bool Equals(object obj)
		{
			OperatorDescriptor operatorDescriptor = obj as OperatorDescriptor;
			if (operatorDescriptor == null || this.value.Length != operatorDescriptor.value.Length)
			{
				return false;
			}
			for (int i = 0; i < this.value.Length; i++)
			{
				if (this.value[i] != operatorDescriptor.value[i])
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			return this.hashCode;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (int num in this.value)
			{
				stringBuilder.Append(num);
				stringBuilder.Append(" ");
			}
			return stringBuilder.ToString().Trim();
		}

		internal const byte TwoByteOperatorFirstByte = 12;

		readonly byte[] value;

		readonly object defaultValue;

		int hashCode;
	}
}
