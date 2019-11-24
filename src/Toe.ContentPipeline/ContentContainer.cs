namespace Toe.ContentPipeline
{
    public class ContentContainer : IContentContainer
    {
        public IAssetContainer<IMesh> Meshes { get; } = new AssetContainer<IMesh>();
        public IAssetContainer<IMaterialAsset> Materials { get; } = new AssetContainer<IMaterialAsset>();
        public IAssetContainer<ISceneAsset> Scenes { get; } = new AssetContainer<ISceneAsset>();
        public IAssetContainer<INodeAsset> Nodes { get; } = new AssetContainer<INodeAsset>();
        public IAssetContainer<ICameraAsset> Cameras { get; } = new AssetContainer<ICameraAsset>();
        public IAssetContainer<ILightAsset> Lights { get; } = new AssetContainer<ILightAsset>();
    }
}