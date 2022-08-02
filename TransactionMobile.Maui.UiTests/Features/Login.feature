@background @login
Feature: Login

Background: 

@PRTest
Scenario: Login as Merchant
	Given I am on the Login Screen
	And the application is in training mode
	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	Then the Merchant Home Page is displayed

@PRTest
Scenario: Logout
	Given I am on the Login Screen
	And the application is in training mode
	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	Then the Merchant Home Page is displayed
	When I tap on Profile
	Then the My Profile Page is displayed
	When I tap on Logout
	Then the Login Screen is displayed