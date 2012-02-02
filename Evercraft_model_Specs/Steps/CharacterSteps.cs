using System;
using NUnit.Framework;
using Rhino.Mocks;
using TechTalk.SpecFlow;
using Heuristics.Library.Extensions;
using Evercraft_model;
using Attribute = Evercraft_model.Attribute;

namespace Evercraft_model_Specs.Steps {

	[Binding]
	public class CharacterSteps : StepsBase {

		protected Character Character {
			get { return ScenarioContext.Current.Get<Character>(); }
			set { ScenarioContext.Current.Set(value); }
		}

		[Given(@"I have created a new character")]
		public void Given_I_have_created_a_new_character() {
			Character = MockRepository.GeneratePartialMock<Character>();
		}

		[Given(@"the Base Hit Points are (\d+)")]
		public void GivenTheBaseHitPointsAre5(int baseHP) {
			Character.BaseHitPoints = baseHP;
		}

		[Given(@"the Base Armor Class is (\d+)")]
		public void Given_the_base_armor_class_is(int baseArmorClass) {
			Character.BaseArmorClass = baseArmorClass;
		}

		[Given(@"the (\w+) modifier is (-?\d+)")]
		public void Given_a_specific_attribute_modifier(string attribute, int modifier) {
			Character
				.Stub(x => x.GetModifier(attribute.ToEnum<Attribute>()))
				.Return(modifier);
		}
		
		[Given(@"the character has taken (\d+) points of damage")]
		public void Given_the_character_has_taken_damage(int damageTaken) {
			Character.TakeDamage(damageTaken);
		}

		[When(@"I name it ""(.*)""")]
		public void When_I_name_it(string name) {
			Character.Name = name;
		}

		[When(@"I give it an alignment of ""(.*)""")]
		public void When_I_give_it_an_alignment_of_(string alignment) {
			Character.Alignment = alignment.ToEnum<Alignment>();
		}

		[When(@"I give it a (.*) attribute of (\d+)")]
		public void When_I_give_it_an_attribute_of(string attribute, int value) {
			Character.SetAttribute(attribute.ToEnum<Attribute>(), value);
		}

		[Then(@"the character name should be ""(.*)""")]
		public void Then_the_character_name_should_be(string name) {
			Assert.That(Character.Name, Is.EqualTo(name));
		}

		[Then(@"the Character alignment should be ""(.*)""")]
		public void Then_the_character_alignment_should_be(string alignment) {
			Assert.That(Character.Alignment, Is.EqualTo(alignment.ToEnum<Alignment>()));
		}

		[Then(@"the Base Hit Points should be (\d+)")]
		public void Then_the_Base_Hit_Points_should_be(int hitPoints) {
			Assert.That(Character.BaseHitPoints, Is.EqualTo(hitPoints));
		}

		[Then(@"the Effective Hit Points should be (-?\d+)")]
		public void Then_the_Effective_Hit_Points_should_be(int hitPoints) {
			Assert.That(Character.EffectiveHitPoints, Is.EqualTo(hitPoints), "Incorrect Effective HP");
		}

		[Then(@"the Armor Class should be (\d+)")]
		public void Then_the_Armor_Class_should_be(int armorClass) {
			Assert.That(Character.BaseArmorClass, Is.EqualTo(armorClass));
		}

		[Then(@"the Effective Armor Class should be (\d+)")]
		public void Then_the_Effective_Armor_Class_should_be(int armorClass) {
			Assert.That(Character.EffectiveArmorClass, Is.EqualTo(armorClass));
		}

		[Then(@"the Experience Points should be (\d+)")]
		public void Then_the_Experience_Points_should_be(int xp) {
			Assert.That(Character.ExperiencePoints, Is.EqualTo(xp));
		}

		[Then(@"the (.*) attribute should be (\d+)")]
		public void Then_the_given_attribute_should_be(string attribute, int value) {
			var actualValue = Character.GetAttribute(attribute.ToEnum<Attribute>());

			Assert.That(actualValue, Is.EqualTo(value), "Incorrect '" + attribute + "' attribute.");
		}

		[Then(@"the system should not let me enter an attribute less than (\d+)")]
		public void Then_the_system_should_not_let_me_enter_an_attribute_less_than(int minValue) {
			var tooLowValue = minValue - 1;

			foreach (string attrName in Enum.GetNames(typeof(Attribute))) {
				var attr = attrName.ToEnum<Attribute>();

				try {
					Character.SetAttribute(attr, tooLowValue);

					Assert.Fail("Expected exception was not thrown setting " + attr.ToString() + " to " + tooLowValue);
				}
				catch (ArgumentOutOfRangeException) { /* this passes test */ }
			}
		}

		[Then(@"the system should not let me enter an attribute greater than (\d+)")]
		public void Then_the_system_should_not_let_me_enter_an_attribute_greater_than(int maxValue) {
			var tooHighValue = maxValue + 1;

			foreach (string attrName in Enum.GetNames(typeof(Attribute))) {
				var attr = attrName.ToEnum<Attribute>();

				try {
					Character.SetAttribute(attr, tooHighValue);

					Assert.Fail("Expected exception was not thrown setting " + attr.ToString() + " to " + tooHighValue);
				}
				catch (ArgumentOutOfRangeException) { /* this passes test */ }
			}
		}

		[Then(@"an attribute value of (\d+) should have a modifier of (-?\d+)")]
		public void Then_a_given_attribute_value_should_have_a_given_modifier(int attributeValue, int modifier) {
			foreach (string attrName in Enum.GetNames(typeof(Attribute))) {
				var attr = attrName.ToEnum<Attribute>();

				Character.SetAttribute(attr, attributeValue);

				Assert.That(Character.GetModifier(attr), Is.EqualTo(modifier)
					, "Incorrect modifier for " + attr.ToString()
				);
			}
		}

		[Then(@"the Effective Armor Class equals the base Armor Class plus the Dexterity modifier")]
		public void Then_the_Effective_Armor_Class_equals_the_base_Armor_Class_plus_the_Dexterity_modifier() {
			var DEXTERITY = Attribute.Dexterity;

			// test with a negative modifier
			Character.SetAttribute(DEXTERITY, 1);
			Assert.That(Character.EffectiveArmorClass, Is.EqualTo(Character.BaseArmorClass + Character.GetModifier(DEXTERITY))
				, "Incorrect effective armor class with a negative dexterity modifier."
			);

			// test with a positive modifier
			Character.SetAttribute(DEXTERITY, 20);
			Assert.That(Character.EffectiveArmorClass, Is.EqualTo(Character.BaseArmorClass + Character.GetModifier(DEXTERITY))
				, "Incorrect effective armor class with a positive dexterity modifier."
			);
		}

		[Then(@"the Effective Hit Points equal the base Hit Points plus the Constitution modifier")]
		public void Then_the_Effective_Hit_Points_equal_the_base_Hit_Points_plus_the_Constitution_modifier() {
			var CONSTITUTION = Attribute.Constitution;

			// test with a negative modifier
			Character
				.Expect(x => x.GetModifier(CONSTITUTION))
				.Return(-1);

			Assert.That(Character.EffectiveHitPoints, Is.EqualTo(Character.BaseHitPoints - 1)
				, "Incorrect effective hit points with a negative modifier."
			);

			// test with a positive modifier
			Character
				.Expect(x => x.GetModifier(CONSTITUTION))
				.Return(1);

			Assert.That(Character.EffectiveHitPoints, Is.EqualTo(Character.BaseHitPoints + 1)
				, "Incorrect effective hit points with a positive modifier."
			);
		}
	}
}

