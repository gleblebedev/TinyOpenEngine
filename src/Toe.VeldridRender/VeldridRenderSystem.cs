using System;
using Toe.EntityComponentSystem;

namespace Toe.VeldridRender
{
    public class VeldridRenderSystem: AbstractSystem
    {
        private readonly VeldridContext _context;

        public VeldridRenderSystem(VeldridContext context)
        {
            _context = context;
        }
    }
}
