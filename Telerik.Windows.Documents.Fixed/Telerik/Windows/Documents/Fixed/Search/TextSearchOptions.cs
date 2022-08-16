using System;
using System.ComponentModel;

namespace Telerik.Windows.Documents.Fixed.Search
{
	public class TextSearchOptions : INotifyPropertyChanged
	{
		public static TextSearchOptions Default
		{
			get
			{
				return new TextSearchOptions(false, false, false);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public TextSearchOptions()
			: this(false)
		{
		}

		public TextSearchOptions(bool caseSensitive)
			: this(caseSensitive, false)
		{
		}

		public TextSearchOptions(bool caseSensitive, bool useRegularExpression)
			: this(caseSensitive, useRegularExpression, false)
		{
		}

		public TextSearchOptions(bool caseSensitive, bool useRegularExpression, bool wholeWordsOnly)
		{
			this.CaseSensitive = caseSensitive;
			this.UseRegularExpression = useRegularExpression;
			this.WholeWordsOnly = wholeWordsOnly;
		}

		public bool UseRegularExpression
		{
			get
			{
				return this.useRegularExpression;
			}
			set
			{
				if (value != this.useRegularExpression)
				{
					this.useRegularExpression = value;
					this.OnPropertyChanged("UseRegularExpression");
				}
			}
		}

		public bool CaseSensitive
		{
			get
			{
				return this.caseSensitive;
			}
			set
			{
				if (value != this.caseSensitive)
				{
					this.caseSensitive = value;
					this.OnPropertyChanged("CaseSensitive");
				}
			}
		}

		public bool WholeWordsOnly
		{
			get
			{
				return this.wholeWordsOnly;
			}
			set
			{
				if (value != this.wholeWordsOnly)
				{
					this.wholeWordsOnly = value;
					this.OnPropertyChanged("WholeWordsOnly");
				}
			}
		}

		void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		internal static readonly string TextSearchLinesSeparator = " ";

		bool useRegularExpression;

		bool caseSensitive;

		bool wholeWordsOnly;
	}
}
