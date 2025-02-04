﻿using FluentAssertions;
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
        SetUpDriver();
        Autorization();
    }

    public void SetUpDriver()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox","--start-maximized", "--disable-extensions");
        
        driver = new ChromeDriver(options);
    }
    
    public void Autorization()
    {
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
        clipboard.SetText("+79222290747");
        
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
    public void NewsInCommunityTest()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");
        
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='CommunitiesCounter']")));
        
        var myCommunity = driver.FindElements(By.CssSelector("[data-tid='Item']"))[2];
        myCommunity.Click();

        var firstCommunity = driver.FindElements(By.CssSelector("[data-tid='Link']"))[0];
        firstCommunity.Click();

        var textModul = driver.FindElement(By.CssSelector("[data-tid='AddButton']"));
        textModul.Click();

        var textInput = driver.FindElement(By.CssSelector("[class='notranslate public-DraftEditor-content']"));
        textInput.SendKeys("Hello");

        var enterButtom = driver.FindElement(By.CssSelector("[class='react-ui-j884du react-ui-button-caption']"));
        enterButtom.Click();

        var testNews = driver.FindElements(By.CssSelector("[data-tid='NewsText']"))[0];

        Assert.That(testNews.Text == "Hello", "Текст в последней новости = " + testNews.Text + " Вместо необходимого Hello");
        
        //Почистим новости за собой

        var deleteNews = driver.FindElements(By.CssSelector("[data-tid='PopupMenu__caption']"))[2];
        deleteNews.Click();

        var deleteButtom = driver.FindElement(By.CssSelector("[data-tid='DeleteRecord']"));
        deleteButtom.Click();
            
        var deleteAccept = driver.FindElement(By.CssSelector("[class='react-ui-aivml8']"));
        deleteAccept.Click();
    }
    


    [TearDown]
    public void TearDown()
    {
        driver.Close();
        driver.Quit();
    }
}