@background @login
Feature: Login

Background: 
	
@PRTest
Scenario: Login as Merchant
	# TODO: Set Training mode
	Given I am on the Login Screen
	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	#Then the Merchant Home Page is displayed