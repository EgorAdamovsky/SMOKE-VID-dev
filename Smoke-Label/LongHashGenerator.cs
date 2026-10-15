using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

public static class LongHashGenerator
{
    public static string GetLongImageHash(Bitmap bitmap)
    {
        // Получаем байты пикселей
        byte[] pixelBytes = GetPixelBytes(bitmap);

        // Создаем хеш SHA-512
        SHA512 sha512 = SHA512.Create();
        byte[] hashBytes = sha512.ComputeHash(pixelBytes);
        sha512.Dispose(); // Явное освобождение ресурсов

        // Возвращаем Base64 (88 символов)
        return Convert.ToBase64String(hashBytes).ToString()
            .Replace("==", "")
            .Replace("+", "")
            .Replace("/", "");
    }

    private static byte[] GetPixelBytes(Bitmap bitmap)
    {
        Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);

        try
        {
            int byteCount = bmpData.Stride * bitmap.Height;
            byte[] pixels = new byte[byteCount];
            IntPtr ptr = bmpData.Scan0;

            // Копируем пиксельные данные
            Marshal.Copy(ptr, pixels, 0, byteCount);
            return pixels;
        }
        finally
        {
            bitmap.UnlockBits(bmpData);
        }
    }
}