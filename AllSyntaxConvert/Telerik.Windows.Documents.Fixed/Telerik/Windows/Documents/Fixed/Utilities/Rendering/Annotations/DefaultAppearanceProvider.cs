using System;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.Utilities.Rendering.Annotations
{
	class DefaultAppearanceProvider : BaseAppearanceProvider
	{
		internal DefaultAppearanceProvider(BaseAppearanceProvider nextProvider)
			: base(nextProvider)
		{
		}

		protected override bool CanProvide(Annotation annotation, AnnotationAppearanceMode annotationAppearanceMode)
		{
			return annotation != null && this.GetCurrentAnnotationAppearances(annotation) != null;
		}

		protected override FormSource ProvideAppearance(Annotation annotation, AnnotationAppearanceMode annotationAppearanceMode)
		{
			SingleStateAppearances currentAnnotationAppearances = this.GetCurrentAnnotationAppearances(annotation);
			return base.GetAppearanceFormSource(currentAnnotationAppearances, annotationAppearanceMode);
		}

		SingleStateAppearances GetCurrentAnnotationAppearances(Annotation annotation)
		{
			SingleStateAppearances result = null;
			if (annotation.Appearances is SingleStateAppearances)
			{
				result = annotation.Appearances as SingleStateAppearances;
			}
			else if (annotation.Appearances is MultiStateAppearances)
			{
				MultiStateAppearances multiStateAppearances = annotation.Appearances as MultiStateAppearances;
				result = multiStateAppearances[multiStateAppearances.CurrentState];
			}
			return result;
		}
	}
}
