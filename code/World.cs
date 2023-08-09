using System.Collections.Generic;
using Godot;


namespace RPGTutorial
{
	using Objects;
	public partial class World : Node2D
	{
		[Export]
		public PackedScene SlimeScene { get; set; }
		[Export]
		public PackedScene PlayerScene { get; set; }
		[Export]
		public int MinX { get; set; } = 0;
		[Export]
		public int MaxX { get; set; } = 416;
		[Export]
		public int MinY { get; set; } = 0;
		[Export]
		public int MaxY { get; set; } = 192;


		private readonly List<GameCharacter> enemiesOnScene = new();

		public override void _Ready()
		{
			Slime slime = SlimeScene.Instantiate<Slime>();
			Player player = PlayerScene.Instantiate<Player>();
			slime.Name = "Jefke";
			slime.Speed = 35;
			enemiesOnScene.Add(slime);
			slime.Position = new Vector2(100, 175);
			slime.CharacterDefeated += () => enemiesOnScene.Remove(slime);
			player.Name = "Berry";
			player.Position = GlobalNode.PlayerSpawnLeft;
			player.CurrentDirection = GlobalNode.PlayerSpawnLeftFacing;
			player.SetMapBoundaries(MinX, MaxX, MinY, MaxY);
			AddChild(player);
			AddChild(slime);
		}

		private void OnExitLeftEntered(Node2D body)
		{
			if (!body.IsInGroup("Enemies"))
			{
				//assume the player is then the one that entered
				GlobalNode.TransitionScene = true;
			}
		}

		private void OnExitLeftExited(Node2D body)
		{
			// Replace with function body.
		}
	}
}
