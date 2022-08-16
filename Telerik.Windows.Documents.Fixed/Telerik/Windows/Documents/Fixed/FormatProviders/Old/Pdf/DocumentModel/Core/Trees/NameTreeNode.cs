using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Trees
{
	class NameTreeNode : PdfObjectOld
	{
		public NameTreeNode(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.children = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "Kids"
			});
			this.names = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "Names"
			});
			this.limits = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "Limits"
			});
		}

		public PdfArrayOld Names
		{
			get
			{
				return this.names.GetValue();
			}
			set
			{
				this.names.SetValue(value);
			}
		}

		public PdfArrayOld Children
		{
			get
			{
				return this.children.GetValue();
			}
			set
			{
				this.children.SetValue(value);
			}
		}

		public PdfArrayOld Limits
		{
			get
			{
				return this.limits.GetValue();
			}
			set
			{
				this.limits.SetValue(value);
			}
		}

		public T FindElement<T>(string name, IConverter converter = null) where T : PdfObjectOld
		{
			if (this.Limits != null && !this.IsInLimit(name))
			{
				return default(T);
			}
			if (this.Names != null)
			{
				return this.FindElementInNames<T>(name, converter);
			}
			if (this.Children != null)
			{
				for (int i = 0; i < this.Children.Count; i++)
				{
					NameTreeNode element = this.Children.GetElement<NameTreeNode>(i);
					if (element.IsInLimit(name))
					{
						return element.FindElement<T>(name, converter);
					}
				}
			}
			return default(T);
		}

		protected bool IsInLimit(string name)
		{
			if (this.Limits == null)
			{
				return false;
			}
			PdfStringOld element = this.Limits.GetElement<PdfStringOld>(0);
			PdfStringOld element2 = this.Limits.GetElement<PdfStringOld>(1);
			return NameTreeNode.CompareNames(name, element.ToString()) >= 0 && NameTreeNode.CompareNames(name, element2.ToString()) <= 0;
		}

		static int CompareNames(string x, string y)
		{
			int num = System.Math.Min(x.Length, y.Length);
			int i = 0;
			while (i < num)
			{
				int num2 = x[i].CompareTo(y[i]);
				if (num2 != 0)
				{
					if (num2 >= 0)
					{
						return 1;
					}
					return -1;
				}
				else
				{
					i++;
				}
			}
			if (x.Length == y.Length)
			{
				return 0;
			}
			if (x.Length < y.Length)
			{
				return -1;
			}
			return 1;
		}

		T FindElementInNames<T>(string name, IConverter converter = null) where T : PdfObjectOld
		{
			if (this.Children != null)
			{
				for (int i = 0; i < this.Children.Count; i++)
				{
					NameTreeNode element = this.Children.GetElement<NameTreeNode>(i);
					if (element.IsInLimit(name))
					{
						return element.FindElement<T>(name, converter);
					}
				}
			}
			if (this.Names != null)
			{
				for (int j = 0; j < this.Names.Count; j += 2)
				{
					PdfStringOld element2 = this.Names.GetElement<PdfStringOld>(j);
					if (element2.ToString() == name)
					{
						return (converter == null) ? this.Names.GetElement<T>(j + 1) : this.Names.GetElement<T>(j + 1, converter);
					}
				}
			}
			return default(T);
		}

		readonly LoadOnDemandProperty<PdfArrayOld> children;

		readonly LoadOnDemandProperty<PdfArrayOld> names;

		readonly LoadOnDemandProperty<PdfArrayOld> limits;
	}
}
