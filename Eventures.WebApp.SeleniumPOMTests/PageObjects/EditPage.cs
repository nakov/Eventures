using OpenQA.Selenium;

namespace Eventures.WebApp.SeleniumPOMTests.PageObjects
{
    public class EditPage : BasePage
    {
        private IWebElement nameField => driver.FindElement(By.Id("Name"));
        private IWebElement confirmEditButton => driver.FindElement(By.XPath("//input[contains(@value,'Edit')]"));
        private IWebElement nameError => driver.FindElement(By.Id("Name-error"));

        public EditPage(IWebDriver driver) : base(driver)
        {
        }

        protected override string PageUrl => "/Events/Edit/";

        public override bool IsOpen(string baseUrl)
        {
            return base.driver.Url.Contains(baseUrl + this.PageUrl);
        }

        public void EditEventName(string name)
        {
            this.nameField.Clear();
            this.nameField.SendKeys(name);

            this.confirmEditButton.Click();
        }

        public string GetNameError()
        {
            return this.nameError.Text;
        }
    }
}
