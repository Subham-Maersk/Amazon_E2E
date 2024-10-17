@Amazon
Feature: Shipment Processing

  Scenario: User validates shipment creation
    Given the user login for manifest creation
    When the user validates the shipment creation
    Then the user should receive a validation response
 
 
  Scenario: User retrieves final mile tracking information
    When the user retrieves the final mile tracking
    Then User get final mile tracking
