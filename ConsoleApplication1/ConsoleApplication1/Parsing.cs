using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Parsing
    {   
        string text;
        const bool IgnoreKeywordsList = true;//игнорировать ли список слов, расположенный ниже
        #region ключевые слова
        string[] key =
        {
            "abstract",
            "as",
            "base",
            "bool",
            "break",
            "byte",
            "case",
            "catch",
            "char",
            "checked",
            "class",
            "const",
            "continue",
            "decimal",
            "default",
            "delegate",
            "do",
            "double",
            "else",
            "enum",
            "event",
            "explicit",
            "extern",
            "false",
            "finally",
            "fixed",
            "float",
            "for",
            "foreach",
            "goto",
            "if",
            "implicit",
            "in",
            "int",
            "interface",
            "internal",
            "is",
            "lock",
            "long",
            "namespace",
            "new",
            "null",
            "object",
            "operator",
            "out",
            "override",
            "params",
            "private",
            "protected",
            "public",
            "readonly",
            "ref",
            "return",
            "sbyte",
            "sealed",
            "short",
            "sizeof",
            "stackalloc",
            "static",
            "string",
            "struct",
            "switch",
            "this",
            "throw",
            "true",
            "try",
            "typeof",
            "uint",
            "ulong",
            "unchecked",
            "unsafe",
            "ushort",
            "using",
            "virtual",
            "void",
            "volatile",
            "while"
        };
        #endregion
        public Parsing(string path)
        {
            text = LoadText(path);   
        }
        string LoadText(string path)
        {
            try
            {
                path = File.ReadAllText(path);
                if (path == "")
                    Console.WriteLine("Файл пуст");
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException)
                    Console.WriteLine("Файл " + path + " не существует!");
                else if (ex is ArgumentException)
                    Console.WriteLine("Недопустимые символы в имени файла!");
                else if (ex is NotSupportedException)
                    Console.WriteLine("Неправильный формат пути к файлу!");
                else if (ex is UnauthorizedAccessException)
                    Console.WriteLine("Нет доступа к файлу!");
                else if (ex is DirectoryNotFoundException)
                    Console.WriteLine("Указанная директория не существует!");
                else if (ex is PathTooLongException)
                    Console.WriteLine("Превышена максимальная длина пути к файлу");
                else Console.WriteLine("Ошибка" + ex.GetType());
                Console.ReadKey();
                return "";
            }
            return path;
        }
        public Dictionary<string, int> GetKeyWords()
        {
            Dictionary<string, int> keyWord = new Dictionary<string, int>();
            if (string.IsNullOrEmpty(text)) return keyWord;
            text = DeleteComments(text);
            for (int i = 0; i < key.Length; i++)
            {
                string pattern;
                if (IgnoreKeywordsList)
                    pattern = $@"[^a-z""]{ key[i] }[^a-z0-9""]";
                else
                    pattern = $@"[^a-z]{ key[i] }[^a-z0-9]";
                Regex reg = new Regex(pattern);
                MatchCollection matches = reg.Matches(text);
                int count = matches.Count;
                //Просмотр самого первого и самого последнего слова
                Regex regStart = new Regex($@"^{ key[i] }[^a-z0-9""]");
                Regex regEnd = new Regex($@"[^a-z]{ key[i] }$");
                if (regStart.IsMatch(text) || regEnd.IsMatch(text))
                    count++;        
                if (count > 0)
                    keyWord.Add(key[i], count);
            }
            return keyWord;
        }
        string DeleteComments(string text)
        {
            text = Regex.Replace(text, @"/\*([^/\*]+)\*/", string.Empty);
            text = Regex.Replace(text, @"//([^\n]+)\n", string.Empty);
            return text;
        }
    }
}
