using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumHello
{
    class SeleniumHello
    {
        static void Main()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // set wait time in case elements load slowly
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            
                // check the news page for anything interesting
                ViewNews(wait, driver);

                // get the most recent game review
                LatestReview(wait, driver);

                // search for news about a game I've been looking forward to to see if there's anything new
                Nioh2News(wait, driver);
            }
        }

        private static void Nioh2News(WebDriverWait wait, IWebDriver driver)
        {
            // Advertisements cause many issues on this site. If one pops up at the moment of a click, click doesn't work or
            // if the focus changes to the ad, enter won't work in the search box. Try/catch lets me use alternate options
            try
            {
                // get the search icon
                IWebElement search = wait.Until(e => e.FindElement(By.ClassName("nav-icon-search")));
                // click to open up the search box
                search.Click();
                // get the search box
                IWebElement textEntry = wait.Until(e => e.FindElement(By.Id("txtsearch2")));
                // try to send Nioh 2 and enter
                try
                {
                    textEntry.SendKeys("Nioh 2" + Keys.Enter);
                }
                // if not, go directly to the search link
                catch (Exception e)
                {
                    driver.Navigate().GoToUrl("https://www.dualshockers.com/?s=Nioh+2");
                    Console.WriteLine("Enter key not working: " + e);
                }
            
                // get the header of the most recent news for the search
                IWebElement result = wait.Until(e => e.FindElement(By.CssSelector("h4>a")));
                Console.WriteLine("Nioh 2 Header: " + result.Text);
                // get the quick summary
                IWebElement summary =
                    wait.Until(e => e.FindElement(By.CssSelector(".article-content h5 > p:nth-child(2)")));
                Console.WriteLine("Nioh 2 Summary: " + summary.Text);
            }
            // webdriver likes to throw random exceptions -- helps me see where it ocurred while allowing the rest of the
            // program to run
            catch (Exception e)
            {
                Console.WriteLine("Nioh exception: " + e);
            }
        }

        private static void LatestReview(WebDriverWait wait, IWebDriver driver)
        {
            // click the link to the latest review
            // BUG: clicking often doesn't work due to ads very frequently popping up in front, trying a redirect instead
            // IWebElement thumb = wait.Until(e => e.FindElement(By.CssSelector(".review-boxart-thumb")));
        
            // go to the link that clicking the image would go to
            IWebElement thumbLink = wait.Until((e => e.FindElement(By.CssSelector(".review-results .ds_image_link"))));
            driver.Navigate().GoToUrl(thumbLink.GetProperty("href"));
            // get the review title and intro text to show that page is loaded and read
            try
            {
                IWebElement reviewTitle = wait.Until(e => e.FindElement(By.CssSelector(".review-value")));
                Console.WriteLine("Review Title: " + reviewTitle.Text);
            
            
                IWebElement reviewIntro = wait.Until(e => e.FindElement(By.CssSelector("h2 > span")));
                Console.WriteLine("Review Intro: " + reviewIntro.Text);
            }
            // not often any exceptions thrown here, but occasionally, this lets the program move on while showing the error
            catch (Exception e)
            {
                Console.WriteLine("Review exception: " + e);
            }
        
        }

        private static void ViewNews(WebDriverWait wait, IWebDriver driver)
        {
            // look at news page for any new news
            driver.Navigate().GoToUrl("https://www.dualshockers.com/category/news");
            // resize window for search button to show for later method (starts as a narrow window for some reason...)
            driver.Manage().Window.Size = new Size(1920, 1080);
            // print news title and summary to show that page is loaded and read
            IWebElement firstTitle = wait.Until(e => e.FindElement(By.CssSelector("h4 > a")));
            Console.WriteLine("News Title: " + firstTitle.Text);
            IWebElement firstSummary = wait.Until(e => e.FindElement(By.CssSelector("h5 > p:nth-child(2)")));
            Console.WriteLine("News Summary: " + firstSummary.Text);
        }
    }
}