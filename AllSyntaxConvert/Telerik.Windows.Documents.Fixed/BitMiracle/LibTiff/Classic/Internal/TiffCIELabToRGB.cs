using System;

namespace BitMiracle.LibTiff.Classic.Internal
{
	class TiffCIELabToRGB
	{
		public void Init(TiffDisplay refDisplay, float[] refWhite)
		{
			this.range = 1500;
			this.display = refDisplay;
			double y = 1.0 / (double)this.display.d_gammaR;
			this.rstep = (this.display.d_YCR - this.display.d_Y0R) / (float)this.range;
			for (int i = 0; i <= this.range; i++)
			{
				this.Yr2r[i] = (float)this.display.d_Vrwr * (float)Math.Pow((double)i / (double)this.range, y);
			}
			y = 1.0 / (double)this.display.d_gammaG;
			this.gstep = (this.display.d_YCR - this.display.d_Y0R) / (float)this.range;
			for (int j = 0; j <= this.range; j++)
			{
				this.Yg2g[j] = (float)this.display.d_Vrwg * (float)Math.Pow((double)j / (double)this.range, y);
			}
			y = 1.0 / (double)this.display.d_gammaB;
			this.bstep = (this.display.d_YCR - this.display.d_Y0R) / (float)this.range;
			for (int k = 0; k <= this.range; k++)
			{
				this.Yb2b[k] = (float)this.display.d_Vrwb * (float)Math.Pow((double)k / (double)this.range, y);
			}
			this.X0 = refWhite[0];
			this.Y0 = refWhite[1];
			this.Z0 = refWhite[2];
		}

		public void CIELabToXYZ(int l, int a, int b, out float X, out float Y, out float Z)
		{
			float num = (float)l * 100f / 255f;
			float num2;
			if (num < 8.856f)
			{
				Y = num * this.Y0 / 903.292f;
				num2 = 7.787f * (Y / this.Y0) + 0.13793103f;
			}
			else
			{
				num2 = (num + 16f) / 116f;
				Y = this.Y0 * num2 * num2 * num2;
			}
			float num3 = (float)a / 500f + num2;
			if (num3 < 0.2069f)
			{
				X = this.X0 * (num3 - 0.13793f) / 7.787f;
			}
			else
			{
				X = this.X0 * num3 * num3 * num3;
			}
			num3 = num2 - (float)b / 200f;
			if (num3 < 0.2069f)
			{
				Z = this.Z0 * (num3 - 0.13793f) / 7.787f;
				return;
			}
			Z = this.Z0 * num3 * num3 * num3;
		}

		public void XYZToRGB(float X, float Y, float Z, out int r, out int g, out int b)
		{
			float num = this.display.d_mat[0][0] * X + this.display.d_mat[0][1] * Y + this.display.d_mat[0][2] * Z;
			float num2 = this.display.d_mat[1][0] * X + this.display.d_mat[1][1] * Y + this.display.d_mat[1][2] * Z;
			float num3 = this.display.d_mat[2][0] * X + this.display.d_mat[2][1] * Y + this.display.d_mat[2][2] * Z;
			num = Math.Max(num, this.display.d_Y0R);
			num2 = Math.Max(num2, this.display.d_Y0G);
			num3 = Math.Max(num3, this.display.d_Y0B);
			num = System.Math.Min(num, this.display.d_YCR);
			num2 = System.Math.Min(num2, this.display.d_YCG);
			num3 = System.Math.Min(num3, this.display.d_YCB);
			int num4 = (int)((num - this.display.d_Y0R) / this.rstep);
			num4 = System.Math.Min(this.range, num4);
			r = TiffCIELabToRGB.rInt(this.Yr2r[num4]);
			num4 = (int)((num2 - this.display.d_Y0G) / this.gstep);
			num4 = System.Math.Min(this.range, num4);
			g = TiffCIELabToRGB.rInt(this.Yg2g[num4]);
			num4 = (int)((num3 - this.display.d_Y0B) / this.bstep);
			num4 = System.Math.Min(this.range, num4);
			b = TiffCIELabToRGB.rInt(this.Yb2b[num4]);
			r = System.Math.Min(r, this.display.d_Vrwr);
			g = System.Math.Min(g, this.display.d_Vrwg);
			b = System.Math.Min(b, this.display.d_Vrwb);
		}

		static int rInt(float R)
		{
			return (int)((R > 0f) ? ((double)R + 0.5) : ((double)R - 0.5));
		}

		public const int CIELABTORGB_TABLE_RANGE = 1500;

		int range;

		float rstep;

		float gstep;

		float bstep;

		float X0;

		float Y0;

		float Z0;

		TiffDisplay display;

		float[] Yr2r = new float[1501];

		float[] Yg2g = new float[1501];

		float[] Yb2b = new float[1501];
	}
}
