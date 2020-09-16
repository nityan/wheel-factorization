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

namespace FactoringAlgorithm
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
				result = BigInteger.TryParse("3233", out number);
			} while (!result);

			var sieveResults = SieveOfEratosthenes(8191);

			var number2 = BigInteger.Multiply(new BigInteger(8191), new BigInteger(int.MaxValue));
			var number3 = BigInteger.Multiply(new BigInteger(6700417), new BigInteger(67280421310721));
			var number4 = BigInteger.Multiply(new BigInteger(524287), BigInteger.Pow(2, 17) - 1);
			var number5 = BigInteger.Multiply(BigInteger.Parse("20988936657440586486151264256610222593863921"), BigInteger.Parse("170141183460469231731687303715884105727"));

			RunTrialDivision(number);
			RunWheelFactorization(number);

			RunTrialDivision(number2);
			RunWheelFactorization(number2);

			RunTrialDivision(number3);
			RunWheelFactorization(number3);

			RunTrialDivision(number4);
			RunWheelFactorization(number4);

			RunWheelFactorization(number5);
			RunTrialDivision(number5);
			

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

		private static void RunTrialDivision(BigInteger number)
		{
			var stopwatch = new Stopwatch();

			stopwatch.Start();

			var results = CalculateFactorsTrialDivision(number);

			stopwatch.Stop();

			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine("Using Trial Division");
			Console.WriteLine($"Factors of {number} are {string.Join(" ", results)}");
			Console.WriteLine($"{stopwatch.Elapsed.Hours}:{stopwatch.Elapsed.Minutes}:{stopwatch.Elapsed.Seconds}:{stopwatch.Elapsed.Milliseconds}");
			Console.ResetColor();
		}

		private static void RunWheelFactorization(BigInteger number)
		{
			var stopwatch = new Stopwatch();

			stopwatch.Start();

			var results = CalculateFactorsWheelFactorization(number);

			stopwatch.Stop();

			Console.ForegroundColor = ConsoleColor.Cyan;
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
		private static IEnumerable<BigInteger> CalculateFactorsTrialDivision(BigInteger number)
		{
			var results = new List<BigInteger>();

			BigInteger potentialFactor = 3;

			while (potentialFactor * potentialFactor <= number)
			{
				potentialFactor += 2;

				if (number % (potentialFactor - 2) == BigInteger.Zero)
				{
					potentialFactor -= 2;
					results.Add(potentialFactor);
					number /= potentialFactor;
				}
			}

			results.Add(number);

			return results;
		}

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
