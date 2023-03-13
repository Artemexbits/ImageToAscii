// Copyright (c) 2023 Artemexbits. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace ImageToAscii;
class Program
{
    static void Main(string[] args)
    {
        string imagefile;
        string outputfile;
        const string IMG_FILENAME = "in.jpg";
        const string OUT_FILENAME = "out.txt";
        if(args.Length < 2) {
            // Console.WriteLine("invalid arguments");
            // Console.WriteLine("expected <image> <outputfile>");
            imagefile = Directory.GetCurrentDirectory() + "\\" + IMG_FILENAME;
            outputfile = Directory.GetCurrentDirectory() + "\\" + OUT_FILENAME;
        } else {
            imagefile = Directory.GetCurrentDirectory() + "\\" + args[0];
            outputfile = Directory.GetCurrentDirectory() + "\\" + args[1];
        }
        

        byte[,] arr = new byte[0, 0];
        Console.WriteLine("IMAGE: " + imagefile);
        try {
        using (var image = new Bitmap(System.Drawing.Image.FromFile(imagefile)))
        {
            int width = image.Width;
            int height = image.Height;

            arr = new byte[height, width];

            for(int i = 0; i < height; i++) {
                for(int j = 0; j < width; j++) {
                    //Console.WriteLine("-------------------");
                    int argb = image.GetPixel(j, i).ToArgb();
                    byte r = (byte) (argb >> 16 & 255);
                    byte g = (byte) (argb >> 8 & 255);
                    byte b = (byte) (argb & 255);
                    // Console.WriteLine("r: " + r);
                    // Console.WriteLine("g: " + g);
                    // Console.WriteLine("b: " + b);
                    // Console.WriteLine("argb: " + argb.ToString());
                    // Console.WriteLine("-------------------");
                    if(ablack(r,g,b, 10)) {
                        arr[i, j] = (int)'#';
                    } else
                    if(ared(r,g,b, 10)) {
                        arr[i, j] = (int)'Z';
                    } else 
                    if(agreen(r,g,b, 10)) {
                        arr[i, j] = (int)'O';
                    } else
                    if(apurple(r,g,b, 10)) {
                        arr[i, j] = (int)'X';
                    } else {
                        arr[i, j] = (int)' ';
                    }
                    
                }
            }
        }
        } catch (Exception e) {
            Console.WriteLine("ERROR: " + e);
        }
        try {
            using(FileStream fs = new FileStream(outputfile, FileMode.Create, FileAccess.Write)) {
                for(int i = 0; i < arr.GetLength(0); i++) {
                    for(int j = 0; j < arr.GetLength(1); j++) {
                        fs.WriteByte(arr[i, j]);
                    }
                    fs.WriteByte(10);
                }
                fs.Flush();
            }
        } catch (Exception e) {
            Console.WriteLine("ERROR: " + e);
        }
    }

    private static bool awhite(byte r, byte g, byte b) {
        return false;
    }
    private static bool ablack(int r, int g, int b, int d) {
        if(r < d*2 && g < d*2 && b < d*2) {
            //Console.WriteLine("black");
            return true;
        } else {
            return false;
        }
    }
    private static bool ared(int r, int g, int b, int d) {
        if(Enumerable.Range(((255-d) < 0 ? 0 : (255-d)), 255).Contains(r) && g < d*2 && b < d*2) {
            //Console.WriteLine("red");
            return true;
        } else {
            return false;
        }
    }
    private static bool agreen(int r, int g, int b, int d) {
        if(Enumerable.Range(((255-d) < 0 ? 0 : (255-d)), 255).Contains(g) && r < d*2 && b < d*2) {
            //Console.WriteLine("green");
            return true;
        } else {
            return false;
        }
    }
    private static bool ablue(int r, int g, int b, int d) {
        if(Enumerable.Range(((255-d) < 0 ? 0 : (255-d)), 255).Contains(b) && g < d*2 && r < d*2) {
            //Console.WriteLine("blue");
            return true;
        } else {
            return false;
        }
    }
    private static bool apurple(int r, int g, int b, int d) {
        if(Enumerable.Range(((255-d) < 0 ? 0 : (255-d)), 255).Contains(b) && g < d*2
        && Enumerable.Range(((128-d) < 0 ? 0 : (128-d)), (128+d)).Contains(r)) {
            //Console.WriteLine("purple");
            return true;
        } else {
            return false;
        }
    }
}
