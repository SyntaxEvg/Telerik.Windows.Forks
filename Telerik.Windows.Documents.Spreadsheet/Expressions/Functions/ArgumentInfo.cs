using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ArgumentInfo
	{
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public string Description
		{
			get
			{
				return this.description;
			}
		}

		public string NameLocalizationKey
		{
			get
			{
				return this.nameLocalizationKey;
			}
		}

		public string DescriptionLocalizationKey
		{
			get
			{
				return this.descriptionLocalizationKey;
			}
		}

		public ArgumentType Type
		{
			get
			{
				return this.type;
			}
		}

		internal bool IsRequired
		{
			get
			{
				return this.isRequired;
			}
		}

		public ArgumentInfo(string name, string description, ArgumentType type, bool isRequired = true, string nameLocalizationKey = null, string descriptionLocalizationKey = null)
		{
			this.name = name;
			this.description = description;
			this.nameLocalizationKey = nameLocalizationKey;
			this.descriptionLocalizationKey = descriptionLocalizationKey;
			this.type = type;
			this.isRequired = isRequired;
		}

		internal bool IsValid(string value)
		{
			value = value.Trim();
			if (string.IsNullOrEmpty(value))
			{
				return !this.IsRequired;
			}
			switch (this.Type)
			{
			case ArgumentType.Any:
			case ArgumentType.Text:
				return true;
			case ArgumentType.Logical:
				throw new NotImplementedException();
			case ArgumentType.Number:
			case ArgumentType.Reference:
				return ArgumentInfo.IsValidNumber(value);
			default:
				throw new ArgumentException("Invalid argument");
			}
		}

		static bool IsValidNumber(string value)
		{
			double num;
			return double.TryParse(value, out num);
		}

		readonly string name;

		readonly string description;

		readonly string nameLocalizationKey;

		readonly string descriptionLocalizationKey;

		readonly ArgumentType type;

		readonly bool isRequired;
	}
}
