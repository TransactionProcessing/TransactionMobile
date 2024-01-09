@background @login @toolbar @profile @shared @transactions @sharedapp @base
Feature: PageNavigation

# Home Page Back Button Tests
#@PRNavTest
Scenario: Back Button from Home Page Screen - user wants to log out
	Given I am on the Login Screen
	And the application is in training mode
	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	Then the Merchant Home Page is displayed
	When I click on the back button
	Then A message is displayed confirming I want to log out
	When I click yes
	Then the Login Page is displayed

#@PRNavTest
Scenario: Back Button from Home Page Screen - user does not want to log out
	Given I am on the Login Screen
	And the application is in training mode
	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	Then the Merchant Home Page is displayed
	When I click on the back button
	Then A message is displayed confirming I want to log out
	When I click no
	Then the Merchant Home Page is displayed

# Transaction Page Back Button Tests
#@PRNavTest
Scenario: Back Button from Transaction Page Screen
	Given I am on the Login Screen
	And the application is in training mode
	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	Then the Merchant Home Page is displayed	
	When I tap on Transactions
	Then the Transaction Page is displayed
	When I click on the back button
	Then the Merchant Home Page is displayed	

#@PRNavTest
Scenario: Back Button from Transaction Select Operator Screen
	Given I am on the Login Screen
	And the application is in training mode
	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	Then the Merchant Home Page is displayed	
	When I tap on Transactions
	Then the Transaction Page is displayed
	When I tap on the Mobile Topup button
	Then the Transaction Select Mobile Topup Operator Page is displayed
	When I click on the back button
	Then the Transaction Page is displayed

# Reports Page Back Button Tests
# My Account Page Back Button Tests
# Support Page Back Button Tests
