using System;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts
{
	class CidWidthVertical
	{
		public CidWidthVertical(int displacementVectorY, int positionVectorX, int positionVectorY)
		{
			this.displacementVectorY = displacementVectorY;
			this.positionVectorX = positionVectorX;
			this.positionVectorY = positionVectorY;
		}

		public int DisplacementVectorX
		{
			get
			{
				return 0;
			}
		}

		public int DisplacementVectorY
		{
			get
			{
				return this.displacementVectorY;
			}
		}

		public int PositionVectorX
		{
			get
			{
				return this.positionVectorX;
			}
		}

		public int PositionVectorY
		{
			get
			{
				return this.positionVectorY;
			}
		}

		readonly int displacementVectorY;

		readonly int positionVectorX;

		readonly int positionVectorY;
	}
}
