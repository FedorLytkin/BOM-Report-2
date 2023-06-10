using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BitmapClass
{
    public static Bitmap GetResizePicture(ShellFile shellFile, int PicSize)
    {
        switch (PicSize)
        {
            case 32:
                return shellFile.Thumbnail.SmallBitmap;
            case 64:
                return shellFile.Thumbnail.LargeBitmap;
            case 128:
                return shellFile.Thumbnail.ExtraLargeBitmap;
            default:
                return resizeImage(shellFile.Thumbnail.ExtraLargeBitmap, PicSize);
        }
    } 

    public static string GetBase32(Bitmap pic)
    {
        if (pic == null) return null;
        using (var ms = new System.IO.MemoryStream())
        {
            pic.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            return Convert.ToBase64String(byteImage);
        }
    }
    public static Bitmap resizeImage(Bitmap imgToResize, int PicSize)
    {
        if (imgToResize == null) return null;
        //Get the image current width  
        int sourceWidth = imgToResize.Width;
        //Get the image current height  
        int sourceHeight = imgToResize.Height;
        float nPercent = 0;
        float nPercentW = 0;
        float nPercentH = 0;
        //Calulate  width with new desired size  
        nPercentW = ((float)PicSize / (float)sourceWidth);
        //Calculate height with new desired size  
        nPercentH = ((float)PicSize / (float)sourceHeight);
        if (nPercentH < nPercentW)
            nPercent = nPercentH;
        else
            nPercent = nPercentW;
        //New Width  
        int destWidth = (int)(sourceWidth * nPercent);
        //New Height  
        int destHeight = (int)(sourceHeight * nPercent);
        Bitmap b = new Bitmap(destWidth, destHeight);
        Graphics g = Graphics.FromImage((System.Drawing.Image)b);
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        // Draw image with new width and height  
        g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
        g.Dispose();
        return b;
    }
}
