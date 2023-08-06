using Godot;

namespace RPGTutorial
{
	using Model;
	using Objects;

	public partial class Slime : GameCharacter
	{
		private Node2D player;

		public override void _PhysicsProcess(double delta)
		{
			if (MainState == PlayerState.Chasing)
			{
				animatedSprite2D.FlipH = player.Position.X - Position.X < 0;
				animatedSprite2D.Play("walk");
				Position += (player.Position - Position) / SpeedModifier;
				MoveAndSlide();
			}
			else if (MainState == PlayerState.KnockedBack)
			{
				Vector2 velocity = Velocity;
				switch (PushedDirection)
				{
					case PlayerDirection.Down:
						//velocity.X = 0;
						velocity.Y = KnockBackVelocity;
						break;
					case PlayerDirection.Right:
						//velocity.Y = 0;
						velocity.X = KnockBackVelocity;
						break;
					case PlayerDirection.Up:
						//velocity.X = 0;
						velocity.Y = -KnockBackVelocity;
						break;
					case PlayerDirection.Left:
						//velocity.Y = 0;
						velocity.X = -KnockBackVelocity;
						break;
				}
				Velocity = velocity;
				MoveAndSlide();
			}
		}

		private void DetectionAreaBodyEntered(Node2D body)
		{
			player = body;
			MainState = PlayerState.Chasing;
		}

		private void DetectionAreaBodyExited(Node2D body)
		{
			player = null;
			MainState = PlayerState.Idle;
		}

		public override void TakeDamage(int damage, PlayerDirection attackSource)
		{
			base.TakeDamage(damage, attackSource);
			Timer knockedBackCooldown = GetNode<Timer>("KnockedBackCooldown");
			knockedBackCooldown.Start();
		}

		private void _on_knocked_back_cooldown_timeout()
		{
			Vector2 velocity = new(0,0);
			Velocity = velocity;
			if (player != null)
			{
				MainState = PlayerState.Chasing;
			}
			else
			{
				MainState = PlayerState.Idle;
			}

		}
	}
}
