using OpenQA.Selenium;

namespace Eventures.WebApp.SeleniumPOMTests.PageObjects
{
    public class HomePage : BasePage
    {
        protected override string PageUrl => "/";
     
        public HomePage(IWebDriver driver) : base(driver)
        {
        }
    }
}
