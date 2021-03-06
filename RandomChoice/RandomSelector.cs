﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomChoice
{
    public class RandomSelector
    {
        /// <summary>
        /// Add a new action to the selection
        /// </summary>
        /// <param name="weight">percentage chance to call this method</param>
        /// <param name="action">method to be called</param>
        public static RandomSelectorInstance Add(double weight, Action action)
        {
            var instance = new RandomSelectorInstance();
            instance.Add(weight, action);

            return instance;
        }

        /// <summary>
        /// Add a new action to the selection
        /// </summary>
        /// <param name="action">method to be called</param>
        public static RandomSelectorInstance Add(Action action)
        {
            var instance = new RandomSelectorInstance();
            instance.Add(action);

            return instance;
        }

        /// <summary>
        /// Sets the Random being used
        /// </summary>
        public static RandomSelectorInstance UsingRandom(Random random)
        {
            var instance = new RandomSelectorInstance();
            instance.UsingRandom(random);

            return instance;
        }
    }

    public class RandomSelectorInstance
    {
        private Random _rand;
        private List<ProportionValue<Action>> _actions;
        private double _remainingProportion = 1.0;

        internal RandomSelectorInstance()
        {
            _rand = new Random();
            _actions = new List<ProportionValue<Action>>();
        }

        /// <summary>
        /// Sets the Random being used
        /// </summary>
        public RandomSelectorInstance UsingRandom(Random random)
        {
            _rand = random;
            return this;
        }

        /// <summary>
        /// Add a new action to the selection
        /// </summary>
        /// <param name="weight">percentage chance to call this method</param>
        /// <param name="action">method to be called</param>
        public RandomSelectorInstance Add(double weight, Action action)
        {
            _remainingProportion -= weight;

            if (_remainingProportion < 0)
                throw new InvalidOperationException("The proportions in the collection do not add up to 1.");

            _actions.Add(new ProportionValue<Action> { Proportion = weight, Value = action });

            return this;
        }

        /// <summary>
        /// Add a new action to the selection
        /// </summary>
        /// <param name="action">method to be called</param>
        public RandomSelectorInstance Add(Action action)
        {
            _actions.Add(new ProportionValue<Action> { Value = action });

            return this;
        }

        /// <summary>
        /// Chooses an action based on the weights provided
        /// </summary>
        public void Choose()
        {
            if (_actions == null || !_actions.Any())
                throw new InvalidOperationException("No Actions set.");

            balanceProportions();

            var rnd = _rand.NextDouble();

            foreach (var pa in _actions)
            {
                if (rnd < pa.Proportion)
                {
                    pa.Value();
                    return;
                }

                rnd -= pa.Proportion.Value;
            }

            throw new InvalidOperationException("The proportions in the collection do not add up to 1.");
        }

        private void balanceProportions()
        {
            var unassignedActions = _actions.Where(a => !a.Proportion.HasValue);

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
