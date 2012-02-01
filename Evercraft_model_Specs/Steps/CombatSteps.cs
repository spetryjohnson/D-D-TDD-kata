using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Evercraft_model;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Rhino.Mocks;

namespace Evercraft_model_Specs.Steps {

	public class CombatContext {
		public Character Character1 { get; set; }
		public Character Character2 { get; set; }

		public bool LastAttackResult { get; set; }
	}

	[Binding]
	public class CombatSteps : StepsBase {

		CombatContext Combat { get; set; }

		public CombatSteps(CombatContext context) {
			Combat = context;
		}

		[Given(@"two Characters are ready for combat")]
		public void Given_two_characters_are_ready_for_combat() {
			Combat.Character1 = MockRepository.GeneratePartialMock<Character>();
			Combat.Character2 = MockRepository.GeneratePartialMock<Character>();
		}

		[Given(@"the defender has (\d+) hit point")]
		public void Given_the_defender_has_specific_hit_points(int hitPoints) {
			Combat.Character2.BaseHitPoints = hitPoints;
		}

		[When(@"the attacker rolls a (\d+)")]
		public void When_the_attacker_rolls_to_attack(int attackRoll) {
			RecordCombatResult(attackRoll);
		}

		[When(@"the attacker rolls a (\d+) against an Armor Class of (\d+)")]
		public void When_the_attacker_rolls_to_attack(int attackRoll, int defenderArmorClass) {
			Combat.Character2
				.Expect(x => x.EffectiveArmorClass)
				.Return(defenderArmorClass);

			RecordCombatResult(attackRoll);
		}

		[When(@"an attack is successful")]
		public void When_an_attack_is_successful() {
			var successfulAttackRoll = Combat.Character2.EffectiveArmorClass + 1;

			RecordCombatResult(successfulAttackRoll);
		}

		[Then(@"the attack succeeds")]
		public void Then_the_attack_succeeds() {
			Assert.That(Combat.LastAttackResult, Is.True, "Expected a successful attack");
		}

		[Then(@"the attack fails")]
		public void Then_the_attack_fails() {
			Assert.That(Combat.LastAttackResult, Is.False, "Expected an unsuccessful attack");
	
		}
		
		[Then(@"the defender takes (\d+) points of damage")]
		public void Then_the_defender_takes_damage(int damage) {
			var baseHP = Combat.Character2.BaseHitPoints;
			var effectiveHP = Combat.Character2.EffectiveHitPoints;

			Assert.That(effectiveHP, Is.EqualTo(baseHP - damage), "Hit points were not reduced");
		}

		[Then(@"the defender dies")]
		public void Then_the_defender_dies() {
			Assert.That(Combat.Character2.IsDead, "Defender did not die");
		}

		private void RecordCombatResult(int attackRoll) {
			Combat.LastAttackResult = Combat.Character1.Attack(attackRoll, Combat.Character2);
		}
	}
}
