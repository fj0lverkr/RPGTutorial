using Godot;
using System.Collections.Generic;

namespace RPGTutorial
{
    using Objects;
    using RPGTutorial.Model;


    public partial class GlobalNode : Node
    {
        //Relates to world and scene transitions
        public static bool TransitionScene { get; set; } = false;
        public static string CurrentScene { get; set; } = "world";
        public static string PreviousScene { get; set; } = "world";

        private static readonly List<GameCharacterData> SavedGameCharacterData = new();

        //Relates to MC
        public static float PlayerHP { get; set; }
        public static string PlayerName { get; set; } = "Berry";

        public static void SaveCharacterState(string sceneName, List<GameCharacter> characters)
        {
            SavedGameCharacterData.Clear();
            foreach (GameCharacter c in characters)
            {
                GameCharacterData temp = new(
                    sceneName, 
                    c.CharacterId, 
                    c.SpeedModifier, 
                    c.Speed, 
                    c.AttackPoints, 
                    c.MaxHitPoints, 
                    c.AttackDiff, 
                    c.KnockBackVelocity, 
                    c.CharacterType, 
                    c.Name, 
                    c.HitPoints,
                    c.Position
                );
                SavedGameCharacterData.Add(temp);
            }
        }

        public static List<GameCharacterData> LoadCharacterStates(string sceneName) {
            return SavedGameCharacterData.FindAll(x => x.ExistsOnScene == sceneName);
        }
    }
}