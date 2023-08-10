using Godot;

namespace RPGTutorial {
    using Model;
    public partial class GlobalNode : Node {

        //Relates to world en scene transitions
        public static bool TransitionScene {get; set;} = false;
        public static string CurrentScene {get; set;} = "world";
        public static string PreviousScene {get; set;} = "world";

        //Relates to MC
        public static float PlayerHP {get; set;}
        public static string PLayerName {get; set;} = "Berry";
        public static Vector2 PlayerSpawnLeft {get; set;} = new(20, 50);
        public const PlayerDirection PlayerSpawnLeftFacing = PlayerDirection.Right;
        public static Vector2 PlayerSpawnRight {get; set;} = new(416, 104);
        public const PlayerDirection PlayerSpawnRightFacing = PlayerDirection.Left;
    }
}