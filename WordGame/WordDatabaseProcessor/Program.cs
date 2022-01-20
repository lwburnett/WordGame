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
            var lines = File.ReadAllLines("C:\\Users\\Luke\\Desktop\\Dev\\WordDatabase.txt");

            var chosenLines = new List<string>();
            foreach (var line in lines.Skip(8805))
            {
                Console.WriteLine(line);
                var character = Console.ReadKey();

                if (ConsoleKey.LeftArrow == character.Key)
                    chosenLines.Add(line);
                else if (ConsoleKey.Q == character.Key)
                {
                    AppendToSecretWordList(chosenLines);
                    return;
                }

                Console.Clear();
            }

            AppendToSecretWordList(chosenLines);
        }

        static void AppendToSecretWordList(IEnumerable<string> iLines)
        {
            var words = iLines.Select(l => l.Split('\t')[0].Trim());

            File.AppendAllLines("C:\\Users\\Luke\\Desktop\\Dev\\SecretWordDatabase.txt", words);
        }
    }
}
