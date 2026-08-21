@background @login @toolbar @profile @base @sharedapp @shared
Feature: LogoutLoginMerchantDetails

Background:

	Given the following security roles exist
	| Role Name |
	| Merchant   |

	Given I create the following api scopes
	| Name                   | DisplayName                         | Description                            |
	| transactionProcessor   | Transaction Processor REST  Scope   | A scope for Transaction Processor REST |
	| transactionProcessorACL | Transaction Processor ACL REST  Scope | A scope for Transaction Processor ACL REST |

	Given the following api resources exist
	| Name                   | DisplayName                    | Secret  | Scopes                  | UserClaims                 |
	| transactionProcessor   | Transaction Processor REST     | Secret1 | transactionProcessor    | merchantId, estateId, role |
	| transactionProcessorACL | Transaction Processor ACL REST | Secret1 | transactionProcessorACL | merchantId, estateId, role |

	Given the following clients exist
	| ClientId        | ClientName        | Secret  | Scopes                                                   | GrantTypes  |
	| serviceClient   | Service Client    | Secret1 | transactionProcessor,transactionProcessorACL             | client_credentials |
	| mobileAppClient | Mobile App Client | Secret1 | transactionProcessorACL,transactionProcessor             | password           |

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

	Given I create the following merchants
	| MerchantName    | AddressLine1        | AddressLine2        | AddressLine3        | AddressLine4        | Town     | Region      | PostalCode | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | test address line 1 | test address line 2 | test address line 3 | test address line 4 | TestTown | Test Region | TE57 1NG   | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |
	| Test Merchant 2 | test address line 1 | test address line 2 | test address line 3 | test address line 4 | TestTown | Test Region | TE57 1NG   | United Kingdom | Test Contact 2 | testcontact2@merchant2.co.uk | Test Estate 1 |

	Given I have assigned the following  operator to the merchants
	| OperatorName     | MerchantName    | MerchantNumber | TerminalNumber | EstateName    |
	| Safaricom        | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |
	| Voucher          | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |
	| PataPawa PostPay | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |
	| PataPawa PrePay  | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |
	| Safaricom        | Test Merchant 2 | 00000002       | 10000002       | Test Estate 1 |
	| Voucher          | Test Merchant 2 | 00000002       | 10000002       | Test Estate 1 |
	| PataPawa PostPay | Test Merchant 2 | 00000002       | 10000002       | Test Estate 1 |
	| PataPawa PrePay  | Test Merchant 2 | 00000002       | 10000002       | Test Estate 1 |

	Given I have assigned the following devices to the merchants
	| MerchantName    | EstateName    |
	| Test Merchant 1 | Test Estate 1 |
	| Test Merchant 2 | Test Estate 1 |

	When I add the following contracts to the following merchants
	| EstateName    | MerchantName    | ContractDescription       |
	| Test Estate 1 | Test Merchant 1 | Safaricom Contract        |
	| Test Estate 1 | Test Merchant 1 | Hospital 1 Contract       |
	| Test Estate 1 | Test Merchant 1 | PataPawa PostPay Contract |
	| Test Estate 1 | Test Merchant 1 | PataPawa PrePay Contract  |
	| Test Estate 1 | Test Merchant 2 | Safaricom Contract        |
	| Test Estate 1 | Test Merchant 2 | Hospital 1 Contract       |
	| Test Estate 1 | Test Merchant 2 | PataPawa PostPay Contract |
	| Test Estate 1 | Test Merchant 2 | PataPawa PrePay Contract  |

	Given I have created the following security users
	| EmailAddress                  | Password | GivenName    | FamilyName | EstateName    | MerchantName    |
	| user1                         | 123456   | TestMerchant | User1      | Test Estate 1 | Test Merchant 1 |
	| user2                         | 123456   | TestMerchant | User2      | Test Estate 1 | Test Merchant 2 |

	Given I have created a config for my device

@PRTest
Scenario: Logout and relogin with a different merchant
	Given I am on the Login Screen
	And my device is registered
	When I enter 'user1' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	Then the Merchant Home Page is displayed
	When I tap on Profile
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
	When I tap on Logout
	Then A message is displayed confirming I want to log out
	When I click yes
	Then the Login Page is displayed
	And I replace the merchants with the following merchants
	| MerchantName    | AddressLine1        | AddressLine2        | AddressLine3        | AddressLine4        | Town     | Region      | PostalCode | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 2 | test address line 1 | test address line 2 | test address line 3 | test address line 4 | TestTown | Test Region | TE57 1NG   | United Kingdom | Test Contact 2 | testcontact2@merchant2.co.uk | Test Estate 1 |
	When I enter 'user2' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	Then the Merchant Home Page is displayed
	When I tap on Profile
	Then the My Profile Page is displayed
	When I tap on the Account Info button
	Then the Account Info Page is displayed
	And the Account Info is displayed
	| Name            | Balance | AvailableBalance |
	| Test Merchant 2 | 0       | 0                |
