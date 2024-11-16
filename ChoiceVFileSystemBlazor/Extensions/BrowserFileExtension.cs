using ChoiceVFileSystemBlazor.Services.Vikunja.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace ChoiceVFileSystemBlazor.Extensions;

public static class BrowserFileExtension
{
    public static string GetContentType(this IBrowserFile browserFile)
    {
        return !string.IsNullOrEmpty(browserFile.ContentType) ? browserFile.ContentType : GetContentTypeFallback(browserFile.Name);
    }

    public static string GetContentType(this VikunjaFile file)
    {
        return GetContentTypeFallback(file.Name);
    }
    
    private static string GetContentTypeFallback(string fileName)
    {
        var fileType = "application/octet-stream";
        
        if (fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg"))
        {
            fileType = "image/jpeg";
        }
        else if (fileName.EndsWith(".png"))
        {
            fileType = "image/png";
        }
        else if (fileName.EndsWith(".gif"))
        {
            fileType = "image/gif";
        }
        else if (fileName.EndsWith(".bmp"))
        {
            fileType = "image/bmp";
        }
        else if (fileName.EndsWith(".pdf"))
        {
            fileType = "application/pdf";
        }
        else if (fileName.EndsWith(".txt") || fileName.EndsWith(".log"))
        {
            fileType = "text/plain";
        }
        else if (fileName.EndsWith(".mp4"))
        {
            fileType = "video/mp4";
        }
        else if (fileName.EndsWith(".webm"))
        {
            fileType = "video/webm";
        }
        else if (fileName.EndsWith(".ogg") || fileName.EndsWith(".oga"))
        {
            fileType = "audio/ogg";
        }
        else if (fileName.EndsWith(".mp3"))
        {
            fileType = "audio/mp3";
        }
        else if (fileName.EndsWith(".wav"))
        {
            fileType = "audio/wav";
        }
        else if (fileName.EndsWith(".svg"))
        {
            fileType = "image/svg+xml";
        }
        
        return fileType; 
    }
}