Feature: Combat
	In order to play Evercraft
	As a character
	I want to be able to kick someone's ass
	
Scenario: Attacks succeed when the attack roll is greater than or equal to the opponents Armor Class
	Given two Characters are ready for combat

	When the attacker rolls a 13 against an Armor Class of 12
	Then the attack succeeds

	When the attacker rolls a 12 against an Armor Class of 12
	Then the attack succeeds

	When the attacker rolls a 11 against an Armor Class of 12
	Then the attack fails


Scenario: Attacks fail when the attack roll is 1 regardless of opponents Armor Class
	Given two Characters are ready for combat
	When the attacker rolls a 1 against an Armor Class of 0
	Then the attack fails

	
Scenario: Successful attacks cause damage equal to 1 point plus attacker's Strength modifier
	Given two Characters are ready for combat
	And the attacker's Strength modifier is 2
	When an attack is successful
	Then the defender takes 3 points of damage

	
Scenario: A negative Strength modifier never reduces damage to less than 1 point
	Given two Characters are ready for combat
	And the attacker's Strength modifier is -2
	When an attack is successful
	Then the defender takes 1 points of damage
	

Scenario: Critical hits do double damage to the defender
	Given two Characters are ready for combat
	When the attacker rolls a 20
	Then the defender takes 2 points of damage
	

Scenario: Critical hits double the Strength modifier when calculating damage
	Given two Characters are ready for combat
	And the attacker's Strength modifier is 2
	When the attacker rolls a 20
	Then the defender takes 6 points of damage


Scenario: Characters die when their Effective Hit Points are equal to or less than zero
	Given two Characters are ready for combat
	And the defender has 1 hit point
	When an attack is successful
	Then the defender dies
