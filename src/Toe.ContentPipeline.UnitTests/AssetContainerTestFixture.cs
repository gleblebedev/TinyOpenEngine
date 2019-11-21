using NUnit.Framework;

namespace Toe.ContentPipeline
{
    [TestFixture]
    public class AssetContainerTestFixture
    {
        public class AssetClass : AbstractAsset
        {
            public AssetClass(string id) : base(id)
            {
            }
        }

        [Test]
        public void Add_AssetWithNullId_AssetFoundByIndex()
        {
            var container = new AssetContainer<AssetClass>(1);

            var asset = new AssetClass(null);
            container.Add(asset);

            Assert.AreEqual(asset, container[0]);
        }

        [Test]
        public void AddRange_AssetsWithDuplicateId_AssetsAssignedUniqueIds()
        {
            var container = new AssetContainer<AssetClass>(2);

            var asset1 = new AssetClass("id");
            var asset2 = new AssetClass("id");
            container.AddRange(new[] {asset1, asset2}, _ => _.Id, (a, id) => new AssetClass(id));

            Assert.AreEqual(asset1.Id, container[0].Id);
            Assert.AreNotEqual(asset2.Id, container[1].Id);
        }

        [Test]
        public void AddRange_NullId_AssetsAssignedUniqueIds()
        {
            var container = new AssetContainer<AssetClass>(2);

            var asset1 = new AssetClass(null);
            var asset2 = new AssetClass(null);
            container.AddRange(new[] {asset1, asset2}, _ => null, (a, id) => new AssetClass(id));

            Assert.NotNull(container[0].Id);
            Assert.NotNull(container[1].Id);
            Assert.AreNotEqual(container[0].Id, container[1].Id);
        }
    }
}