using Grenadiers;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace Nil_resolving_specs
{
    public class Registered_by_default
    {
        [Test]
        public void IPAddress() => Assert.That(Nil.Object<IPAddress>(), Is.EqualTo(Nil.IPAddress));

        [Test]
        public void String() => Assert.That(Nil.Object<string>(), Is.EqualTo(Nil.String));

        [Test]
        public void Task() => Assert.That(Nil.Object<Task>(), Is.EqualTo(Nil.Task));
    }

    public class Field
    {
        [Test]
        public void from_Nil() => Assert.That(Nil.Object<WithNilField>(), Is.Not.Null);

        [Test]
        public void from_None() => Assert.That(Nil.Object<WithNoneField>(), Is.Not.Null);

        [Test]
        public void from_Default() => Assert.That(Nil.Object<WithDefaultField>(), Is.Not.Null);

        [Test]
        public void from_Empty() => Assert.That(Nil.Object<WithEmptyField>(), Is.Not.Null);

        internal class WithNilField { public static readonly WithNilField Nil = new(); }
        internal class WithNoneField { public static readonly WithNoneField None = new(); }
        internal class WithDefaultField { public static readonly WithDefaultField Default = new(); }
        internal class WithEmptyField { public static readonly WithEmptyField Empty = new(); }
    }

    public class Property
    {
        [Test]
        public void from_Nil() => Assert.That(Nil.Object<WithNilProperty>(), Is.Not.Null);

        [Test]
        public void from_None() => Assert.That(Nil.Object<WithNoneProperty>(), Is.Not.Null);

        [Test]
        public void from_Default() => Assert.That(Nil.Object<WithDefaultProperty>(), Is.Not.Null);

        [Test]
        public void from_Empty() => Assert.That(Nil.Object<WithEmptyProperty>(), Is.Not.Null);

        internal class WithNilProperty { public static WithNilProperty Nil => new(); }
        internal class WithNoneProperty { public static WithNoneProperty None => new(); }
        internal class WithDefaultProperty { public static WithDefaultProperty Default => new(); }
        internal class WithEmptyProperty { public static WithEmptyProperty Empty => new(); }
    }

    public class Method
    {
        [Test]
        public void from_Nil() => Assert.That(Nil.Object<WithNilMethod>(), Is.Not.Null);

        [Test]
        public void from_None() => Assert.That(Nil.Object<WithNoneMethod>(), Is.Not.Null);

        [Test]
        public void from_Default() => Assert.That(Nil.Object<WithDefaultMethod>(), Is.Not.Null);

        [Test]
        public void from_Empty() => Assert.That(Nil.Object<WithEmptyMethod>(), Is.Not.Null);

        internal class WithNilMethod { public static WithNilMethod Nil() => new(); }
        internal class WithNoneMethod { public static WithNoneMethod None() => new(); }
        internal class WithDefaultMethod { public static WithDefaultMethod Default() => new(); }
        internal class WithEmptyMethod { public static WithEmptyMethod Empty() => new(); }
    }

    public class Async
    {
        [Test]
        public async Task Null_task()
        {
            Task<WithNil> task = null;
            Assert.That(await task.OrNilObject(), Is.Not.Null);
        }

        [Test]
        public async Task Task_from_null()
            => Assert.That(await NulltAsync().OrNilObject(), Is.Not.Null);

        private Task<WithNil> NulltAsync() => Task.FromResult<WithNil>(null);
    }

    internal class WithNil { public static WithNil Nil() => new(); }
}
