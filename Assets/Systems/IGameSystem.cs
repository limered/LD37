using System.Collections.Generic;

namespace Assets.Systems
{
    public interface IGameSystem
    {
        int Priority { get; }
        List<IGameComponent> SystemComponents { get; }
        void Init();
        void RegisterComponent(IGameComponent component);
    }
}