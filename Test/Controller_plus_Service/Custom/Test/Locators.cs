namespace Amazon_E2E_copy.Locators
{
    public static class CustomLocators
    {
        public const string UsernameInput = "//input[@id='username']";
        public const string PasswordInput = "//input[@id='password']";
        public const string SubmitButton = "//button[@type='submit']";
        public const string CustomsToolsLink = "//a[text()='Customs Tools']";
        public const string InternationalCustomsLink = "(//a[text()='International Customs'])[1]";
        public const string CreateButton = "//button[@class='dt-button buttons-create']";
        public const string ArrivalDateInput = "//input[@id='DTE_Field_InternationalCustoms-ArrivalDate']";
        public const string DayButton = "(//button[@class='editor-datetime-button editor-datetime-day'])[7]";
        public const string OriginAirportDropdown = "//select[@id='DTE_Field_InternationalCustoms-LKAirportOriginID']";
        public const string DestinationAirportDropdown = "//select[@id='DTE_Field_InternationalCustoms-LKAirportDestinationID']";
        public const string CreateDispatchButton = "//button[text()='Create']";
        public const string OverpacksDropdown = "//div[@id='myDropdown3339']";
        public const string DispatchNumbersTextarea = "//textarea[@id='dispatch_numbers']";
        public const string SendDispatchButton = "//button[@id='send-dispatches']";
        public const string CancelButton = "//a[@class='uk-button uk-button-orange uk-button-cancel uk-icon uk-align-right uk-margin-remove-bottom']";
        public const string DocumentsGeneratedMessage = "//span[text()='All documents generated']";
        public const string Button = "(//button[@class='dt-button'])[1]";
    }
}
