using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Data
{
	class PositionStack
	{
		public PositionStack(IPosition initialPosition)
		{
			Guard.ThrowExceptionIfNull<IPosition>(initialPosition, "initialPosition");
			this.positionsStack = new Stack<IPosition>();
			this.Position = initialPosition.Clone();
		}

		public IPosition Position
		{
			get
			{
				return this.position;
			}
			set
			{
				Guard.ThrowExceptionIfNull<IPosition>(value, "Position");
				this.position = value;
			}
		}

		public IDisposable SavePosition()
		{
			this.positionsStack.Push(this.Position.Clone());
			return new DisposableObject(new Action(this.RestorePosition));
		}

		public void RestorePosition()
		{
			this.Position = this.positionsStack.Pop();
		}

		readonly Stack<IPosition> positionsStack;

		IPosition position;
	}
}
