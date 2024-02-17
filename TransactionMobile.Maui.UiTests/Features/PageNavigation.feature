@background @login @toolbar @profile @shared @transactions @sharedapp @base @reports
Feature: PageNavigation

# Home Page Back Button Tests
@PRNavTest
Scenario: Back Button from Home Page Screen
	Given I am on the Login Screen
	And the application is in training mode
	When I enter 'user1' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	Then the Merchant Home Page is displayed
	When I click on the back button
	Then A message is displayed confirming I want to log out
	When I click no
	Then the Merchant Home Page is displayed
	When I click on the back button
	Then A message is displayed confirming I want to log out
	When I click yes
	Then the Login Page is displayed

# Transaction Page Back Button Tests
@PRNavTest
Scenario: Back Button from Transaction Page Screen
	Given I am on the Login Screen
	And the application is in training mode
	When I enter 'user1' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	Then the Merchant Home Page is displayed	
	When I tap on Transactions
	Then the Transaction Page is displayed
	When I tap on the Mobile Topup button
	Then the Transaction Select Mobile Topup Operator Page is displayed
	When I click on the back button
	Then the Transaction Page is displayed
	When I click on the back button
	Then the Merchant Home Page is displayed	

# Reports Page Back Button Tests
@PRNavTest
Scenario: Back Button from Reports Page Screen
	Given I am on the Login Screen
	And the application is in training mode
	When I enter 'user1' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	Then the Merchant Home Page is displayed	
	When I tap on Reports
	Then the Reports Page is displayed
	When I tap on the Sales Analysis Button
	Then the Sales Analysis Report is displayed
	When I click on the back button
	Then the Reports Page is displayed
	When I click on the back button
	Then the Merchant Home Page is displayed	


# My Account Page Back Button Tests
# Support Page Back Button Tests
