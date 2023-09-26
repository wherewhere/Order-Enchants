﻿using System.Collections.Generic;
using System.Text;

#if WINRT
using Windows.Foundation;
#endif

namespace EnchantsOrder.Models
{
    /// <summary>
    /// The result of ordering.
    /// </summary>
    /// <param name="steps">The steps of enchant.</param>
    /// <param name="penalty">The penalty of item.</param>
    /// <param name="maxExperience">The max experience level request during enchant.</param>
    /// <param name="totalExperience">The total experience level request during enchant.</param>
    public
#if WINRT
        sealed
#endif
        class OrderingResults(IList<IEnchantmentStep> steps, int penalty, double maxExperience, double totalExperience) : IOrderingResults
#if WINRT
        , IStringable
#endif
    {
        /// <summary>
        /// Get or set the penalty of item.
        /// </summary>
        public int Penalty { get; set; } = penalty;

        /// <summary>
        /// Get or set the max experience level request during enchant.
        /// </summary>
        public double MaxExperience { get; set; } = maxExperience;

        /// <summary>
        /// Get or set the total experience level request during enchant.
        /// </summary>
        public double TotalExperience { get; set; } = totalExperience;

        /// <summary>
        /// Get or set the steps of enchant.
        /// </summary>
        public IList<IEnchantmentStep> Steps { get; set; } = steps;

        /// <summary>
        /// Get the status whether it is too expensive.
        /// </summary>
        /// <remarks>Too expensive because max experience level max than 39.</remarks>
        public bool IsTooExpensive => MaxExperience > max_experience;

        /// <summary>
        /// Get the status whether it is too many penalty.
        /// </summary>
        /// <remarks>Max penalty max than 6 so that you cannot enchant any more.</remarks>
        public bool IsTooManyPenalty => Penalty > max_penalty;

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder builder = new();
            foreach (IEnchantmentStep step in Steps)
            {
                _ = builder.AppendLine(step.ToString());
            }
            return builder.AppendLine($"Penalty Level: {Penalty}")
                .AppendLine($"Max Experience Level: {MaxExperience}")
                .Append($"Total Experience Level: {TotalExperience}")
                .ToString();
        }

        /// <inheritdoc/>
        public int CompareTo(IOrderingResults other)
        {
            int value = Penalty.CompareTo(other.Penalty);
            if (value == 0)
            {
                value = TotalExperience.CompareTo(other.TotalExperience);
                if (value == 0)
                {
                    value = MaxExperience.CompareTo(other.MaxExperience);
                    if (value == 0)
                    {
                        static int GetStepNum(IList<IEnchantmentStep> steps)
                        {
                            int num = 0;
                            foreach (IEnchantmentStep step in steps)
                            {
                                num += step.Count;
                            }
                            return num;
                        }
                        value = GetStepNum(Steps).CompareTo(GetStepNum(other.Steps));
                    }
                }
            }
            return value;
        }

#if !WINRT
        /// <inheritdoc/>
        public static bool operator >(OrderingResults left, OrderingResults right) => left.CompareTo(right) == 1;

        /// <inheritdoc/>
        public static bool operator >=(OrderingResults left, OrderingResults right) => left.CompareTo(right) != -1;

        /// <inheritdoc/>
        public static bool operator <(OrderingResults left, OrderingResults right) => left.CompareTo(right) == -1;

        /// <inheritdoc/>
        public static bool operator <=(OrderingResults left, OrderingResults right) => left.CompareTo(right) != 1;
#endif

        internal const short max_penalty = 6;
        internal const short max_experience = 39;
    }
}
