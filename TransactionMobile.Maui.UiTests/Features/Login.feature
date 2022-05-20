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
	Then the Merchanmert Home Page is displayed