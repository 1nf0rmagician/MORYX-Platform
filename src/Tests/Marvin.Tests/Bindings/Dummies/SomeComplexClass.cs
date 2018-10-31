﻿using Marvin.Bindings;

namespace Marvin.Tests.Bindings
{
    public class SomeComplexClass
    {
        public int Value { get; set; }
        public SomeComplexClass SubClass { get; set; }
        public SomeComplexClass OtherSubClass { get; set; }
    }

    internal class SomeComplexClassResolver : BindingResolverBase
    {
        protected override object Resolve(object source)
        {
            var someComplexClass = source as SomeComplexClass;
            return someComplexClass?.OtherSubClass;
        }

        protected override bool Update(object source, object value)
        {
            throw new System.NotImplementedException();
        }
    }
}
