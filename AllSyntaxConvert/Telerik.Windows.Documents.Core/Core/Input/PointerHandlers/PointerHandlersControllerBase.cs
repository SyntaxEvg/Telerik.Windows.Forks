using System;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Documents.Core.Input.PointerHandlers.Args;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Input.PointerHandlers
{
	public abstract class PointerHandlersControllerBase
	{
		protected PointerHandlersControllerBase()
		{
			this.handlers = new PointerHandlersStack();
		}

		internal abstract Cursor DefaultCursor { get; }

		internal PointerHandlersStack Handlers
		{
			get
			{
				return this.handlers;
			}
		}

		internal IPointerHandler GetHandlerByName(string name)
		{
			return this.handlers.GetElementByName(name);
		}

		internal void Initialize()
		{
			foreach (IPointerHandler pointerHandler in this.handlers)
			{
				pointerHandler.Initialize();
			}
		}

		internal virtual Cursor GetCursor(Point position, SourceType source)
		{
			if (this.capturedHandler != null)
			{
				return this.capturedHandler.GetCursor(position, source);
			}
			IPointerHandler firstHandler = this.GetFirstHandler(position, source);
			if (firstHandler != null)
			{
				return firstHandler.GetCursor(position, source);
			}
			return this.DefaultCursor;
		}

		internal virtual void RegisterRightButtonDown(PointerRightButtonDownArgs args)
		{
			foreach (IPointerHandler pointerHandler in this.handlers)
			{
				if (pointerHandler.ShouldHandle(args.Position, args.Source))
				{
					pointerHandler.RegisterRightButtonDown(args);
					if (args.IsHandled)
					{
						this.lastButtonDownArgs = args;
						this.Capture(pointerHandler);
						break;
					}
				}
			}
		}

		internal virtual void RegisterRightButtonUp(PointerRightButtonUpArgs args)
		{
			if (this.capturedHandler != null)
			{
				this.capturedHandler.RegisterRightButtonUp(args);
				this.ReleaseCapture();
			}
		}

		internal virtual void RegisterLeftButtonDown(PointerLeftButtonDownArgs args)
		{
			foreach (IPointerHandler pointerHandler in this.handlers)
			{
				if (pointerHandler.ShouldHandle(args.Position, args.Source))
				{
					pointerHandler.RegisterLeftButtonDown(args);
					if (args.IsHandled)
					{
						this.lastButtonDownArgs = args;
						this.Capture(pointerHandler);
						break;
					}
				}
			}
		}

		internal virtual void RegisterLeftButtonUp(PointerLeftButtonUpArgs args)
		{
			if (this.capturedHandler != null)
			{
				this.capturedHandler.RegisterLeftButtonUp(args);
				this.ReleaseCapture();
			}
		}

		internal virtual void RegisterMove(PointerMoveArgs args)
		{
			if (this.capturedHandler != null)
			{
				this.capturedHandler.RegisterMove(args);
			}
		}

		internal virtual void CaptureNext(IPointerHandler handlerToSkip)
		{
			if (this.lastButtonDownArgs == null)
			{
				throw new InvalidOperationException("Cannot call CaptureNext before calling RegisterLeftButtonDown or RegisterRightButtonDown!");
			}
			bool flag = true;
			foreach (IPointerHandler pointerHandler in this.handlers)
			{
				if (pointerHandler == handlerToSkip)
				{
					flag = false;
				}
				else if (!flag && pointerHandler.ShouldHandle(this.lastButtonDownArgs.Position, this.lastButtonDownArgs.Source))
				{
					bool flag2 = this.TryRegisterLastButtonDown(pointerHandler);
					if (flag2)
					{
						this.Capture(pointerHandler);
						break;
					}
				}
			}
		}

		internal virtual void RegisterLostCapture(PointerLostCaptureArgs args)
		{
			if (this.capturedHandler != null)
			{
				this.capturedHandler.RegisterLostCapture(args);
				this.capturedHandler = null;
			}
		}

		internal void Release()
		{
			foreach (IPointerHandler pointerHandler in this.handlers)
			{
				pointerHandler.Release();
			}
		}

		void Capture(IPointerHandler handler)
		{
			Guard.ThrowExceptionIfNull<IPointerHandler>(handler, "handler");
			this.capturedHandler = handler;
			this.capturedHandler.Capture(new PointerCaptureArgs());
		}

		void ReleaseCapture()
		{
			if (this.capturedHandler != null)
			{
				this.capturedHandler.ReleaseCapture(new PointerReleaseCaptureArgs());
				this.capturedHandler = null;
			}
		}

		IPointerHandler GetFirstHandler(Point position, SourceType source)
		{
			foreach (IPointerHandler pointerHandler in this.handlers)
			{
				if (pointerHandler.ShouldHandle(position, source))
				{
					return pointerHandler;
				}
			}
			return null;
		}

		bool TryRegisterLastButtonDown(IPointerHandler handler)
		{
			bool result = false;
			if (this.lastButtonDownArgs.ButtonType == ButtonType.Left)
			{
				PointerLeftButtonDownArgs pointerLeftButtonDownArgs = new PointerLeftButtonDownArgs
				{
					IsSimulated = true,
					CtrlPressed = this.lastButtonDownArgs.CtrlPressed,
					ShiftPressed = this.lastButtonDownArgs.ShiftPressed,
					Source = this.lastButtonDownArgs.Source,
					Position = this.lastButtonDownArgs.Position,
					Clicks = this.lastButtonDownArgs.Clicks
				};
				handler.RegisterLeftButtonDown(pointerLeftButtonDownArgs);
				result = pointerLeftButtonDownArgs.IsHandled;
			}
			else if (this.lastButtonDownArgs.ButtonType == ButtonType.Right)
			{
				PointerRightButtonDownArgs pointerRightButtonDownArgs = new PointerRightButtonDownArgs
				{
					IsSimulated = true,
					CtrlPressed = this.lastButtonDownArgs.CtrlPressed,
					ShiftPressed = this.lastButtonDownArgs.ShiftPressed,
					Source = this.lastButtonDownArgs.Source,
					Position = this.lastButtonDownArgs.Position,
					Clicks = this.lastButtonDownArgs.Clicks
				};
				handler.RegisterRightButtonDown(pointerRightButtonDownArgs);
				result = pointerRightButtonDownArgs.IsHandled;
			}
			return result;
		}

		const string InvalidCaptureNextCallExceptionMessage = "Cannot call CaptureNext before calling RegisterLeftButtonDown or RegisterRightButtonDown!";

		readonly PointerHandlersStack handlers;

		PointerButtonDownArgs lastButtonDownArgs;

		IPointerHandler capturedHandler;
	}
}
