using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.Data;
using Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings.Operators;
using Telerik.Windows.Documents.Core.Fonts.Type1.Data;
using Telerik.Windows.Documents.Core.Fonts.Type1.Encoding;
using Telerik.Windows.Documents.Core.Fonts.Type1.Utils;
using Telerik.Windows.Documents.Core.PostScript.Data;
using Telerik.Windows.Documents.Core.Shapes;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings
{
	class BuildChar
	{
		internal static Point CalculatePoint(BuildChar interpreter, int dx, int dy)
		{
			interpreter.CurrentPoint = new Point(interpreter.CurrentPoint.X + (double)dx, interpreter.CurrentPoint.Y + (double)dy);
			return new Point(interpreter.CurrentPoint.X, -interpreter.CurrentPoint.Y);
		}

		static void InitializePathConstructionOperators()
		{
			BuildChar.operators[new OperatorDescriptor(1)] = new HintOperator();
			BuildChar.vStem = new HintOperator();
			BuildChar.operators[new OperatorDescriptor(3)] = BuildChar.vStem;
			BuildChar.operators[new OperatorDescriptor(4)] = new VMoveTo();
			BuildChar.operators[new OperatorDescriptor(5)] = new RLineTo();
			BuildChar.operators[new OperatorDescriptor(6)] = new HLineTo();
			BuildChar.operators[new OperatorDescriptor(7)] = new VLineTo();
			BuildChar.operators[new OperatorDescriptor(8)] = new RRCurveTo();
			BuildChar.endChar = new OperatorDescriptor(14);
			BuildChar.operators[BuildChar.endChar] = new EndChar();
			BuildChar.operators[new OperatorDescriptor(18)] = new HintOperator();
			BuildChar.operators[new OperatorDescriptor(19)] = new HintMaskOperator();
			BuildChar.operators[new OperatorDescriptor(20)] = new HintMaskOperator();
			BuildChar.operators[new OperatorDescriptor(21)] = new RMoveTo();
			BuildChar.operators[new OperatorDescriptor(22)] = new HMoveTo();
			BuildChar.operators[new OperatorDescriptor(23)] = new HintOperator();
			BuildChar.operators[new OperatorDescriptor(24)] = new RCurveLine();
			BuildChar.operators[new OperatorDescriptor(25)] = new RLineCurve();
			BuildChar.operators[new OperatorDescriptor(26)] = new VVCurveTo();
			BuildChar.operators[new OperatorDescriptor(27)] = new HHCurveTo();
			BuildChar.operators[new OperatorDescriptor(30)] = new VHCurveTo();
			BuildChar.operators[new OperatorDescriptor(31)] = new HVCurveTo();
			BuildChar.operators[new OperatorDescriptor(11)] = new Return();
			BuildChar.operators[new OperatorDescriptor(10)] = new CallSubr();
			BuildChar.operators[new OperatorDescriptor(9)] = new ClosePath();
			BuildChar.operators[new OperatorDescriptor(13)] = new Hsbw();
			BuildChar.operators[new OperatorDescriptor(29)] = new CallGSubr();
			BuildChar.operators[new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 12 }))] = new Div();
			BuildChar.operators[new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 35 }))] = new Flex();
			BuildChar.operators[new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 37 }))] = new Flex1();
			BuildChar.operators[new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 34 }))] = new HFlex();
			BuildChar.operators[new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 36 }))] = new HFlex1();
			BuildChar.operators[new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 16 }))] = new CallOtherSubr();
			BuildChar.operators[new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 17 }))] = new Pop();
			BuildChar.operators[new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 6 }))] = new Seac();
			BuildChar.operators[new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 7 }))] = new Sbw();
			BuildChar.operators[new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 33 }))] = new SetCurrentPoint();
		}

		static bool IsOperator(byte b)
		{
			return b != 28 && 0 <= b && b <= 31;
		}

		static bool IsTwoByteOperator(byte b)
		{
			return b == 12;
		}

		static BuildChar()
		{
			BuildChar.InitializePathConstructionOperators();
		}

		public BuildChar(IBuildCharHolder subrsHodler)
		{
			this.buildCharHolder = subrsHodler;
		}

		internal OperandsCollection Operands
		{
			get
			{
				return this.operands;
			}
		}

		internal OperandsCollection PostScriptStack
		{
			get
			{
				return this.postScriptStack;
			}
		}

		internal PathFigure CurrentPathFigure { get; set; }

		internal GlyphOutlinesCollection GlyphOutlines { get; set; }

		internal Point CurrentPoint { get; set; }

		internal Point BottomLeft { get; set; }

		internal int? Width { get; set; }

		internal int Hints { get; set; }

		public void ExecuteSubr(int index)
		{
			byte[] subr = this.buildCharHolder.GetSubr(index);
			this.ExecuteInternal(subr);
		}

		public void ExecuteGlobalSubr(int index)
		{
			byte[] globalSubr = this.buildCharHolder.GetGlobalSubr(index);
			this.ExecuteInternal(globalSubr);
		}

		public void CombineChars(string accentedChar, string baseChar, int dx, int dy)
		{
			GlyphData glyphData = this.buildCharHolder.GetGlyphData(accentedChar);
			GlyphOutlinesCollection glyphOutlinesCollection = glyphData.Oultlines.Clone();
			GlyphData glyphData2 = this.buildCharHolder.GetGlyphData(baseChar);
			GlyphOutlinesCollection collection = glyphData2.Oultlines.Clone();
			this.GlyphOutlines.AddRange(collection);
			glyphOutlinesCollection.Transform(new Matrix(1.0, 0.0, 0.0, 1.0, (double)dx, (double)dy));
			this.GlyphOutlines.AddRange(glyphOutlinesCollection);
		}

		public void Execute(byte[] data)
		{
			this.postScriptStack = new OperandsCollection();
			this.operands = new OperandsCollection();
			this.GlyphOutlines = new GlyphOutlinesCollection();
			this.CurrentPoint = default(Point);
			this.Width = null;
			this.Hints = 0;
			this.ExecuteInternal(data);
		}

		void ExecuteInternal(byte[] data)
		{
			EncodedDataReader encodedDataReader = new EncodedDataReader(data, ByteEncoding.CharStringByteEncodings);
			while (!encodedDataReader.EndOfFile)
			{
				byte b = encodedDataReader.Peek(0);
				if (BuildChar.IsOperator(b))
				{
					OperatorDescriptor operatorDescriptor;
					if (BuildChar.IsTwoByteOperator(b))
					{
						operatorDescriptor = new OperatorDescriptor(Helper.CreateByteArray(new byte[]
						{
							encodedDataReader.Read(),
							encodedDataReader.Read()
						}));
					}
					else
					{
						operatorDescriptor = new OperatorDescriptor(encodedDataReader.Read());
					}
					this.ExecuteOperator(operatorDescriptor, encodedDataReader);
					if (operatorDescriptor.Equals(BuildChar.endChar))
					{
						return;
					}
				}
				else
				{
					this.Operands.AddLast(encodedDataReader.ReadOperand());
				}
			}
		}

		int GetMaskSize()
		{
			int num = this.Hints / 8;
			if (this.Hints % 8 != 0)
			{
				num++;
			}
			return num;
		}

		void ExecuteOperator(OperatorDescriptor descr, EncodedDataReader reader)
		{
			Operator @operator;
			if (!BuildChar.operators.TryGetValue(descr, out @operator))
			{
				this.Operands.Clear();
				return;
			}
			if (@operator is HintMaskOperator)
			{
				int num;
				BuildChar.vStem.Execute(this, out num);
				this.Hints += num;
				byte[] array = new byte[this.GetMaskSize()];
				reader.Read(array, array.Length);
				((HintMaskOperator)@operator).Execute(this, array);
				return;
			}
			if (@operator is HintOperator)
			{
				int num;
				((HintOperator)@operator).Execute(this, out num);
				this.Hints += num;
				return;
			}
			@operator.Execute(this);
		}

		static readonly Dictionary<OperatorDescriptor, Operator> operators = new Dictionary<OperatorDescriptor, Operator>();

		static OperatorDescriptor endChar;

		static HintOperator vStem;

		readonly IBuildCharHolder buildCharHolder;

		OperandsCollection postScriptStack;

		OperandsCollection operands;
	}
}
