namespace Toe.ContentPipeline
{
    public interface IContentContainer
    {
        IAssetContainer<IImageAsset> Images { get; }
        IAssetContainer<IMaterialAsset> Materials { get; }

        IAssetContainer<IMesh> Meshes { get; }

        IAssetContainer<ISceneAsset> Scenes { get; }
    }
}