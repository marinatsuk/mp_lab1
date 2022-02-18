using System;
using System.IO;

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            int pageLines = 45;

            int[] counter = new int[0];
            string[] words = new string[0];
            int[][] pages = new int[0][];

            int currentPage = 1;
            int size = 0;
            int i, j;
            int strCount = 0;
            string word = "";

            StreamReader reader = new StreamReader("C:/Users/serg/Desktop/CHAT/task2input.txt");
            //"цикл" зчитування файлу
            StartFileReader:
            {
                if (reader.EndOfStream)
                    goto StopFileReader;

                string line = reader.ReadLine();
                //Console.WriteLine(line);

                if (strCount == pageLines)
                {
                    currentPage++;
                    strCount = 0;
                }
                strCount++;
                j = 0;

                StartSymbolReader:
                {
                    if (j == line.Length)
                        goto EndSymbolReader;

                    char symbol = line[j];

                    if ((char) 97 <= symbol && symbol <= (char) 122) // a <= symbol <= z
                    {
                        word += symbol;
                        if (j + 1 < line.Length)
                            goto EndSymbolReader;
                    }
                    else if ((char) 65 <= symbol && symbol <= (char) 90) // A <= symbol <= Z
                    {
                        word += ((char) (symbol + 32)).ToString();
                        if (j + 1 < line.Length)
                            goto EndSymbolReader;
                    }

                    if (word != "" && symbol != '\'')
                    {
                        i = 0;
                        //перевірка чи це нове слово
                        checkWord:
                        {
                            if (i == size)
                                goto InsertWord;
                            if (word == words[i])
                            {
                                word = "";
                                if (counter[i] > 100)
                                {
                                    goto EndSymbolReader;
                                }
                                counter[i]++;
                                if (counter[i] <= pages[i].Length)
                                {
                                    pages[i][counter[i] - 1] = currentPage;
                                }
                                else
                                {
                                    //збільшення масиву pages
                                    int[] pagesTmp = new int[counter[i] * 2];
                                    int p = 0;
                                    copyPages:
                                    {
                                        pagesTmp[p] = pages[i][p];
                                        p++;
                                        if (p < counter[i] - 1)
                                            goto copyPages;
                                    }
                                    pages[i] = pagesTmp;
                                    pages[i][counter[i] - 1] = currentPage;
                                }

                                goto EndSymbolReader;
                            }

                            i++;
                            goto checkWord;
                        }

                        //"цикл" додавання нового слова
                        InsertWord:

                        if (size == words.Length)
                        {
                            string[] newWords = new string[(size + 1) * 2];
                            int[] newCounter = new int[(size + 1) * 2];
                            int[][] newPages = new int[(size + 1) * 2][];

                            i = 0;
                            //"цикл" для дублювання масиву
                            copyArray:
                            {
                                if (i == size)
                                {
                                    words = newWords;
                                    counter = newCounter;
                                    pages = newPages;
                                    goto stopCopyArray;
                                }

                                newWords[i] = words[i];
                                newCounter[i] = counter[i];
                                newPages[i] = pages[i];
                                i++;
                                goto copyArray;
                            }
                        }

                        stopCopyArray:
                        words[size] = word;
                        counter[size] = 1;
                        pages[size] = new int[] {currentPage};
                        size++;
                        word = "";
                    }

                    EndSymbolReader:
                    j++;
                    if (j < line.Length)
                        goto StartSymbolReader;
                }

                if (!reader.EndOfStream)
                    goto StartFileReader;
            }

            StopFileReader:
            reader.Close();
            
            //сортування
            int currentNumber;
            int sorter;
            int[] currPages;
            i = 1;
            StartSort:
            {
                currentNumber = counter[i];
                word = words[i];
                currPages = pages[i];
                sorter = i - 1;
                whileSort:
                {
                    if (sorter >= 0)
                    {
                        int currentSymbol = 0;
                        //сортування слів за алфавітом
                        compWords: 
                        {
                            if (currentSymbol == words[sorter].Length || words[sorter][currentSymbol] < word[currentSymbol])
                                goto endWhile;

                            if (currentSymbol + 1 < word.Length && words[sorter][currentSymbol] == word[currentSymbol])
                            {
                                currentSymbol++;
                                goto compWords;
                            }
                        }
                        
                        counter[sorter + 1] = counter[sorter];
                        words[sorter + 1] = words[sorter];
                        pages[sorter + 1] = pages[sorter];
                        sorter--;
                        goto whileSort;
                    }
                }
                endWhile:
                counter[sorter + 1] = currentNumber;
                words[sorter + 1] = word;
                pages[sorter + 1] = currPages;
                i++;
                if (i < size)
                    goto StartSort;
            }

            //вивід результату
            i = 0;
            EndWrite:
            {
                if (counter[i] <= 100) { 
                    Console.Write(words[i] + " - " + pages[i][0]);
                    j = 1;
                    WritePages:
                    {
                        if (j == counter[i])
                            goto EndWritePages;
                        
                        if(pages[i][j] != pages[i][j - 1]) 
                            Console.Write(", "+pages[i][j]);
                        j++;
                        goto WritePages;
                    }
                    EndWritePages:
                    Console.WriteLine();
                }
                i++;
                if (i < size)
                    goto EndWrite;
            }
        }
    }
}
