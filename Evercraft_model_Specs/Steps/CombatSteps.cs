using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Evercraft_model;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Rhino.Mocks;
using Heuristics.Library.Extensions;

using Attribute = Evercraft_model.Attribute;

namespace Evercraft_model_Specs.Steps {

	public class CombatContext {
		public Character Attacker { get; set; }
		public Character Defender { get; set; }
		public Encounter Encounter { get; set; }

		public AttackResult LastAttackResult { get; set; }

		public CombatContext() {
			Encounter = new Encounter();
		}
	}

	[Binding]
	public class CombatSteps : StepsBase {

		CombatContext Combat { get; set; }

		public CombatSteps(CombatContext context) {
			Combat = context;
		}

		[Given(@"two Characters are ready for combat")]
		public void Given_two_characters_are_ready_for_combat() {
			Combat.Attacker = MockRepository.GeneratePartialMock<Character>();
			Combat.Defender = MockRepository.GeneratePartialMock<Character>();
		}

		[Given(@"the defender has (\d+) hit point")]
		public void Given_the_defender_has_specific_hit_points(int hitPoints) {
			Combat.Defender.BaseHitPoints = hitPoints;
		}

		[Given(@"the attacker's (\w+) modifier is (-?\d+)")]
		public void Given_the_attacker_has_a_specified_attribute_modifier(string attribute, int modifier) {
			Combat.Attacker
				.Stub(x => x.GetModifier(attribute.ToEnum<Attribute>()))
				.Return(modifier)
				.Repeat.Any();
		}

		[When(@"the attacker rolls a (\d+)")]
		public void When_the_attacker_rolls_to_attack(int attackRoll) {
			RecordCombatResult(attackRoll);
		}

		[When(@"the attacker rolls a (\d+) against an Armor Class of (\d+)")]
		public void When_the_attacker_rolls_to_attack(int attackRoll, int defenderArmorClass) {
			Combat.Defender
				.Expect(x => x.EffectiveArmorClass)
				.Return(defenderArmorClass);

			RecordCombatResult(attackRoll);
		}

		[When(@"an attack is successful")]
		public void When_an_attack_is_successful() {
			var successfulAttackRoll = 19;	// HACK: high enough that it works for current scenarios, but not critical. Probably needs changed.

			RecordCombatResult(successfulAttackRoll);

			Assert.That(Combat.LastAttackResult.Success, Is.True, "Bad test setup: attack was not successful!");
		}

		[Then(@"the attack succeeds")]
		public void Then_the_attack_succeeds() {
			Assert.That(Combat.LastAttackResult.Success, Is.True, "Expected a successful attack");
		}

		[Then(@"the attack fails")]
		public void Then_the_attack_fails() {
			Assert.That(Combat.LastAttackResult.Success, Is.False, "Expected an unsuccessful attack");
		}
		
		[Then(@"the defender takes (\d+) points of damage")]
		public void Then_the_defender_takes_damage(int damage) {
			Assert.That(Combat.LastAttackResult.Damage, Is.EqualTo(damage), "Incorrect damage result");
		}

		[Then(@"the defender dies")]
		public void Then_the_defender_dies() {
			Assert.That(Combat.Defender.IsDead, "Defender did not die");
		}
		
		[Then(@"the attacker earns (\d+) Experience Points")]
		public void Then_the_attacker_earns_experience_points(int earnedXP) {
			Assert.That(Combat.LastAttackResult.EarnedXP, Is.EqualTo(earnedXP), "Incorrect XP earned");
		}

		private void RecordCombatResult(int attackRoll) {
			Combat.LastAttackResult = Combat.Encounter.Attack(Combat.Attacker, Combat.Defender, attackRoll);
		}
	}
}
