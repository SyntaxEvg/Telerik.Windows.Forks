using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	class MultiStateAppearances : AnnotationAppearances, IEnumerable<KeyValuePair<string, SingleStateAppearances>>, IEnumerable
	{
		public MultiStateAppearances()
		{
			this.stateToAppearances = new Dictionary<string, SingleStateAppearances>();
		}

		public sealed override AnnotationAppearancesType AppearancesType
		{
			get
			{
				return AnnotationAppearancesType.MultiStateAppearances;
			}
		}

		internal string CurrentState { get; set; }

		public SingleStateAppearances this[string stateName]
		{
			get
			{
				return this.stateToAppearances[stateName];
			}
			set
			{
				Guard.ThrowExceptionIfNull<string>(stateName, "stateName");
				Guard.ThrowExceptionIfNull<SingleStateAppearances>(value, "value");
				this.stateToAppearances[stateName] = value;
			}
		}

		public void AddAppearance(string stateName, SingleStateAppearances stateAppearances)
		{
			Guard.ThrowExceptionIfNull<string>(stateName, "stateName");
			Guard.ThrowExceptionIfNull<SingleStateAppearances>(stateAppearances, "stateAppearances");
			this.stateToAppearances.Add(stateName, stateAppearances);
		}

		public bool TryGetAppearance(string stateName, out SingleStateAppearances stateAppearances)
		{
			return this.stateToAppearances.TryGetValue(stateName, out stateAppearances);
		}

		public override AnnotationAppearances Clone(RadFixedDocumentCloneContext cloneContext)
		{
			MultiStateAppearances multiStateAppearances = new MultiStateAppearances();
			foreach (KeyValuePair<string, SingleStateAppearances> keyValuePair in this)
			{
				multiStateAppearances.AddAppearance(keyValuePair.Key, (SingleStateAppearances)keyValuePair.Value.Clone(cloneContext));
			}
			multiStateAppearances.CurrentState = this.CurrentState;
			return multiStateAppearances;
		}

		public IEnumerator<KeyValuePair<string, SingleStateAppearances>> GetEnumerator()
		{
			return this.stateToAppearances.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.stateToAppearances.GetEnumerator();
		}

		readonly Dictionary<string, SingleStateAppearances> stateToAppearances;
	}
}
