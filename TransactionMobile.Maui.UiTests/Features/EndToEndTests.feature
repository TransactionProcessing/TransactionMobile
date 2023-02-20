@background @login @toolbar @profile @base @shared
Feature: EndToEndTests

Background: 

	Given the following security roles exist
	| RoleName |
	| Merchant   |

	Given I create the following api scopes
	| Name                 | DisplayName                       | Description                            |
	| estateManagement     | Estate Managememt REST Scope      | A scope for Estate Managememt REST     |
	| transactionProcessor | Transaction Processor REST  Scope | A scope for Transaction Processor REST |
	| transactionProcessorACL | Transaction Processor ACL REST  Scope | A scope for Transaction Processor ACL REST |
	| voucherManagement | Voucher Management REST  Scope | A scope for Voucher Management REST |

	Given the following api resources exist
	| ResourceName            | DisplayName                    | Secret  | Scopes                  | UserClaims                 |
	| estateManagement        | Estate Managememt REST         | Secret1 | estateManagement        | merchantId, estateId, role |
	| transactionProcessor    | Transaction Processor REST     | Secret1 | transactionProcessor    |                            |
	| transactionProcessorACL | Transaction Processor ACL REST | Secret1 | transactionProcessorACL | merchantId, estateId, role |
	| voucherManagement       | Voucher Management REST        | Secret1 | voucherManagement       |                            |

	Given the following clients exist
	| ClientId        | ClientName        | Secret  | AllowedScopes                                                                   | AllowedGrantTypes  |
	| serviceClient   | Service Client    | Secret1 | estateManagement,transactionProcessor,transactionProcessorACL,voucherManagement | client_credentials |
	| mobileAppClient | Mobile App Client | Secret1 | transactionProcessorACL                                                         | password           |

	Given I have a token to access the estate management and transaction processor acl resources
	| ClientId      | 
	| serviceClient | 

	Given I have created the following estates
	| EstateName    |
	| Test Estate 1 |

	Given I have created the following operators
	| EstateName    | OperatorName | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
	| Test Estate 1 | Safaricom    | True                        | True                        |
	| Test Estate 1 | Voucher      | True                        | True                        |

	Given I create a contract with the following values
	| EstateName    | OperatorName    | ContractDescription |
	| Test Estate 1 | Safaricom | Safaricom Contract |
	| Test Estate 1 | Voucher      | Hospital 1 Contract |

	When I create the following Products
	| EstateName    | OperatorName | ContractDescription | ProductName    | DisplayText | Value |
	| Test Estate 1 | Safaricom    | Safaricom Contract  | Variable Topup | Custom      |       |
	| Test Estate 1 | Voucher      | Hospital 1 Contract | 10 KES         | 10 KES      |       |

	When I add the following Transaction Fees
	| EstateName    | OperatorName | ContractDescription | ProductName    | CalculationType | FeeDescription      | Value |
	| Test Estate 1 | Safaricom    | Safaricom Contract  | Variable Topup | Fixed           | Merchant Commission | 2.50  |

	Given I create the following merchants
	| MerchantName    | AddressLine1        | AddressLine2        | AddressLine3        | AddressLine4        | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | test address line 1 | test address line 2 | test address line 3 | test address line 4 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |

	Given I have assigned the following  operator to the merchants
	| OperatorName | MerchantName    | MerchantNumber | TerminalNumber | EstateName    |
	| Safaricom    | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |
	| Voucher      | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |

	Given I have assigned the following devices to the merchants
	| MerchantName    | EstateName    |
	| Test Merchant 1 | Test Estate 1 |

	Given I have created the following security users
	| EmailAddress                  | Password | GivenName    | FamilyName | EstateName    | MerchantName    |
	| merchantuser@testmerchant1.co.uk | 123456   | TestMerchant | User1      | Test Estate 1 | Test Merchant 1 |

	Given I have created a config for my application

	Given I have created a config for my device

@PRTest
Scenario: EndToEnd
	Given I am on the Login Screen
	And my device is registered
	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	Then the Merchant Home Page is displayed
	When I tap on Profile
	Then the My Profile Page is displayed
	When I tap on the Addresses button
	Then the Address List Page is displayed
	And the Primary Address is displayed
	| AddressLine1        | AddressLine2        | AddressLine3        | AddressLine4        | AddressTown | AddressRegion | AddressPostCode |
	| test address line 1 | test address line 2 | test address line 3 | test address line 4 | Town        | Region        | TE57 1NG        |
	When I click on the back button
	Then the My Profile Page is displayed
	#When I tap on the Contacts button
	#Then the Contact List Page is displayed
	#And the Primary Contact is displayed
	#| Name         | EmailAddress                 | MobileNumber |
	#| Test Contact | stuart_ferguson1@outlook.com | 123456789    |
	#When I click on the back button
	#Then the My Profile Page is displayed
	When I tap on the Account Info button
	Then the Account Info Page is displayed
	And the Account Info is displayed
	| Name            | Balance | AvailableBalance |
	| Test Merchant 1 | 0       | 0                |
	When I click on the back button
	Then the My Profile Page is displayed
	When I tap on Logout
	Then the Login Screen is displayed