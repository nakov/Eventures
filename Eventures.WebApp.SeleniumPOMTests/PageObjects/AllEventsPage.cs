using System.Linq;

using OpenQA.Selenium;

namespace Eventures.WebApp.SeleniumPOMTests.PageObjects
{
    public class AllEventsPage : BasePage
    {
        private IWebElement lastRow => driver.FindElements(By.TagName("tr")).Last();
        private IWebElement lastDeleteButton => 
            driver.FindElements(By.XPath("//a[contains(.,'Delete')]")).Last();
        private IWebElement lastEditButton =>
            driver.FindElements(By.XPath("//a[contains(.,'Edit')]")).Last();

        public AllEventsPage(IWebDriver driver) : base(driver)
        {
        }

        protected override string PageUrl => "/Events/All";

        public string GetLastEventRow()
        {
            return this.lastRow.Text;
        }

        public void PressLastDeleteButton()
        {
            this.lastDeleteButton.Click();
        }

        public void PressLastEditButton()
        {
            this.lastEditButton.Click();
        }
    }
}
