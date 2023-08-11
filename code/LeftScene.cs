using Godot;

namespace RPGTutorial
{
	public partial class LeftScene : Node2D
	{
		[Export]
		public PackedScene PlayerScene { get; set; }
		[Export]
		public int MinX { get; set; } = 0;
		[Export]
		public int MaxX { get; set; } = 495;
		[Export]
		public int MinY { get; set; } = 0;
		[Export]
		public int MaxY { get; set; } = 495;

		private readonly Vector2 playerSpawn = new(490, 220);

		public override void _Ready()
		{
			Player player = PlayerScene.Instantiate<Player>();
			player.Name = GlobalNode.PLayerName;
			player.HitPoints = GlobalNode.PlayerHP;
			player.Position = playerSpawn;
			player.CurrentDirection = GlobalNode.PlayerSpawnLeftFacing;
			player.SetMapBoundaries(MinX, MaxX, MinY, MaxY);
			AddChild(player);
		}

		private void OnExitEntered(Node2D body)
		{
			if (body.IsInGroup("MC"))
			{
				GlobalNode.PreviousScene = GlobalNode.CurrentScene;
				GlobalNode.CurrentScene = "world";
				GlobalNode.TransitionScene = true;
				GetTree().ChangeSceneToFile("res://scenes/world.tscn");
			}
		}

		private void OnExitExited(Node2D body)
		{
			if (body.IsInGroup("MC"))
			{
				GlobalNode.TransitionScene = false;
			}
		}
	}
}
