@Amazon
Feature: Label Generation
  Scenario: Generate label 
    Given I have a valid JWT token for shipment service
    When User hit label generation api
    Then the label response should contain the expected 'ams' ID
