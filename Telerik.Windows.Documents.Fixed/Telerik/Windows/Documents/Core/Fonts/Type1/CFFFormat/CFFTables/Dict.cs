using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.Data;
using Telerik.Windows.Documents.Core.Fonts.Type1.Data;
using Telerik.Windows.Documents.Core.Fonts.Type1.Encoding;
using Telerik.Windows.Documents.Core.Fonts.Type1.Utils;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables
{
	class Dict : CFFTable
	{
		static bool IsOperator(byte b)
		{
			return 0 <= b && b <= 21;
		}

		static bool IsTwoByteOperator(byte b)
		{
			return b == 12;
		}

		public Dict(ICFFFontFile file, long offset, int length)
			: base(file, offset)
		{
			this.length = length;
		}

		protected Dictionary<OperatorDescriptor, OperandsCollection> Data { get; set; }

		public long SkipOffset
		{
			get
			{
				return base.Offset + (long)this.length;
			}
		}

		protected int GetInt(OperatorDescriptor op)
		{
			OperandsCollection operandsCollection;
			if (this.Data.TryGetValue(op, out operandsCollection))
			{
				return operandsCollection.GetLastAsInt();
			}
			if (op.DefaultValue != null)
			{
				return (int)op.DefaultValue;
			}
			throw new ArgumentException("Operator not found");
		}

		protected bool TryGetOperands(OperatorDescriptor op, out OperandsCollection operands)
		{
			return this.Data.TryGetValue(op, out operands);
		}

		protected double GetNumber(OperatorDescriptor op)
		{
			OperandsCollection operandsCollection;
			if (this.Data.TryGetValue(op, out operandsCollection))
			{
				return operandsCollection.GetLastAsReal();
			}
			if (op.DefaultValue != null)
			{
				return (double)op.DefaultValue;
			}
			throw new ArgumentException("Operator not found");
		}

		protected PostScriptArray GetArray(OperatorDescriptor op)
		{
			OperandsCollection operandsCollection;
			if (this.Data.TryGetValue(op, out operandsCollection))
			{
				PostScriptArray postScriptArray = new PostScriptArray();
				while (operandsCollection.Count > 0)
				{
					postScriptArray.Add(operandsCollection.GetFirst());
				}
				return postScriptArray;
			}
			if (op.DefaultValue != null)
			{
				return (PostScriptArray)op.DefaultValue;
			}
			throw new ArgumentException("Operator not found");
		}

		public override void Read(CFFFontReader reader)
		{
			byte[] array = new byte[this.length];
			reader.Read(array, this.length);
			EncodedDataReader encodedDataReader = new EncodedDataReader(array, ByteEncoding.DictByteEncodings);
			this.Data = new Dictionary<OperatorDescriptor, OperandsCollection>();
			OperandsCollection operandsCollection = new OperandsCollection();
			while (!encodedDataReader.EndOfFile)
			{
				byte b = encodedDataReader.Peek(0);
				if (Dict.IsOperator(b))
				{
					OperatorDescriptor key;
					if (Dict.IsTwoByteOperator(b))
					{
						key = new OperatorDescriptor(Helper.CreateByteArray(new byte[]
						{
							encodedDataReader.Read(),
							encodedDataReader.Read()
						}));
					}
					else
					{
						key = new OperatorDescriptor(encodedDataReader.Read());
					}
					this.Data[key] = operandsCollection;
					operandsCollection = new OperandsCollection();
				}
				else
				{
					operandsCollection.AddLast(encodedDataReader.ReadOperand());
				}
			}
		}

		readonly int length;
	}
}
