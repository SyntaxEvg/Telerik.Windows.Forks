using System;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	abstract class ImageDataSource<T> : ImageDataSource where T : class
	{
		public ImageDataSource(int width, int height, T data)
			: base(width, height)
		{
			this.Data = data;
		}

		public override bool IsEmpty
		{
			get
			{
				return this.Data == null;
			}
		}

		public T Data { get; set; }
	}
}
