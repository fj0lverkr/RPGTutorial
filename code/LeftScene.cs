using Godot;

namespace RPGTutorial
{
	using Model;

	public partial class LeftScene : Node2D
	{
		[Export]
		public PackedScene PlayerScene { get; set; }

		public int MinX = 0;
		public int MaxX = 495;
		public int MinY= 0;
		public int MaxY = 495;

		private readonly Vector2 playerSpawn = new(490, 220);

		public override void _Ready()
		{
			Player player = PlayerScene.Instantiate<Player>();
			player.Name = GlobalNode.PlayerName;
			player.HitPoints = GlobalNode.PlayerHP;
			player.Position = playerSpawn;
			player.CurrentDirection = PlayerDirection.Left;
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
