using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Sorting
    {
        //сортировка по значению
        public Dictionary<string, int> GetSortValues(Dictionary<string, int> keyWord)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            
            List<int> intToSort = new List<int>();
            foreach (var item in keyWord)
            {
                intToSort.Add(item.Value);
            }
            intToSort.Sort();

            foreach (var itemList in intToSort)
            {
                KeyValuePair<string, int> keyValuePair = new KeyValuePair<string, int>();
                foreach (var itemDict in keyWord)
                {
                    if (itemDict.Value == itemList)
                    {
                        keyValuePair = itemDict;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(keyValuePair.Key))
                    break;
                result.Add(keyValuePair.Key, keyValuePair.Value);
                keyWord.Remove(keyValuePair.Key);
            }

            keyWord = result;
            return keyWord;
        }
    }
}
