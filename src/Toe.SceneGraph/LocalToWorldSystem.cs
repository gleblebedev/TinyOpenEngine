namespace Toe.SceneGraph
{
    //TODO: Implement ECS
    //[EcsInject]
    public class LocalToWorldSystem //: IEcsRunSystem
    {
        private readonly Scene _scene;

        public LocalToWorldSystem(Scene scene)
        {
            _scene = scene;
        }

        public void Run()
        {
            _scene.UpdateWorldTransforms();
        }
    }
}