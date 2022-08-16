using System;

namespace BitMiracle.LibTiff.Classic.Internal
{
	class TiffYCbCrToRGB
	{
		public TiffYCbCrToRGB()
		{
			this.clamptab = new byte[1024];
			this.Cr_r_tab = new int[256];
			this.Cb_b_tab = new int[256];
			this.Cr_g_tab = new int[256];
			this.Cb_g_tab = new int[256];
			this.Y_tab = new int[256];
		}

		public void Init(float[] luma, float[] refBlackWhite)
		{
			Array.Clear(this.clamptab, 0, 256);
			for (int i = 0; i < 256; i++)
			{
				this.clamptab[256 + i] = (byte)i;
			}
			int num = 512;
			int num2 = num + 512;
			for (int j = num; j < num2; j++)
			{
				this.clamptab[j] = byte.MaxValue;
			}
			float num3 = luma[0];
			float num4 = luma[1];
			float num5 = luma[2];
			float num6 = 2f - 2f * num3;
			int num7 = TiffYCbCrToRGB.fix(num6);
			float x = num3 * num6 / num4;
			int num8 = -TiffYCbCrToRGB.fix(x);
			float num9 = 2f - 2f * num5;
			int num10 = TiffYCbCrToRGB.fix(num9);
			float x2 = num5 * num9 / num4;
			int num11 = -TiffYCbCrToRGB.fix(x2);
			int k = 0;
			int num12 = -128;
			while (k < 256)
			{
				int num13 = TiffYCbCrToRGB.code2V(num12, refBlackWhite[4] - 128f, refBlackWhite[5] - 128f, 127f);
				int num14 = TiffYCbCrToRGB.code2V(num12, refBlackWhite[2] - 128f, refBlackWhite[3] - 128f, 127f);
				this.Cr_r_tab[k] = num7 * num13 + 32768 >> 16;
				this.Cb_b_tab[k] = num10 * num14 + 32768 >> 16;
				this.Cr_g_tab[k] = num8 * num13;
				this.Cb_g_tab[k] = num11 * num14 + 32768;
				this.Y_tab[k] = TiffYCbCrToRGB.code2V(num12 + 128, refBlackWhite[0], refBlackWhite[1], 255f);
				k++;
				num12++;
			}
		}

		public void YCbCrtoRGB(int Y, int Cb, int Cr, out int r, out int g, out int b)
		{
			Y = TiffYCbCrToRGB.hiClamp(Y, 255);
			Cb = TiffYCbCrToRGB.clamp(Cb, 0, 255);
			Cr = TiffYCbCrToRGB.clamp(Cr, 0, 255);
			r = (int)this.clamptab[256 + this.Y_tab[Y] + this.Cr_r_tab[Cr]];
			g = (int)this.clamptab[256 + this.Y_tab[Y] + (this.Cb_g_tab[Cb] + this.Cr_g_tab[Cr] >> 16)];
			b = (int)this.clamptab[256 + this.Y_tab[Y] + this.Cb_b_tab[Cb]];
		}

		static int fix(float x)
		{
			return (int)((double)(x * 65536f) + 0.5);
		}

		static int code2V(int c, float RB, float RW, float CR)
		{
			return (int)((float)(c - (int)RB) * CR / (((int)(RW - RB) != 0) ? (RW - RB) : 1f));
		}

		static int clamp(int f, int min, int max)
		{
			if (f < min)
			{
				return min;
			}
			if (f <= max)
			{
				return f;
			}
			return max;
		}

		static int hiClamp(int f, int max)
		{
			if (f <= max)
			{
				return f;
			}
			return max;
		}

		const int clamptabOffset = 256;

		const int SHIFT = 16;

		const int ONE_HALF = 32768;

		byte[] clamptab;

		int[] Cr_r_tab;

		int[] Cb_b_tab;

		int[] Cr_g_tab;

		int[] Cb_g_tab;

		int[] Y_tab;
	}
}
