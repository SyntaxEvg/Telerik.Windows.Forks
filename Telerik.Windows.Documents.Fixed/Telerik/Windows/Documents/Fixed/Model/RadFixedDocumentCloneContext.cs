using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Fixed.Model.Navigation;

namespace Telerik.Windows.Documents.Fixed.Model
{
	class RadFixedDocumentCloneContext
	{
		public RadFixedDocumentCloneContext()
		{
			this.pageToClonedPage = new Dictionary<RadFixedPage, RadFixedPage>();
			this.clippingToClonedClipping = new Dictionary<Clipping, Clipping>();
			this.destinationToClonedDestination = new Dictionary<Destination, Destination>();
			this.radioOptionToClonedRadioOption = new Dictionary<RadioOption, RadioOption>();
			this.choiceOptionToClonedChoiceOption = new Dictionary<ChoiceOption, ChoiceOption>();
			this.annotationToClonedAnnotation = new Dictionary<Annotation, Annotation>();
			this.formFieldToClonedFormField = new Dictionary<FormField, FormField>();
		}

		public RadFixedPage GetClonedPage(RadFixedPage originalPage)
		{
			return this.GetClonedObject<RadFixedPage>(originalPage, this.pageToClonedPage);
		}

		public Clipping GetClonedClipping(Clipping originalClipping)
		{
			return this.GetClonedObject<Clipping>(originalClipping, this.clippingToClonedClipping);
		}

		public Destination GetClonedDestination(Destination originalDestination)
		{
			return this.GetClonedObject<Destination>(originalDestination, this.destinationToClonedDestination);
		}

		internal Annotation GetClonedAnnotation(Annotation originalAnnotation)
		{
			return this.GetClonedObject<Annotation>(originalAnnotation, this.annotationToClonedAnnotation);
		}

		internal ChoiceOption GetClonedOption(ChoiceOption option)
		{
			return this.GetClonedObject<ChoiceOption>(option, this.choiceOptionToClonedChoiceOption);
		}

		internal RadioOption GetClonedOption(RadioOption option)
		{
			return this.GetClonedObject<RadioOption>(option, this.radioOptionToClonedRadioOption);
		}

		internal FormField GetClonedField(FormField field)
		{
			FormField clonedInstanceWithoutWidgets;
			if (!this.formFieldToClonedFormField.TryGetValue(field, out clonedInstanceWithoutWidgets))
			{
				clonedInstanceWithoutWidgets = field.GetClonedInstanceWithoutWidgets(this);
				this.formFieldToClonedFormField.Add(field, clonedInstanceWithoutWidgets);
			}
			return clonedInstanceWithoutWidgets;
		}

		T GetClonedObject<T>(T originalObject, Dictionary<T, T> objectToClonedObject) where T : class, IContextClonable<T>
		{
			if (originalObject == null)
			{
				return default(T);
			}
			T t;
			if (!objectToClonedObject.TryGetValue(originalObject, out t))
			{
				t = originalObject.Clone(this);
				objectToClonedObject.Add(originalObject, t);
			}
			return t;
		}

		readonly Dictionary<RadFixedPage, RadFixedPage> pageToClonedPage;

		readonly Dictionary<Clipping, Clipping> clippingToClonedClipping;

		readonly Dictionary<Destination, Destination> destinationToClonedDestination;

		readonly Dictionary<RadioOption, RadioOption> radioOptionToClonedRadioOption;

		readonly Dictionary<ChoiceOption, ChoiceOption> choiceOptionToClonedChoiceOption;

		readonly Dictionary<Annotation, Annotation> annotationToClonedAnnotation;

		readonly Dictionary<FormField, FormField> formFieldToClonedFormField;
	}
}
