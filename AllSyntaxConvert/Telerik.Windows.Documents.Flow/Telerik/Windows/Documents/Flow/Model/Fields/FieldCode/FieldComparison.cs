using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.FieldCode
{
	public class FieldComparison
	{
		internal FieldComparison(string left, bool isLeftQuoted, string op, string right, bool isRightQuoted)
		{
			this.LeftArgument = left;
			this.IsLeftArgumentQuoted = isLeftQuoted;
			this.Operator = op;
			this.RightArgument = right;
			this.IsRightArgumentQuoted = isRightQuoted;
		}

		public string LeftArgument { get; set; }

		public bool IsLeftArgumentQuoted { get; set; }

		public string Operator { get; set; }

		public string RightArgument { get; set; }

		public bool IsRightArgumentQuoted { get; set; }
	}
}
