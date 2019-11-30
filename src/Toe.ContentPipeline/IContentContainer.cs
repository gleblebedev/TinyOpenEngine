namespace Toe.ContentPipeline
{
    public interface IContentContainer
    {
        IAssetContainer<ITextureAsset> Textures { get; }
        IAssetContainer<IMaterialAsset> Materials { get; }

        IAssetContainer<IMesh> Meshes { get; }

        IAssetContainer<ISceneAsset> Scenes { get; }

        IAssetContainer<INodeAsset> Nodes { get; }
    }
}