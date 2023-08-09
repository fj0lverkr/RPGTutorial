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
		private readonly Vector2 playerSpawnLeft = new(5, 50);
		private readonly Vector2 playerSpawnRight = new(416, 104);

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
			if (GlobalNode.PreviousScene == "world" || GlobalNode.PreviousScene == "leftScene")
			{
				player.Position = playerSpawnLeft;
				player.CurrentDirection = GlobalNode.PlayerSpawnLeftFacing;
			}
			else
			{
				player.Position = playerSpawnRight;
				player.CurrentDirection = GlobalNode.PlayerSpawnRightFacing;
			}
			player.SetMapBoundaries(MinX, MaxX, MinY, MaxY);
			AddChild(player);
			AddChild(slime);
		}

		private void OnExitLeftEntered(Node2D body)
		{
			if (body.IsInGroup("MC"))
			{
				GlobalNode.PreviousScene = GlobalNode.CurrentScene;
				GlobalNode.CurrentScene = "leftScene";
				GetTree().ChangeSceneToFile("res://scenes/left_scene.tscn");
			}
		}

		private void OnExitLeftExited(Node2D body)
		{
			if (body.IsInGroup("MC"))
			{
				GlobalNode.TransitionScene = false;
			}
		}
	}
}
