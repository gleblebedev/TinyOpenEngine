namespace Toe.ContentPipeline
{
    public interface IContentContainer
    {
        IAssetContainer<ISceneAsset> Scenes { get; }

        IAssetContainer<INodeAsset> Nodes { get; }
    }
}