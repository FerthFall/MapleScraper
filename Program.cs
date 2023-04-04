// See https://aka.ms/new-console-template for more information
using MapleScreenScraper;
using System.Drawing;

string lastLine = string.Empty;

//await Discord.PushMessageToDiscordChannel("Scraper Initalzing");
Console.WriteLine("This is is a screenscraper bot for maplestory. Please select the chat region");
Console.ReadKey();

Rectangle rect = RegionSelection.GetSelection();
Bitmap screenShot = Screenshot.TakeScreenshot(rect);
screenShot.Save("test.png");
string scrapedText = Screenshot.ReadTextFromBitmap(screenShot);
List<string> textLines;

if (scrapedText.Length > 0)
{
    textLines = scrapedText.Split('\n').ToList();
    textLines = textLines.Where(t => !string.IsNullOrEmpty(t) && !string.IsNullOrWhiteSpace(t)).ToList();
    lastLine = textLines.Last();

    await Discord.PushMessageToDiscordChannel(string.Join("\n", textLines));
}

while (true)
{
    Thread.Sleep(3000);
    screenShot = Screenshot.TakeScreenshot(rect);
    screenShot.Save("test.png");
    textLines = new List<string>();
    scrapedText = Screenshot.ReadTextFromBitmap(screenShot);
    textLines = scrapedText.Split('\n').ToList();
    textLines = textLines.Where(t => !string.IsNullOrEmpty(t) && !string.IsNullOrWhiteSpace(t)).ToList();
    textLines.RemoveRange(0, textLines.FindLastIndex(t => t.Contains(lastLine)) + 1);
    if (textLines.Count > 0)
    {
        lastLine = textLines.Last();
        await Discord.PushMessageToDiscordChannel(string.Join("\n", textLines));
    }    
}

