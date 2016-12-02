using System;
using System.Collections.Generic;

namespace Assets.Systems
{
    public interface IGameSystem
    {
        int Priority { get; }
        List<Type> SystemComponents { get; }
        void Init();
        void RegisterComponent(IGameComponent component);
    }
}