Feature: Shipment Processing

  Scenario: User logs in and validates shipment creation
    Given the user provides valid credentials
    When the user submits the login request
    Then the user should receive a JWT token

  Scenario: User validates the shipment creation
    #When the user validates the shipment creation
    #Then the user should receive a validation response

  #Scenario: User retrieves final mile tracking information
    #When the user retrieves the final mile tracking
    #Then the user should receive the final mile tracking response
