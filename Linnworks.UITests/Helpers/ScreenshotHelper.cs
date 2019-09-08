using System;
using System.IO;
using Linnworks.UITests.Core;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Linnworks.UITests.Helpers
{
    public class ScreenshotHelper
    {
        public static void TakeScreenshot()
        {
            var screenshotTaker = DriverManager.Current.Driver as ITakesScreenshot;
            var screenshotFolderPath = Directory.GetCurrentDirectory() + "/screenshots/";
            var screenshotFile = screenshotFolderPath + TestContext.CurrentContext.Test.FullName + ".png";
            
            if (!Directory.Exists(screenshotFolderPath))
            {
                Directory.CreateDirectory(screenshotFolderPath);
            }
            
            var screenshot = screenshotTaker.GetScreenshot();
            screenshot.SaveAsFile(screenshotFile, ScreenshotImageFormat.Png);
            
            Console.WriteLine($"Saved screenshot for failed test - {screenshotFile}");
        }
    }
}
