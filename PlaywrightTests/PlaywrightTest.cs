using Microsoft.Playwright;
using System.Reflection;
using System.Threading.Tasks;


namespace WebApp.PlaywrightTest
{
    [TestClass]
    [TestCategory("PlaywrightTests")]
    public class PlaywrightTest
    {
        public static readonly string[] LanguageToSelect = new string[] {"English (American)"};
        public static readonly string[] LanguageFromSelect = new string[] {"Spanish"};
        public static readonly string[] LanguageFromSelectDL = new string[] {"Detect Language"};

        /// <summary>
        /// Represents an asynchronous operation that produces a result.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the result of the asynchronous operation.
        /// </returns>
        [TestMethod]
        public async Task TranslationWithoutLanguageChange()
        {
            // set test data
            string originalTestText = "Example Test";
            string translatedTestText = "Beispiel Test";
            string OriginTextString = "null";
            string TranslatedTextString = "null";

            // create playwright instance
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();//new BrowserTypeLaunchOptions { Headless = false, SlowMo = 1000 });
            var page = await browser.NewPageAsync();

            // open the website
            await page.GotoAsync("http://localhost:5095/");

            // enter the text
            await page.ClickAsync("[id=OriginalTextField]");
            await page.FillAsync("[id=OriginalTextField]", originalTestText);
            await page.ClickAsync("[id=TranslateButton]");

            await page.WaitForSelectorAsync("[id='TranslatedTextField']", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });

            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await page.WaitForSelectorAsync("[id='TranslatedTextField']");


            string translatedText = await page.InnerTextAsync("[id='TranslatedTextField']");
            Assert.AreEqual(translatedTestText, translatedText);

            await page.ClickAsync("text=Back to List");    

            var originalTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{originalTestText}\")");
            Assert.IsNotNull(originalTextFound);

            // Click on the delete link of the created translation
            var deleteLink = await originalTextFound.QuerySelectorAsync("xpath=ancestor::tr//a[contains(@href, '/Delete')]");
            Assert.IsNotNull(deleteLink);
            await deleteLink.ClickAsync();

            // Check the values on the details page
            var deleteOriginTextElement = await page.QuerySelectorAsync("#OriginText");
            if (deleteOriginTextElement != null)
            {
                OriginTextString = await deleteOriginTextElement.InnerTextAsync();
            }
            var deleteTranslatedTextElement = await page.QuerySelectorAsync("#TranslatedText");
            if (deleteTranslatedTextElement != null)
            {
                TranslatedTextString = await deleteTranslatedTextElement.InnerTextAsync();
            }
            Assert.AreEqual(translatedTestText, TranslatedTextString);
            Assert.AreEqual(originalTestText, OriginTextString);

            // Click on the delete button
            await page.ClickAsync("[value=Delete]");

            // Check if the home page is displayed
            var titleElement = await page.QuerySelectorAsync("title");
            if (titleElement != null)
            {
                string title = await titleElement.InnerTextAsync();
                Assert.AreEqual("History - WebApp", title);
            } 
            else 
            {
                Assert.Fail("Title element not found");
            }

 

            // Check if the original text is displayed in the table
            originalTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{originalTestText}\")");
            Assert.IsNull(originalTextFound);

            // Check if the translated text is displayed in the table
            var translatedTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{translatedTestText}\")");
            Assert.IsNull(translatedTextFound);

            await browser.CloseAsync();
        }

        /// <summary>
        /// Represents an asynchronous operation that produces a result of type <see cref="System.Threading.Tasks.Task"/>.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task TranslationWithLanguageChange()
        {
            // set test data
            string originalTestText = "Un test en otro idioma";
            string translatedTestText = "A test in another language";

            // create playwright instance
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();//new BrowserTypeLaunchOptions { Headless = false, SlowMo = 1000 });
            var page = await browser.NewPageAsync();

            // open the website
            await page.GotoAsync("http://localhost:5095/");
            
            // select the language Spanish
            await page.ClickAsync("[id=languageFromDropdown]");
            await page.SelectOptionAsync("[id=languageFromDropdown]", LanguageFromSelect);
            // insert the text
            await page.ClickAsync("[id=OriginalTextField]");
            await page.FillAsync("[id=OriginalTextField]", originalTestText);
            // select american english
            await page.ClickAsync("[id=languageToDropdown]");
            await page.SelectOptionAsync("[id=languageToDropdown]", LanguageToSelect);
            // translate
            await page.ClickAsync("[id=TranslateButton]");

            // wait for the translation to be displayed
            await page.WaitForSelectorAsync("[id='TranslatedTextField']", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await page.WaitForSelectorAsync("[id='TranslatedTextField']");


            string translatedText = await page.InnerTextAsync("[id='TranslatedTextField']");
            Assert.AreEqual(translatedTestText, translatedText);

            await page.ClickAsync("text=Back to List");    

            // check if the original text is displayed in the table
            var originalTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{originalTestText}\")");
            Assert.IsNotNull(originalTextFound);

            // check if the translated text is displayed in the table
            var translatedTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{translatedTestText}\")");
            Assert.IsNotNull(translatedTextFound);

            // click on the details link of the first translation
            var detailsLink = await originalTextFound.QuerySelectorAsync("xpath=ancestor::tr//a[contains(@href, '/Details')]");
            Assert.IsNotNull(detailsLink);
            await detailsLink.ClickAsync();

            // Declare variables for the details
            string OriginLanguageString = "null";
            string OriginTextString = "null";
            string translatedLanguageString = "null";
            string TranslatedTextString = "null";

            // check the values on the details page
            var translatedLanguageElement = await page.QuerySelectorAsync("#TranlationLanguage");
            if (translatedLanguageElement != null)
            {
                translatedLanguageString = await translatedLanguageElement.InnerTextAsync();
            }

            var TranslatedTextElement = await page.QuerySelectorAsync("#TranslatedText");
            if (TranslatedTextElement != null)
            {
                TranslatedTextString = await TranslatedTextElement.InnerTextAsync();
            }
            
            var OriginLanuageElement = await page.QuerySelectorAsync("#OriginLanguage");
            if (OriginLanuageElement != null)
            {
                OriginLanguageString = await OriginLanuageElement.InnerTextAsync();
            }
            
            var OriginTextElement = await page.QuerySelectorAsync("#OriginText");
            if (OriginTextElement != null)
            {
                OriginTextString = await OriginTextElement.InnerTextAsync();
            }

            // assert the values on the details page
            Assert.AreEqual("Spanish", OriginLanguageString);
            Assert.AreEqual(originalTestText, OriginTextString);
            Assert.AreEqual("English (American)", translatedLanguageString);
            Assert.AreEqual(translatedTestText, TranslatedTextString);

            // click on the back link
            await page.ClickAsync("text=Back to List");

            // check if the original text is displayed in the table
            originalTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{originalTestText}\")");
            Assert.IsNotNull(originalTextFound);

            // check if the translated text is displayed in the table
            translatedTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{translatedTestText}\")");
            Assert.IsNotNull(translatedTextFound);

            // click on the delete link of the created translation
            var deleteLink = await originalTextFound.QuerySelectorAsync("xpath=ancestor::tr//a[contains(@href, '/Delete')]");
            Assert.IsNotNull(deleteLink);
            await deleteLink.ClickAsync();

            // check the values on the details page
            var deleteOriginTextElement = await page.QuerySelectorAsync("#OriginText");
            if (deleteOriginTextElement != null)
            {
                OriginTextString = await deleteOriginTextElement.InnerTextAsync();
            }
            var deleteTranslatedTextElement = await page.QuerySelectorAsync("#TranslatedText");
            if (deleteTranslatedTextElement != null)
            {
                TranslatedTextString = await deleteTranslatedTextElement.InnerTextAsync();
            }
            Assert.AreEqual(translatedTestText, TranslatedTextString);
            Assert.AreEqual(originalTestText, OriginTextString);

            // click on the delete button
            await page.ClickAsync("[value=Delete]");

            // check if the home page is displayed
            var titleElement = await page.QuerySelectorAsync("title");
            if (titleElement != null)
            {
                string title = await titleElement.InnerTextAsync();
                Assert.AreEqual("History - WebApp", title);
            } 
            else 
            {
                Assert.Fail("Title element not found");
            }

 

            // check if the original text is displayed in the table
            originalTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{originalTestText}\")");
            Assert.IsNull(originalTextFound);

            // check if the translated text is displayed in the table
            translatedTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{translatedTestText}\")");
            Assert.IsNull(translatedTextFound);

            await browser.CloseAsync();
        }



        /// <summary>
        /// Represents an asynchronous operation that produces a result of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the asynchronous operation.</typeparam>
        [TestMethod]
        public async Task TranslationWithLanguageChangeDL()
        {
            // set test data
            string originalTestText = "Αυτό είναι ένα τεστ";
            string translatedTestText = "This is a test";

            // create playwright instance
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();//new BrowserTypeLaunchOptions { Headless = false, SlowMo = 1000 });
            var page = await browser.NewPageAsync();

            // open the website
            await page.GotoAsync("http://localhost:5095/");
            
            // select the language Spanish
            await page.ClickAsync("[id=languageFromDropdown]");
            await page.SelectOptionAsync("[id=languageFromDropdown]", LanguageFromSelectDL);
            // insert the text
            await page.ClickAsync("[id=OriginalTextField]");
            await page.FillAsync("[id=OriginalTextField]", originalTestText);
            // select american english
            await page.ClickAsync("[id=languageToDropdown]");
            await page.SelectOptionAsync("[id=languageToDropdown]", LanguageToSelect);
            // translate
            await page.ClickAsync("[id=TranslateButton]");

            // wait for the translation to be displayed
            await page.WaitForSelectorAsync("[id='TranslatedTextField']", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await page.WaitForSelectorAsync("[id='TranslatedTextField']");


            string translatedText = await page.InnerTextAsync("[id='TranslatedTextField']");
            Assert.AreEqual(translatedTestText, translatedText);

            await page.ClickAsync("text=Back to List");    

            // check if the original text is displayed in the table
            var originalTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{originalTestText}\")");
            Assert.IsNotNull(originalTextFound);

            // check if the translated text is displayed in the table
            var translatedTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{translatedTestText}\")");
            Assert.IsNotNull(translatedTextFound);

            // Klicke auf den Details-Link der ersten Übersetzung
            // click on the details link of the first translation
            var detailsLink = await originalTextFound.QuerySelectorAsync("xpath=ancestor::tr//a[contains(@href, '/Details')]");
            Assert.IsNotNull(detailsLink);
            await detailsLink.ClickAsync();

            // Declare variables for the details    
            string OriginLanguageString = "null";
            string OriginTextString = "null";
            string translatedLanguageString = "null";
            string TranslatedTextString = "null";

            // check the values on the details page
            var translatedLanguageElement = await page.QuerySelectorAsync("#TranlationLanguage");
            if (translatedLanguageElement != null)
            {
                translatedLanguageString = await translatedLanguageElement.InnerTextAsync();
            }

            var TranslatedTextElement = await page.QuerySelectorAsync("#TranslatedText");
            if (TranslatedTextElement != null)
            {
                TranslatedTextString = await TranslatedTextElement.InnerTextAsync();
            }
            
            var OriginLanuageElement = await page.QuerySelectorAsync("#OriginLanguage");
            if (OriginLanuageElement != null)
            {
                OriginLanguageString = await OriginLanuageElement.InnerTextAsync();
            }
            
            var OriginTextElement = await page.QuerySelectorAsync("#OriginText");
            if (OriginTextElement != null)
            {
                OriginTextString = await OriginTextElement.InnerTextAsync();
            }

            // assert the values on the details page
            Assert.AreEqual("Detect Language", OriginLanguageString);
            Assert.AreEqual(originalTestText, OriginTextString);
            Assert.AreEqual("English (American)", translatedLanguageString);
            Assert.AreEqual(translatedTestText, TranslatedTextString);

            // click on the back link
            await page.ClickAsync("text=Back to List");

            // check if the original text is displayed in the table
            originalTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{originalTestText}\")");
            Assert.IsNotNull(originalTextFound);

            // check if the translated text is displayed in the table
            translatedTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{translatedTestText}\")");
            Assert.IsNotNull(translatedTextFound);

            // click on the delete link of the created translation
            var deleteLink = await originalTextFound.QuerySelectorAsync("xpath=ancestor::tr//a[contains(@href, '/Delete')]");
            Assert.IsNotNull(deleteLink);
            await deleteLink.ClickAsync();

            // check the values on the details page
            var deleteOriginTextElement = await page.QuerySelectorAsync("#OriginText");
            if (deleteOriginTextElement != null)
            {
                OriginTextString = await deleteOriginTextElement.InnerTextAsync();
            }
            var deleteTranslatedTextElement = await page.QuerySelectorAsync("#TranslatedText");
            if (deleteTranslatedTextElement != null)
            {
                TranslatedTextString = await deleteTranslatedTextElement.InnerTextAsync();
            }
            Assert.AreEqual(translatedTestText, TranslatedTextString);
            Assert.AreEqual(originalTestText, OriginTextString);

            // click on the delete button
            await page.ClickAsync("[value=Delete]");

            // Check if the home page is displayed
            var titleElement = await page.QuerySelectorAsync("title");
            if (titleElement != null)
            {
                string title = await titleElement.InnerTextAsync();
                Assert.AreEqual("History - WebApp", title);
            } 
            else 
            {
                Assert.Fail("Title element not found");
            }

 

            // check if the original text is displayed in the table
            originalTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{originalTestText}\")");
            Assert.IsNull(originalTextFound);

            // check if the translated text is displayed in the table
            translatedTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{translatedTestText}\")");
            Assert.IsNull(translatedTextFound);

            await browser.CloseAsync();
        }
    }
}
