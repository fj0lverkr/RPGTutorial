using System.Collections.Generic;
using Godot;


namespace RPGTutorial
{
	using Objects;
	using Model;
	public partial class World : Node2D
	{
		[Export]
		public PackedScene SlimeScene { get; set; }
		[Export]
		public PackedScene PlayerScene { get; set; }

		public int MinX = 0;
		public int MaxX = 416;
		public int MinY = 0;
		public int MaxY = 192;

		private readonly Vector2 playerSpawnLeft = new(5, 50);
		private readonly Vector2 playerSpawnRight = new(416, 104);
		private readonly List<GameCharacter> EnemiesOnScene = new();

		public override void _Ready()
		{
			Slime slime = SlimeScene.Instantiate<Slime>();
			Player player = PlayerScene.Instantiate<Player>();
			List<GameCharacterData> characterData = GlobalNode.LoadCharacterStates("world");
			if (characterData.Count == 0)
			{
				slime.CharacterType = GameCharacterType.Slime;
				slime.Position = new Vector2(100, 175);
				slime.Name = "Jefke";
				slime.Speed = 35;
				slime.CharacterDefeated += () => EnemiesOnScene.Remove(slime);
				EnemiesOnScene.Add(slime);
				AddChild(slime);
			}
			else
			{
				foreach (GameCharacterData data in characterData)
				{
					switch (data.CharacterType)
					{
						case GameCharacterType.Slime:
							slime.Position = data.Position;
							slime.Name = data.CharacterName;
							slime.Speed = data.Speed;
							slime.HitPoints = data.HitPoints;
							slime.MaxHitPoints = data.MaxHitPoints;
							slime.AttackDiff = 50;
							slime.CharacterDefeated += () => EnemiesOnScene.Remove(slime);
							EnemiesOnScene.Add(slime);
							AddChild(slime);
							break;
					}
				}
			}

			player.Name = GlobalNode.PlayerName;
			player.HitPoints = GlobalNode.PlayerHP;
			if (GlobalNode.PreviousScene == "world" || GlobalNode.PreviousScene == "leftScene")
			{
				player.Position = playerSpawnLeft;
				player.CurrentDirection = PlayerDirection.Right;
			}
			else
			{
				player.Position = playerSpawnRight;
				player.CurrentDirection = PlayerDirection.Left;
			}
			player.SetMapBoundaries(MinX, MaxX, MinY, MaxY);
			AddChild(player);
		}

		private void OnExitLeftEntered(Node2D body)
		{
			if (body.IsInGroup("MC"))
			{
				GlobalNode.SaveCharacterState("world", EnemiesOnScene);
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
