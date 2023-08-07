using Godot;
using System;
using System.Linq;

namespace RPGTutorial
{


	using Model;
	using Objects;

	public partial class Player : GameCharacter
	{


		private Vector2 mapBoundMin;
		private Vector2 mapBoundMax;
		private Camera2D playerCam;

		public override void _Ready()
		{
			base._Ready();
 			playerCam = GetNode<Camera2D>("Camera2D");
			playerCam.LimitBottom = (int)mapBoundMax.Y;
			playerCam.LimitTop = (int)mapBoundMin.Y;
			playerCam.LimitLeft = (int)mapBoundMin.X;
			playerCam.LimitRight = (int)mapBoundMax.X; 
		}

		public override void _PhysicsProcess(double delta)
		{
			if (CurrentState != PlayerState.Attacking) PlayerMovement(delta);
		}

		public void SetMapBoundaries(int minX, int maxX, int minY, int maxY)
		{
			mapBoundMin = new(minX, minY);
			mapBoundMax = new(maxX, maxY);
		}

		public void PlayerMovement(double delta)
		{
			Vector2 velocity = Velocity;

			if (Input.IsActionPressed("move_right"))
			{
				CurrentDirection = PlayerDirection.Right;
				CurrentState = PlayerState.Walking;
				velocity.X = Speed;
				velocity.Y = 0;
			}
			else if (Input.IsActionPressed("move_left"))
			{
				CurrentDirection = PlayerDirection.Left;
				CurrentState = PlayerState.Walking;
				velocity.X = -Speed;
				velocity.Y = 0;
			}
			else if (Input.IsActionPressed("move_up"))
			{
				CurrentDirection = PlayerDirection.Up;
				CurrentState = PlayerState.Walking;
				velocity.X = 0;
				velocity.Y = -Speed;
			}
			else if (Input.IsActionPressed("move_down"))
			{
				CurrentDirection = PlayerDirection.Down;
				CurrentState = PlayerState.Walking;
				velocity.X = 0;
				velocity.Y = Speed;
			}
			else if (Input.IsActionPressed("attack"))
			{
				if (!IsOnAttackCoolDown)
				{
					IsOnAttackCoolDown = true;
					CurrentState = PlayerState.Attacking;
					velocity.X = 0;
					velocity.Y = 0;

					foreach (GameCharacter enemy in EnemiesInMeleeRange.Cast<GameCharacter>())
					{
						float diffY = Math.Abs(enemy.Position.Y - Position.Y);
						float diffX = Math.Abs(enemy.Position.X - Position.X);
						switch (CurrentDirection)
						{
							case PlayerDirection.Left:
								if (enemy.Position.X <= Position.X && diffY <= AttackDiff)
								{
									enemy.TakeDamage(AttackPoints, diffY, CurrentDirection);
								}
								break;
							case PlayerDirection.Right:
								if (enemy.Position.X >= Position.X && diffY <= AttackDiff)
								{
									enemy.TakeDamage(AttackPoints, diffY, CurrentDirection);
								}
								break;
							case PlayerDirection.Up:
								if (enemy.Position.Y <= Position.Y && diffX <= AttackDiff)
								{
									enemy.TakeDamage(AttackPoints, diffX, CurrentDirection);
								}
								break;
							case PlayerDirection.Down:
								if (enemy.Position.Y >= Position.Y && diffX <= AttackDiff)
								{
									enemy.TakeDamage(AttackPoints, diffX, CurrentDirection);
								}
								break;
						}
					}
				}
			}
			else
			{
				if (CurrentState != PlayerState.Attacking)
				{
					CurrentState = PlayerState.Idle;
					velocity.X = 0;
					velocity.Y = 0;
				}
			}

			Velocity = velocity;
			PlayAnimation();
			MoveAndSlide();
		}

		private void OnPlayerAttackAreaBodyEntered(Node2D body)
		{
			if (body.IsInGroup("Enemies"))
			{
				EnemiesInMeleeRange.Add(body);
			}
		}

		private void OnPlayerAttackAreaBodyExited(Node2D body)
		{
			if (body.IsInGroup("Enemies"))
			{
				EnemiesInMeleeRange.Remove(body);
			}
		}

		private void PlayAnimation()
		{
			switch (CurrentState)
			{
				case PlayerState.Walking:
					{
						switch (CurrentDirection)
						{
							case PlayerDirection.Left: animatedSprite2D.FlipH = true; animatedSprite2D.Play("walk_side"); break;
							case PlayerDirection.Up: animatedSprite2D.Play("walk_back"); break;
							case PlayerDirection.Down: animatedSprite2D.Play("walk_front"); break;
							case PlayerDirection.Right: animatedSprite2D.FlipH = false; animatedSprite2D.Play("walk_side"); break;
						}
						break;
					}
				case PlayerState.Idle:
					{
						switch (CurrentDirection)
						{
							case PlayerDirection.Left: animatedSprite2D.FlipH = true; animatedSprite2D.Play("idle_side"); break;
							case PlayerDirection.Up: animatedSprite2D.Play("idle_back"); break;
							case PlayerDirection.Down: animatedSprite2D.Play("idle_front"); break;
							case PlayerDirection.Right: animatedSprite2D.FlipH = false; animatedSprite2D.Play("idle_side"); break;
						}
						break;
					}
				case PlayerState.Attacking:
					{

						switch (CurrentDirection)
						{
							case PlayerDirection.Left: animatedSprite2D.FlipH = true; animatedSprite2D.Play("attack_side"); break;
							case PlayerDirection.Up: animatedSprite2D.Play("attack_back"); break;
							case PlayerDirection.Down: animatedSprite2D.Play("attack_front"); break;
							case PlayerDirection.Right: animatedSprite2D.FlipH = false; animatedSprite2D.Play("attack_side"); break;
						}
						break;
					}
			}
		}

		private void OnAnimationFinished()
		{
			if (CurrentState == PlayerState.Attacking)
			{
				CurrentState = PlayerState.Idle;
				IsOnAttackCoolDown = false;
			}
		}
	}
}
