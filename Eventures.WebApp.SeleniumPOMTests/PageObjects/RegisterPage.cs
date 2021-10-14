using OpenQA.Selenium;

namespace Eventures.WebApp.SeleniumPOMTests.PageObjects
{
    public class RegisterPage : BasePage
    {
        private IWebElement usernameField => driver.FindElement(By.Id("Input_Username"));
        private IWebElement emailField => driver.FindElement(By.Id("Input_Email"));
        private IWebElement passwordField => driver.FindElement(By.Id("Input_Password"));
        private IWebElement confirmPasswordField => driver.FindElement(By.Id("Input_ConfirmPassword"));
        private IWebElement firstNameField => driver.FindElement(By.Id("Input_FirstName"));
        private IWebElement lastNameField => driver.FindElement(By.Id("Input_LastName"));
        private IWebElement registerButton => 
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Register')]"));

        public RegisterPage(IWebDriver driver) : base(driver)
        {
        }

        protected override string PageUrl => "/Identity/Account/Register";

        public void RegisterUser(string username, string email, string password, 
            string confirmPassword, string firstName, string lastName)
        {
            this.usernameField.SendKeys(username);
            this.emailField.SendKeys(email);
            this.passwordField.SendKeys(password);
            this.confirmPasswordField.SendKeys(confirmPassword);
            this.firstNameField.SendKeys(firstName);
            this.lastNameField.SendKeys(lastName);

            this.registerButton.Click();
        }
    }
}
