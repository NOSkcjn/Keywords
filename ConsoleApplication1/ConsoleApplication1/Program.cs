using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    class Program
    {
        static void PrintResult(Dictionary<string, int> keyWord)
        {
            Console.WriteLine("По алфавиту:");
            foreach (var word in keyWord) Console.WriteLine($"Слово {word.Key}: {word.Value}");
            Console.WriteLine();
            Console.WriteLine("По возрастанию:");
            Sorting sort = new Sorting();
            keyWord = sort.GetSortValues(keyWord);
            foreach (var word in keyWord) Console.WriteLine($"Слово {word.Key}: {word.Value}");
            Console.WriteLine();
            Console.WriteLine("ТОП 5:");
            List<KeyValuePair<string, int>> top = new List<KeyValuePair<string, int>>();
            foreach (var item in keyWord) top.Add(item);
            int counter = 0;
            for (int i = top.Count - 1; i > -1; i--)
            {
                counter++;
                Console.WriteLine($"Слово {top[i].Key}: {top[i].Value}");
                if (counter > 4) break;
            }
        }
        static int Main(string[] args)
        {
            string pathprog;
            Dictionary<string, int> keyWord = new Dictionary<string, int>();
            if (args.Length < 1) pathprog = "Program.cs";
            else pathprog = args[0];
            Parsing pars = new Parsing(pathprog);
            keyWord = pars.GetKeyWords();
            if(keyWord.Count == 0) return 0;
            PrintResult(keyWord);
            Console.ReadKey();
            return 1;
        }
    }
}