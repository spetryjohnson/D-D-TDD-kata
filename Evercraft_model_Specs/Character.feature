Feature: Character
	In order to play the game
	As a player
	I want to be able to create a new character

Scenario: Characters have names
	Given I have created a new character
	When I name it "Funny Ass Name" 
	Then the character name should be "Funny Ass Name"


Scenario: Characters have alignments
	Given I have created a new character

	When I give it an alignment of "Evil"
	Then the Character alignment should be "Evil"

	When I give it an alignment of "Good"
	Then the Character alignment should be "Good"

	When I give it an alignment of "Neutral"
	Then the Character alignment should be "Neutral"


Scenario: Characters have Hit Points
	Given I have created a new character
	Then the Base Hit Points should be 5


Scenario: Characters have an Armor Class
	Given I have created a new character
	Then the Armor Class should be 10


Scenario: Characters attributes have default values
	Given I have created a new character
	Then the Strength attribute should be 10
	And the Dexterity attribute should be 10
	And the Constitution attribute should be 10
	And the Wisdom attribute should be 10
	And the Intelligence attribute should be 10
	And the Charisma attribute should be 10


Scenario: Character attributes can be modified from their default
	Given I have created a new character
	When I give it a Strength attribute of 9
	And I give it a Wisdom attribute of 11
	Then the Strength attribute should be 9
	And the Wisdom attribute should be 11


Scenario: Character attributes must be between 1 and 20, inclusive
	Given I have created a new character
	Then the system should not let me enter an attribute less than 1
	And the system should not let me enter an attribute greater than 20


Scenario: Character attributes have modifiers based upon their values
	Given I have created a new character
	Then an attribute value of 1 should have a modifier of -5
	And an attribute value of 2 should have a modifier of -4
	And an attribute value of 3 should have a modifier of -4
	And an attribute value of 4 should have a modifier of -3
	And an attribute value of 5 should have a modifier of -3
	And an attribute value of 6 should have a modifier of -2
	And an attribute value of 7 should have a modifier of -2
	And an attribute value of 8 should have a modifier of -1
	And an attribute value of 9 should have a modifier of -1
	And an attribute value of 10 should have a modifier of 0
	And an attribute value of 11 should have a modifier of 0
	And an attribute value of 12 should have a modifier of 1
	And an attribute value of 13 should have a modifier of 1
	And an attribute value of 14 should have a modifier of 2
	And an attribute value of 15 should have a modifier of 2
	And an attribute value of 16 should have a modifier of 3
	And an attribute value of 17 should have a modifier of 3
	And an attribute value of 18 should have a modifier of 4
	And an attribute value of 19 should have a modifier of 4
	And an attribute value of 20 should have a modifier of 5


Scenario: The Effective Armor Class is calculated by adding the Dexterity modifier to the base armor class
	Given I have created a new character
	Then the Effective Armor Class equals the base Armor Class plus the Dexterity modifier

	Given I have created a new character
	And the Base Armor Class is 5
	And the Dexterity modifier is 5
	Then the Effective Armor Class should be 10


Scenario: The Effective Hit Points are calculated by adding the Constitution modifier to hit points
	The constitution modifier should add to/subtract from the base HP. This modifier alone can never
	drop a character's HP below 1, but combined with damage can kill the character.

	Given I have created a new character
	Then the Effective Hit Points equal the base Hit Points plus the Constitution modifier

	Given I have created a new character
	And the Base Hit Points are 5
	And the Constitution modifier is -5
	Then the Effective Hit Points should be 1

	Given I have created a new character
	And the Base Hit Points are 5
	And the Constitution modifier is -5
	And the character has taken 1 points of damage
	Then the Effective Hit Points should be 0
