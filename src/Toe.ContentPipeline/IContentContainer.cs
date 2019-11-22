namespace Toe.ContentPipeline
{
    public interface IContentContainer
    {
        IAssetContainer<IMaterialAsset> Materials { get; }

        IAssetContainer<IMesh> Meshes { get; }

        IAssetContainer<ISceneAsset> Scenes { get; }

        IAssetContainer<INodeAsset> Nodes { get; }
    }
}