using System;
using NUnit.Framework;

namespace Toe.ContentPipeline
{
    [TestFixture]
    public class ImmutableIdTestFixture
    {
        [Test]
        public void NewStructure_IdIsNull()
        {
            var id = new ImmutableId();

            Assert.Null(id.Id);
        }
        [Test]
        public void SetId_NullValue_IdIsNull()
        {
            var id = new ImmutableId();

            id.Id = null;

            Assert.Null(id.Id);
        }
        [Test]
        public void SetId_EmptyStringValue_IdIsNull()
        {
            var id = new ImmutableId();

            id.Id = string.Empty;

            Assert.Null(id.Id);
        }
        [Test]
        public void SetId_NotEmptyStringValue_IdIsSet()
        {
            var id = new ImmutableId();

            id.Id = "bla";

            Assert.AreEqual("bla",id.Id);
        }
        [Test]
        public void SetIdTwice_NullValue_ThrowsException()
        {
            var id = new ImmutableId("bla");
            
            Assert.Throws<InvalidOperationException>(()=>id.Id = null);
        }
        [Test]
        public void SetIdTwice_EmptyStringValue_IdIsNull()
        {
            var id = new ImmutableId("bla");

            Assert.Throws<InvalidOperationException>(() => id.Id = string.Empty);

        }
        [Test]
        public void SetIdTwice_NotEmptyStringValue_IdIsSet()
        {
            var id = new ImmutableId("bla");

            Assert.Throws<InvalidOperationException>(() => id.Id = "bla2");
        }

        [Test]
        public void SetIdTwice_NullValueThenNullValue_DoesntThow()
        {
            var id = new ImmutableId(null);

            Assert.DoesNotThrow(() => id.Id = null);
        }
        [Test]
        public void SetIdTwice_EmptyStringValueThenNullValue_DoesntThow()
        {
            var id = new ImmutableId(string.Empty);

            Assert.DoesNotThrow(() => id.Id = null);
        }
        [Test]
        public void SetIdTwice_NullValueThenEmptyStringValue_DoesntThow()
        {
            var id = new ImmutableId(null);

            Assert.DoesNotThrow(() => id.Id = string.Empty);
        }
    }
}