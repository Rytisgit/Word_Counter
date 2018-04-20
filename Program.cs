using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace ConsoleApp3
{
    class Program
    {
        static LinkedList<string> FillListWithWords(string input)
        {
            Regex rx = new Regex(@"[^\W\d][\w'-]*(?<=\w)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
            LinkedList<string> list = new LinkedList<string>();
            foreach (Match match in rx.Matches(input))
            {
                list.AddLast(match.Value.ToLower());
            }
            if (list.Count == 0) throw new Exception ("no words found");
            return list;
        }
        static bool CheckIfSameWord(LinkedListNode<string> first, string comparison)
        {
            return first.Value == comparison;
        }
        static bool CheckIfSameLength(LinkedListNode<string> first, string length)
        {
            return first.Value.Length == length.Length;
        }
        static int RemoveByPredicate(ref LinkedList<string> list, string comparison, Func<LinkedListNode<string>, string, bool> pred)
        {
            LinkedListNode<string> iterator = list.First;
            LinkedListNode<string> tempnode;
            int count = 1;
            while (iterator != null)
            {
                tempnode = iterator.Next;
                if (pred(iterator, comparison))
                {
                    list.Remove(iterator);
                    count++;
                }
                iterator = tempnode;
            }
            return count;
        }
        static List<KeyValuePair<string, int>> ListOfSameWordsAndCounts(LinkedList<string> list)
        {
            List<KeyValuePair<string, int>> listwords = new List<KeyValuePair<string, int>>();
            LinkedListNode<string> iterator = list.First;
            string comparison = list.First.Value;
            int count = 1;
            list.Remove(iterator);
            while (list.Count > 0)
            {
                listwords.Add(new KeyValuePair<string, int>(comparison, RemoveByPredicate(ref list, comparison, CheckIfSameWord)));
                if (list.Count > 0)
                {
                    iterator = list.First;
                    comparison = iterator.Value;
                    list.Remove(iterator);
                    count = 1;
                }
                else { count = 0; }
            }
            if (count > 0) listwords.Add(new KeyValuePair<string, int>(comparison, count));
            return listwords;
        }
        static List<KeyValuePair<int, int>> ListOfWordLengthsAndCounts(LinkedList<string> list)
        {
            List<KeyValuePair<int, int>> listlengths = new List<KeyValuePair<int, int>>();
            LinkedListNode<string> iterator = list.First;
            string comparison = list.First.Value;
            int count = 1;
            list.Remove(iterator);
            while (list.Count > 0)
            {
                listlengths.Add(new KeyValuePair<int, int>(comparison.Length, RemoveByPredicate(ref list, comparison, CheckIfSameLength)));
                if (list.Count > 0)
                {
                    iterator = list.First;
                    comparison = iterator.Value;
                    list.Remove(iterator);
                    count = 1;
                }
                else { count = 0; }
            }
            if (count > 0) listlengths.Add(new KeyValuePair<int, int>(comparison.Length, count));
            return listlengths;
        }
            static Tuple<List<KeyValuePair<string, int>>, List<KeyValuePair<int, int>>> WordCounter(string input)
        {
            LinkedList<string> list = FillListWithWords(input);
            LinkedList<string> listcopy = new LinkedList<string>();
            foreach (var item in list)
            {
                listcopy.AddLast(item);
            }
            List<KeyValuePair<string, int>> listwords = ListOfSameWordsAndCounts(list);
            List<KeyValuePair<int, int>> listlengths = ListOfWordLengthsAndCounts(listcopy);
            listlengths.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));
            return new Tuple<List<KeyValuePair<string, int>>, List<KeyValuePair<int, int>>>(listwords, listlengths);
        }

        static void Print(List<KeyValuePair<int, int>> list)
        {
            foreach (var item in list)
            {
                Console.Write("\"" + item.Key + ", " + item.Value + "\" ");
            }
            Console.WriteLine();
        }
        static void Print(List<KeyValuePair<string, int>> list)
        {
            foreach (var item in list)
            {
                Console.Write("\"" + item.Key + ", " + item.Value + "\" ");
            }
            Console.WriteLine();
        }


        static void Main(string[] args)
        {
#if false                  //file reading(change filename to a valid .txt file path to work)  or  manual input(enter into console)
            string filename = @"C:\Users\SuperPC\Desktop\text.txt";
            string newPath = Path.GetFullPath(filename);
            string input;
            using (StreamReader streamReader = new StreamReader(newPath, Encoding.UTF8))
            {
                input = streamReader.ReadToEnd();
            }
#else
            Console.WriteLine("Iveskite zodzius");
            string input = Console.ReadLine();
#endif
            Tuple<List<KeyValuePair<string, int>>, List<KeyValuePair<int, int>>> twolists = WordCounter(input);
            Print(twolists.Item1);//item 1 is same word repetition counter
            Print(twolists.Item2);//item 2 is same length repetition counter
            Console.Read();
        }
    }
}
