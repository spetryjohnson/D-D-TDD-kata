using System;
using System.Collections.Generic;

namespace Evercraft_model {
	public class Character {
		public const int DEFAULT_AC = 10;
		public const int DEFAULT_HP = 5;

		public string Name { get; set; }
		public Alignment Alignment { get; set; }
		
		/// <summary>
		/// The character's base hit points before factoring in bonuses/penalties. 
		/// </summary>
		public int BaseHitPoints { get; set; }

		/// <summary>
		/// How much damage has been taken.
		/// </summary>
		public int DamageTaken { get; protected set; }

		/// <summary>
		/// The base Hit Points, plus any modifications from bonuses/penalties, minus damage taken. The character
		/// dies when this reaches 0.
		/// </summary>
		public int EffectiveHitPoints {
			get {
				var effectiveHP = BaseHitPoints + GetModifier(Attribute.Constitution) - DamageTaken;

				// can't die from modifiers alone
				if (DamageTaken == 0 && effectiveHP <= 0)
					effectiveHP = 1;

				// dead is dead; no sense in allowing negatives
				if (effectiveHP < 0)
					effectiveHP = 0;

				return effectiveHP;
			}
		}
		
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

		public int ExperiencePoints { get; set; }

		public bool IsDead {
			get { return EffectiveHitPoints <= 0; }
		}

		public Character() {
			BaseArmorClass = DEFAULT_AC;
			BaseHitPoints = DEFAULT_HP;

			Attributes = new Dictionary<Attribute, int>();
			SetAttribute(Attribute.Strength, 10);
			SetAttribute(Attribute.Dexterity, 10);
			SetAttribute(Attribute.Constitution, 10);
			SetAttribute(Attribute.Wisdom, 10);
			SetAttribute(Attribute.Intelligence, 10);
			SetAttribute(Attribute.Charisma, 10);
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

		public void TakeDamage(int damage) {
			DamageTaken += damage;
		}
	}
}
