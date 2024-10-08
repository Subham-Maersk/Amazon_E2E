Feature: Sort Service JWT Token and Container Assignment

  Scenario: Retrieve Sorting JWT Token
    Given valid credentials
    When requesting a sorting JWT token
    Then a valid sorting JWT token is returned

Scenario: Assign Container to Parcels
    Given valid containerId and packageBarcode 
    When assigning the container to parcels
    Then the container is assigned successfully

