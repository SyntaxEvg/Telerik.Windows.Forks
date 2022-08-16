using System;

namespace Telerik.Windows.Documents.Core.Input.PointerHandlers.Args
{
	class PointerButtonDownArgs : PointerHandlerArgs
	{
		public PointerButtonDownArgs(ButtonType button)
		{
			this.ButtonType = button;
			this.Clicks = 1;
		}

		public ButtonType ButtonType { get; set; }

		public bool CtrlPressed { get; set; }

		public bool ShiftPressed { get; set; }

		public int Clicks { get; set; }

		public bool IsSimulated { get; set; }
	}
}
