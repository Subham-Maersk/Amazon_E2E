Feature: Shipment Tracking via GTS

  Scenario: Authenticate and retrieve JWT token from GTS
    Given I have valid credentials for GTS login
    When I request a JWT token from GTS
    Then I should receive a valid JWT token

  Scenario: Retrieve package barcode using GTS tracking service
    Given I have a valid GTS JWT token
    And I have a shipment tracking number
    When I request the package barcode from GTS tracking service
    Then the package barcode should be returned successfully
