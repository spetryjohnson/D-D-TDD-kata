using Evercraft_model;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace Evercraft_model_Specs.Steps {
	
	[Binding]
	public class GlobalSteps : StepsBase {

		[Then(@"the system should display_an_error_message")]
		public void Then_the_system_should_display_an_error_message() {
			Assert.That(ThrownException, Is.Not.Null);
		}
	}
}
