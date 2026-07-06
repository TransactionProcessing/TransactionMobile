@background @login @toolbar @profile @base @sharedapp @shared @transactions @reports
Feature: Reporting

Background:

	Given the following security roles exist
	| Role Name |
	| Merchant  |

	Given I create the following api scopes
	| Name                  | DisplayName                        | Description                             |
	| transactionProcessor  | Transaction Processor REST Scope   | A scope for Transaction Processor REST   |
	| transactionProcessorACL | Transaction Processor ACL REST Scope | A scope for Transaction Processor ACL REST |

	Given the following api resources exist
	| Name                  | DisplayName                     | Secret  | Scopes                               | UserClaims                 |
	| transactionProcessor  | Transaction Processor REST      | Secret1 | transactionProcessor                  | merchantId, estateId, role |
	| transactionProcessorACL | Transaction Processor ACL REST | Secret1 | transactionProcessorACL               | merchantId, estateId, role |

	Given the following clients exist
	| ClientId       | ClientName        | Secret  | Scopes                               | GrantTypes  |
	| serviceClient   | Service Client    | Secret1 | transactionProcessor,transactionProcessorACL | client_credentials |
	| mobileAppClient | Mobile App Client | Secret1 | transactionProcessorACL,transactionProcessor  | password           |

	Given I have a token to access the estate management and transaction processor acl resources
	| ClientId      |
	| serviceClient |

	Given I have created the following estates
	| EstateName    |
	| Test Estate 1 |

	Given I have created the following operators
	| EstateName    | OperatorName | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
	| Test Estate 1 | Safaricom    | True                        | True                        |

	And I have assigned the following operators to the estates
	| EstateName    | OperatorName |
	| Test Estate 1 | Safaricom    |

	Given I create a contract with the following values
	| EstateName    | OperatorName | ContractDescription |
	| Test Estate 1 | Safaricom    | Safaricom Contract  |

	When I create the following Products
	| EstateName    | OperatorName | ContractDescription | ProductName    | DisplayText | Value | ProductType |
	| Test Estate 1 | Safaricom    | Safaricom Contract  | Variable Topup | Custom      |       | MobileTopup |

	When I add the following Transaction Fees
	| EstateName    | OperatorName | ContractDescription | ProductName    | CalculationType | FeeDescription      | Value |
	| Test Estate 1 | Safaricom    | Safaricom Contract  | Variable Topup | Fixed           | Merchant Commission | 2.50  |

	Given I create the following merchants
	| MerchantName    | AddressLine1        | AddressLine2        | AddressLine3        | AddressLine4        | Town     | Region      | PostalCode | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | test address line 1 | test address line 2 | test address line 3 | test address line 4 | TestTown | Test Region | TE57 1NG   | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |

	Given I have assigned the following  operator to the merchants
	| OperatorName | MerchantName    | MerchantNumber | TerminalNumber | EstateName    |
	| Safaricom    | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |

	Given I have assigned the following devices to the merchants
	| MerchantName    | EstateName    |
	| Test Merchant 1 | Test Estate 1 |

	When I add the following contracts to the following merchants
	| EstateName    | MerchantName    | ContractDescription |
	| Test Estate 1 | Test Merchant 1 | Safaricom Contract  |

	Given I have created the following security users
	| EmailAddress | Password | GivenName    | FamilyName | EstateName    | MerchantName    |
	| user1        | 123456   | TestMerchant | User1      | Test Estate 1 | Test Merchant 1 |

	Given I have created a config for my device

@PRTest
Scenario: View Transaction Mix Report
	Given I am on the Login Screen
	And my device is registered
	When I enter 'user1' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
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
	When I click on the back button
	Then the Merchant Home Page is displayed
	When I tap on Reports
	Then the Reports Page is displayed
	When I tap on the Transaction Mix Button
	Then the Transaction Mix Report is displayed
	And the Transaction Mix Chart is displayed
