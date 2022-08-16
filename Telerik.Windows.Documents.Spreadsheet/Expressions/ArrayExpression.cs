using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class ArrayExpression : ConstantExpression<RadExpression[,]>, IEnumerable<RadExpression>, IEnumerable
	{
		public int RowCount
		{
			get
			{
				return this.array.GetLength(0);
			}
		}

		public int ColumnCount
		{
			get
			{
				return this.array.GetLength(1);
			}
		}

		public RadExpression this[int rowIndex, int columnIndex]
		{
			get
			{
				return this.array[rowIndex, columnIndex];
			}
		}

		public ArrayExpression(RadExpression[,] array)
			: base(array)
		{
			Guard.ThrowExceptionIfNull<RadExpression[,]>(array, "array");
			this.arrayExpressionToStrings = new Dictionary<CultureInfo, string>();
			this.array = array;
			base.AttachToChildrenEvent(this);
			this.ToConstant();
		}

		protected override RadExpression GetValueOverride()
		{
			RadExpression[,] array = new RadExpression[this.RowCount, this.ColumnCount];
			for (int i = 0; i < this.RowCount; i++)
			{
				for (int j = 0; j < this.ColumnCount; j++)
				{
					array[i, j] = this[i, j].GetValueAsConstantExpression();
				}
			}
			return new ArrayExpression(array);
		}

		internal ErrorExpression GetContainingError()
		{
			for (int i = 0; i < this.RowCount; i++)
			{
				for (int j = 0; j < this.ColumnCount; j++)
				{
					ErrorExpression errorExpression = this.array[i, j] as ErrorExpression;
					if (errorExpression != null)
					{
						return errorExpression;
					}
					ArrayExpression arrayExpression = this.array[i, j].GetValueAsConstant() as ArrayExpression;
					if (arrayExpression != null)
					{
						ErrorExpression containingError = arrayExpression.GetContainingError();
						if (containingError != null)
						{
							return containingError;
						}
					}
				}
			}
			return null;
		}

		internal ArrayExpression GetRowAsArrayExpression(int rowIndex)
		{
			RadExpression[,] array = new RadExpression[1, this.ColumnCount];
			int columnCount = this.ColumnCount;
			for (int i = 0; i < columnCount; i++)
			{
				array[0, i] = this[rowIndex, i];
			}
			return new ArrayExpression(array);
		}

		internal ArrayExpression GetColumnAsArrayExpression(int columnIndex)
		{
			RadExpression[,] array = new RadExpression[this.RowCount, 1];
			int rowCount = this.RowCount;
			for (int i = 0; i < rowCount; i++)
			{
				array[i, 0] = this[i, columnIndex];
			}
			return new ArrayExpression(array);
		}

		internal override string GetValueAsString(SpreadsheetCultureHelper cultureInfo)
		{
			return this.ToString(cultureInfo);
		}

		internal override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			string text;
			if (!this.arrayExpressionToStrings.TryGetValue(cultureInfo.CultureInfo, out text))
			{
				text = this.BuildExpressionToString(cultureInfo);
				this.arrayExpressionToStrings[cultureInfo.CultureInfo] = text;
			}
			return text;
		}

		void ToConstant()
		{
			for (int i = 0; i < this.RowCount; i++)
			{
				for (int j = 0; j < this.ColumnCount; j++)
				{
					this.array[i, j] = this.array[i, j].GetValue();
				}
			}
		}

		string BuildExpressionToString(SpreadsheetCultureHelper cultureInfo)
		{
			StringBuilder stringBuilder = new StringBuilder("{");
			for (int i = 0; i < this.RowCount; i++)
			{
				for (int j = 0; j < this.ColumnCount; j++)
				{
					stringBuilder.Append(this[i, j].ToString(cultureInfo));
					if (j != this.ColumnCount - 1)
					{
						stringBuilder.Append(cultureInfo.ArrayListSeparator);
					}
				}
				if (i != this.RowCount - 1)
				{
					stringBuilder.Append(cultureInfo.ArrayRowSeparator);
				}
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		internal override bool SimilarEquals(object obj)
		{
			ArrayExpression arrayExpression = obj as ArrayExpression;
			if (arrayExpression == null)
			{
				return false;
			}
			if (this.RowCount != arrayExpression.RowCount || this.ColumnCount != arrayExpression.ColumnCount)
			{
				return false;
			}
			for (int i = 0; i < this.RowCount; i++)
			{
				for (int j = 0; j < this.ColumnCount; j++)
				{
					if (!this[i, j].SimilarEquals(arrayExpression[i, j]))
					{
						return false;
					}
				}
			}
			return true;
		}

		public IEnumerator<RadExpression> GetEnumerator()
		{
			for (int i = 0; i < this.array.GetLength(0); i++)
			{
				for (int j = 0; j < this.array.GetLength(1); j++)
				{
					yield return this.array[i, j];
				}
			}
			yield break;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.array.GetEnumerator();
		}

		readonly RadExpression[,] array;

		readonly Dictionary<CultureInfo, string> arrayExpressionToStrings;
	}
}
