using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Core.Data
{
	class ValueMapper<TFrom, TTo>
	{
		public ValueMapper()
		{
			this.fromToToType = new Dictionary<TFrom, TTo>();
			this.toToFromType = new Dictionary<TTo, TFrom>();
		}

		public ValueMapper(TFrom defaultFromValue, TTo defaultToValue)
			: this()
		{
			this.defaultFromValue = new ValueBox<TFrom>(defaultFromValue);
			this.defaultToValue = new ValueBox<TTo>(defaultToValue);
		}

		public int Count
		{
			get
			{
				return this.fromToToType.Count;
			}
		}

		public ValueBox<TFrom> DefaultFromValue
		{
			get
			{
				return this.defaultFromValue;
			}
			set
			{
				this.defaultFromValue = value;
			}
		}

		public ValueBox<TTo> DefaultToValue
		{
			get
			{
				return this.defaultToValue;
			}
			set
			{
				this.defaultToValue = value;
			}
		}

		public IEnumerable<TFrom> FromValues
		{
			get
			{
				return this.fromToToType.Keys;
			}
		}

		public IEnumerable<TTo> ToValues
		{
			get
			{
				return this.toToFromType.Keys;
			}
		}

		public void AddPair(TFrom from, TTo to)
		{
			this.fromToToType[from] = to;
			this.toToFromType[to] = from;
		}

		public TFrom GetFromValue(TTo value)
		{
			TFrom value2;
			if (!this.toToFromType.TryGetValue(value, out value2))
			{
				if (this.defaultFromValue == null)
				{
					throw new InvalidOperationException("Not supported to value: " + value.ToString());
				}
				value2 = this.defaultFromValue.Value;
			}
			return value2;
		}

		public bool TryGetFromValue(TTo value, out TFrom result)
		{
			return this.toToFromType.TryGetValue(value, out result);
		}

		public bool TryGetToValue(TFrom value, out TTo result)
		{
			return this.fromToToType.TryGetValue(value, out result);
		}

		public TTo GetToValue(TFrom value)
		{
			TTo value2;
			if (!this.fromToToType.TryGetValue(value, out value2))
			{
				if (this.defaultToValue == null)
				{
					throw new InvalidOperationException("Not supported from value: " + value.ToString());
				}
				value2 = this.defaultToValue.Value;
			}
			return value2;
		}

		public bool ContainsToValue(TTo to)
		{
			return this.toToFromType.ContainsKey(to);
		}

		public bool ContainsFromValue(TFrom from)
		{
			return this.fromToToType.ContainsKey(from);
		}

		readonly Dictionary<TFrom, TTo> fromToToType;

		readonly Dictionary<TTo, TFrom> toToFromType;

		ValueBox<TFrom> defaultFromValue;

		ValueBox<TTo> defaultToValue;
	}
}
