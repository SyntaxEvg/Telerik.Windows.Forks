using System;
using System.Collections.Generic;
using System.IO;

namespace Telerik.Windows.Zip
{
	abstract class ExtraFieldBase
	{
		public ExtraFieldType ExtraFieldType
		{
			get
			{
				return ExtraFieldBase.GetExtraFieldType(this.HeaderId);
			}
		}

		protected abstract ushort HeaderId { get; }

		public static IEnumerable<ExtraFieldBase> GetExtraFields(FileHeaderInfoBase headerInfo)
		{
			MemoryStream stream = new MemoryStream(headerInfo.ExtraFieldsData);
			using (BinaryReader reader = new BinaryReader(stream))
			{
				while (stream.Position < stream.Length)
				{
					ExtraFieldBase field = ExtraFieldBase.GetExtraField(headerInfo, reader);
					yield return field;
				}
			}
			yield break;
		}

		public static byte[] GetExtraFieldsData(IEnumerable<ExtraFieldBase> fields)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					foreach (ExtraFieldBase extraFieldBase in fields)
					{
						binaryWriter.Write(extraFieldBase.HeaderId);
						byte[] block = extraFieldBase.GetBlock();
						binaryWriter.Write((ushort)block.Length);
						binaryWriter.Write(block);
					}
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		protected abstract void ParseBlock(byte[] extraData);

		protected abstract byte[] GetBlock();

		static ExtraFieldBase GetExtraField(FileHeaderInfoBase headerInfo, BinaryReader reader)
		{
			ushort num = reader.ReadUInt16();
			ExtraFieldType extraFieldType = ExtraFieldBase.GetExtraFieldType(num);
			ushort count = reader.ReadUInt16();
			byte[] extraData = reader.ReadBytes((int)count);
			ExtraFieldType extraFieldType2 = extraFieldType;
			ExtraFieldBase extraFieldBase;
			if (extraFieldType2 != ExtraFieldType.Zip64)
			{
				if (extraFieldType2 == ExtraFieldType.AesEncryption)
				{
					extraFieldBase = new AesEncryptionExtraField();
				}
				else
				{
					extraFieldBase = new UnknownExtraField(num);
				}
			}
			else
			{
				extraFieldBase = new Zip64ExtraField(headerInfo);
			}
			extraFieldBase.ParseBlock(extraData);
			return extraFieldBase;
		}

		static ExtraFieldType GetExtraFieldType(ushort headerID)
		{
			if (Enum.IsDefined(typeof(ExtraFieldType), (int)headerID))
			{
				return (ExtraFieldType)headerID;
			}
			return ExtraFieldType.Unknown;
		}
	}
}
