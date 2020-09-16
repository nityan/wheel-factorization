/*
 * Copyright 2020-2020 Nityan Khanna
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: Nityan Khanna
 * Date: 2020-9-15
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace WheelFactorization
{
	/// <summary>
	/// Determines the factors of a given number.
	/// </summary>
	public class Program
	{
		private static readonly Dictionary<BigInteger, bool> numbers = new Dictionary<BigInteger, bool>();

		/// <summary>
		/// Defines the entry point of the application.
		/// </summary>
		/// <param name="args">The arguments.</param>
		private static void Main(string[] args)
		{
			Console.WriteLine($"Enter a number to determine it's factors: {Environment.NewLine}");

			bool result;
			BigInteger number;

			do
			{
				var value = Console.ReadLine();
				result = BigInteger.TryParse(value, out number);

				if (!result)
				{
					Console.WriteLine($"{value} is not a valid number");
				}

			} while (!result);

			RunWheelFactorization(number);

			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}

		private static IEnumerable<BigInteger> SieveOfEratosthenes(int number)
		{
			for (var i = 2; i < number; i++)
			{
				if (i % 2 != 0)
				{
					numbers.Add(new BigInteger(i), true);
				}
			}

			for (var i = 2; i < (int)Math.Sqrt(number); i++)
			{
				if (i % 2 == 0)
				{
					continue;
				}

				if (numbers[i])
				{
					for (int k = 1, j = (int)Math.Pow(i, 2); j < number; j = (int)Math.Pow(i, 2) + i * k, k++)
					{
						numbers[j] = false;
					}
				}
			}

			return numbers.Where(c => c.Value).Select(c => c.Key);
		}

		/// <summary>
		/// Runs the wheel factorization.
		/// </summary>
		/// <param name="number">The number.</param>
		private static void RunWheelFactorization(BigInteger number)
		{
			var stopwatch = new Stopwatch();

			Console.ForegroundColor = ConsoleColor.Cyan;

			stopwatch.Start();

			Console.WriteLine("Factoring in progress");
			var results = CalculateFactorsWheelFactorization(number);

			stopwatch.Stop();

			Console.WriteLine("Using Wheel Factorization");
			Console.WriteLine($"Factors of {number} are {string.Join(" ", results)}");
			Console.WriteLine($"{stopwatch.Elapsed.Hours}:{stopwatch.Elapsed.Minutes}:{stopwatch.Elapsed.Seconds}:{stopwatch.Elapsed.Milliseconds}");

			Console.ResetColor();
		}

		/// <summary>
		/// Calculates the factors of a given number.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <returns>Returns the factors of a given number.</returns>
		private static IEnumerable<BigInteger> CalculateFactorsWheelFactorization(BigInteger number)
		{
			var results = new List<BigInteger>();

			while (number % 2 == 0)
			{
				results.Add(2);
				number /= 2;
			}

			while (number % 3 == 0)
			{
				results.Add(3);
				number /= 3;
			}

			while (number % 5 == 0)
			{
				results.Add(5);
				number /= 5;
			}

			BigInteger k = 7;
			uint i = 1;
			var increments = new[] { new BigInteger(4), new BigInteger(2), new BigInteger(4), new BigInteger(2), new BigInteger(4), new BigInteger(6), new BigInteger(2), new BigInteger(6) };

			while (k * k <= number)
			{
				if (number % k == 0)
				{
					results.Add(k);
					number /= k;
				}
				else
				{
					k += increments[i - 1];

					if (i < 8)
					{
						i++;
					}
					else
					{
						i = 1;
					}
				}
			}

			results.Add(number);

			return results;
		}
	}
}
