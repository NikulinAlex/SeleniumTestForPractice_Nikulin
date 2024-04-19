using FluentAssertions;
using System;
using System.IO.Packaging;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TextCopy;


namespace SeleniumTestForPractics;


public class SeleniumTest
{
    public ChromeDriver driver;

    [SetUp]
    public void SetUp()
    {
        Autorization();
    }
    
    public void Autorization()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox","--start-maximized", "--disable-extensions");
        
        driver = new ChromeDriver(options);
        
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(6);
        
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("alexey_nik_ural@mail.ru");
        
        var password = driver.FindElement(By.Name("Password"));
        password.SendKeys(".AlexeyTitan2001.");
        
        var enter = driver.FindElement(By.Name("button"));
        enter.Click();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/news"));
    }

    [Test]
    public void AutorizationTest()
    {
      
        var news = driver.FindElement(By.CssSelector("[data-tid='Feed']"));
        var currentUrl = driver.Url;

        Assert.That(currentUrl == "https://staff-testing.testkontur.ru/news",
        "current url = " + currentUrl + " а должен быть https://staff-testing.testkontur.ru/news");
        
    }

    [Test]

    public void NavigationToCommunityTest()
    {

        var community = driver.FindElement(By.CssSelector("[data-tid='Community']"));
        community.Click();

        Assert.That(driver.Url == "https://staff-testing.testkontur.ru/communities",
            "Адрес текущей страницы =" + driver.Url +
            "вместо ожидаемого: https://staff-testing.testkontur.ru/communities");
    }

    [Test]

    public void ModalPageCreateFolderTest()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/files");
        
        var waitNavigate = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        waitNavigate.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='Title']")));
        
        var create = driver.FindElements(By.CssSelector("[data-tid='DropdownButton']"))[1];
        create.Click();
        
        var createFolder = driver.FindElement(By.CssSelector("[data-tid='CreateFolder']"));
        createFolder.Click();
        
        var ModalPageCreate = driver.FindElement(By.CssSelector("[data-tid='ModalPageHeader']"));
        Assert.That(ModalPageCreate.Displayed  != false, "Модальное окно Создание Папки не отобразилось");
    }
    
    [Test]
    public void PhoneInputTest()
    {
        var clipboard = new Clipboard();
        clipboard.SetText("9222290747");
        
        var Avatar = driver.FindElement(By.CssSelector("[data-tid='Avatar']"));
        Avatar.Click();
        
        var profileEdit = driver.FindElement(By.CssSelector("[data-tid='ProfileEdit']"));
        profileEdit.Click();
        
        var phoneInput = driver.FindElements(By.CssSelector("[data-tid='Input']"))[7];
        phoneInput.Click();
        phoneInput.SendKeys(Keys.LeftControl+"v");
        //При вставке из буфера обмена 11-ти значного номера он должен подставляться без кода страны, однако в данном случае номер вставляетя с учетом 7.
        Assert.That(phoneInput.Text == "+7 922 229-07-47", "Указанный телефон = " + phoneInput.Text + " а должен быть +7 922 229-07-47");
    }

    [Test]
    public void pro()
    {
        
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }
}