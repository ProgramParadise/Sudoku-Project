using System.Globalization;
using Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SudokuGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // for (int i = 1; i <= 9; i++)
            // {
            //     for (int j = 1; j <= 9; j++)
            //     {
            //         Console.Write(j + ",");
            //     }
            // }
            Console.WriteLine("input 'all at once' or 'one at a time'");
            //assign values
            List<List<string>> items = new List<List<string>>();

            if (Console.ReadLine().ToLowerInvariant() == "all at once")
            {
                Console.WriteLine("Output the matrix with each digit separated by a comma.");
                string num = Console.ReadLine();
                List<string> tokens = new List<string>(num.Split(","));
                for (int i = 0; i != 9; i++)
                {
                    items.Add(tokens.GetRange(i * 9, 9));
                }
            }
            if(Console.ReadLine().ToLowerInvariant() == "one at a time")
            {
                List<string> tokens = new List<string>();
                int width = 9;
                int height = 9;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        string input;
                        int inputValue;
                        do
                        {
                            Console.WriteLine($"Input value for ({x},{y}): ");
                        }
                        while (!int.TryParse(input = Console.ReadLine(), out inputValue));
                        tokens.Add("" + inputValue);
                    }
                    items.Add(tokens.GetRange(x * 9, 9));
                }
            }
            //write values
            Console.WriteLine("'code' or 'matrix'\n");
            Print2DList(items, Console.ReadLine());
        }
        public static void Print2DList<T>(List<List<T>> matrix, string s)
        {
            int h = matrix.Count;
            int w = matrix.Max(l => l.Count);

            if (s.ToLowerInvariant() == "code")
            {
                Console.WriteLine("new int[,]\n{ ");
                for (int i = 0; i < h; i++)
                {
                    Console.Write("\t{");
                    for (int j = 0; j < w; j++)
                    {
                        if (j < matrix[i].Count)
                            Console.Write(matrix[i][j]);
                        if (j < (w - 1) && i < h) Console.Write(",");
                    }
                    if (i != h - 1) Console.Write("},");
                    else Console.Write("} \n}//,");
                    Console.WriteLine();
                }
            }
            if (s.ToLowerInvariant() == "matrix")
            {
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        if (j < matrix[i].Count)
                        {
                            Console.Write(matrix[i][j]);
                            Console.Write(" ");
                        }   
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}