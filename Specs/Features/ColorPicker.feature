@UI
Feature: Color Picker
	In order to paint the grid
	As a user
	I want to be able to select a color

Scenario: Open Color Picker
	Given that the cursor is over the current color box
	When I left click
	Then Color picker should open

Scenario: Select a Color
	Given that the color picker is open
	When I move the picker handle
	Then the current color should change

