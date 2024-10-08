Feature: Label Generation after Shipment Processing

  Scenario: Generate label after shipment processing
    Given I have a valid JWT token for shipment service
    And I have a label request JSON file at "Test_Access_Data_Layer/labelRequest.json"
    When I generate a label using the label generator service
    Then the label should be generated successfully
    And the label response should contain the expected 'ams' ID
