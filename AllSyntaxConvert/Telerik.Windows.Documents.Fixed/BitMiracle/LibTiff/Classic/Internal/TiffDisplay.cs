using System;

namespace BitMiracle.LibTiff.Classic.Internal
{
	class TiffDisplay
	{
		public TiffDisplay()
		{
		}

		public TiffDisplay(float[] mat0, float[] mat1, float[] mat2, float YCR, float YCG, float YCB, int Vrwr, int Vrwg, int Vrwb, float Y0R, float Y0G, float Y0B, float gammaR, float gammaG, float gammaB)
		{
			this.d_mat = new float[][] { mat0, mat1, mat2 };
			this.d_YCR = YCR;
			this.d_YCG = YCG;
			this.d_YCB = YCB;
			this.d_Vrwr = Vrwr;
			this.d_Vrwg = Vrwg;
			this.d_Vrwb = Vrwb;
			this.d_Y0R = Y0R;
			this.d_Y0G = Y0G;
			this.d_Y0B = Y0B;
			this.d_gammaR = gammaR;
			this.d_gammaG = gammaG;
			this.d_gammaB = gammaB;
		}

		internal float[][] d_mat;

		internal float d_YCR;

		internal float d_YCG;

		internal float d_YCB;

		internal int d_Vrwr;

		internal int d_Vrwg;

		internal int d_Vrwb;

		internal float d_Y0R;

		internal float d_Y0G;

		internal float d_Y0B;

		internal float d_gammaR;

		internal float d_gammaG;

		internal float d_gammaB;
	}
}
