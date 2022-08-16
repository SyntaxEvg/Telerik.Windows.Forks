using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	class ComparisonFieldResult : FieldResult
	{
		internal ComparisonFieldResult(string result, bool error)
			: base(result, error)
		{
		}

		internal ComparisonFieldResult(string result)
			: base(result)
		{
		}

		public bool CompareValue { get; internal set; }
	}
}
