using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LicenceModule
{
    public static class ExtensionMethods
    {
        public static Tuple<int, int> CoordinatesOf<T>(this T[,] matrix, T value)
        {
            int w = matrix.GetLength(0); // width
            int h = matrix.GetLength(1); // height

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[x, y].Equals(value))
                        return Tuple.Create(x, y);
                }
            }

            return Tuple.Create(-1, -1);
        }

        private static char GetOriginalKey(int rand, char val)
        {
            char[,] randArr = generateRandomSecret(rand);
            var newXY = ExtensionMethods.CoordinatesOf(originalArr, val);
            var originalKey = randArr[newXY.Item1, newXY.Item2];
            return originalKey;
        }

        private static char GetAlternativeKey(char[,] arr, char[,] randArr, char key)
        {

            var originalXY = ExtensionMethods.CoordinatesOf(arr, key);
            var newKey = randArr[originalXY.Item1, originalXY.Item2];
            return newKey;
        }



        private static int GenerateRandom()
        {
            Random random = new Random();
            return random.Next(1, 4);
        }

        private static char[,] generateRandomSecret(int rand)
        {
            char[,] array2D = new char[4, 7];


            char[] array = getAlphabetArray();
            int[] indexOrder = getIndexOrder(rand);

            int setidx = 0;
            int idx = 0;

            bool done = false;

            while (!done)
            {
                foreach (var item in indexOrder)
                {
                    if (item == idx)
                    {
                        int i = item;
                        for (int j = 0; j < array2D.GetLength(1); j++)
                        {
                            array2D[Array.IndexOf(indexOrder, item), j] = array[setidx++];
                        }
                        idx++;
                        if (item == 3)
                        {
                            done = true;
                        }
                    }

                }
            }


            return array2D;
        }

        private static int[] getIndexOrder(int rand)
        {
            int[] indexOrder = new int[4];
            int x = rand - 1;
            for (int i = 0; i < 4; i++)
            {
                if (x < 4)
                {
                    indexOrder[x++] = i;
                }
                else
                {
                    indexOrder[x++ - 4] = i;
                }
            }

            Console.Write("Index Order Secret :");
            foreach (var item in indexOrder)
            {
                Console.Write(item.ToString());
            }
            Console.WriteLine();

            return indexOrder;
        }

        private static char[] getAlphabetArray()
        {
            char[] array = new char[28];

            int index = 0;
            for (char c = 'A'; c <= 'Z'; c++)
            {
                array[index] = c;
                index++;
            }

            for (int x = 0; index < array.Length; index++)
            {
                array[index] = Convert.ToChar(x.ToString());
                x++;
            }
            return array;
        }

        private static char[,] getGeneralSecret()
        {
            char[,] array2D = new char[4, 7];

            char[] array = getAlphabetArray();

            int setidx = 0;
            for (int i = 0; i < array2D.GetLength(0); i++)
            {
                for (int j = 0; j < array2D.GetLength(1); j++)
                {
                    array2D[i, j] = array[setidx++];
                }
            }
            return array2D;
        }
        private static char[,] getGeneralOppositeSecret()
        {
            char[,] array2D = new char[4, 7];

            char[] array = getAlphabetArray();

            int setidx = 0;
            for (int i = 0; i < array2D.GetLength(1); i++)
            {
                for (int j = 0; j < array2D.GetLength(0); j++)
                {
                    array2D[j, i] = array[setidx++];
                }
            }



            return array2D;
        }

        private static char[,] getCustomSecret()
        {
            char[,] array2D = new char[4, 7] {
            { 'Y' , 'D' , 'H' , 'T' , 'P' , 'B' , 'I' },
            { 'M' , '0' , 'O' , 'G' , 'E' , 'R' , '1' },
            { 'X' , 'U' , 'A' , 'S' , 'W' , 'K' , 'J' },
            { 'Q' , 'F' , 'L' , 'N' , 'C' , 'Z' , 'V' } };
            return array2D;
        }
        public static char[,] originalArr { get; set; }

        public static bool ValidateClient(int rand, char original, char random)
        {
            char[,] Arr = originalArr = getCustomSecret();
            char[,] randArr = generateRandomSecret(rand);
            return (original == GetOriginalKey(rand, random));
            
        }
    }
}
