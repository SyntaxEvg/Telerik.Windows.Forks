using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Core.PostScript.Data;
using Telerik.Windows.Documents.Core.Shapes;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Enums;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.XObjects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Readers;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Parsers
{
	class ContentStreamParser
	{
		public ContentStreamParser(PdfContentManager contentManager, byte[] data, Rect clip, PdfResourceOld resources, Brush stencilBrush, IndirectReferenceOld contentOwnerReference)
			: this(contentManager, data, clip, resources, contentOwnerReference)
		{
			Guard.ThrowExceptionIfNull<Brush>(stencilBrush, "stencilBrush");
			this.context.GraphicsState.Brush = stencilBrush;
		}

		public ContentStreamParser(PdfContentManager contentManager, byte[] data, Rect clip, PdfResourceOld resources, IndirectReferenceOld contentOwnerReference)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			this.contentManager = contentManager;
			this.resources = resources;
			this.reader = new ContentStreamReader(data);
			this.context = new PdfContext(this.contentManager, clip);
			this.contentOwnerReference = contentOwnerReference;
		}

		public IEnumerable<Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement> ParseContent()
		{
			return this.ParseContent(false);
		}

		public IEnumerable<Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement> ParseOnlyTextRelatedContent()
		{
			return this.ParseContent(true);
		}

		IEnumerable<Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement> ParseContent(bool skipNonTextRelatedContent)
		{
			List<Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement> list = new List<Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement>();
			list.Add(this.context.GraphicsState.ClippingContainer);
			for (;;)
			{
				object[] pars = this.ReadObjectsToToken(Token.Operator);
				if (this.reader.TokenType == Token.EndOfFile)
				{
					break;
				}
				this.InvokeOperator(this.reader.Result, pars, skipNonTextRelatedContent);
			}
			return list;
		}

		internal event EventHandler<OnDocumentExceptionEventArgs> OnException;

		object[] ReadObjectsToToken(Token endToken)
		{
			return this.ReadObjectsToToken(endToken, false);
		}

		object[] ReadObjectsToToken(Token endToken, bool replaceAbbreviations)
		{
			Stack<object> stack = new Stack<object>();
			Token token;
			while ((token = this.reader.ReadToken()) != endToken)
			{
				switch (token)
				{
				case Token.Integer:
					stack.Push(this.ParseCurrentTokenToInt());
					break;
				case Token.Real:
					stack.Push(this.ParseCurrentTokenToReal());
					break;
				case Token.Name:
					stack.Push(this.ParseCurrentTokenToName(replaceAbbreviations));
					break;
				case Token.ArrayStart:
					stack.Push(this.ParseCurrentTokenToArray(replaceAbbreviations));
					break;
				case Token.String:
					stack.Push(this.ParseCurrentTokenToString());
					break;
				case Token.Boolean:
					stack.Push(this.ParseCurrentTokenToBool());
					break;
				case Token.DictionaryStart:
					stack.Push(this.ParseCurrentTokenToDictionary(replaceAbbreviations));
					break;
				case Token.Null:
					stack.Push(null);
					break;
				case Token.EndOfFile:
					return new object[0];
				}
			}
			if (stack.Count == 0)
			{
				return new object[0];
			}
			int i = stack.Count - 1;
			object[] array = new object[i + 1];
			while (i >= 0)
			{
				array[i] = stack.Pop();
				i--;
			}
			return array;
		}

		void OnOnException(OnDocumentExceptionEventArgs args)
		{
			if (this.OnException != null)
			{
				this.OnException(this, args);
			}
		}

		FontBaseOld GetFont(PdfNameOld name)
		{
			Guard.ThrowExceptionIfNull<PdfNameOld>(name, "name");
			return this.resources.GetFont(name);
		}

		ShadingOld GetShanding(PdfNameOld name)
		{
			Guard.ThrowExceptionIfNull<PdfNameOld>(name, "name");
			return this.resources.GetShading(name);
		}

		XObject GetXObject(PdfNameOld name)
		{
			Guard.ThrowExceptionIfNull<PdfNameOld>(name, "name");
			return this.resources.GetXObject(name);
		}

		ColorSpaceOld GetColorSpace(PdfNameOld name)
		{
			Guard.ThrowExceptionIfNull<PdfNameOld>(name, "name");
			if (ColorSpaceOld.IsColorSpace(name.Value))
			{
				return ColorSpaceOld.CreateColorSpace(this.contentManager, name.Value);
			}
			return this.resources.GetColorSpace(name);
		}

		ExtGStateOld GetExtGState(PdfNameOld name)
		{
			Guard.ThrowExceptionIfNull<PdfNameOld>(name, "name");
			return this.resources.GetExtGState(name);
		}

		void AddContentElement(Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement element)
		{
			if (element != null)
			{
				this.context.GraphicsState.ClippingContainer.Content.AddChild(element);
			}
		}

		void ReadInlineImage(bool skipImage)
		{
			int id = ++this.inlineImageCount;
			ImageResourceKey resourceKey = this.CreateInlineImageResourceKey(this.contentOwnerReference, id);
			if (skipImage)
			{
				this.SkipInlineImage(resourceKey);
				return;
			}
			Image element = this.ReadAndRegisterInlineImageSource(resourceKey);
			this.AddContentElement(element);
		}

		ImageResourceKey CreateInlineImageResourceKey(IndirectReferenceOld reference, int id)
		{
			Guard.ThrowExceptionIfNull<IndirectReferenceOld>(reference, "reference");
			return new ImageResourceKey
			{
				Id = id,
				Reference = reference,
				Type = ResourceType.Local
			};
		}

		void SkipInlineImage(ImageResourceKey resourceKey)
		{
			InlineImageInfo inlineImageInfo;
			if (this.context.ResourcesManager.TryGetInlineImageInfo(resourceKey, out inlineImageInfo))
			{
				this.reader.Seek(inlineImageInfo.EndOfImagePosition, SeekOrigin.Begin);
				return;
			}
			PdfDictionaryOld pdfDictionaryOld;
			byte[] array;
			this.ReadRawInlineImageData(out pdfDictionaryOld, out array);
		}

		Image ReadAndRegisterInlineImageSource(ImageResourceKey resourceKey)
		{
			Guard.ThrowExceptionIfNull<ImageResourceKey>(resourceKey, "resourceKey");
			Image image = new Image();
			image.ImageSourceKey = resourceKey;
			image.TransformMatrix = this.context.GraphicsState.Ctm;
			InlineImageInfo inlineImageInfo;
			if (this.context.ResourcesManager.TryGetInlineImageInfo(resourceKey, out inlineImageInfo))
			{
				this.ApplyInlineImageStencilColor(resourceKey, inlineImageInfo.XImage);
				this.reader.Seek(inlineImageInfo.EndOfImagePosition, SeekOrigin.Begin);
			}
			else
			{
				PdfDictionaryOld dictionary;
				byte[] rawData;
				this.ReadRawInlineImageData(out dictionary, out rawData);
				XImage ximage = new XImage(this.contentManager);
				ximage.Load(dictionary);
				this.ApplyInlineImageStencilColor(resourceKey, ximage);
				inlineImageInfo = new InlineImageInfo(ximage, dictionary, rawData, this.reader.Position);
				this.contentManager.ResourceManager.RegisterInlineImage(inlineImageInfo, resourceKey);
			}
			return image;
		}

		void ReadRawInlineImageData(out PdfDictionaryOld imageDictionary, out byte[] rawImageData)
		{
			object[] content = this.ReadObjectsToToken(Token.Operator, true);
			imageDictionary = new PdfDictionaryOld(this.contentManager);
			imageDictionary.Load(content);
			if (imageDictionary.ContainsKey("ColorSpace"))
			{
				PdfNameOld element = imageDictionary.GetElement<PdfNameOld>("ColorSpace", Converters.PdfNameConverter);
				if (element != null)
				{
					imageDictionary["ColorSpace"] = this.GetColorSpace(element);
				}
			}
			this.reader.SkipWhiteSpaces();
			long position = this.reader.Position;
			StreamPart part = new StreamPart(this.reader.Stream, position, "EI");
			rawImageData = this.reader.ReadRawData(position, part);
			this.reader.ReadToken();
		}

		void ApplyInlineImageStencilColor(ImageResourceKey resourceKey, XImage xImage)
		{
			if (xImage.ImageMask != null && xImage.ImageMask.Value)
			{
				resourceKey.StencilColor = new Color?(this.context.StencilColor);
			}
		}

		void InvokeGeneralGraphicsStateOperator(string op, object[] pars)
		{
			Guard.ThrowExceptionIfNullOrEmpty(op, "op");
			int num = ((pars != null) ? pars.Length : 0);
			switch (op)
			{
			case "w":
			{
				double lineWidth;
				Helper.UnboxDouble(pars[num - 1], out lineWidth);
				this.context.SetLineWidth(lineWidth);
				return;
			}
			case "J":
				this.context.SetLineCap((int)pars[num - 1]);
				return;
			case "j":
				this.context.SetLineJoin((int)pars[num - 1]);
				return;
			case "M":
			{
				double mitterLimit;
				Helper.UnboxDouble(pars[num - 1], out mitterLimit);
				this.context.SetMitterLimit(mitterLimit);
				return;
			}
			case "d":
			{
				double dashOffset;
				Helper.UnboxDouble(pars[num - 1], out dashOffset);
				this.context.SetDashPattern((PdfArrayOld)pars[num - 2], dashOffset);
				return;
			}
			case "ri":
				this.context.SetRenderingIntent(pars[num - 1] as string);
				return;
			case "i":
			{
				double flatness;
				Helper.UnboxDouble(pars[num - 1], out flatness);
				this.context.SetFlatness(flatness);
				return;
			}
			case "gs":
				this.context.SetGraphicState(this.GetExtGState((PdfNameOld)pars[num - 1]));
				return;
			case "q":
				this.context.SaveGraphicState();
				return;
			case "Q":
				this.context.RestoreGraphicState();
				return;
			case "cm":
			{
				double a;
				Helper.UnboxDouble(pars[num - 6], out a);
				double b;
				Helper.UnboxDouble(pars[num - 5], out b);
				double c;
				Helper.UnboxDouble(pars[num - 4], out c);
				double d;
				Helper.UnboxDouble(pars[num - 3], out d);
				double e;
				Helper.UnboxDouble(pars[num - 2], out e);
				double f;
				Helper.UnboxDouble(pars[num - 1], out f);
				this.context.SetMatrix(a, b, c, d, e, f);
				break;
			}

				return;
			}
		}

		void InvokeColorOperator(string op, object[] pars)
		{
			Guard.ThrowExceptionIfNullOrEmpty(op, "op");
			int num = ((pars != null) ? pars.Length : 0);
			switch (op)
			{
			case "CS":
				this.context.SetStrokeColorSpace(this.GetColorSpace(pars[num - 1] as PdfNameOld));
				return;
			case "cs":
				this.context.SetColorSpace(this.GetColorSpace(pars[num - 1] as PdfNameOld));
				return;
			case "SCN":
			case "SC":
				this.context.SetStrokeColor(this.context.GraphicsState.StrokeColorSpace.GetBrush(this.resources, pars));
				return;
			case "sc":
			case "scn":
				this.context.SetColor(this.context.GraphicsState.ColorSpace.GetBrush(this.resources, pars));
				return;
			case "g":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 1], out num3);
				this.context.SetGrayColor(num3);
				return;
			}
			case "G":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 1], out num3);
				this.context.SetGrayStrokeColor(num3);
				return;
			}
			case "RG":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 3], out num3);
				double num4;
				Helper.UnboxDouble(pars[num - 2], out num4);
				double num5;
				Helper.UnboxDouble(pars[num - 1], out num5);
				this.context.SetRgbStrokeColor(Color.FromArgb(1.0, num3, num4, num5));
				return;
			}
			case "rg":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 3], out num3);
				double num4;
				Helper.UnboxDouble(pars[num - 2], out num4);
				double num5;
				Helper.UnboxDouble(pars[num - 1], out num5);
				this.context.SetRgbColor(Color.FromArgb(1.0, num3, num4, num5));
				return;
			}
			case "K":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 4], out num3);
				double num4;
				Helper.UnboxDouble(pars[num - 3], out num4);
				double num5;
				Helper.UnboxDouble(pars[num - 2], out num5);
				double black;
				Helper.UnboxDouble(pars[num - 1], out black);
				this.context.SetCmykStrokeColor(Color.FromCmyk(num3, num4, num5, black));
				return;
			}
			case "k":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 4], out num3);
				double num4;
				Helper.UnboxDouble(pars[num - 3], out num4);
				double num5;
				Helper.UnboxDouble(pars[num - 2], out num5);
				double black;
				Helper.UnboxDouble(pars[num - 1], out black);
				this.context.SetCmykColor(Color.FromCmyk(num3, num4, num5, black));
				break;
			}

				return;
			}
		}

		void InvokeTextOperator(string op, object[] pars)
		{
			Guard.ThrowExceptionIfNullOrEmpty(op, "op");
			int num = ((pars != null) ? pars.Length : 0);
			switch (op)
			{
			case "BT":
				this.context.BeginText();
				return;
			case "ET":
				this.context.EndText();
				return;
			case "Tw":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 1], out num3);
				this.context.SetWordSpacing(num3);
				return;
			}
			case "Tz":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 1], out num3);
				this.context.SetHorizontalScaling(num3);
				return;
			}
			case "TL":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 1], out num3);
				this.context.SetTextLeading(num3);
				return;
			}
			case "Tf":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 1], out num3);
				FontBaseOld font = this.GetFont(pars[num - 2] as PdfNameOld);
				this.context.SetFont(font, num3);
				return;
			}
			case "Tr":
				this.context.SetRenderingMode((RenderingMode)pars[num - 1]);
				return;
			case "Ts":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 1], out num3);
				this.context.SetRise(num3);
				return;
			}
			case "Td":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 2], out num3);
				double num4;
				Helper.UnboxDouble(pars[num - 1], out num4);
				this.context.MoveToNextLine(num3, num4);
				return;
			}
			case "TD":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 2], out num3);
				double num4;
				Helper.UnboxDouble(pars[num - 1], out num4);
				this.context.MoveToNextLineWithTextLeading(num3, num4);
				return;
			}
			case "Tm":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 6], out num3);
				double num4;
				Helper.UnboxDouble(pars[num - 5], out num4);
				double c;
				Helper.UnboxDouble(pars[num - 4], out c);
				double d;
				Helper.UnboxDouble(pars[num - 3], out d);
				double e;
				Helper.UnboxDouble(pars[num - 2], out e);
				double f;
				Helper.UnboxDouble(pars[num - 1], out f);
				this.context.SetTextMatrix(num3, num4, c, d, e, f);
				return;
			}
			case "Tj":
				this.AddContentElement(this.context.DrawText((PdfStringOld)pars[num - 1]));
				return;
			case "T*":
				this.context.MoveToNextLineWithCurrentTextLeading();
				return;
			case "'":
				this.AddContentElement(this.context.MoveToNextLineAndDrawText((PdfStringOld)pars[num - 1]));
				return;
			case "\"":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 3], out num3);
				double num4;
				Helper.UnboxDouble(pars[num - 2], out num4);
				this.AddContentElement(this.context.MoveToNextLineAndDrawText((PdfStringOld)pars[num - 1], num3, num4));
				return;
			}
			case "TJ":
				this.AddContentElement(this.context.DrawText((PdfArrayOld)pars[num - 1]));
				return;
			case "Tc":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 1], out num3);
				this.context.SetCharSpacing(num3);
				break;
			}

				return;
			}
		}

		void InvokePathOperator(string op, object[] pars)
		{
			Guard.ThrowExceptionIfNullOrEmpty(op, "op");
			int num = ((pars != null) ? pars.Length : 0);
			switch (op)
			{
			case "m":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 2], out num3);
				double num4;
				Helper.UnboxDouble(pars[num - 1], out num4);
				this.context.MoveTo(num3, num4);
				return;
			}
			case "l":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 2], out num3);
				double num4;
				Helper.UnboxDouble(pars[num - 1], out num4);
				this.context.LineTo(num3, num4);
				return;
			}
			case "c":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 6], out num3);
				double num4;
				Helper.UnboxDouble(pars[num - 5], out num4);
				double num5;
				Helper.UnboxDouble(pars[num - 4], out num5);
				double num6;
				Helper.UnboxDouble(pars[num - 3], out num6);
				double x;
				Helper.UnboxDouble(pars[num - 2], out x);
				double y;
				Helper.UnboxDouble(pars[num - 1], out y);
				this.context.CurveTo(num3, num4, num5, num6, x, y);
				return;
			}
			case "v":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 4], out num3);
				double num4;
				Helper.UnboxDouble(pars[num - 3], out num4);
				double num5;
				Helper.UnboxDouble(pars[num - 2], out num5);
				double num6;
				Helper.UnboxDouble(pars[num - 1], out num6);
				this.context.HCurveTo(num3, num4, num5, num6);
				return;
			}
			case "y":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 4], out num3);
				double num4;
				Helper.UnboxDouble(pars[num - 3], out num4);
				double num5;
				Helper.UnboxDouble(pars[num - 2], out num5);
				double num6;
				Helper.UnboxDouble(pars[num - 1], out num6);
				this.context.VCurveTo(num3, num4, num5, num6);
				return;
			}
			case "h":
				this.context.Close();
				return;
			case "re":
			{
				double num3;
				Helper.UnboxDouble(pars[num - 4], out num3);
				double num4;
				Helper.UnboxDouble(pars[num - 3], out num4);
				double num5;
				Helper.UnboxDouble(pars[num - 2], out num5);
				double num6;
				Helper.UnboxDouble(pars[num - 1], out num6);
				this.context.Rectangle(num3, num4, num5, num6);
				return;
			}
			case "S":
				this.AddContentElement(this.context.Stroke());
				return;
			case "s":
				this.AddContentElement(this.context.CloseAndStroke());
				return;
			case "f":
				this.AddContentElement(this.context.Fill());
				return;
			case "F":
				this.AddContentElement(this.context.Fill());
				return;
			case "f*":
				this.AddContentElement(this.context.Fill(FillRule.EvenOdd));
				return;
			case "B":
				this.AddContentElement(this.context.FillAndStroke());
				return;
			case "B*":
				this.AddContentElement(this.context.FillAndStroke(FillRule.EvenOdd));
				return;
			case "b":
				this.AddContentElement(this.context.CloseFillAndStroke());
				return;
			case "b*":
				this.AddContentElement(this.context.CloseFillAndStroke(FillRule.EvenOdd));
				return;
			case "n":
				this.context.EndPath();
				return;
			case "W":
				this.context.SetClippingPath();
				return;
			case "W*":
				this.context.SetClippingPath(FillRule.EvenOdd);
				return;
			case "sh":
				this.AddContentElement(this.context.ApplyShading(this.GetShanding((PdfNameOld)pars[num - 1])));
				break;

				return;
			}
		}

		void InvokeXObjectOperator(string op, object[] pars, bool skipNonTextRelatedContent)
		{
			Guard.ThrowExceptionIfNullOrEmpty(op, "op");
			Guard.ThrowExceptionIfNull<object[]>(pars, "pars");
			PdfNameOld name = pars[pars.Length - 1] as PdfNameOld;
			XObject xobject = this.GetXObject(name);
			XImage ximage = xobject as XImage;
			if (ximage != null)
			{
				if (!skipNonTextRelatedContent)
				{
					this.AddContentElement(this.context.CreateXImage(ximage));
					return;
				}
				return;
			}
			else
			{
				XForm xform = xobject as XForm;
				if (xform != null)
				{
					this.AddContentElement(this.context.CreateXForm(this.resources, xform, skipNonTextRelatedContent));
					return;
				}
				throw new NotSupportedXObjectTypeException(xobject.GetType().ToString());
			}
		}

		void InvokeImageOperator(string op, bool skipImage)
		{
			if (op != null)
			{
				if (!(op == "BI"))
				{
					if (!(op == "EI"))
					{
						return;
					}
				}
				else
				{
					this.ReadInlineImage(skipImage);
				}
			}
		}

		void InvokeOperator(string op, object[] pars, bool skipNonTextRelatedOperators)
		{
			try
			{
				if (TextOperators.IsOperator(op))
				{
					this.InvokeTextOperator(op, pars);
				}
				else if (GeneralGraphicsStateOperators.IsOperator(op))
				{
					this.InvokeGeneralGraphicsStateOperator(op, pars);
				}
				else if (XObjectOperators.IsOperator(op))
				{
					this.InvokeXObjectOperator(op, pars, skipNonTextRelatedOperators);
				}
				else if (ImageOperators.IsOperator(op))
				{
					this.InvokeImageOperator(op, skipNonTextRelatedOperators);
				}
				else if (!skipNonTextRelatedOperators)
				{
					if (ColorOperators.IsOperator(op))
					{
						this.InvokeColorOperator(op, pars);
					}
					else if (PathOperators.IsOperator(op))
					{
						this.InvokePathOperator(op, pars);
					}
				}
			}
			catch (Exception documentException)
			{
				this.OnOnException(new OnDocumentExceptionEventArgs(documentException));
			}
		}

		bool ParseCurrentTokenToBool()
		{
			return this.reader.Result == "true";
		}

		int ParseCurrentTokenToInt()
		{
			int result;
			int.TryParse(this.reader.Result, out result);
			return result;
		}

		double ParseCurrentTokenToReal()
		{
			double result;
			double.TryParse(this.reader.Result, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out result);
			return result;
		}

		PdfStringOld ParseCurrentTokenToString()
		{
			if (this.reader.TokenType == Token.String)
			{
				return new PdfStringOld(this.contentManager, this.reader.BytesToken);
			}
			return null;
		}

		PdfNameOld ParseCurrentTokenToName(bool replaceAbbreviation)
		{
			if (this.reader.TokenType != Token.Name)
			{
				return null;
			}
			if (!replaceAbbreviation)
			{
				return new PdfNameOld(this.contentManager, this.reader.Result);
			}
			return new PdfNameOld(this.contentManager, Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.PdfNames.GetNameFromAbbreviation(this.reader.Result));
		}

		PdfArrayOld ParseCurrentTokenToArray(bool replaceAbbreviations)
		{
			if (this.reader.TokenType == Token.ArrayStart)
			{
				PdfArrayOld pdfArrayOld = new PdfArrayOld(this.contentManager);
				object[] content = this.ReadObjectsToToken(Token.ArrayEnd, replaceAbbreviations);
				pdfArrayOld.Load(content);
				return pdfArrayOld;
			}
			return null;
		}

		PdfDictionaryOld ParseCurrentTokenToDictionary(bool replaceAbbreviations)
		{
			if (this.reader.TokenType == Token.DictionaryStart)
			{
				PdfDictionaryOld pdfDictionaryOld = new PdfDictionaryOld(this.contentManager);
				object[] content = this.ReadObjectsToToken(Token.DictionaryEnd, replaceAbbreviations);
				pdfDictionaryOld.Load(content);
				return pdfDictionaryOld;
			}
			return null;
		}

		readonly PdfContentManager contentManager;

		readonly ContentStreamReader reader;

		readonly PdfContext context;

		readonly PdfResourceOld resources;

		readonly IndirectReferenceOld contentOwnerReference;

		int inlineImageCount;
	}
}
