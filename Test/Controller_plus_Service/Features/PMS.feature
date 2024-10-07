Feature: PMS Service

  Scenario: User Login with Valid Credentials
    Given the user provides valid credentials
    When the user submits the login request
    Then the user should receive a JWT token

  Scenario: Validate Shipment Creation and Get Final Mile Tracking
    Given I have valid JWT tokens for authentication
    When I call the shipment API with the manifest number "Testing_20240_123" and customer identifier "AMAEU0001" using the file "Test_Access_Data_Layer/AMS_LAX_2709.json"
    Then the response should not be null
    And the shipment status should be processed
    When I validate the manifest creation with manifest number "Testing_20240_123" and customer identifier "AMAEU0001"
    Then the validation response should not be null
    And the status of the validation response should not be "PreProcessing"
    When I extract the order ID from the response
    Then the order ID should not be null
    When I get the final mile tracking information for the order ID
    Then the final mile tracking response should not be null
    And the final mile tracking number should be available

  
