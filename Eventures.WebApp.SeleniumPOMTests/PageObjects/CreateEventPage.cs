using OpenQA.Selenium;

namespace Eventures.WebApp.SeleniumPOMTests.PageObjects
{
    public class CreateEventPage : BasePage
    {
        private IWebElement nameField => driver.FindElement(By.Id("Name"));
        private IWebElement placeField => driver.FindElement(By.Id("Place"));
        private IWebElement ticketsField => driver.FindElement(By.Id("TotalTickets"));
        private IWebElement priceField => driver.FindElement(By.Id("PricePerTicket"));
        private IWebElement createButton => driver.FindElement(By.XPath("//input[contains(@value,'Create')]"));
        private IWebElement nameError => driver.FindElement(By.Id("Name-error"));
     
        public CreateEventPage(IWebDriver driver) : base(driver)
        {
        }

        protected override string PageUrl => "/Events/Create";


        public void CreateEvent(string name, string place, string tickets, string price)
        {
            this.nameField.Clear();
            this.nameField.SendKeys(name);

            this.placeField.Clear();
            this.placeField.SendKeys(place);

            this.ticketsField.Clear();
            this.ticketsField.SendKeys(tickets);

            this.priceField.Clear();
            this.priceField.SendKeys(price);

            this.createButton.Click();
        }

        public string GetNameError()
        {
            return this.nameError.Text;
        }
    }
}
