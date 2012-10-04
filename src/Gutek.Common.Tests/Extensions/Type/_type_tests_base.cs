using System;

namespace Gutek.Common.Tests.Extensions.type
{
    public class type_tests_base
    {
        protected readonly Type IFAKE_TYPE = typeof(IFake);
        protected readonly Type IFAKE2_TYPE = typeof(IFake2);
        protected readonly Type IIHERITES_IFAKE_TYPE = typeof(IInheritesIFake);
        protected readonly Type FAKE_STRUCT_TYPE = typeof(FakeStruct);
        protected readonly Type FAKING_CLASS__TYPE = typeof(FakingClass);
        protected readonly Type FAKE_CLASS__TYPE = typeof(FakeClass);
        protected readonly Type FAKE_INHERITES_FAKING_CLASS_TYPE = typeof(FakeInheritsFakingClass);
        protected readonly Type FAKE_INHERITES_FAKE_CLASS_TYPE = typeof(FakeInheritsFakeClass);
        protected readonly Type FAKE_INHERITES_IINHERITES_IFAKE_TYPE = typeof(FakeInheritsIInheritesIFake);
        protected readonly Type NULL_TYPE = null;

        public interface IFake { }
        public interface IFake2 { }
        public interface IInheritesIFake : IFake { }

        public struct FakeStruct { }
        public class FakingClass { }
        public class FakeClass : IFake { }
        public class FakeInheritsFakingClass : FakingClass { }
        public class FakeInheritsFakeClass : FakeClass { }
        public class FakeInheritsIInheritesIFake : IInheritesIFake { }
    }
}