@UI
Feature: Playback
	In order to better view the changes in generations
	As a user
	I want to control playback of the simulation

Scenario: Play
	Given the simulation isn't running
		And the play/pause buton shows play
	When I press play/pause
	Then the simulation will start
		And the play/pause button will show the pause image

Scenario: Pause
	Given the simulation is running
		And the play/pause buton shows pause
	When I press play/pause
	Then the simulation will stop
		And the play/pause button will show the play image

Scenario: Reset
	Given the current generation is greater than zero
	When I press reset
	Then the simulation will stop
		And the grid will return to it's intial configuration
		And the play/pause button will show the play image

Scenario: Decrease speed
	Given the simulation is running
	When I select a delay
	Then the simulation will slow down