﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
  https://stackoverflow.com/questions/2434156/webbrowser-drawtobitmap-or-other-methods/2434231#2434231
*/
namespace LitmusApp
{

    public static class Utilities
    {
        public const int SRCCOPY = 13369376;
        public static Image CaptureScreen()
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }

        public static Image CaptureWindow(IntPtr handle)
        {

            IntPtr hdcSrc = User32.GetWindowDC(handle);

            RECT windowRect = new RECT();
            User32.GetWindowRect(handle, ref windowRect);

            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;

            IntPtr hdcDest = Gdi32.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = Gdi32.CreateCompatibleBitmap(hdcSrc, width, height);

            IntPtr hOld = Gdi32.SelectObject(hdcDest, hBitmap);
            Gdi32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, SRCCOPY);
            Gdi32.SelectObject(hdcDest, hOld);
            Gdi32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);

            Image image = Image.FromHbitmap(hBitmap);
            Gdi32.DeleteObject(hBitmap);

            return image;
        }
    }
}
