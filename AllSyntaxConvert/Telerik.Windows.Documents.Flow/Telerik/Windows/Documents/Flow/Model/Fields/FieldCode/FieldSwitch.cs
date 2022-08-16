using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.FieldCode
{
	public class FieldSwitch
	{
		public FieldSwitch(string switchValue)
		{
			this.switchValue = switchValue;
		}

		public FieldArgument Argument
		{
			get
			{
				return this.argument;
			}
			set
			{
				this.argument = value;
			}
		}

		public string SwitchValue
		{
			get
			{
				return this.switchValue;
			}
		}

		readonly string switchValue;

		FieldArgument argument;
	}
}
