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

        [TestMethod]
        public async Task TranslationWithoutLanguageChange()
        {
            string originalTestText = "Example Test";
            string translatedTestText = "Beispiel Test";
            string OriginTextString = "null";
            string TranslatedTextString = "null";

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();//new BrowserTypeLaunchOptions { Headless = false, SlowMo = 1000 });
            var page = await browser.NewPageAsync();
            await page.GotoAsync("http://localhost:5095/");
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

            // Klicke auf den Löschen-Link der erstellten Übersetzung
            var deleteLink = await originalTextFound.QuerySelectorAsync("xpath=ancestor::tr//a[contains(@href, '/Delete')]");
            Assert.IsNotNull(deleteLink);
            await deleteLink.ClickAsync();

            // Überprüfe die Werte auf der Details-Seite
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

            // Klicke auf den Löschen-Button
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

 

            // Überprüfe, ob der Originaltext in der Tabelle angezeigt wird
            originalTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{originalTestText}\")");
            Assert.IsNull(originalTextFound);

            // Überprüfe, ob der übersetzte Text in der Tabelle angezeigt wird
            var translatedTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{translatedTestText}\")");
            Assert.IsNull(translatedTextFound);

            await browser.CloseAsync();
        }

        [TestMethod]
        public async Task TranslationWithLanguageChange()
        {
            string originalTestText = "Un test en otro idioma";
            string translatedTestText = "A test in another language";

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();//new BrowserTypeLaunchOptions { Headless = false, SlowMo = 1000 });
            var page = await browser.NewPageAsync();

            // Webseite aufrufen
            await page.GotoAsync("http://localhost:5095/");
            
            // Spanish auswählen
            await page.ClickAsync("[id=languageFromDropdown]");
            await page.SelectOptionAsync("[id=languageFromDropdown]", LanguageFromSelect);
            // Text eingeben
            await page.ClickAsync("[id=OriginalTextField]");
            await page.FillAsync("[id=OriginalTextField]", originalTestText);
            // Amerikanisch Englisch auswählen
            await page.ClickAsync("[id=languageToDropdown]");
            await page.SelectOptionAsync("[id=languageToDropdown]", LanguageToSelect);
            // Übersetzen
            await page.ClickAsync("[id=TranslateButton]");

            // Warte auf die Anzeige der Übersetzung
            await page.WaitForSelectorAsync("[id='TranslatedTextField']", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await page.WaitForSelectorAsync("[id='TranslatedTextField']");


            string translatedText = await page.InnerTextAsync("[id='TranslatedTextField']");
            Assert.AreEqual(translatedTestText, translatedText);

            await page.ClickAsync("text=Back to List");    

            // Überprüfe, ob der Originaltext in der Tabelle angezeigt wird
            var originalTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{originalTestText}\")");
            Assert.IsNotNull(originalTextFound);

            // Überprüfe, ob der übersetzte Text in der Tabelle angezeigt wird
            var translatedTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{translatedTestText}\")");
            Assert.IsNotNull(translatedTextFound);

            // Klicke auf den Details-Link der ersten Übersetzung
            var detailsLink = await originalTextFound.QuerySelectorAsync("xpath=ancestor::tr//a[contains(@href, '/Details')]");
            Assert.IsNotNull(detailsLink);
            await detailsLink.ClickAsync();

            // Declare variables for the details
            string OriginLanguageString = "null";
            string OriginTextString = "null";
            string translatedLanguageString = "null";
            string TranslatedTextString = "null";

            // Überprüfe die Werte auf der Details-Seite
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

            // Assertions für die Details
            Assert.AreEqual("Spanish", OriginLanguageString);
            Assert.AreEqual(originalTestText, OriginTextString);
            Assert.AreEqual("English (American)", translatedLanguageString);
            Assert.AreEqual(translatedTestText, TranslatedTextString);

            // Klicke auf den Zurück-Link
            await page.ClickAsync("text=Back to List");

            // Überprüfe, ob der Originaltext in der Tabelle angezeigt wird
            originalTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{originalTestText}\")");
            Assert.IsNotNull(originalTextFound);

            // Überprüfe, ob der übersetzte Text in der Tabelle angezeigt wird
            translatedTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{translatedTestText}\")");
            Assert.IsNotNull(translatedTextFound);

            // Klicke auf den Löschen-Link der erstellten Übersetzung
            var deleteLink = await originalTextFound.QuerySelectorAsync("xpath=ancestor::tr//a[contains(@href, '/Delete')]");
            Assert.IsNotNull(deleteLink);
            await deleteLink.ClickAsync();

            // Überprüfe die Werte auf der Details-Seite
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

            // Klicke auf den Löschen-Button
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

 

            // Überprüfe, ob der Originaltext in der Tabelle angezeigt wird
            originalTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{originalTestText}\")");
            Assert.IsNull(originalTextFound);

            // Überprüfe, ob der übersetzte Text in der Tabelle angezeigt wird
            translatedTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{translatedTestText}\")");
            Assert.IsNull(translatedTextFound);

            await browser.CloseAsync();
        }



        [TestMethod]
        public async Task TranslationWithLanguageChangeDL()
        {
            string originalTestText = "Αυτό είναι ένα τεστ";
            string translatedTestText = "This is a test";

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();//new BrowserTypeLaunchOptions { Headless = false, SlowMo = 1000 });
            var page = await browser.NewPageAsync();

            // Webseite aufrufen
            await page.GotoAsync("http://localhost:5095/");
            
            // Spanish auswählen
            await page.ClickAsync("[id=languageFromDropdown]");
            await page.SelectOptionAsync("[id=languageFromDropdown]", LanguageFromSelectDL);
            // Text eingeben
            await page.ClickAsync("[id=OriginalTextField]");
            await page.FillAsync("[id=OriginalTextField]", originalTestText);
            // Amerikanisch Englisch auswählen
            await page.ClickAsync("[id=languageToDropdown]");
            await page.SelectOptionAsync("[id=languageToDropdown]", LanguageToSelect);
            // Übersetzen
            await page.ClickAsync("[id=TranslateButton]");

            // Warte auf die Anzeige der Übersetzung
            await page.WaitForSelectorAsync("[id='TranslatedTextField']", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await page.WaitForSelectorAsync("[id='TranslatedTextField']");


            string translatedText = await page.InnerTextAsync("[id='TranslatedTextField']");
            Assert.AreEqual(translatedTestText, translatedText);

            await page.ClickAsync("text=Back to List");    

            // Überprüfe, ob der Originaltext in der Tabelle angezeigt wird
            var originalTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{originalTestText}\")");
            Assert.IsNotNull(originalTextFound);

            // Überprüfe, ob der übersetzte Text in der Tabelle angezeigt wird
            var translatedTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{translatedTestText}\")");
            Assert.IsNotNull(translatedTextFound);

            // Klicke auf den Details-Link der ersten Übersetzung
            var detailsLink = await originalTextFound.QuerySelectorAsync("xpath=ancestor::tr//a[contains(@href, '/Details')]");
            Assert.IsNotNull(detailsLink);
            await detailsLink.ClickAsync();

            // Declare variables for the details
            string OriginLanguageString = "null";
            string OriginTextString = "null";
            string translatedLanguageString = "null";
            string TranslatedTextString = "null";

            // Überprüfe die Werte auf der Details-Seite
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

            // Assertions für die Details
            Assert.AreEqual("Detect Language", OriginLanguageString);
            Assert.AreEqual(originalTestText, OriginTextString);
            Assert.AreEqual("English (American)", translatedLanguageString);
            Assert.AreEqual(translatedTestText, TranslatedTextString);

            // Klicke auf den Zurück-Link
            await page.ClickAsync("text=Back to List");

            // Überprüfe, ob der Originaltext in der Tabelle angezeigt wird
            originalTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{originalTestText}\")");
            Assert.IsNotNull(originalTextFound);

            // Überprüfe, ob der übersetzte Text in der Tabelle angezeigt wird
            translatedTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{translatedTestText}\")");
            Assert.IsNotNull(translatedTextFound);

            // Klicke auf den Löschen-Link der erstellten Übersetzung
            var deleteLink = await originalTextFound.QuerySelectorAsync("xpath=ancestor::tr//a[contains(@href, '/Delete')]");
            Assert.IsNotNull(deleteLink);
            await deleteLink.ClickAsync();

            // Überprüfe die Werte auf der Details-Seite
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

            // Klicke auf den Löschen-Button
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

 

            // Überprüfe, ob der Originaltext in der Tabelle angezeigt wird
            originalTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{originalTestText}\")");
            Assert.IsNull(originalTextFound);

            // Überprüfe, ob der übersetzte Text in der Tabelle angezeigt wird
            translatedTextFound = await page.QuerySelectorAsync($".table tbody td:has-text(\"{translatedTestText}\")");
            Assert.IsNull(translatedTextFound);

            await browser.CloseAsync();
        }
    }
}
