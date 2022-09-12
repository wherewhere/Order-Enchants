﻿using System;
using EnchantsOrder.Common;

namespace EnchantsOrder.Models
{
    public class Enchantment : IComparable<Enchantment>
    {
        public string Name { get; set; }

        public int Level { get; set; }
        public int Weight { get; set; }
        public long Experience => (long)Level * Weight;

        /// <summary>
        /// Initializes a new instance of <see cref="Enchantment" />.
        /// </summary>
        public Enchantment(string name, int level, int weight)
        {
            Name = name;
            Level = level;
            Weight = weight;
        }

        /// <inheritdoc/>
        public override string ToString() => $"{Name} {Level.GetLoumaNumber()}";

        /// <inheritdoc/>
        public int CompareTo(Enchantment other)
        {
            int value = Experience.CompareTo(other.Experience);
            if (value == 0)
            {
                value = Level.CompareTo(other.Level);
                if (value == 0)
                {
                    value = Name.CompareTo(other.Name);
                }
            }
            return value;
        }
    }
}
