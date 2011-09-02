using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomChoice
{
    internal class ProportionValue<T>
    {
        public double? Proportion { get; set; }
        public T Value { get; set; }
    }
}
