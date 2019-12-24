namespace Toe.ContentPipeline
{
    public interface IContentContainer
    {
        IAssetContainer<IImageAsset> Images { get; }
        IAssetContainer<IMaterialAsset> Materials { get; }

        IAssetContainer<ICameraAsset> Cameras { get; }
        IAssetContainer<ILightAsset> Lights { get; }
        IAssetContainer<IMesh> Meshes { get; }

        IAssetContainer<ISceneAsset> Scenes { get; }
    }
}