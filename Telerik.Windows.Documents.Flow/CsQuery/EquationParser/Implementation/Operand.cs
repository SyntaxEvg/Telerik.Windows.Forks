using System;

namespace CsQuery.EquationParser.Implementation
{
	abstract class Operand : IOperand, IConvertible, ICloneable
	{
		public IOperand Clone()
		{
			return this.CopyTo(this.GetNewInstance());
		}

		protected abstract IOperand GetNewInstance();

		protected abstract IOperand CopyTo(IOperand operand);

		protected virtual bool IsValidType(Type type)
		{
			return Utils.IsNumericType(type);
		}

		protected virtual bool AllowNullValues(Type type)
		{
			return Utils.IsNumericType(type);
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		public IConvertible Value
		{
			get
			{
				return this.GetValue();
			}
		}

		protected abstract IConvertible GetValue();

		public virtual bool IsInteger
		{
			get
			{
				if (this._IsInt != null)
				{
					return this._IsInt.Value;
				}
				return Utils.IsIntegralValue(this.Value);
			}
		}

		public bool IsFloatingPoint
		{
			get
			{
				return this.IsNumber && !this.IsInteger;
			}
		}

		public bool IsNumber
		{
			get
			{
				if (this._IsNumber != null)
				{
					return this._IsNumber.Value;
				}
				return Utils.IsNumericType(this.Value);
			}
		}

		public bool IsText
		{
			get
			{
				if (this._IsText != null)
				{
					return this._IsText.Value;
				}
				return this.Value is string;
			}
		}

		public bool IsBoolean
		{
			get
			{
				if (this._IsBoolean != null)
				{
					return this._IsBoolean.Value;
				}
				return this.Value is bool;
			}
		}

		public virtual TypeCode GetTypeCode()
		{
			return this.Value.GetTypeCode();
		}

		public virtual bool ToBoolean(IFormatProvider provider)
		{
			return this.intValue != 0;
		}

		public virtual byte ToByte(IFormatProvider provider)
		{
			if (this.intValue >= 0 || this.intValue < 255)
			{
				return (byte)this.intValue;
			}
			return this.ConversionException<byte>();
		}

		public virtual char ToChar(IFormatProvider provider)
		{
			if (this.intValue < 0 || this.intValue > 65535)
			{
				return (char)this.intValue;
			}
			return this.ConversionException<char>();
		}

		public virtual DateTime ToDateTime(IFormatProvider provider)
		{
			return this.ConversionException<DateTime>();
		}

		public virtual decimal ToDecimal(IFormatProvider provider)
		{
			return (decimal)this.doubleValue;
		}

		public virtual double ToDouble(IFormatProvider provider)
		{
			return this.doubleValue;
		}

		public virtual short ToInt16(IFormatProvider provider)
		{
			if (this.intValue < -32768 || this.intValue > 32767)
			{
				return this.ConversionException<short>();
			}
			return (short)this.intValue;
		}

		public virtual int ToInt32(IFormatProvider provider)
		{
			return this.intValue;
		}

		public virtual long ToInt64(IFormatProvider provider)
		{
			return (long)this.intValue;
		}

		public virtual sbyte ToSByte(IFormatProvider provider)
		{
			return (sbyte)Convert.ChangeType(this.intValue, typeof(sbyte));
		}

		public virtual float ToSingle(IFormatProvider provider)
		{
			return (float)Convert.ChangeType(this.doubleValue, typeof(sbyte));
		}

		public virtual string ToString(IFormatProvider provider)
		{
			return this.Value.ToString();
		}

		public virtual object ToType(Type conversionType, IFormatProvider provider)
		{
			return Convert.ChangeType(this.Value, conversionType);
		}

		public virtual ushort ToUInt16(IFormatProvider provider)
		{
			return (ushort)Convert.ChangeType(this.intValue, typeof(ushort));
		}

		public virtual uint ToUInt32(IFormatProvider provider)
		{
			return (uint)Convert.ChangeType(this.intValue, typeof(uint));
		}

		public virtual ulong ToUInt64(IFormatProvider provider)
		{
			return (ulong)Convert.ChangeType(this.intValue, typeof(ulong));
		}

		protected U ConversionException<U>()
		{
			throw new InvalidCastException(string.Concat(new object[]
			{
				"Cannot convert value '",
				this.Value,
				"' to type ",
				typeof(U).ToString()
			}));
		}

		protected int intValue;

		protected double doubleValue;

		protected bool? _IsNumber;

		protected bool? _IsInt;

		protected bool? _IsText;

		protected bool? _IsBoolean;
	}
}
