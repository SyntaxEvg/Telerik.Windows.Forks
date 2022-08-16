using System;
using System.Collections.Generic;
using CsQuery.StringScanner;
using CsQuery.Utility;

namespace CsQuery.Engine
{
	abstract class PseudoSelector : IPseudoSelector
	{
		protected virtual string[] Parameters { get; set; }

		protected virtual QuotingRule ParameterQuoted(int index)
		{
			return QuotingRule.NeverQuoted;
		}

		public virtual string Arguments
		{
			get
			{
				return this._Arguments;
			}
			set
			{
				string[] parameters = null;
				if (!string.IsNullOrEmpty(value))
				{
					if (this.MaximumParameterCount > 1 || this.MaximumParameterCount < 0)
					{
						parameters = this.ParseArgs(value);
					}
					else
					{
						parameters = new string[] { this.ParseSingleArg(value) };
					}
				}
				this.ValidateParameters(parameters);
				this._Arguments = value;
				this.Parameters = parameters;
			}
		}

		public virtual int MinimumParameterCount
		{
			get
			{
				return 0;
			}
		}

		public virtual int MaximumParameterCount
		{
			get
			{
				return 0;
			}
		}

		public virtual string Name
		{
			get
			{
				return Support.FromCamelCase(base.GetType().Name);
			}
		}

		protected string[] ParseArgs(string value)
		{
			List<string> list = new List<string>();
			int num = 0;
			IStringScanner stringScanner = Scanner.Create(value);
			while (!stringScanner.Finished)
			{
				switch (this.ParameterQuoted(num))
				{
				case QuotingRule.NeverQuoted:
					stringScanner.Seek(',', true);
					break;
				case QuotingRule.AlwaysQuoted:
					stringScanner.Expect(MatchFunctions.Quoted());
					break;
				case QuotingRule.OptionallyQuoted:
					stringScanner.Expect(MatchFunctions.OptionallyQuoted(","));
					break;
				default:
					throw new NotImplementedException("Unimplemented quoting rule");
				}
				list.Add(stringScanner.Match);
				if (!stringScanner.Finished)
				{
					stringScanner.Next();
					num++;
				}
			}
			return list.ToArray();
		}

		protected string ParseSingleArg(string value)
		{
			IStringScanner stringScanner = Scanner.Create(value);
			switch (this.ParameterQuoted(0))
			{
			case QuotingRule.NeverQuoted:
				return value;
			case QuotingRule.AlwaysQuoted:
				stringScanner.Expect(MatchFunctions.Quoted());
				if (!stringScanner.Finished)
				{
					throw new ArgumentException(this.InvalidArgumentsError());
				}
				return stringScanner.Match;
			case QuotingRule.OptionallyQuoted:
				stringScanner.Expect(MatchFunctions.OptionallyQuoted(null));
				if (!stringScanner.Finished)
				{
					throw new ArgumentException(this.InvalidArgumentsError());
				}
				return stringScanner.Match;
			default:
				throw new NotImplementedException("Unimplemented quoting rule");
			}
		}

		protected virtual void ValidateParameters(string[] parameters)
		{
			if (parameters == null)
			{
				if (this.MinimumParameterCount != 0)
				{
					throw new ArgumentException(this.ParameterCountMismatchError());
				}
				return;
			}
			else
			{
				if (parameters.Length < this.MinimumParameterCount || (this.MaximumParameterCount >= 0 && parameters.Length > this.MaximumParameterCount))
				{
					throw new ArgumentException(this.ParameterCountMismatchError());
				}
				return;
			}
		}

		protected string ParameterCountMismatchError()
		{
			if (this.MinimumParameterCount == this.MaximumParameterCount)
			{
				if (this.MinimumParameterCount == 0)
				{
					return string.Format("The :{0} pseudoselector cannot have arguments.", this.Name);
				}
				return string.Format("The :{0} pseudoselector must have exactly {1} arguments.", this.Name, this.MinimumParameterCount);
			}
			else
			{
				if (this.MaximumParameterCount >= 0)
				{
					return string.Format("The :{0} pseudoselector must have between {1} and {2} arguments.", this.Name, this.MinimumParameterCount, this.MaximumParameterCount);
				}
				return string.Format("The :{0} pseudoselector must have between {1} and {2} arguments.", this.Name, this.MinimumParameterCount, this.MaximumParameterCount);
			}
		}

		protected string InvalidArgumentsError()
		{
			return string.Format("The :{0} pseudoselector has some invalid arguments.", this.Name);
		}

		string _Arguments;
	}
}
