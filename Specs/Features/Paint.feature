@UI
Feature: Paint
	In order to setup the grid
	As a user
	I want to be able to paint the grid

Scenario: Paint Cell
	Given a color is selected
		And the cursor is over a cell(s)
	When I left click
	Then the color should be applied to the cell(s)

Scenario: Erase
	Given the cursor is over a cell(s)
		And the shift key is pressed
	When I left click
	Then the color should be removed

Scenario: Increase Brush Size
	Given the cursor is over a cell(s)
	When the ] key is pressed
	Then the brush cursor should increase

Scenario: Decrease Brush Size
	Given the cursor is over a cell(s)
	When the [ key is pressed
	Then the brush cursor should decrease