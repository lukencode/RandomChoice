using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomChoice
{
    public class RandomSelector<T>
    {
        /// <summary>
        /// Add a new value to the selection
        /// </summary>
        /// <param name="weight">percentage chance to call this method</param>
        /// <param name="value">method to be called</param>
        public static RandomSelectorInstance<T> Add(double weight, T value)
        {
            var instance = new RandomSelectorInstance<T>();
            instance.Add(weight, value);

            return instance;
        }

        /// <summary>
        /// Add a new value to the selection
        /// </summary>
        /// <param name="value">method to be called</param>
        public static RandomSelectorInstance<T> Add(T value)
        {
            var instance = new RandomSelectorInstance<T>();
            instance.Add(value);

            return instance;
        }

        /// <summary>
        /// Sets the Random being used
        /// </summary>
        public static RandomSelectorInstance<T> UsingRandom(Random random)
        {
            var instance = new RandomSelectorInstance<T>();
            instance.UsingRandom(random);

            return instance;
        }
    }


    public class RandomSelectorInstance<T>
    {
        private Random _rand;
        private List<ProportionValue<T>> _values;
        private double _remainingProportion = 1.0;

        internal RandomSelectorInstance()
        {
            _rand = new Random();
            _values = new List<ProportionValue<T>>();
        }

        /// <summary>
        /// Sets the Random being used
        /// </summary>
        public RandomSelectorInstance<T> UsingRandom(Random random)
        {
            _rand = random;
            return this;
        }

        /// <summary>
        /// Add a new value to the selection
        /// </summary>
        /// <param name="weight">percentage return this value</param>
        /// <param name="value">method to be called</param>
        public RandomSelectorInstance<T> Add(double weight, T value)
        {
            _remainingProportion -= weight;

            if (_remainingProportion < 0)
                throw new InvalidOperationException("The proportions in the collection do not add up to 1.");

            _values.Add(new ProportionValue<T> { Proportion = weight, Value = value });

            return this;
        }

        /// <summary>
        /// Add a new value to the selection
        /// </summary>
        /// <param name="value">method to be called</param>
        public RandomSelectorInstance<T> Add(T value)
        {
            _values.Add(new ProportionValue<T> { Value = value });

            return this;
        }

        /// <summary>
        /// Chooses an value based on the weights provided
        /// </summary>
        public T Choose()
        {
            if (_values == null || !_values.Any())
                throw new InvalidOperationException("No values set.");

            balanceProportions();

            var rnd = _rand.NextDouble();

            foreach (var pa in _values)
            {
                if (rnd < pa.Proportion)
                {
                    return pa.Value;
                }

                rnd -= pa.Proportion.Value;
            }

            throw new InvalidOperationException("The proportions in the collection do not add up to 1.");
        }

        private void balanceProportions()
        {
            var unassignedActions = _values.Where(a => !a.Proportion.HasValue);

            if (unassignedActions.Any())
            {
                var calculatedProportion = _remainingProportion / (double)unassignedActions.Count();

                foreach (var a in unassignedActions)
                {
                    a.Proportion = calculatedProportion;
                }
            }
        }
    }
}
