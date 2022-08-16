using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature
{
	class SignatureExportInfo
	{
		internal int DocumentStartPosition
		{
			get
			{
				return this.documentStartPosition;
			}
		}

		internal int DocumentEndPosition
		{
			get
			{
				return this.documentEndPosition;
			}
		}

		internal void SetSignatureFieldName(string signatureFieldName)
		{
			if (!string.IsNullOrEmpty(this.currentSignatureFieldName))
			{
				throw new ArgumentException("New signature export is triggered before the last one has finished.");
			}
			this.currentSignatureFieldName = signatureFieldName;
			this.signaturePositionsStore.Add(this.currentSignatureFieldName, new SignaturePositions());
		}

		internal void ClearSignatureFieldName()
		{
			this.currentSignatureFieldName = null;
		}

		internal void AddDocumentStartPosition(int position)
		{
			this.documentStartPosition = position;
		}

		internal void AddDocumentEndPosition(int position)
		{
			this.documentEndPosition = position;
		}

		internal void AddByteRangeStartPosition(int position)
		{
			this.signaturePositionsStore[this.currentSignatureFieldName].ByteRangeStartPosition = position;
		}

		internal void AddByteRangeEndPosition(int position)
		{
			this.signaturePositionsStore[this.currentSignatureFieldName].ByteRangeEndPosition = position;
		}

		internal void AddContentStartPosition(int position)
		{
			this.signaturePositionsStore[this.currentSignatureFieldName].ContentStartPosition = position;
		}

		internal void AddContentEndPosition(int position)
		{
			this.signaturePositionsStore[this.currentSignatureFieldName].ContentEndPosition = position;
		}

		internal int[] ComposeByteRangeArray(string signatureFieldName)
		{
			return new int[]
			{
				this.DocumentStartPosition,
				this.signaturePositionsStore[signatureFieldName].ContentStartPosition - this.DocumentStartPosition,
				this.signaturePositionsStore[signatureFieldName].ContentEndPosition + 1,
				this.DocumentEndPosition - this.signaturePositionsStore[signatureFieldName].ContentEndPosition
			};
		}

		internal SignaturePositions GetSignaturePositions(string signatureFieldName)
		{
			return this.signaturePositionsStore[signatureFieldName];
		}

		Dictionary<string, SignaturePositions> signaturePositionsStore = new Dictionary<string, SignaturePositions>();

		int documentStartPosition;

		int documentEndPosition;

		string currentSignatureFieldName;
	}
}
