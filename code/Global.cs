using Godot;

namespace RPGTutorial {
    using Model;
    public partial class GlobalNode : Node {
        public static bool TransitionScene {get; set;} = false;
        public static Vector2 PlayerSpawnLeft {get; private set;} = new(2, 50);
        public const PlayerDirection PlayerSpawnLeftFacing = PlayerDirection.Right;
    }
}