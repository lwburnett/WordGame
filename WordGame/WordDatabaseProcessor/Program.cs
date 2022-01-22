using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordDatabaseProcessor
{
    class Program
    {
        static void Main()
        {
            var allWords = File.ReadAllLines("C:\\Users\\Luke\\Desktop\\Dev\\WordDatabase.txt").Select(w => w.ToUpperInvariant()).ToList();
            var secretWords = File.ReadAllLines("C:\\Users\\Luke\\Desktop\\Dev\\SecretWordDatabase.txt").Select(w => w.ToUpperInvariant()).ToList();
            
            foreach (var secretWord in secretWords)
            {
                if (!allWords.Contains(secretWord))
                    Console.WriteLine(secretWord);
            }
        }
    }
}
