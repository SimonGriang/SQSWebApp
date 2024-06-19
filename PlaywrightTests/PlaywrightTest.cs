using Microsoft.Playwright;
using System.Threading.Tasks;

[TestClass]
public class PlaywrightTest
{
    [TestMethod]
    public async Task TestMethod1()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var page = await browser.NewPageAsync();
        await page.GotoAsync("https://www.example.com");
        var title = await page.TitleAsync();
        Assert.AreEqual("Example Domain", title);
    }

    [TestMethod]
    public async Task TranslateEngGerTest()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();//(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 1000 });
        var page = await browser.NewPageAsync();
        await page.GotoAsync("http://localhost:5095/");
        await page.ClickAsync("[id=OriginalTextField]");
        await page.FillAsync("[id=OriginalTextField]", "Example Test");
        await page.ClickAsync("[id=TranslateButton]");

        await page.WaitForSelectorAsync("[id='TranslatedTextField']", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });

        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await page.WaitForSelectorAsync("[id='TranslatedTextField']");


        string translatedText = await page.InnerTextAsync("[id='TranslatedTextField']");
        Assert.AreEqual("Beispiel Test", translatedText);
    }

    [TestMethod]
    [Obsolete]
    public async Task CheckIfTranslationSaved()
    {
        string originalTestText = "Un test en otro idioma";
        string translatedTestText = "A test in another language";

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(); //(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 1000 });
        var page = await browser.NewPageAsync();

        // Webseite aufrufen
        await page.GotoAsync("http://localhost:5095/");
        
        // Spanish auswählen
        await page.ClickAsync("[id=languageFromDropdown]");
        await page.SelectOptionAsync("[id=languageFromDropdown]", new [] { "Spanish" });
        // Text eingeben
        await page.ClickAsync("[id=OriginalTextField]");
        await page.FillAsync("[id=OriginalTextField]", originalTestText);
        // Amerikanisch Englisch auswählen
        await page.ClickAsync("[id=languageToDropdown]");
        await page.SelectOptionAsync("[id=languageToDropdown]", new [] { "English (American)" });
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

        await browser.CloseAsync();
    }
}
