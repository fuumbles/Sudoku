using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuBiz
{
    static class ShuffleList
    {
        //used for shuffling the list of puzzle IDs for a random puzzle every time the button is clicked
        private static Random rng = new Random();  

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    
    }
    
}
