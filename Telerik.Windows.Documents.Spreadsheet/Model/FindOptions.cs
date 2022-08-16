using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class FindOptions
	{
		public string FindWhat
		{
			get
			{
				if (this.findWhat == null)
				{
					this.findWhat = string.Empty;
				}
				return this.findWhat;
			}
			set
			{
				if (this.findWhat != value)
				{
					this.findWhat = value;
				}
				this.SetFindWhatRegex();
			}
		}

		internal string FindWhatRegex { get; set; }

		internal RegexOptions FindWhatRegexOptions { get; set; }

		public FindWithin FindWithin
		{
			get
			{
				return this.findWithin;
			}
			set
			{
				if (this.findWithin != value)
				{
					this.findWithin = value;
				}
			}
		}

		public FindBy FindBy
		{
			get
			{
				return this.findBy;
			}
			set
			{
				if (this.findBy != value)
				{
					this.findBy = value;
				}
			}
		}

		public FindInContentType FindIn
		{
			get
			{
				return this.findIn;
			}
			set
			{
				if (this.findIn != value)
				{
					this.findIn = value;
				}
			}
		}

		public bool MatchCase
		{
			get
			{
				return this.matchCase;
			}
			set
			{
				if (this.matchCase != value)
				{
					this.matchCase = value;
				}
				this.FindWhatRegexOptions = (this.matchCase ? RegexOptions.None : RegexOptions.IgnoreCase);
			}
		}

		public bool MatchEntireCellContents
		{
			get
			{
				return this.matchEntireCellContents;
			}
			set
			{
				if (this.matchEntireCellContents != value)
				{
					this.matchEntireCellContents = value;
				}
				this.SetFindWhatRegex();
			}
		}

		public WorksheetCellIndex StartCell
		{
			get
			{
				return this.startCell;
			}
			set
			{
				if (this.startCell != value)
				{
					this.startCell = value;
				}
			}
		}

		public IEnumerable<CellRange> SearchRanges
		{
			get
			{
				return this.searchRange;
			}
			set
			{
				if (this.searchRange != value)
				{
					this.searchRange = value;
				}
			}
		}

		public FindOptions()
		{
			this.findWithin = FindWithin.Sheet;
			this.findBy = FindBy.Rows;
			this.findIn = FindInContentType.Formulas;
		}

		public void CopyPropertiesFrom(FindOptions options)
		{
			this.StartCell = options.StartCell;
			this.FindBy = options.FindBy;
			this.FindIn = options.FindIn;
			this.FindWhat = options.FindWhat;
			this.FindWithin = options.FindWithin;
			this.MatchCase = options.MatchCase;
			this.MatchEntireCellContents = options.MatchEntireCellContents;
			this.SearchRanges = options.SearchRanges;
		}

		public override bool Equals(object obj)
		{
			FindOptions findOptions = obj as FindOptions;
			return findOptions != null && (this.StartCell.Equals(findOptions.StartCell) && this.FindWhat.Equals(findOptions.FindWhat) && this.FindWithin.Equals(findOptions.FindWithin) && this.FindBy.Equals(findOptions.FindBy) && this.FindIn.Equals(findOptions.FindIn) && this.MatchCase.Equals(findOptions.MatchCase)) && this.MatchEntireCellContents.Equals(this.MatchEntireCellContents);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.StartCell.GetHashCodeOrZero(), this.FindWhat.GetHashCodeOrZero(), this.FindWithin.GetHashCodeOrZero(), this.FindBy.GetHashCodeOrZero(), this.FindIn.GetHashCodeOrZero(), this.MatchCase.GetHashCodeOrZero(), this.MatchEntireCellContents.GetHashCodeOrZero());
		}

		void SetFindWhatRegex()
		{
			string format = (this.MatchEntireCellContents ? "^{0}$" : "{0}");
			this.FindWhatRegex = string.Format(format, this.FindWhat.FromWildcardStringToRegex());
		}

		string findWhat;

		FindWithin findWithin;

		FindBy findBy;

		FindInContentType findIn;

		bool matchCase;

		bool matchEntireCellContents;

		WorksheetCellIndex startCell;

		IEnumerable<CellRange> searchRange;
	}
}
