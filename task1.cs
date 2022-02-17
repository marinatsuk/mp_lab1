using System;
using System.IO;

namespace MP_Lab1
{
    class task1
    {
        public static void Main(string[] args)
        {
	        //ініціалізація змінних
	        String[] stopWords = {"a", "an", "the", "on", "in", "at", "for", "after", 
									"me", "my", "he", "his", "she", "her", "it", "its",
									"you", "your", "they", "their", "we", "our", 
									"out",  "up", "neither", "nor", "either", "or", "to", "etc" };
            
            int mostFrequentWord = 25;
            String[] words = new string[0];
            int[] counter = new int[0];
            String word = "";
            int length = 0, i;

           
            StreamReader reader = new StreamReader("C:/Users/serg/Desktop/CHAT/task1input.txt"); // ініціалізація документу з якого зчитуються данні
            //"цикл" для зчитування файлу
            StartReading:
            {
                if (reader.EndOfStream)
                    goto endReading;

                char symbol = (char)reader.Read();
                
                if ((char) 97 <= symbol && symbol <= (char) 122)   // a <= symbol <= z
                {
                    word += symbol;
                    if (!reader.EndOfStream)
                        goto StartReading;
                } else if ((char) 65 <= symbol && symbol <= (char) 90)  // A <= symbol <= Z
                {
                    word += ((char)(symbol + 32)).ToString();  //перетворення в нижній регістр
                    if (!reader.EndOfStream)
                        goto StartReading;
                }

                if (word != "" && symbol!='\'')
                {
                    i = 0;
                    //"цикл", що перевіряє чи це стоп-слово
                    checkStopWord: 
                    {
                        if (word == stopWords[i])
                        {
                            word = "";
                            if (reader.EndOfStream)
                                goto endReading;
                            goto StartReading;
                        }
                        i++;
                        if (i < stopWords.Length)
                            goto checkStopWord;
                    }
                    i = 0;
                    
                    //"цикл", що перевіряє чи зустрічалось це слово раніше
                    checkWord: 
                    {
                        if (i == length)
                            goto InsertWord;
                        if (word == words[i])
                        {
                            counter[i]++;
                            word = "";
                            if (reader.EndOfStream)
                                goto endReading;
                            goto StartReading;
                        }
                        i++;
                        goto checkWord;
                    }
                    
                    //"цикл" для додавання нового слова
                    InsertWord:
                    if (length == words.Length)
                    {
                        string[] newWords = new string[(length + 1) * 2];
                        int[] newCounts = new int[(length + 1) * 2];

                        i = 0;
                        //"цикл" для дублювання масиву
                        copyArray:
                        {
                            if (i == length)
                            {
                                words = newWords;
                                counter = newCounts;
                                goto stopCopyArray;
                            }
                            newWords[i] = words[i];
                            newCounts[i] = counter[i];
                            i++;
                            goto copyArray;
                        }
                    }
                    stopCopyArray:
                    words[length] = word;
                    counter[length] = 1;
                    word = "";
                    length++;
                }
                if(!reader.EndOfStream)
                    goto StartReading;
            }

            endReading:
            reader.Close();
            
            //bubble sort
            int tempNum;
            string tempWord;
            int write = 0;
            int size = counter.Length;
            
            OuterSorter:
            write++;
            int sort = 0;
            
            InnerSorter:
            if (counter[sort] < counter[sort + 1])
            {
                tempNum = counter[sort + 1];
                counter[sort + 1] = counter[sort];
                counter[sort] = tempNum;

                tempWord = words[sort + 1];
                words[sort + 1] = words[sort];
                words[sort] = tempWord;

            }
            sort++;
            if (sort < size - 1)
                goto InnerSorter;
            
            if (write < size)
                goto OuterSorter;
            
            i = 0;
            
            endWrite:
            {
                Console.WriteLine(words[i] + " - " + counter[i]);
                i++;
                if (i < mostFrequentWord && i < length)
                    goto endWrite;
            }
        }
    }
}
