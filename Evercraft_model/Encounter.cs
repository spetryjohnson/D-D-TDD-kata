using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evercraft_model {
	
	/// <summary>
	/// Represents a combat scenario between characters. This provides the context for the combat and
	/// is responsible for managing the states of involved characters.
	/// </summary>
	public class Encounter {

		public AttackResult Attack(Character attacker, Character defender, int? attackRoll = null) {
			if (attackRoll == null) {
				throw new NotImplementedException("Random dice roll not implemented; specify a roll");
			}

			var isCriticalHit = (attackRoll == 20);
			var isCriticalMiss = (attackRoll == 1);
			var attackDoesNotBeatArmor = (attackRoll < defender.EffectiveArmorClass);

			if (isCriticalMiss || attackDoesNotBeatArmor) {
				return new AttackResult { AttackRoll = attackRoll.Value, Success = false };
			}

			var baseDamage = 1;
			var damageTaken = baseDamage + attacker.GetModifier(Attribute.Strength);

			if (isCriticalHit)
				damageTaken = damageTaken * 2;

			// strength modifier should never result in less than 1 point of damage			
			if (damageTaken < 1)
				damageTaken = 1;

			defender.TakeDamage(damageTaken);

			return new AttackResult { AttackRoll = attackRoll.Value, Success = true, Damage = damageTaken };
		}
	}
}
