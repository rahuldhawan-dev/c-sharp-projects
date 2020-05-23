Feature: GetValues
	GetVaules feature
	
Background: 
	Given test server is setup

@mytag
Scenario: get values
	Given nothing
	When get values multiple is called
	Then multiple values are returned
