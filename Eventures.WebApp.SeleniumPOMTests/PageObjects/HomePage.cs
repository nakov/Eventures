using OpenQA.Selenium;

namespace Eventures.WebApp.SeleniumPOMTests.PageObjects
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) : base(driver)
        {
        }

        protected override string PageUrl => "/";
    }
}
