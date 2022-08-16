using System;
using Telerik.Windows.Documents.Flow.Model.Fields.Expressions;
using Telerik.Windows.Documents.Flow.Model.Fields.FieldCode;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	public abstract class ComparisonFieldBase : Field
	{
		protected ComparisonFieldBase(RadFlowDocument document)
			: base(document)
		{
		}

		internal override bool ExpectComparisonArgument
		{
			get
			{
				return true;
			}
		}

		internal static ComparisonFieldResult Compare(FieldComparison comparison, RadFlowDocument document)
		{
			if (!ComparisonFieldBase.IsComaprisonOperatorvalid(comparison.Operator))
			{
				return new ComparisonFieldResult(string.Format("Invalid comparison operator: {0}", comparison.Operator), true);
			}
			if (string.IsNullOrEmpty(comparison.LeftArgument))
			{
				return new ComparisonFieldResult(string.Format("Missing left argument{0}", comparison.Operator), true);
			}
			if (string.IsNullOrEmpty(comparison.RightArgument))
			{
				return new ComparisonFieldResult(string.Format("Missing right argument{0}", comparison.Operator), true);
			}
			bool flag;
			if (comparison.IsLeftArgumentQuoted)
			{
				flag = ComparisonFieldBase.CompareArgumentsAsString(comparison);
			}
			else
			{
				flag = ComparisonFieldBase.CompareArgumentsAsExpressions(comparison, document);
			}
			return new ComparisonFieldResult(flag ? "1" : "0")
			{
				CompareValue = flag
			};
		}

		static bool CompareArgumentsAsExpressions(FieldComparison comparison, RadFlowDocument document)
		{
			Expression expression = ExpressionParser.Parse(comparison.LeftArgument, document);
			Expression expression2 = ExpressionParser.Parse(comparison.RightArgument, document);
			double value = expression.GetResult().Value;
			double value2 = expression2.GetResult().Value;
			double res = value - value2;
			return ComparisonFieldBase.CompareCore(comparison.Operator, res);
		}

		static bool CompareArgumentsAsString(FieldComparison comparison)
		{
			int num = string.Compare(comparison.LeftArgument, comparison.RightArgument, StringComparison.Ordinal);
			return ComparisonFieldBase.CompareCore(comparison.Operator, (double)num);
		}

		static bool CompareCore(string op, double res)
		{
			bool result = false;
			if (op != null)
			{
				if (!(op == "="))
				{
					if (!(op == "<>"))
					{
						if (!(op == ">"))
						{
							if (!(op == ">="))
							{
								if (!(op == "<"))
								{
									if (op == "<=")
									{
										result = res <= 0.0;
									}
								}
								else
								{
									result = res < 0.0;
								}
							}
							else
							{
								result = res >= 0.0;
							}
						}
						else
						{
							result = res > 0.0;
						}
					}
					else
					{
						result = res != 0.0;
					}
				}
				else
				{
					result = res == 0.0;
				}
			}
			return result;
		}

		static bool IsComaprisonOperatorvalid(string op)
		{
			return string.Equals(op, "=", StringComparison.Ordinal) || string.Equals(op, "<>", StringComparison.Ordinal) || string.Equals(op, ">", StringComparison.Ordinal) || string.Equals(op, ">=", StringComparison.Ordinal) || string.Equals(op, "<", StringComparison.Ordinal) || string.Equals(op, "<=", StringComparison.Ordinal);
		}
	}
}
