@background @login
Feature: Login

Background: 
	
#	Given I have created the following estates
#	| EstateName    |
#	| Test Estate 1 |
#
#	Given I have created the following operators
#	| EstateName    | OperatorName | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
#	| Test Estate 1 | Safaricom    | True                        | True                        |
#
#	Given I create the following merchants
#	| MerchantName    | EstateName    | EmailAddress                     | Password | GivenName    | FamilyName |
#	| Test Merchant 1 | Test Estate 1 | merchantuser@testmerchant1.co.uk | 123456   | TestMerchant | User1      |
#
#	Given I make the following manual merchant deposits 
#	| Amount  | DateTime  | MerchantName    | EstateName    |
#	| 1000.00 | Today     | Test Merchant 1 | Test Estate 1 |
#	| 1000.00 | Yesterday | Test Merchant 1 | Test Estate 1 |
#
#	Given the application in in test mode

@PRTest
Scenario: Login as Merchant
	Given I am on the Login Screen
	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	Then the Merchant Home Page is displayed
	#And the available balance is shown as 2000.00