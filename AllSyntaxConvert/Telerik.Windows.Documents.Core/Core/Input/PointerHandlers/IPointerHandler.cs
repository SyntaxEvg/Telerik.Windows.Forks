using System;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Input.PointerHandlers.Args;

namespace Telerik.Windows.Documents.Core.Input.PointerHandlers
{
	interface IPointerHandler : IStackCollectionElement
	{
		void Initialize();

		bool ShouldHandle(Point position, SourceType source);

		Cursor GetCursor(Point position, SourceType source);

		void RegisterLeftButtonDown(PointerLeftButtonDownArgs args);

		void RegisterLeftButtonUp(PointerLeftButtonUpArgs args);

		void RegisterRightButtonDown(PointerRightButtonDownArgs args);

		void RegisterRightButtonUp(PointerRightButtonUpArgs args);

		void RegisterMove(PointerMoveArgs args);

		void RegisterLostCapture(PointerLostCaptureArgs args);

		void Capture(PointerCaptureArgs args);

		void ReleaseCapture(PointerReleaseCaptureArgs args);

		void Release();
	}
}
