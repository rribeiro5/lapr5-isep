const {By}=require("selenium-webdriver")
const {login,submitPost, getDriver, findElementWithText} = require("./utils");


const email="1@gmail.com"
const pw="Password1?"


async function createPost(driver){
    const postText="O meu post de teste1"
    await submitPost(driver,postText)
    await findElementWithText(driver,postText) //wait until comment shows on feed
}


describe("Create comment",()=>{
    jest.setTimeout(60000)
    test("Create comment",async ()=>{
        const driver=await getDriver()
        await login(driver,email,pw)
        await createPost(driver)
        const commentArea=await driver.findElement(By.className("comment-form-input-text"))
        const commentText="comentario teste1"
        await commentArea.sendKeys(commentText)
        const submitBtn=await driver.findElement(By.className("comment-submit"))
        await submitBtn.click()
        const commentPanelBtn=await driver.findElement(By.id("react-collapsed-toggle-1"))
        await commentPanelBtn.click()
        const element=await findElementWithText(driver,commentText)
        const elementText=await element.getText();
        const result=await elementText.includes(commentText)
        expect(result).toBe(true)
        driver.quit()
    })
})