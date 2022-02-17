const chrome = require("selenium-webdriver/chrome");
const {Builder, By,Capabilities} = require("selenium-webdriver");
const path = require('chromedriver').path;

export async function getDriver(url="http://localhost:3000"){
    var caps=Capabilities.chrome()
    caps.set('chromeOptions',{
        'args': ['--ignore-certificate-errors-spki-list']
    })
    let options=new chrome.Options()
    options.addArguments("--ignore-certificate-errors-spki-list")
    options.addArguments('--headless')
    options.setChromeBinaryPath(path)
    const driver = new Builder()
        .forBrowser("chrome")
        .withCapabilities(caps)
        .build();
    await driver.manage().setTimeouts( { implicit: 10000 } )
    await driver.get(url)
    return driver
}

export async function login(driver,email, pw){
    const emailField=await driver.findElement(By.id("Email"))
    await emailField.sendKeys(email)
    const pwField=await driver.findElement(By.id("password"))
    pwField.sendKeys(pw)
    const btn=await driver.findElement(By.xpath("/html/body/div/div/div[1]/div[1]/div[2]/div/div[2]/form/button"))
    await btn.click()
}

export async function submitPost(driver,expectedPostText){
    const post=await driver.findElement(By.className("post-form-input-text"))
    await post.sendKeys(expectedPostText)
    const submitBtn=await driver.findElement(By.id("submit-post-btn"))
    await submitBtn.click()
}

export async function findElementWithText(driver, text){
    return await driver.findElement(By.xpath(`//*[text()='${text}']`))
}