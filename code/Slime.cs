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
			switch (MainState)
			{
				case PlayerState.Chasing:
					{
						animatedSprite2D.FlipH = player.Position.X - Position.X < 0;
						animatedSprite2D.Play("walk");
						Position += (player.Position - Position) / SpeedModifier;
						MoveAndSlide();
						break;
					}
				case PlayerState.KnockedBack:
					{
						Vector2 velocity = Velocity;
						switch (PushedDirection)
						{
							case PlayerDirection.Down:
								velocity.Y = KnockBackVelocity;
								break;
							case PlayerDirection.Right:
								velocity.X = KnockBackVelocity;
								break;
							case PlayerDirection.Up:
								velocity.Y = -KnockBackVelocity;
								break;
							case PlayerDirection.Left:
								velocity.X = -KnockBackVelocity;
								break;
						}
						Velocity = velocity;
						MoveAndSlide();
						break;
					}
				case PlayerState.Defeated:
					{
						BecomeDefeated();
						MainState = PlayerState.None;
						break;
					}
			}
		}

		private void DetectionAreaBodyEntered(Node2D body)
		{
			if (MainState != PlayerState.None && MainState != PlayerState.Defeated && MainState != PlayerState.KnockedBack)
			{
				player = body;
				MainState = PlayerState.Chasing;
			}
		}

		private void DetectionAreaBodyExited(Node2D body)
		{
			if (body == player && MainState != PlayerState.None && MainState != PlayerState.Defeated && MainState != PlayerState.KnockedBack)
			{
				player = null;
				MainState = PlayerState.Idle;
			}
		}

		public override void TakeDamage(int damage, float modifier, PlayerDirection attackSource)
		{
			base.TakeDamage(damage, modifier, attackSource);
			HitEffect.Emitting = true;
			Timer knockedBackCooldown = GetNode<Timer>("KnockedBackCooldown");
			knockedBackCooldown.Start();
		}

		private void OnKnockedBackCooldownDone()
		{
			HitEffect.Emitting = false;
			Vector2 velocity = new(0, 0);
			Velocity = velocity;
			if (HitPoints > 0)
			{
				if (player != null)
				{
					MainState = PlayerState.Chasing;
				}
				else
				{
					MainState = PlayerState.Idle;
				}
			}
			else
			{
				MainState = PlayerState.Defeated;
			}
		}
	}
}
