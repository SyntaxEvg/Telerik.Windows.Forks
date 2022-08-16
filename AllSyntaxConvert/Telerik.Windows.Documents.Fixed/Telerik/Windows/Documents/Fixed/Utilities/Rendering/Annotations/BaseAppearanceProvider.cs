using System;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.Utilities.Rendering.Annotations
{
	abstract class BaseAppearanceProvider
	{
		internal BaseAppearanceProvider(BaseAppearanceProvider nextProvider)
		{
			this.nextProvider = nextProvider;
		}

		public bool TryProvideAppearance(Annotation annotation, AnnotationAppearanceMode annotationAppearanceMode, out FormSource appearance)
		{
			bool result = false;
			appearance = null;
			if (this.CanProvide(annotation, annotationAppearanceMode))
			{
				appearance = this.ProvideAppearance(annotation, annotationAppearanceMode);
				result = appearance != null;
			}
			else if (this.nextProvider != null)
			{
				result = this.nextProvider.TryProvideAppearance(annotation, annotationAppearanceMode, out appearance);
			}
			return result;
		}

		protected FormSource GetAppearanceFormSource(SingleStateAppearances singleStateAppearances, AnnotationAppearanceMode annotationAppearanceMode)
		{
			FormSource result;
			switch (annotationAppearanceMode)
			{
			case AnnotationAppearanceMode.Normal:
				result = singleStateAppearances.NormalAppearance;
				break;
			case AnnotationAppearanceMode.Rollover:
				result = singleStateAppearances.MouseOverAppearance;
				break;
			case AnnotationAppearanceMode.Down:
				result = singleStateAppearances.MouseDownAppearance;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		protected abstract bool CanProvide(Annotation annotation, AnnotationAppearanceMode annotationAppearanceMode);

		protected abstract FormSource ProvideAppearance(Annotation annotation, AnnotationAppearanceMode annotationAppearanceMode);

		readonly BaseAppearanceProvider nextProvider;
	}
}
