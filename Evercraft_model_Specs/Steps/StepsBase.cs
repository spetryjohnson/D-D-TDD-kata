using System;
using Evercraft_model;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Evercraft_model_Specs.Steps {
	
	public class StepsBase {
		
		protected Exception ThrownException {
			get { return ScenarioContext.Current.Get<Exception>(); }
			set { ScenarioContext.Current.Set(value); }
		}

		public void SetContext(string key, object value) {
			ScenarioContext.Current[key] = value;
		}

		public T GetContext<T>(string key) {
			return ScenarioContext.Current.Get<T>(key);
		}
	}
}