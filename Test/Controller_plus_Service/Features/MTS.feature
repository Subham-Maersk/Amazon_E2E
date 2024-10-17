@Amazon
Feature: MTS Data Update
  Scenario: Delivery to customer
  Given User login usps api
  When User update data using the MTS API
  Then User should receive a success response
