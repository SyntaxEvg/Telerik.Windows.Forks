using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Internal.Annotations
{
	class MultiStateAppearancesOld : AnnotationAppearancesOld
	{
		public MultiStateAppearancesOld(string currentState, IEnumerable<KeyValuePair<string, SingleStateAppearancesOld>> statesToAppearances)
		{
			this.statesToAppearance = new Dictionary<string, SingleStateAppearancesOld>();
			foreach (KeyValuePair<string, SingleStateAppearancesOld> keyValuePair in statesToAppearances)
			{
				this.statesToAppearance.Add(keyValuePair.Key, keyValuePair.Value);
			}
			this.CurrentState = currentState;
		}

		public string CurrentState
		{
			get
			{
				return this.currentState;
			}
			set
			{
				Guard.ThrowExceptionIfNullOrEmpty(value, "value");
				if (this.currentState != value)
				{
					this.currentState = value;
					this.DoOnCurrentStateChanged();
				}
			}
		}

		public IEnumerable<string> States
		{
			get
			{
				return this.statesToAppearance.Keys;
			}
		}

		SingleStateAppearancesOld CurrentAppearances { get; set; }

		public override Container GetNormalAppearance()
		{
			if (this.CurrentAppearances == null)
			{
				return null;
			}
			return this.CurrentAppearances.GetNormalAppearance();
		}

		public override Container GetMouseDownAppearance()
		{
			if (this.CurrentAppearances == null)
			{
				return null;
			}
			return this.CurrentAppearances.GetMouseDownAppearance();
		}

		public override Container GetMouseOverAppearance()
		{
			if (this.CurrentAppearances == null)
			{
				return null;
			}
			return this.CurrentAppearances.GetMouseOverAppearance();
		}

		void DoOnCurrentStateChanged()
		{
			SingleStateAppearancesOld currentAppearances;
			if (this.statesToAppearance.TryGetValue(this.CurrentState, out currentAppearances))
			{
				this.CurrentAppearances = currentAppearances;
				return;
			}
			this.CurrentAppearances = null;
		}

		readonly Dictionary<string, SingleStateAppearancesOld> statesToAppearance;

		string currentState;
	}
}
