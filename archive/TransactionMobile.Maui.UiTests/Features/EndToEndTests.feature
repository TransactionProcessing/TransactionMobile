@background @login @toolbar @profile @base @sharedapp @shared @transactions
Feature: EndToEndTests

Background: 

	Given the following bills are available at the PataPawa PostPaid Host
	| AccountNumber | AccountName    | DueDate | Amount |
	| 12345678      | Test Account 1 | Today   | 100.00 |

	Given the following users are available at the PataPawa PrePay Host
	| Username | Password |
	| operatora    | 1234567898   |

	Given the following meters are available at the PataPawa PrePay Host
	| MeterNumber | CustomerName |
	| 00000001    | Customer 1   |
	| 00000002    | Customer 2   |
	| 00000003    | Customer 3   |

	Given the following security roles exist
	| Role Name |
	| Merchant   |

	Given I create the following api scopes
	| Name                 | DisplayName                       | Description                            |
	| transactionProcessor | Transaction Processor REST  Scope | A scope for Transaction Processor REST |
	| transactionProcessorACL | Transaction Processor ACL REST  Scope | A scope for Transaction Processor ACL REST |
	| voucherManagement | Voucher Management REST  Scope | A scope for Voucher Management REST |

	Given the following api resources exist
	| Name            | DisplayName                    | Secret  | Scopes                  | UserClaims                 |
	| transactionProcessor    | Transaction Processor REST     | Secret1 | transactionProcessor    | merchantId, estateId, role                           |
	| transactionProcessorACL | Transaction Processor ACL REST | Secret1 | transactionProcessorACL | merchantId, estateId, role |
	| voucherManagement       | Voucher Management REST        | Secret1 | voucherManagement       |                            |

	Given the following clients exist
	| ClientId        | ClientName        | Secret  | Scopes                                                                   | GrantTypes  |
	| serviceClient   | Service Client    | Secret1 | transactionProcessor,transactionProcessorACL,voucherManagement | client_credentials |
	| mobileAppClient | Mobile App Client | Secret1 | transactionProcessorACL,transactionProcessor                                                         | password           |

	Given I have a token to access the estate management and transaction processor acl resources
	| ClientId      | 
	| serviceClient | 

	Given I have created the following estates
	| EstateName    |
	| Test Estate 1 |

	Given I have created the following operators
	| EstateName    | OperatorName     | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
	| Test Estate 1 | Safaricom        | True                        | True                        |
	| Test Estate 1 | Voucher          | True                        | True                        |
	| Test Estate 1 | PataPawa PostPay | True                        | True                        |
	| Test Estate 1 | PataPawa PrePay  | True                        | True                        |

	And I have assigned the following operators to the estates
	| EstateName    | OperatorName     |
	| Test Estate 1 | Safaricom        |
	| Test Estate 1 | Voucher          |
	| Test Estate 1 | PataPawa PostPay |
	| Test Estate 1 | PataPawa PrePay  |


	Given I create a contract with the following values
	| EstateName    | OperatorName     | ContractDescription       |
	| Test Estate 1 | Safaricom        | Safaricom Contract        |
	| Test Estate 1 | Voucher          | Hospital 1 Contract       |
	| Test Estate 1 | PataPawa PostPay | PataPawa PostPay Contract |
	| Test Estate 1 | PataPawa PrePay  | PataPawa PrePay Contract  |

	When I create the following Products
	| EstateName    | OperatorName     | ContractDescription       | ProductName       | DisplayText     | Value | ProductType |
	| Test Estate 1 | Safaricom        | Safaricom Contract        | Variable Topup    | Custom          |       | MobileTopup |
	| Test Estate 1 | Voucher          | Hospital 1 Contract       | 10 KES            | 10 KES          | 10.00 | Voucher     |
	| Test Estate 1 | PataPawa PostPay | PataPawa PostPay Contract | Post Pay Bill Pay | Bill Pay (Post) |       | BillPayment |
	| Test Estate 1 | PataPawa PrePay  | PataPawa PrePay Contract  | Pre Pay Bill Pay  | Bill Pay (Pre)  |       | BillPayment |

	When I add the following Transaction Fees
	| EstateName    | OperatorName     | ContractDescription       | ProductName       | CalculationType | FeeDescription      | Value |
	| Test Estate 1 | Safaricom        | Safaricom Contract        | Variable Topup    | Fixed           | Merchant Commission | 2.50  |
	| Test Estate 1 | PataPawa PostPay | PataPawa PostPay Contract | Post Pay Bill Pay | Percentage      | Merchant Commission | 0.50  |
	| Test Estate 1 | PataPawa PrePay  | PataPawa PrePay Contract  | Pre Pay Bill Pay  | Percentage      | Merchant Commission | 0.50  |

	Given I create the following merchants
	| MerchantName    | AddressLine1        | AddressLine2        | AddressLine3        | AddressLine4        | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | test address line 1 | test address line 2 | test address line 3 | test address line 4 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |

	Given I have assigned the following  operator to the merchants
	| OperatorName     | MerchantName    | MerchantNumber | TerminalNumber | EstateName    |
	| Safaricom        | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |
	| Voucher          | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |
	| PataPawa PostPay | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |
	| PataPawa PrePay  | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |

	Given I have assigned the following devices to the merchants
	| MerchantName    | EstateName    |
	| Test Merchant 1 | Test Estate 1 |

	When I add the following contracts to the following merchants
	| EstateName    | MerchantName    | ContractDescription       |
	| Test Estate 1 | Test Merchant 1 | Safaricom Contract        |
	| Test Estate 1 | Test Merchant 1 | Hospital 1 Contract       |
	| Test Estate 1 | Test Merchant 1 | PataPawa PostPay Contract |
	| Test Estate 1 | Test Merchant 1 | PataPawa PrePay Contract  |

	Given I have created the following security users
	| EmailAddress                  | Password | GivenName    | FamilyName | EstateName    | MerchantName    |
	| user1 | 123456   | TestMerchant | User1      | Test Estate 1 | Test Merchant 1 |

	Given I make the following manual merchant deposits 
	| Reference | Amount | DateTime | MerchantName    | EstateName    |
	| Deposit1  | 100.00 | Today    | Test Merchant 1 | Test Estate 1 |

	Given I have created a config for my application

	Given I have created a config for my device

@PRTest
Scenario: EndToEnd
	Given I am on the Login Screen
	And my device is registered
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
	| test address line 1 | TestTown    |
	When I click on the back button
	Then the My Profile Page is displayed
	When I tap on the Account Info button
	Then the Account Info Page is displayed
	And the Account Info is displayed
	| Name            | Balance | AvailableBalance |
	| Test Merchant 1 | 0       | 0                |
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
	When I tap on the 'PataPawa PostPay' button
	Then the Select Product Page is displayed
	When I tap on the 'Bill Pay (Post)' product button
	Then the Enter Account Details Page is displayed
	When I enter '12345678' as the Account Number
	And I tap on the Get Account Button
	Then the Make Bill Payment page is displayed
	And the following Bill Details are displayed
	| AccountNumber | AccountHolder  | DueDate | Balance |
	| 12345678      | Test Account 1 | Today   | 100.00  |
	When I enter '07777777775' as the Customer Mobile Number 	
	And I enter 10.00 as the Payment Amount
	And I tap on the Make Payment Button
	Then the Bill Payment Successful Page is displayed
	And I tap on Complete
	Then the Transaction Page is displayed
	When I tap on the Bill Payment button
	Then the Transaction Select Bill Payment Operator Page is displayed
	When I tap on the 'PataPawa PrePay' button
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
	When I click on the back button
	Then A message is displayed confirming I want to log out
	When I click yes
	Then the Login Page is displayed
