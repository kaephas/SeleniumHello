using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

class SeleniumHello
{
    static void Main()
    {
        using (IWebDriver driver = new ChromeDriver())
        {
            
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            // look at news page for any new news
            driver.Navigate().GoToUrl("https://www.dualshockers.com/category/news");
            driver.Manage().Window.Size = new Size(1920, 1080);
            
            // click the link to the latest review
            // TODO: clicking often doesn't work due to ads popping up in front, trying a redirect instead
            IWebElement thumb = wait.Until(e => e.FindElement(By.CssSelector(".review-boxart-thumb")));
            // go to the link that clicking the image would go to
            IWebElement thumbLink = wait.Until((e => e.FindElement(By.CssSelector(".review-results .ds_image_link"))));
            driver.Navigate().GoToUrl(thumbLink.GetProperty("href"));
            
            // search for news about a game I've been looking forward to to see if there's anything new
            IWebElement search = wait.Until(e => e.FindElement(By.ClassName("nav-icon-search")));
            search.Click();
            IWebElement textEntry = wait.Until(e => e.FindElement(By.Id("txtsearch2")));
            textEntry.SendKeys("Nioh 2" + Keys.Enter);
            //
            // // IWebElement firstResult = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("h4>a")));
            IWebElement result = wait.Until(e => e.FindElement(By.CssSelector("h4>a")));
            Console.WriteLine("Header: " + result.Text);
            IWebElement summary =
                wait.Until(e => e.FindElement(By.CssSelector(".article-content h5 > p:nth-child(2)")));
            Console.WriteLine("Summary" + summary.Text);
        }
    }
}