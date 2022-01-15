using NUnit.Framework;
using System;
using System.Linq;

namespace Grenadiers.Tests
{
    public class GuardTest
    {
        [Test]
        public void NotNull_Instance_Guards()
        {
            var parameter = new object();
            Assert.AreEqual(parameter, Guard.NotNull(parameter, nameof(parameter)));
        }

        [Test]
        public void NotNull_Null_Throws()
        {
            object parameter = null;
            Assert.Throws<ArgumentNullException>(() => Guard.NotNull(parameter, nameof(parameter)));
        }

        [Test]
        public void NotDefault_Instance_Guards()
        {
            int? parameter = 12;
            Assert.AreEqual(parameter, Guard.NotDefault(parameter, nameof(parameter)));
        }

        [Test]
        public void NotDefault_DefaultInt_Throws()
        {
            int parameter = 0;
            Assert.Throws<ArgumentException>(() => Guard.NotDefault(parameter, nameof(parameter)));
        }

        [Test]
        public void NotDefault_Null_Throws()
        {
            int? parameter = null;
            Assert.Throws<ArgumentException>(() => Guard.NotDefault(parameter, nameof(parameter)));
        }

        [Test]
        public void HasAny_Empty_Throws()
        {
            var parameter = Array.Empty<int>();
            Assert.Throws<ArgumentException>(() => Guard.HasAny(parameter, nameof(parameter)));
        }

        [Test]
        public void HasAny_1Item_Guards()
        {
            var parameter = new[] { 1 };
            var actual = Guard.HasAny(parameter, nameof(parameter));
            Assert.AreEqual(parameter, actual);
        }

        [Test]
        public void HasAny_EmptyEnumeration_Throws()
        {
            var parameter = Enumerable.Empty<int>();
            Assert.Throws<ArgumentException>(() => Guard.HasAny(parameter, nameof(parameter)));
        }

        [Test]
        public void HasAny_NotEmptyEnumeration_Guards()
        {
            var parameter = (new[] { 1 }).AsEnumerable();
            var actual = Guard.HasAny(parameter, nameof(parameter));
            Assert.AreEqual(parameter, actual);
        }

        [Test]
        public void In_NotInAllowedRange_Throws()
        {
            var parameter = 1;
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.In(parameter, nameof(parameter), 2, 3, 4));
        }

        [Test]
        public void In_InAllowedRange_Guards()
        {
            var parameter = 1;
            var actual =  Guard.In(parameter, nameof(parameter), 1, 2, 3, 4);
            Assert.AreEqual(parameter, actual);
        }

        [Test]
        public void NotIn_InAllowedRange_Throws()
        {
            var parameter = 2;
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotIn(parameter, nameof(parameter), 2, 3, 4));
        }

        [Test]
        public void NotIn_NotInAllowedRange_Guards()
        {
            var parameter = 1;
            var actual = Guard.NotIn(parameter, nameof(parameter),  2, 3, 4);
            Assert.AreEqual(parameter, actual);
        }

        [Test]
        public void InstanceOf_NotInt_Throws()
        {
            object parameter = DateTime.Now;
            Assert.Throws<ArgumentException>(() => Guard.IsInstanceOf<int>(parameter, nameof(parameter)));
        }

        [Test]
        public void InstanceOf_Int_Guards()
        {
            object parameter = 1;
            int actual = Guard.IsInstanceOf<int>(parameter, nameof(parameter));
            Assert.AreEqual(1, actual);
        }

        [Test]
        public void NotEmpty_GuidEmpty_Throws()
        {
            Guid? parameter = Guid.Empty;
            Assert.Throws<ArgumentException>(() => Guard.NotEmpty(parameter, nameof(parameter)));
        }

        [Test]
        public void NotEmpty_newGuid_Guards()
        {
            Guid? parameter = Guid.NewGuid();
            Guid actual = Guard.NotEmpty(parameter, nameof(parameter));
            Assert.AreEqual(parameter.Value, actual);
        }

        [Test]
        public void DefinedEnum_NamedEnumValue_Guards()
        {
            var parameter = Base64FormattingOptions.None;
            var actual = Guard.DefinedEnum(parameter, nameof(parameter));
            Assert.AreEqual(parameter, actual);
        }

        [Test]
        public void DefinedEnum_NotNamedEnumValue_Throws()
        {
            var parameter = (Base64FormattingOptions)20;
            Assert.Throws<ArgumentOutOfRangeException>(()=> Guard.DefinedEnum(parameter, nameof(parameter)));
        }

        [Test]
        public void DefinedEnum_NoEnum_Throws()
        {
            var parameter = Guid.Empty;
            Assert.Throws<ArgumentException>(() => Guard.DefinedEnum(parameter, nameof(parameter)));
        }
    }
}
