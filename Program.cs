using System;
using System.Drawing;
using System.Drawing.Imaging;

internal class Program
{
    private static char[] ascii = new char[16]
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'A', 'B', 'C', 'D', 'E', 'F'
    };

    private static string txt = null;

    private static int SimplifyColorComponent(int Value)
    {
        if (Value >= 52)
        {
            return 63;
        }
        if (Value >= 32)
        {
            return 42;
        }
        if (Value >= 12)
        {
            return 21;
        }
        return 0;
    }

    private static byte DecreaseColor256(byte Red, byte Green, byte Blue)
    {
        byte[,] array = new byte[16, 3]
        {
            { 0, 0, 0 },
            { 0, 0, 42 },
            { 0, 42, 0 },
            { 0, 42, 42 },
            { 42, 0, 0 },
            { 42, 0, 42 },
            { 42, 42, 0 },
            { 42, 42, 42 },
            { 0, 0, 21 },
            { 0, 0, 63 },
            { 0, 42, 21 },
            { 0, 42, 63 },
            { 42, 0, 21 },
            { 42, 0, 63 },
            { 42, 42, 21 },
            { 42, 42, 63 }
        };
        byte b = 0;
        byte b2 = 0;
        byte b3 = 0;
        byte b4 = 0;
        for (byte b5 = 0; b5 <= 2; b5++)
        {
            switch (b5)
            {
                case 0:
                    b = (byte)SimplifyColorComponent(Red / 4);
                    break;
                case 1:
                    b = (byte)SimplifyColorComponent(Green / 4);
                    break;
                case 2:
                    b = (byte)SimplifyColorComponent(Blue / 4);
                    break;
            }
            byte b6 = 0;
            while (b != array[b6, b5])
            {
                b6++;
                if (b6 > 15)
                {
                    b -= 21;
                    b6 = 0;
                }
            }
            switch (b5)
            {
                case 0:
                    b2 = b;
                    break;
                case 1:
                    b3 = b;
                    break;
                case 2:
                    b4 = b;
                    break;
            }
        }
        for (byte b6 = 0; b6 <= 15; b6++)
        {
            if (array[b6, 0] == b2 && array[b6, 1] == b3 && array[b6, 2] == b4)
            {
                return b6;
            }
        }
        return 0;
    }

    private static void ASASCII(string filename)
    {
        Image image = Image.FromFile(filename);
        if (ImageAnimator.CanAnimate(image))
        {
            FrameDimension dimension = new FrameDimension(image.FrameDimensionsList[0]);
            int frameCount = image.GetFrameCount(dimension);
            for (int i = 0; i < frameCount; i++)
            {
                image.SelectActiveFrame(dimension, i);
                Console.WriteLine("Processing frame " + (i + 1) + " of " + frameCount);
                Bitmap bitmap = image.Clone() as Bitmap;
                int num = bitmap.Width / 100;
                int num2 = bitmap.Height / 40;
                for (int j = 0; j < bitmap.Height - num2; j += num2)
                {
                    for (int k = 0; k < bitmap.Width - num; k += num)
                    {
                        Color pixel = bitmap.GetPixel(k, j);
                        txt += ascii[DecreaseColor256(pixel.R, pixel.G, pixel.B)];
                    }
                    txt += "\n";
                }
                txt += "$";
            }
            Console.Clear();
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(80, 43);
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            string[] array = txt.Split('$');
            for (int l = 0; l < array.Length; l++)
            {
                Console.SetCursorPosition(0, 0);
                string text = array[l];
                for (int m = 0; m < text.Length; m++)
                {
                    if (text[m] == '\n')
                    {
                        Console.WriteLine();
                        continue;
                    }
                    Console.BackgroundColor = (ConsoleColor)Convert.ToInt32(text[m].ToString(), 16);
                    Console.Write(" ");
                }
            }
        }
        else
        {
            Bitmap bitmap2 = (Bitmap)image;
            int num3 = bitmap2.Width / 100;
            int num4 = bitmap2.Height / 40;
            for (int n = 0; n < bitmap2.Height - num4; n += num4)
            {
                for (int num5 = 0; num5 < bitmap2.Width - num3; num5 += num3)
                {
                    Color pixel2 = bitmap2.GetPixel(num5, n);
                    txt += ascii[DecreaseColor256(pixel2.R, pixel2.G, pixel2.B)];
                }
                txt += "\n";
            }
            Console.Clear();
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(80, 43);
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            for (int num6 = 0; num6 < txt.Length; num6++)
            {
                if (txt[num6] == '\n')
                {
                    Console.WriteLine();
                    continue;
                }
                Console.BackgroundColor = (ConsoleColor)Convert.ToInt32(txt[num6].ToString(), 16);
                Console.Write(" ");
            }
        }
        Console.ResetColor();
    }

    private static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("GIF16 <image file>");
            return;
        }
        ASASCII(args[0]);
        Console.WriteLine("Press Enter to quit...");
        Console.ReadLine();
    }
}

