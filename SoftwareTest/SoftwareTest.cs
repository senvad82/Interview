using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareTest
{
    /**
     * Welcome to the Software Test. Please make sure you
     * read the instructions carefully.
     *
     * FAQ:
     * Can I use linq? Yes.
     * Can I cheat and look things up on Stack Overflow? Yes.
     * Can I use a database? No.
     */

    /// There are two challenges in this file
    /// The first one should takes ~10 mins with the
    /// second taking between ~30-40 mins.
    public interface IChallenge
    {
        /// Are you a winner?
        bool Winner();
    }

    /// Lets find out
    public class Program
    {
        /// <summary>
        /// Challenge Uno - NumberCalculator
        ///
        /// Fill out the TODOs with your own code and make any
        /// other appropriate improvements to this class.
        /// </summary>
        public class NumberCalculator : IChallenge
        {
            public int FindMax(int[] numbers)
            {
                // TODO: Find the highest number
                
                int max = 0;
                // we can also use return numbers.Max()
                for (int i=0; i<numbers.Length; i++)
                    if (numbers[i] > max) max = numbers[i];
                
                return max;
            }

            public int[] FindMax(int[] numbers, int n)
            {
                // TODO: Find the 'n' highest numbers
                var sortedArray = Sort(numbers);// we can do orderby descending as well i wanted to reuse our code.
                int len = numbers.Length;
                if (n > len) n = len; //Incase if n is greater than array size
                int[] maxArray = new int[n];
                int counter = 0;
                for (int i = len - 1; i > len-1-n; i--) { 
                    maxArray[counter] = numbers[i];
                    counter++;
                }
                return maxArray;
            }

            public int[] Sort(int[] numbers)
            {
                // TODO: Sort the numbers
                return numbers.OrderBy(i => i).ToArray();
                
            }

            public bool Winner()
            {
                var numbers = new[] { 5, 7, 5, 3, 6, 7, 9 };
                var sorted = Sort(numbers); 
                var maxes = FindMax(numbers, 2);
                var maxes2 = FindMax(numbers, 9);

                // TODO: Are the following test cases sufficient, to prove your code works
                // as expected? If not either write more test cases and/or describe what
                // other tests cases would be needed.

                return sorted.First() == 3
                       && sorted.Last() == 9
                       && FindMax(numbers) == 9
                       && maxes[0] == 9
                       && maxes[1] == 7;
            }
        }

        /// <summary>
        /// Challenge Due - Run Length Encoding
        ///
        /// RLE is a simple compression scheme that encodes runs of data into
        /// a single data value and a count. It's useful for data that has lots
        /// of contiguous values (for example it was used in fax machines), but
        /// also has lots of downsides.
        ///
        /// For example, aaaaaaabbbbccccddddd would be encoded as
        ///
        /// 7a4b4c5d
        ///
        /// You can find out more about RLE here...
        /// http://en.wikipedia.org/wiki/Run-length_encoding
        ///
        /// In this exercise you will need to write an RLE **Encoder** which will take
        /// a byte array and return an RLE encoded byte array.
        /// </summary>
        public class RunLengthEncodingChallenge : IChallenge
        {
            public byte[] Encode(byte[] original)
            {
                // TODO: Write your encoder here
                var len = original.Length;
                byte[] result = new byte[2 * len];
                byte prevByte = new byte();
                byte currentByte;
                int counter=1;
                int resultCounter = 0;
                for (int i = 0;i< len; i++)
                {
                    currentByte = original[i];
                    while(i+1 < len && original[i] == original[i + 1])
                    {
                        counter++;
                        i++;
                    }
                    result[resultCounter++] = Convert.ToByte(counter);
                    result[resultCounter++] = currentByte;
                    counter = 1;
                                        
                }
                Array.Resize<byte>(ref result, resultCounter); // resizing adds O(m) complexity as it copies reduced m size to new array
                return result;
            }

            public bool Winner()
            {
                // TODO: Are the following test cases sufficient, to prove your code works
                // as expected? If not either write more test cases and/or describe what
                // other tests cases would be needed.

                var testCases = new[]
                {
                    new Tuple<byte[], byte[]>(new byte[]{0x01, 0x02, 0x03, 0x04}, new byte[]{0x01, 0x01, 0x01, 0x02, 0x01, 0x03, 0x01, 0x04}),
                    new Tuple<byte[], byte[]>(new byte[]{0x01, 0x01, 0x01, 0x01}, new byte[]{0x04, 0x01}),
                    new Tuple<byte[], byte[]>(new byte[]{0x01, 0x01, 0x02, 0x02}, new byte[]{0x02, 0x01, 0x02, 0x02})
                };

                // TODO: What limitations does your algorithm have (if any)?
                // TODO: What do you think about the efficiency of this algorithm for encoding data?
                //-----------------------------------------
                // Efficiency of this algortithm is O(m+n) where m is size of compressed array n size of original array 
                //--------------------------------------------

                foreach (var testCase in testCases)
                {
                    var encoded = Encode(testCase.Item1);
                    var isCorrect = encoded.SequenceEqual(testCase.Item2);

                    if (!isCorrect)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public static void Main(string[] args)
        {
            var challenges = new IChallenge[]
            {
                new NumberCalculator(),
                new RunLengthEncodingChallenge()
            };

            foreach (var challenge in challenges)
            {
                var challengeName = challenge.GetType().Name;

                var result = challenge.Winner()
                    ? string.Format("You win at challenge {0}", challengeName)
                    : string.Format("You lose at challenge {0}", challengeName);

                Console.WriteLine(result);
            }

            Console.ReadLine();
        }
    }
}
