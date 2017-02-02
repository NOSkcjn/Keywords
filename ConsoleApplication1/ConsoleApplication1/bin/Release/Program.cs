using System;
using System.Text;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        const bool IgnoreKeywordsList = true;//игнорировать ли список слов, расположенный ниже
        static string[] key =
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

        static int[] startComment = new int[2];
        static int[] endComment = new int[2];
        static string DeleteComments(string s)
        {
            /*int s1 = s.IndexOf("/*");
            int s2;
            if (s1 >= 0) s2 = s.IndexOf("**", s1);
            else s2 = -1;
            int s3 = s.IndexOf("//");
            int s4;
            if (s3 >= 0) s4 = s.IndexOf("\n", s3);
            else s4 = -1;
            while (true)
            {
                if (s1 >= 0 && s2 >= 0)
                {
                    s = s.Remove(s1, s2 - s1 + 2);
                    s1 = s.IndexOf("/*");
                    if(s1 >= 0) s2 = s.IndexOf("**", s1);
                }
                else if(s3 >= 0)
                {
                    //ищем и удаляем однострочные комментарии
                    if (s4 == -1 || s4 < s3) s4 = s.Length - 1;
                    s = s.Remove(s3, s4 - s3 + 1);
                    s3 = s.IndexOf("//");
                    if (s3 >= 0) s4 = s.IndexOf("\n", s3);
                }
                else break; //если комментариев не осталось - завершаем цикл
            }*/
            while(SearchComment(s, true))
            {
                s = s.Remove(startComment[1], endComment[1] - startComment[1] + 2);
            }
            return s;
        }
        static bool SearchComment(string s, bool type)
        {
            string start, end;
            byte typeV = Convert.ToByte(type);
            if (type)
            {
                start = "/*";
                end = "*/";
            }
            else
            {
                start = "//";
                end = "\n";
            }
            startComment[typeV] = s.IndexOf(start);
            if (startComment[typeV] >= 0)
            {
                endComment[typeV] = s.IndexOf(end, startComment[typeV]);
                return true;
            }
            else
            {
                endComment[typeV] = -1;
                return false;
            }
        }
        static int Main(string[] args)
        {     
            string str;
            int[] k = new int[key.Length];
            //открываем файл с текстом программы по умолчанию - Program.cs
            if (args.Length < 1)
            {
                if (File.Exists("Program.cs")) str = File.ReadAllText("Program.cs");
                else
                {
                    Console.WriteLine("Файл Program.cs не существует!");
                    Console.ReadKey();
                    return 0;
                }
            }
            //открываем заданный файл с текстом программы
            else
            {
                if (File.Exists(args[0])) str = File.ReadAllText(args[0]);
                else
                {
                    Console.WriteLine("Файл " + args[0] + " не существует!");
                    Console.ReadKey();
                    return 0;
                }
            }
            //сортировка по алфавиту
            Console.WriteLine("По алфавиту:");
            for (int i = 0; i < key.Length; i++)
            {
                for (int j = 0; j < key.Length - 1; j++)
                {
                    if (NeedChange(key[j], key[j + 1]))
                    {
                        //меняем слова местами
                        string s = key[j];
                        key[j] = key[j + 1];
                        key[j + 1] = s;
                    }
                }
            }
            int count = 0;
            str = DeleteComments(str);//удаляем комментарии из исходного текста
            for (int i = 0; i < key.Length; i++)//i - номер текущего ключевого слова
            {
                //ищем вхождение слова, начиная с первого символа текста
                int start = str.IndexOf(key[i], 0);
                while (start >= 0)
                {
                    //first - код символа, стоящего перед искомым словом, last - после
                    int first;
                    if (start == 0) first = -1;//если слово стоит в начале текста, значит перед ним нет других символов
                    else first = (int)str[start - 1];
                    int last;
                    if (start + key[i].Length == str.Length) last = -1;//если слово стоит в конце текста, значит после него нет других символов
                    else last = (int)str[start + key[i].Length];
                    //проверяем, является ли это слово частью другого слова; если нет, увеличиваем количество найденных слов
                    if (IsWord(last) && IsWord(first))
                    {
                        //если слово стоит в кавычках и включен флаг IgnoreKeywordsList - не считаем его
                        if (!(IgnoreKeywordsList == true && first == 34 && last == 34)) k[i]++;
                    }               
                    /*ищем вхождение слова, начиная с символа, стоящего в тексте после предыдущего найденного слова
                    если такого нет, цикл завершается*/
                    start = str.IndexOf(key[i], start+key[i].Length);
                }
                if (k[i] > 0) { Console.WriteLine("Слово " + key[i] + ": " + k[i]); count++; }
            }
            //сортировка по возрастанию
            for (int i = 0; i < key.Length - 1; i++)
            {
                for (int j = i + 1; j < key.Length; j++)
                {
                    if(k[i] > 0 && k[j] > 0)
                    {
                        if (k[i] > k[j])//если i-е слово встречается чаще чем j-e - меняем их местами
                        {
                            int temp = k[i];
                            k[i] = k[j];
                            k[j] = temp;
                            string s = key[i];
                            key[i] = key[j];
                            key[j] = s;
                        }
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("По возрастанию:");
            string[] TOP = new string[count];
            int t = 0;
            for (int i = 0; i < key.Length; i++)//i - номер текущего ключевого слова
            {
                //формируем массив только из тех слов, которые есть в тексте программы
                if (k[i] > 0)
                {
                    Console.WriteLine("Слово " + key[i] + ": " + k[i]);
                    TOP[t] = key[i];
                    t++;
                }
            }
            Console.WriteLine();
            //ТОП 5 часто встречающихся слов
            Console.WriteLine("ТОП 5:");
            //если слов меньше пяти, выводим их все, иначе - последние пять
            if (count < 5) for (int i = count - 1; i > -1; i--) Console.WriteLine(TOP[i]);
            else for (int i=count-1;i>count-6;i--) Console.WriteLine(TOP[i]);
            Console.ReadKey();
            return 1;
        }
        static bool NeedChange(string s1, string s2)//сравнивает два слова по буквам. возвращает true если их надо поменять местами
        {
            for (int i = 0; i < (s1.Length > s2.Length ? s2.Length : s1.Length); i++)//максимальное число итераций цикла = длине меньшего слова
            {
                if (s1.ToCharArray()[i] < s2.ToCharArray()[i]) return false;
                if (s1.ToCharArray()[i] > s2.ToCharArray()[i]) return true;
            }
            return false;
        }
        static bool IsWord(int c)//проверяет, является ли символ цифрой или буквой
        {
            if (!((c > 64) && (c < 91)) && !((c > 96) && (c < 123)) && !((c > 47) && (c < 58))) return true;
            else return false;
        }
    }
}