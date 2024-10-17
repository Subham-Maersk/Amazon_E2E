@Amazon
Feature: Sort Service Assignment
  Scenario: Containerization
    Given user login for containerization
    When assign the container to parcel with barcode "container" and "barcode"
    Then User receive successful response
