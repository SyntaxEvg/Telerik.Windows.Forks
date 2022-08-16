using System;
using System.Text.RegularExpressions;
using CsQuery.ExtensionMethods;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class RegexExtension : PseudoSelectorFilter
	{
		public override bool Matches(IDomObject element)
		{
			switch (this.Mode)
			{
			case RegexExtension.Modes.Data:
				return this.Expression.IsMatch(element.Cq().DataRaw(this.Property) ?? "");
			case RegexExtension.Modes.Css:
				return this.Expression.IsMatch(element.Style[this.Property] ?? "");
			case RegexExtension.Modes.Attr:
				return this.Expression.IsMatch(element[this.Property] ?? "");
			default:
				throw new NotImplementedException();
			}
		}

		void Configure()
		{
			Regex regex = new Regex("^(data|css):");
			if (regex.IsMatch(this.Parameters[0]))
			{
				string[] array = this.Parameters[0].Split(new char[] { ':' });
				string a = array[0];
				if (a == "data")
				{
					this.Mode = RegexExtension.Modes.Data;
				}
				else
				{
					if (!(a == "css"))
					{
						throw new ArgumentException("Unknown mode for regex pseudoselector.");
					}
					this.Mode = RegexExtension.Modes.Css;
				}
				this.Property = array[1];
			}
			else
			{
				this.Mode = RegexExtension.Modes.Attr;
				this.Property = this.Parameters[0];
			}
			this.Expression = new Regex(this.Parameters[1].RegexReplace("^\\s+|\\s+$", ""), RegexOptions.IgnoreCase | RegexOptions.Multiline);
		}

		public override string Arguments
		{
			get
			{
				return base.Arguments;
			}
			set
			{
				base.Arguments = value;
				this.Configure();
			}
		}

		protected override QuotingRule ParameterQuoted(int index)
		{
			return QuotingRule.OptionallyQuoted;
		}

		public override int MaximumParameterCount
		{
			get
			{
				return 2;
			}
		}

		public override int MinimumParameterCount
		{
			get
			{
				return 2;
			}
		}

		public override string Name
		{
			get
			{
				return "regex";
			}
		}

		string Property;

		RegexExtension.Modes Mode;

		Regex Expression;

		enum Modes
		{
			Data = 1,
			Css,
			Attr
		}
	}
}
