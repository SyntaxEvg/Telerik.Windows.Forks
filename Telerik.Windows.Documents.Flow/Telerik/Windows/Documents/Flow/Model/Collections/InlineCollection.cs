using System;
using Telerik.Windows.Documents.Flow.Model.Shapes;

namespace Telerik.Windows.Documents.Flow.Model.Collections
{
	public sealed class InlineCollection : DocumentElementCollection<InlineBase, Paragraph>
	{
		internal InlineCollection(Paragraph parent)
			: base(parent)
		{
		}

		public Run AddRun()
		{
			Run run = new Run(base.Owner.Document);
			base.Add(run);
			return run;
		}

		public Run AddRun(string text)
		{
			Run run = new Run(base.Owner.Document);
			run.Text = text;
			base.Add(run);
			return run;
		}

		public ImageInline AddImageInline()
		{
			ImageInline imageInline = new ImageInline(base.Owner.Document);
			base.Add(imageInline);
			return imageInline;
		}

		public FloatingImage AddFloatingImage()
		{
			FloatingImage floatingImage = new FloatingImage(base.Owner.Document);
			base.Add(floatingImage);
			return floatingImage;
		}
	}
}
