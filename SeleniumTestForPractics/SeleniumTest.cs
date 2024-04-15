using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTestForPractics;

public class SeleniumTest
{
    [Test]
    public void Autorization()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
        
        var driver = new ChromeDriver(options);
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        Thread.Sleep(5000);
        
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("alexey_nik_ural@mail.ru");
        
        var password = driver.FindElement(By.Name("Password"));
        password.SendKeys(".AlexeyTitan2001.");
        Thread.Sleep(3000);
        
        var enter = driver.FindElement(By.Name("button"));
        enter.Click();
        Thread.Sleep(3000);
        
        var currentUrl = driver.Url;
        Assert.That(currentUrl == "https://staff-testing.testkontur.ru/news");
        driver.Quit();
    }
}