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

		const int XP_EARNED_FOR_SUCCESSFUL_ATTACK = 10;

		public AttackResult Attack(Character attacker, Character defender, int? attackRoll = null) {
			if (attackRoll == null) {
				throw new NotImplementedException("Random dice roll not implemented; specify a roll");
			}

			var result = new AttackResult { AttackRoll = attackRoll.Value };

			var isCriticalHit = (attackRoll == 20);
			var isCriticalMiss = (attackRoll == 1);
			var attackDoesNotBeatArmor = ((attackRoll + attacker.GetModifier(Attribute.Strength)) < defender.EffectiveArmorClass);

			if (isCriticalMiss || attackDoesNotBeatArmor) {
				result.Success = false;
				return result;
			}

			result.Success = true;
			result.EarnedXP = XP_EARNED_FOR_SUCCESSFUL_ATTACK;

			var baseDamage = 1;
			var damageTaken = baseDamage + attacker.GetModifier(Attribute.Strength);

			if (isCriticalHit)
				damageTaken = damageTaken * 2;

			// strength modifier should never result in less than 1 point of damage			
			if (damageTaken < 1)
				damageTaken = 1;

			result.Damage = damageTaken;
			defender.TakeDamage(damageTaken);

			return result;
		}
	}
}
