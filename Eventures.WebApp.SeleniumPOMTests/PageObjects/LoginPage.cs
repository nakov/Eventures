using OpenQA.Selenium;

namespace Eventures.WebApp.SeleniumPOMTests.PageObjects
{
    public class LoginPage : BasePage
    {
        private IWebElement usernameField => driver.FindElement(By.Id("Input_Username"));
        private IWebElement passwordField => driver.FindElement(By.Id("Input_Password"));
        private IWebElement loginButton => 
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Log in')]"));

        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        protected override string PageUrl => "/Identity/Account/Login";

        public void LogInUser(string username, string password)
        {
            this.usernameField.SendKeys(username);
            this.passwordField.SendKeys(password);

            this.loginButton.Click();
        }

        public bool IsOpenWhenUnauthorized(string baseUrl)
        {
            // The page URL of the "Login" page is different
            // when the user is redirected because they are unauthorized
            var pageUrl = baseUrl + "/Identity/Account/LogIn?ReturnUrl=%2FEvents%2FAll";
            return this.driver.Url == pageUrl;
        }
    }
}
