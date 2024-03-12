using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;

namespace Problem
{

    public class Problem : ProblemBase, IProblem
    {
        #region ProblemBase Methods
        public override string ProblemName { get { return "FasterSort"; } }

        public override void TryMyCode()
        {

            /* WRITE 4~6 DIFFERENT CASES FOR TRACE, EACH WITH
             * 1) SMALL INPUT SIZE
             * 2) EXPECTED OUTPUT
             * 3) RETURNED OUTPUT FROM THE FUNCTION
             * 4) PRINT THE CASE 
             */
    
            float[] output;
            {
                float[] arr = { 1, 8, -1 };
                float[] expected = {-1, 1, 8};
                output = PROBLEM_CLASS.RequiredFuntion((float[])arr.Clone(), arr.Length);
                PrintCase(arr, output, expected);
            }

            {
                float[] arr = { 1, 2, 3 , 4, 5, 6, 7};
                float[] expected = { 1, 2, 3, 4, 5, 6, 7 };
                output = PROBLEM_CLASS.RequiredFuntion((float[])arr.Clone(), arr.Length);
                PrintCase(arr, output, expected);
            }

            {
                float[] arr = { -1, -2, -3, -4, -5, -6, -7 };
                float[] expected = { -7, -6, -5, -4, -3, -2, -1};
                output = PROBLEM_CLASS.RequiredFuntion((float[])arr.Clone(), arr.Length);
                PrintCase(arr, output, expected);
            }

            {
                float[] arr = { 1, 1, 1, 1, 1 };
                float[] expected = { 1, 1, 1, 1, 1 };
                output = PROBLEM_CLASS.RequiredFuntion((float[])arr.Clone(), arr.Length);
                PrintCase(arr, output, expected);
            }

            {
                float[] arr = { 2.001f, 1.11f, 1.01f, 1.21f, 1.013f, 1.012f, 1.011f };
                float[] expected = { 1.01f, 1.011f, 1.012f, 1.013f, 1.11f, 1.21f, 2.001f};
                output = PROBLEM_CLASS.RequiredFuntion((float[])arr.Clone(), arr.Length);
                PrintCase(arr, output, expected);
            }
        }

        Thread tstCaseThr;
        bool caseTimedOut ;
        bool caseException;

        protected override void RunOnSpecificFile(string fileName, HardniessLevel level, int timeOutInMillisec)
        {
            /* READ THE TEST CASES FROM THE SPECIFIED FILE, FOR EACH CASE DO:
             * 1) READ ITS INPUT & EXPECTED OUTPUT
             * 2) READ ITS EXPECTED TIMEOUT LIMIT (IF ANY)
             * 3) CALL THE FUNCTION ON THE GIVEN INPUT USING THREAD WITH THE GIVEN TIMEOUT 
             * 4) CHECK THE OUTPUT WITH THE EXPECTED ONE
             */
            
            int testCases;
            int N = 0;
            float[] arr = null;
            float[] actualResult= null;
            float[] output=null;

            Stream s = new FileStream(fileName, FileMode.Open);
            BinaryReader br = new BinaryReader(s);

            testCases = br.ReadInt32();

            int totalCases = testCases;
            int correctCases = 0;
            int wrongCases = 0;
            int timeLimitCases = 0;

            float avgTime = 0;
            float maxTime = 0;
            int i = 1;
            while (testCases-- > 0)
            {
                N = br.ReadInt32();
                arr = new float[N];
                for (int j = 0; j < N; j++)
                {
                    arr[j] = br.ReadSingle();
                }

                //if (level == HardniessLevel.Easy)
                //{
                //    Console.WriteLine("Array: ");
                //    for (int j = 0; j < arr.Length; j++)
                //    {
                //        Console.Write(arr[j] + " ");
                //    }
                //    Console.WriteLine();
                //}
                actualResult = new float[N];
                arr.CopyTo(actualResult, 0);

                Stopwatch sw0 = Stopwatch.StartNew();
                Array.Sort(actualResult);
                sw0.Stop();
                timeOutInMillisec = (int)sw0.ElapsedMilliseconds;
                Stopwatch sw = null;

                caseTimedOut = true;
                caseException = false;
                {
                    tstCaseThr = new Thread(() =>
                    {
                        try
                        {
                            //int sum = 0;
                            int numOfRep = 1;
                            sw = Stopwatch.StartNew();
                            for (int x = 0; x < numOfRep; x++)
                            {
                                output = PROBLEM_CLASS.RequiredFuntion(arr, N);
                           
                            }
                            //output = sum / numOfRep;
                            sw.Stop();

                           // Console.WriteLine("n = {0}, time in ms = {1}, time limit in ms = {2}", arr.Length, sw.ElapsedMilliseconds, timeOutInMillisec);
                        }
                        catch
                        {
                            caseException = true;
                            output = null;
                            
                        }
                        caseTimedOut = false;
                    });

                    //if (readTimeFromFile)
                    //{
                    //    timeOutInMillisec = br.ReadInt32();
                    //}
                    /*LARGE TIMEOUT FOR SAMPLE CASES TO ENSURE CORRECTNESS ONLY*/
                   
                    if (level == HardniessLevel.Easy )
                    {
                        timeOutInMillisec = 1000; //Large Value 
                    }
                    /*=========================================================*/
                    tstCaseThr.Start();
                    tstCaseThr.Join(timeOutInMillisec);
                }

                Console.WriteLine("Test Case {0}: timeOutInMillisec = {1}", i, timeOutInMillisec);
                if (caseTimedOut)       //Timedout
                {
                    Console.WriteLine("Time Limit Exceeded in Case {0}.", i);
                    tstCaseThr.Abort();
                    timeLimitCases++;
                }
                else if (caseException) //Exception 
                {
                    Console.WriteLine("Exception in Case {0}.", i);
                    wrongCases++;
                }
                
                else if(output.Length ==0 && actualResult.Length == 0)
                {
                    Console.WriteLine("Test Case {0} Passed!", i);
                    //Console.WriteLine(" your answer = " + output + ", correct answer = " + actualResult);
                    correctCases++;
                }
                else if (output.Length != 0 && actualResult.Length != 0)   //Passed
                {    
                    if (output.SequenceEqual(actualResult))
                    {
                        Console.WriteLine("Test Case {0} Passed!", i);
                        correctCases++;
                        if (sw.ElapsedMilliseconds > maxTime)
                            maxTime = sw.ElapsedMilliseconds;

                        avgTime += sw.ElapsedMilliseconds;
                    }
                    else
                    {
                        Console.WriteLine("Wrong Answer in Case {0}.", i);
                        wrongCases++;
                    }
                }
                else                    //WrongAnswer
                {
                    Console.WriteLine("Wrong Answer in Case {0}.", i);
                    wrongCases++;
                }

                i++;
            }
            s.Close();
            br.Close();
            Console.WriteLine();
            Console.WriteLine("# correct = {0}", correctCases);
            Console.WriteLine("# time limit = {0}", timeLimitCases);
            Console.WriteLine("# wrong = {0}", wrongCases);
            Console.WriteLine("\nFINAL EVALUATION (%) = {0}", Math.Round((float)correctCases / totalCases * 100, 0));
            Console.WriteLine("MAX TIME (ms) = {0}, AVG TIME (ms) = {1}", maxTime, Math.Round(avgTime / correctCases, 2));

        }

        protected override void OnTimeOut(DateTime signalTime)
        {
        }

        /// <summary>
        /// Generate a file of test cases according to the specified params
        /// </summary>
        /// <param name="level">Easy or Hard</param>
        /// <param name="numOfCases">Required number of cases</param>
        /// <param name="includeTimeInFile">specify whether to include the expected time for each case in the file or not</param>
        /// <param name="timeFactor">factor to be multiplied by the actual time</param>
        public override void GenerateTestCases(HardniessLevel level, int numOfCases, bool includeTimeInFile = false, float timeFactor = 1)
        {
           throw new NotImplementedException();
        }

        #endregion

        #region Helper Methods
        private static void PrintCase(float[] arr1, float [] output, float [] expected)
        {
            /* PRINT THE FOLLOWING
             * 1) INPUT
             * 2) EXPECTED OUTPUT
             * 3) RETURNED OUTPUT
             * 4) WHETHER IT'S CORRECT OR WRONG
             * */
            Console.WriteLine("Array: ");
            for(int i=0; i<arr1.Length; i++)
            {
                Console.Write(arr1[i]+" ");
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Expected Output: ");
            for (int i = 0; i < expected.Length; i++)
            {
                Console.Write(expected[i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Returned Output: ");
            for (int i = 0; i < output.Length; i++)
            {
                Console.Write(output[i] + " ");
            }
            Console.WriteLine();
            if (output.SequenceEqual(expected))
            {
                Console.WriteLine("Correct!!");
            }
            else
            {
                Console.WriteLine("Wrong Answer");
            }
            Console.WriteLine("-----------------------------");

        }

        #endregion

    }
}
