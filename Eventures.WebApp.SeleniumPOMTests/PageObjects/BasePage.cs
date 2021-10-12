using OpenQA.Selenium;

namespace Eventures.WebApp.SeleniumPOMTests.PageObjects
{
    public class BasePage
    {
        protected readonly IWebDriver driver;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        protected virtual string PageUrl { get; }

        public IWebElement AllEventsPageLink =>
           this.driver.FindElement(By.XPath("//a[@href='/Events/All'][contains(.,'all events')]"));

        public IWebElement LoginPageLinkInNav =>
           this.driver.FindElement(By.XPath("(//a[@href='/Identity/Account/Login'])[1]"));

        public IWebElement LogoutButton =>
           this.driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Logout')]"));

        private IWebElement ElementPageHeading =>
           this.driver.FindElement(By.CssSelector("main > h1"));

        public void Open(string baseUrl)
        {
            this.driver.Navigate().GoToUrl(baseUrl + this.PageUrl);
        }

        public virtual bool IsOpen(string baseUrl)
        {
            return this.driver.Url == baseUrl + this.PageUrl;
        }

        public bool Contains(string text)
        {
            return this.driver.PageSource.Contains(text);
        }

        public string GetPageTitle()
        {
            return this.driver.Title;
        }

        public string GetPageHeadingText()
        {
            return this.ElementPageHeading.Text;
        }
    }
}
