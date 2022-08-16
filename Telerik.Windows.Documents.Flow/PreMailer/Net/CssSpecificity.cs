using System;

namespace PreMailer.Net
{
	class CssSpecificity
	{
		public int Ids { get; protected set; }

		public int ClassesAttributesPseudoElements { get; protected set; }

		public int Elements { get; protected set; }

		public static CssSpecificity None
		{
			get
			{
				return new CssSpecificity(0, 0, 0);
			}
		}

		internal int Classes { get; set; }

		public CssSpecificity(int ids, int classesAttributesPseudoElements, int elements)
		{
			this.Ids = ids;
			this.ClassesAttributesPseudoElements = classesAttributesPseudoElements;
			this.Elements = elements;
		}

		public int ToInt()
		{
			string s = this.ToString();
			int result = 0;
			int.TryParse(s, out result);
			return result;
		}

		public override string ToString()
		{
			return string.Format("{0}{1}{2}", this.Ids, this.ClassesAttributesPseudoElements, this.Elements);
		}

		public static CssSpecificity operator +(CssSpecificity first, CssSpecificity second)
		{
			return new CssSpecificity(first.Ids + second.Ids, first.ClassesAttributesPseudoElements + second.ClassesAttributesPseudoElements, first.Elements + second.Elements)
			{
				Classes = first.Classes + second.Classes
			};
		}
	}
}
