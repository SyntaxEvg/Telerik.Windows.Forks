using System;
using System.Collections.Generic;
using System.Text;

namespace PreMailer.Net
{
	class StyleClass
	{
		public StyleClass()
		{
			this.Attributes = new SortedList<string, string>();
		}

		public string Name { get; set; }

		public SortedList<string, string> Attributes { get; set; }

		public void Merge(StyleClass styleClass, bool canOverwrite)
		{
			foreach (KeyValuePair<string, string> keyValuePair in styleClass.Attributes)
			{
				if (!this.Attributes.ContainsKey(keyValuePair.Key))
				{
					this.Attributes.Add(keyValuePair.Key, keyValuePair.Value);
				}
				else if (canOverwrite)
				{
					this.Attributes[keyValuePair.Key] = keyValuePair.Value;
				}
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> keyValuePair in this.Attributes)
			{
				stringBuilder.AppendFormat("{0}: {1};", keyValuePair.Key, keyValuePair.Value);
			}
			return stringBuilder.ToString();
		}
	}
}
