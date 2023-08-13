using Godot;
using System.Collections.Generic;

namespace RPGTutorial
{
    using Objects;
    using RPGTutorial.Model;


    public partial class GlobalNode : Node
    {

        //Relates to world en scene transitions
        public static bool TransitionScene { get; set; } = false;
        public static string CurrentScene { get; set; } = "world";
        public static string PreviousScene { get; set; } = "world";
        
        //Relates to MC
        public static float PlayerHP { get; set; }
        public static string PLayerName { get; set; } = "Berry";
    }
}