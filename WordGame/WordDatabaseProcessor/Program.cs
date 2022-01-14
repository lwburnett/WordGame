﻿using System.Collections.Generic;
using System.IO;

namespace WordDatabaseProcessor
{
    class Program
    {
        static void Main()
        {
            var lines = File.ReadAllLines("C:\\Users\\Luke\\Desktop\\Dev\\WordDatabase.txt");

            var chosenWords = new List<string>();
            foreach (var line in lines)
            {
                if (line.Trim().Length == 5)
                    chosenWords.Add(line.Trim());
            }

            File.WriteAllLines("C:\\Users\\Luke\\Desktop\\Dev\\WordDatabase_Slimmed.txt", chosenWords);
        }
    }
}
