using Toe.SceneGraph;

namespace Toe.EntityComponentSystem
{
    //TODO: Implement ECS
    //[EcsInject]
    public class LocalToWorldSystem //: IEcsRunSystem
    {
        private readonly Scene<NodeComponent> _scene;

        public LocalToWorldSystem(Scene<NodeComponent> scene)
        {
            _scene = scene;
        }

        public void Run()
        {
            _scene.UpdateWorldTransforms();
        }
    }
}