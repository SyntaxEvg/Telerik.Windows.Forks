using System;
using Telerik.Windows.Documents.Fixed.Model.DigitalSignatures;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature
{
	abstract class TransformParametersElement<T> : TransformParametersElementBase where T : TransformParameters
	{
		protected abstract void CopyPropertiesTo(T transformParameters);

		internal override void CopyPropertiesTo(TransformParameters transformParameters)
		{
			this.CopyPropertiesTo((T)((object)transformParameters));
		}
	}
}
