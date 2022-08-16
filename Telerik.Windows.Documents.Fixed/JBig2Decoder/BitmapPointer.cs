using System;

namespace JBig2Decoder
{
	class BitmapPointer
	{
		public BitmapPointer(JBIG2Bitmap bitmap)
		{
			this.bitmap = bitmap;
			this.height = bitmap.getHeight();
			this.width = bitmap.getWidth();
		}

		public void setPointer(long x, long y)
		{
			this.x = x;
			this.y = y;
			this.output = true;
			if (y < 0L || y >= this.height || x >= this.width)
			{
				this.output = false;
			}
			this.count = y * this.width;
		}

		public int nextPixel()
		{
			if (!this.output)
			{
				return 0;
			}
			if (this.x < 0L || this.x >= this.width)
			{
				this.x += 1L;
				return 0;
			}
			FastBitSet data = this.bitmap.data;
			long num = this.count;
			long num2;
			this.x = (num2 = this.x) + 1L;
			if (!data.get((long)((int)(num + num2))))
			{
				return 0;
			}
			return 1;
		}

		long x;

		long y;

		long width;

		long height;

		long count;

		bool output;

		JBIG2Bitmap bitmap;
	}
}
