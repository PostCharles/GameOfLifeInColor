@UI
Feature: Zoom
	In order to to better view the grid
	As a user
	I want to be able to magnify and reduce the view

Scenario: Magnify
	Given that zoom is not at maximum magnification
	When I press magnify
	Then the cell size should increase

Scenario: Magnify Disabled
	Given that zoom is at maximum magnification
	When I press magnify
	Then magnfication should be disabled

Scenario: Reduce
	Given that zoom is not at minimum magnification
	When I press reduce
	Then the cell size should decrease

Scenario: Reduce Disabled
	Given that zoom is at minimum magnification
	When I press reduce2
	Then reduce should be disabled
