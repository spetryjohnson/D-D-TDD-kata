using System;
using System.Collections.Generic;

namespace Evercraft_model {
	public class Character {
		public const int DEFAULT_AC = 10;

		public string Name { get; set; }
		public Alignment Alignment { get; set; }
		public int HitPoints { get; set; }
		public Dictionary<Attribute, int> Attributes { get; set; }

		/// <summary>
		/// The base Armor Class before it is modified by attributes or equipment.
		/// </summary>
		public int BaseArmorClass { get; set; }

		/// <summary>
		/// The base Armor Class plus any modifications afforded by attribues, equipment, etc.
		/// </summary>
		public virtual int EffectiveArmorClass {
			get { return BaseArmorClass + GetModifier(Attribute.Dexterity); }
		}

		public bool IsDead {
			get { return HitPoints <= 0; }
		}

		public Character() {
			BaseArmorClass = DEFAULT_AC;
			HitPoints = 5;

			Attributes = new Dictionary<Attribute, int>();
			SetAttribute(Attribute.Strength, 10);
			SetAttribute(Attribute.Dexterity, 10);
			SetAttribute(Attribute.Constitution, 10);
			SetAttribute(Attribute.Wisdom, 10);
			SetAttribute(Attribute.Intelligence, 10);
			SetAttribute(Attribute.Charisma, 10);
		}

		public bool Attack(int attackRoll, Character opponent) {
			var success = (attackRoll == 1)
				? false
				: attackRoll >= opponent.EffectiveArmorClass;

			if (success) {
				opponent.TakeDamage(attackRoll == 20);
			}

			return success;
		}

		public virtual int GetModifier(Attribute attribute) {
			var attrValue = GetAttribute(attribute);

			return (attrValue % 2 == 1)
					   ? (int)Math.Round((decimal)(attrValue - 11) / 2, 0) // odd number
					   : (int)Math.Round((decimal)(attrValue - 10) / 2, 0); // even number
		}

		public virtual int GetAttribute(Attribute attributeEnum) {
			return Attributes[attributeEnum];
		}

		public virtual void SetAttribute(Attribute attribute, int value) {
			if (value <= 0 || value >= 21)
				throw new ArgumentOutOfRangeException("Attributes must be within range of 1 - 20!");

			Attributes[attribute] = value;
		}

		private void TakeDamage(bool isCriticalHit) {
			var damageAmount = isCriticalHit ? 2 : 1;
			HitPoints -= damageAmount;
		}
	}
}
