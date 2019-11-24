namespace Toe.SceneGraph
{
    //TODO: Implement ECS
    //[EcsInject]
    public class LocalToWorldSystem<TEntity> //: IEcsRunSystem
    {
        private readonly Scene<TEntity> _scene;

        public LocalToWorldSystem(Scene<TEntity> scene)
        {
            _scene = scene;
        }

        public void Run()
        {
            _scene.UpdateWorldTransforms();
        }
    }
}