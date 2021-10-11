using OpenQA.Selenium;

namespace Eventures.WebApp.SeleniumPOMTests.PageObjects
{
    public class BasePage
    {
        protected readonly IWebDriver driver;
        protected virtual string PageUrl { get; }

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IWebElement HomePageLink =>
            driver.FindElement(By.XPath("//a[contains(., 'Home')]"));

        public IWebElement AllEventsPageLink =>
           driver.FindElement(By.XPath("//a[@href='/Events/All'][contains(.,'all events')]"));

        public IWebElement LoginPageLinkInNav =>
           driver.FindElement(By.XPath("(//a[@href='/Identity/Account/Login'])[1]"));

        public IWebElement LogoutButton =>
           driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Logout')]"));

        public IWebElement ElementPageHeading =>
            driver.FindElement(By.CssSelector("main > h1"));

        public void Open(string baseUrl)
        {
            driver.Navigate().GoToUrl(baseUrl + this.PageUrl);
        }

        public virtual bool IsOpen(string baseUrl)
        {
            return driver.Url == baseUrl + this.PageUrl;
        }

        public bool Contains(string text)
        {
            return driver.PageSource.Contains(text);
        }

        public string GetPageTitle()
        {
            return driver.Title;
        }

        public string GetPageHeadingText()
        {
            return ElementPageHeading.Text;
        }
    }
}
