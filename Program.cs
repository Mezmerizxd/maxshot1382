using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string baseFolder = "maxshot1382";
        string customFolder = null;

#if DEBUG
        int screenWidth = 2560;
        int screenHeight = 1440;
        int imageWidth = 500;
        int imageHeight = 500;
        string imageName = "test_1";
        int offsetX = 0;
        int offsetY = 0;
        customFolder = "test";
#else
        if (args.Length < 7)
        {
            Console.WriteLine("Usage: maxshot1382.exe <screenWidth> <screenHeight> <imageWidth> <imageHeight> <imageName> <offsetX> <offsetY> [customFolder]");
            return;
        }

        // Parse command line arguments
        if (!int.TryParse(args[0], out int screenWidth) ||
            !int.TryParse(args[1], out int screenHeight) ||
            !int.TryParse(args[2], out int imageWidth) ||
            !int.TryParse(args[3], out int imageHeight) ||
            !int.TryParse(args[5], out int offsetX) ||
            !int.TryParse(args[6], out int offsetY))
        {
            Console.WriteLine("Invalid arguments. Please provide integer values for all arguments.");
            return; 
        }

        string imageName = args[4];
        if (imageName == null)
        {
            Console.WriteLine("Invalid arguments. Please provide integer values for all arguments.");
            return;
        }

        if (args.Length > 7)
        {
            customFolder = args[7];
        }
        if (customFolder == null)
        {
            Console.WriteLine("Invalid arguments. Please provide integer values for all arguments.");
            return;
        }
#endif

        string savePath = GetSavePath(baseFolder, customFolder, imageName);

        // Capture a screenshot of the specified region
        Bitmap screenshot = CaptureScreenCenter(screenWidth, screenHeight, imageWidth, imageHeight, offsetX, offsetY);

        // Save the screenshot as a PNG image
        SaveAsPng(screenshot, savePath);

        Console.WriteLine("Screenshot saved as " + savePath);
    }

    static Bitmap CaptureScreenCenter(int screenWidth, int screenHeight, int imageWidth, int imageHeight, int offsetX, int offsetY)
    {
        // Calculate the coordinates for the center of the screen
        int centerX = (screenWidth - imageWidth) / 2 + offsetX;
        int centerY = (screenHeight - imageHeight) / 2 + offsetY;

        // Ensure that the specified dimensions are within the bounds of the screen
        if (centerX < 0) centerX = 0;
        if (centerY < 0) centerY = 0;
        if (centerX + imageWidth > screenWidth) centerX = screenWidth - imageWidth;
        if (centerY + imageHeight > screenHeight) centerY = screenHeight - imageHeight;

        // Capture the screenshot of the specified region
        using (Bitmap screenshot = new Bitmap(imageWidth, imageHeight))
        {
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(centerX, centerY, 0, 0, new Size(imageWidth, imageHeight));
            }

            return new Bitmap(screenshot);
        }
    }

    static void SaveAsPng(Bitmap bitmap, string outputPath)
    {
        bitmap.Save(outputPath, ImageFormat.Webp);
    }

    static string GetSavePath(string baseFolder, string customFolder, string imageName)
    {
        string folderPath = baseFolder;

        if (!string.IsNullOrEmpty(customFolder))
        {
            folderPath = Path.Combine(baseFolder, customFolder);
        }

        // Create directory if it doesn't exist
        Directory.CreateDirectory(folderPath);

        return Path.Combine(folderPath, imageName + ".webp");
    }
}
