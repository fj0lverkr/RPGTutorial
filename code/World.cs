using System.Collections.Generic;
using Godot;


namespace RPGTutorial
{
	using Objects;
	public partial class World : Node2D
	{
		[Export]
		public PackedScene SlimeScene { get; set; }

		private readonly List<GameCharacter> enemiesOnScene = new();

		public override void _Ready()
		{
			Slime slime = SlimeScene.Instantiate<Slime>();
			slime.Name = "Jefke";
			slime.Speed = 35;
			enemiesOnScene.Add(slime);
			slime.Position = new Vector2(10, 10);
			AddChild(slime);
		}
	}
}
