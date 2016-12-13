using UniRx;

namespace Assets.Systems.GameControl.Components
{
    public class StageComponent : GameComponent
    {
        public bool IsActive;
        public BoolReactiveProperty IsDone = new BoolReactiveProperty(false);
        public StageComponent nextStage;
    }
}