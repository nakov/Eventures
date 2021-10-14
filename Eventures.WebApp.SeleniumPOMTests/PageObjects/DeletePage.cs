using OpenQA.Selenium;

namespace Eventures.WebApp.SeleniumPOMTests.PageObjects
{
    public class DeletePage : BasePage
    {
        private IWebElement confirmDeleteButton => 
            driver.FindElement(By.XPath("//input[contains(@value,'Delete')]"));

        public DeletePage(IWebDriver driver) : base(driver)
        {
        }

        protected override string PageUrl => "/Events/Delete/";

        public override bool IsOpen(string baseUrl)
        {
            return base.driver.Url.Contains(baseUrl + this.PageUrl);
        }

        public void PressConfirmDeleteButton()
        {
            this.confirmDeleteButton.Click();
        }
    }
}
