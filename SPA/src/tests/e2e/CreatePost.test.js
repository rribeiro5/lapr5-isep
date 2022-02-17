const {By, until}=require("selenium-webdriver")
const {login,getDriver,submitPost, findElementWithText} = require("./utils");


async function getPostText(driver,text){
    const submittedPost=await findElementWithText(driver,text)
    return await submittedPost.getText()
}


describe("Create Post",()=>{
    jest.setTimeout(60000)
    test("Create Post",async ()=>{
        const email="1@gmail.com"
        const pw="Password1?"
        const expectedPostText="O meu post 33"
        const driver=await getDriver()
        await login(driver,email,pw)
        await submitPost(driver,expectedPostText)
        const resultPostText=await getPostText(driver,expectedPostText)
        const result=await resultPostText.includes(expectedPostText)
        expect(result).toBe(true)
        driver.quit()
    })
})
