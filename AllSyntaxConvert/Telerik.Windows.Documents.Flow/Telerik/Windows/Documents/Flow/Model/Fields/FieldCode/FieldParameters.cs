using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Flow.Model.Fields.FieldCode
{
	public class FieldParameters
	{
		public FieldParameters()
		{
			this.switches = new Dictionary<string, FieldSwitch>();
			this.fieldArguments = new List<FieldArgument>();
		}

		public FieldArgument FirstArgument
		{
			get
			{
				return this.fieldArguments.FirstOrDefault<FieldArgument>();
			}
		}

		public FieldArgument SecondArgument
		{
			get
			{
				if (this.fieldArguments.Count < 2)
				{
					return null;
				}
				return this.fieldArguments[1];
			}
		}

		public string Expression { get; internal set; }

		public FieldComparison Comparison { get; internal set; }

		public bool TryGetSwitch(string switchKey, out FieldSwitch result)
		{
			return this.switches.TryGetValue(switchKey, out result);
		}

		public string GetSwitchArgument(string switchKey)
		{
			string result = null;
			FieldSwitch fieldSwitch = null;
			if (this.TryGetSwitch(switchKey.ToString(), out fieldSwitch))
			{
				result = ((fieldSwitch.Argument != null) ? fieldSwitch.Argument.Value : string.Empty);
			}
			return result;
		}

		public bool IsSwitchDefined(string switchKey)
		{
			return this.switches.ContainsKey(switchKey.ToString());
		}

		internal void AddSwitch(FieldSwitch fieldSwitch)
		{
			this.switches[fieldSwitch.SwitchValue] = fieldSwitch;
		}

		internal void AddArgument(FieldArgument value)
		{
			this.fieldArguments.Add(value);
		}

		readonly Dictionary<string, FieldSwitch> switches;

		readonly List<FieldArgument> fieldArguments;
	}
}
