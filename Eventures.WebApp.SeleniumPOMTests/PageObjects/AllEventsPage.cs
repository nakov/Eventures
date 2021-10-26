using System.Linq;
using System.Collections.Generic;

using OpenQA.Selenium;

namespace Eventures.WebApp.SeleniumPOMTests.PageObjects
{
    public class AllEventsPage : BasePage
    {
        private List<IWebElement> eventRows => driver.FindElements(By.TagName("tr")).ToList();
       
        public AllEventsPage(IWebDriver driver) : base(driver)
        {
        }

        protected override string PageUrl => "/Events/All";

        public string GetEventRow(string eventName)
        {
            return this.eventRows
                .FirstOrDefault(r => r.Text.Contains(eventName)).Text;
        }

        public void PressEventDeleteButton(string eventName)
        {
            var deleteButton = driver
                .FindElement(By.XPath($"//tr//td[contains(text(), '{eventName}')]/..//a[contains(.,'Delete')]"));
            deleteButton.Click();
        }

        public void PressEventEditButton(string eventName)
        {
            var editButton = driver
                .FindElement(By.XPath($"//tr//td[contains(text(), '{eventName}')]/..//a[contains(.,'Edit')]"));
            editButton.Click();
        }
    }
}
