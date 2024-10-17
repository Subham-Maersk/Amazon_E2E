@Amazon
Feature: GTS Authentication and Tracking

  Scenario: Successfully retrieve a JWT token and package barcode
    Given I have valid credentials for GTS login
    When I request a JWT token from GTS
    Then I should receive a valid GTS JWT token
    Given I have a valid GTS JWT token
    #Given I have a shipment tracking number
    #When I request the package barcode from GTS tracking service
   #Then the package barcode should be returned successfully
