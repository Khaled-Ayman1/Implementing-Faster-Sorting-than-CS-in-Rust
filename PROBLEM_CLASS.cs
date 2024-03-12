using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class PROBLEM_CLASS
    {
        #region YOUR CODE IS HERE 

        //Your Code is Here:
        //==================
        /// <summary>
        /// Sort the given array in ascending order
        /// At least, should beat the default sorting algorithm of the C# (Array.Sort())
        /// </summary>
        /// <param name="arr"> array to be sorted in ascending order </param>
        /// <param name="N"> array size </param>
        /// <returns> sorted array </returns>

        static public float[] RequiredFuntion(float[] arr, int N)
        {
            QuickSort(arr, 0, N - 1, 200);
            return arr;
        }

        static void QuickSort(float[] arr, int start, int end, int threshold)
        {
            if (start < end)
            {
                if ((end - start) + 1 <= threshold)
                {
                    InsertionSort(arr, start, end);
                }
                else
                {                              
                    int pivotIndex = Divide(arr, start, end);

                    Parallel.Invoke(
                    () => QuickSort(arr, start, pivotIndex - 1, threshold),
                    () => QuickSort(arr, pivotIndex + 1, end, threshold)
                    );
               }
            }
        }
       
        static int Divide(float[] arr, int start, int end)
        {
            float pivot = arr[end];
            int swapOut = start - 1;

            for (int left = start; left < end; left++)
            {
                if (arr[left] <= pivot)
                {
                    swapOut++;
                    Swap(arr, left, swapOut);
                }
            }

            Swap(arr, swapOut + 1, end);
            return swapOut + 1;
        }

        static void InsertionSort(float[] arr, int start, int end)
        {
            for (int i = start + 1; i <= end; i++)
            {
                float index = arr[i];
                int j = i - 1;

                while (j >= start && arr[j] > index)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }

                arr[j + 1] = index;
            }
        }

        static void Swap(float[] array, int i, int j)
        {
            float temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        #endregion
    }
}

