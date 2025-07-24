@background @login @toolbar @profile @base @sharedapp @shared @transactions
Feature: iOSEndToEndTests

Background: 



@iOSPRTest
Scenario: EndToEnd Training Mode
	Given I am on the Login Screen
	And the application is in training mode
	When I enter 'user1' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	Then the Merchant Home Page is displayed	
	When I tap on Profile
	Then the My Profile Page is displayed
	When I tap on the Addresses button
	Then the Address List Page is displayed
	And the Primary Address is displayed
	| AddressLine1        | AddressTown |
	| test address line 1 | Town    |
	When I click on the back button
	Then the My Profile Page is displayed
	When I tap on the Account Info button
	Then the Account Info Page is displayed
	And the Account Info is displayed
	| Name           | Balance | AvailableBalance |
	| Dummy Merchant | 99     | 100               |
	When I click on the back button
	Then the My Profile Page is displayed
	When I click on the back button
	Then the Merchant Home Page is displayed
	When I tap on Transactions
	Then the Transaction Page is displayed
	When I tap on the Mobile Topup button
	Then the Transaction Select Mobile Topup Operator Page is displayed
	When I tap on the 'Safaricom' button
	Then the Select Product Page is displayed
	When I tap on the 'Custom' product button
	Then the Enter Topup Details Page is displayed
	When I enter '07777777775' as the Customer Mobile Number
	And I enter 10.00 as the Topup Amount
	And I tap on Perform Topup
	Then the Mobile Topup Successful Page is displayed
	And I tap on Complete
	Then the Transaction Page is displayed
	When I tap on the Voucher button
	Then the Transaction Select Voucher Operator Page is displayed
	When I tap on the 'Hospital 1 Contract' button
	Then the Select Product Page is displayed
	When I tap on the '10 KES' product button
	Then the Enter Voucher Issue Details Page is displayed
	When I enter '07777777775' as the Recipient Mobile Number
	And I tap on Issue Voucher
	Then the Voucher Issue Successful Page is displayed
	And I tap on Complete
	Then the Transaction Page is displayed
	When I tap on the Bill Payment button
	Then the Transaction Select Bill Payment Operator Page is displayed
	When I tap on the 'Pata Pawa PostPay' button
	Then the Select Product Page is displayed
	When I tap on the 'Bill Pay (Post)' product button
	Then the Enter Account Details Page is displayed
	When I enter '12345678' as the Account Number
	And I tap on the Get Account Button
	Then the Make Bill Payment page is displayed
	And the following Bill Details are displayed
	| AccountNumber | AccountHolder  | DueDate | Balance |
	| 12345678      | Mr Test Customer | 2025-04-17   | 100.00  |
	When I enter '07777777775' as the Customer Mobile Number 	
	And I enter 10.00 as the Payment Amount
	And I tap on the Make Payment Button
	Then the Bill Payment Successful Page is displayed
	And I tap on Complete
	Then the Transaction Page is displayed
	When I tap on the Bill Payment button
	Then the Transaction Select Bill Payment Operator Page is displayed
	When I tap on the 'Pata Pawa PrePay' button
	Then the Select Product Page is displayed
	When I tap on the 'Bill Pay (Pre)' product button
	Then the Enter Meter Details Page is displayed
	When I enter '00000001' as the Meter Number
	And I tap on the Get Meter Button
	Then the Make Bill Payment page is displayed
	And the following Meter Details are displayed
	| MeterNumber | 
	| 00000001      |
	When I enter 10.00 as the Payment Amount
	And I tap on the Make Payment Button
	Then the Bill Payment Successful Page is displayed
	And I tap on Complete
	Then the Transaction Page is displayed
	When I click on the back button
	Then the Merchant Home Page is displayed
	#When I click on the back button
	#Then A message is displayed confirming I want to log out
	#When I click yes
	#Then the Login Page is displayed