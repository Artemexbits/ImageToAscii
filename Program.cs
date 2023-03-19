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
        string[] image_paths = new string[0];
        string image_dir_path = "";
        string output_dir_path = "";
        if(args.Length == 1) {
            image_dir_path = Directory.GetCurrentDirectory() + "\\" + args[0];
            Console.WriteLine("image directory: " + image_dir_path);
            image_paths = Directory.GetFiles(image_dir_path, "*.jpg");
        } else
        if(args.Length == 2) {
            image_dir_path = Directory.GetCurrentDirectory() + "\\" + args[0];
            Console.WriteLine("image directory: " + image_dir_path);
            image_paths = Directory.GetFiles(image_dir_path, "*.jpg");

            output_dir_path = Directory.GetCurrentDirectory() + "\\" + args[1];
            Console.WriteLine("output directory: " + output_dir_path);
            if(!Directory.Exists(output_dir_path)) {
                Directory.CreateDirectory(output_dir_path);
            }
        } else 
        if(args.Length == 0) {
            image_dir_path = Directory.GetCurrentDirectory();
            Console.WriteLine("image directory: " + image_dir_path);
            image_paths = Directory.GetFiles(image_dir_path, "*.jpg");

            output_dir_path = Directory.GetCurrentDirectory();
            Console.WriteLine("output directory: " + output_dir_path);
            if(!Directory.Exists(output_dir_path)) {
                Directory.CreateDirectory(output_dir_path);
            }
        }

        Parallel.For(0, image_paths.Length, i => convertToFile(image_paths[i], output_dir_path));

        // List<Task<bool>> convert_tasks = new List<Task<bool>>();
        // for(int i = 0; i < image_paths.Length; i++) {
        //     convert_tasks.Add(convertToFile(image_paths[i], output_dir_path));
        // } 
        // await Task.WhenAll(convert_tasks);
    }

    private static void convertToFile(string image_path, string output_dir_path) {
        try {
            byte[,] arr = extractCharsFromImage(image_path);
            string outputfile = image_path;
            if(!string.IsNullOrWhiteSpace(output_dir_path)) {
                outputfile =  output_dir_path + outputfile.Substring(outputfile.LastIndexOf("\\"));
            }
            writeWorldFile(outputfile.Replace(".jpg", ".txt"), arr);
        } catch (Exception) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR: converting {image_path} failed");
        }
        Console.ForegroundColor = ConsoleColor.White;
    }

    private static void writeWorldFile(string outputfile, byte[,] arr) {
        using(FileStream fs = new FileStream(outputfile, FileMode.Create, FileAccess.Write)) {
            for(int i = 0; i < arr.GetLength(0); i++) {
                for(int j = 0; j < arr.GetLength(1); j++) {
                    fs.WriteByte(arr[i, j]);
                }
                fs.WriteByte(10);
            }
            fs.Flush();
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("created: " + outputfile);
        Console.ForegroundColor = ConsoleColor.White;
    }

    private static byte[,] extractCharsFromImage(string imagefile) {
        Console.WriteLine("loading: " + imagefile);
        byte[,] arr = new byte[0, 0];
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
        return arr;
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
