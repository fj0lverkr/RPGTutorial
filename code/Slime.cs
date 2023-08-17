using System;
using Godot;

namespace RPGTutorial
{
	using System.Linq;
	using Model;
	using Objects;

	public partial class Slime : GameCharacter
	{
		private Node2D player;
		private Timer cooldown;

		public override void _Ready()
		{
			base._Ready();
			cooldown = GetNode<Timer>("EnemyAttackCooldown");
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);
			switch (MainState)
			{
				case PlayerState.Chasing:
					{
						animatedSprite2D.FlipH = player.Position.X - Position.X < 0;
						float distance = Position.DistanceSquaredTo(player.Position);
						if (distance > 250)
						{
							animatedSprite2D.Play("walk");
							Position += (player.Position - Position) / SpeedModifier;
							MoveAndSlide();
						}
						else if (distance < 200)
						{
							animatedSprite2D.Play("walk");
							Vector2 velocity = Velocity;
							Vector2 playerDirection = Position.DirectionTo(player.Position);
							if (Math.Round(playerDirection.X) == 0)
							{
								if (Math.Round(playerDirection.Y) > 0)
								{
									velocity.Y = -Speed;
								}
								else
								{
									velocity.Y = Speed;
								}
							}
							else
							{
								if (Math.Round(playerDirection.X) > 0)
								{
									velocity.X = -Speed;
								}
								else
								{
									velocity.X = Speed;
								}
							}
							Velocity = velocity;
							MoveAndSlide();
							Timer knockedBackCooldown = GetNode<Timer>("KnockedBackCooldown");
							knockedBackCooldown.Start();
						}
						else
						{
							TryAttackPlayer();
						}
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
				default:
					{
						//the slime should do some roaming around
						break;
					}
			}
		}

		private void DetectionAreaBodyEntered(Node2D body)
		{
			if (MainState != PlayerState.None && MainState != PlayerState.Defeated && MainState != PlayerState.KnockedBack)
			{
				player = body;
				EnemiesInMeleeRange.Add(body);
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

		private void TryAttackPlayer()
		{
			if (!IsOnAttackCoolDown)
			{
				IsOnAttackCoolDown = true;
				Vector2 velocity = Velocity;
				velocity.X = 0;
				velocity.Y = 0;

				foreach (Player attackPlayer in EnemiesInMeleeRange.Cast<Player>())
				{
					float diffY = Math.Abs(attackPlayer.Position.Y - Position.Y);
					if (player.Position.X - Position.X < 0)
					{
						animatedSprite2D.FlipH = true;
						if (attackPlayer.Position.X <= Position.X && diffY <= AttackDiff)
						{
							attackPlayer.TakeDamage(AttackPoints, diffY, CurrentDirection);
						}
					}
					else
					{
						animatedSprite2D.FlipH = false;
						if (attackPlayer.Position.X >= Position.X && diffY <= AttackDiff)
						{
							attackPlayer.TakeDamage(AttackPoints, diffY, CurrentDirection);
						}
					}
					animatedSprite2D.Play("attack");
				}
				Velocity = velocity;
			}
			else
			{
				if (cooldown.IsStopped())
				{
					Random random = new();
					double waitTime = 0;
					while (waitTime < 0.5)
					{
						waitTime = random.NextDouble() * 3;
					}
					cooldown.WaitTime = waitTime;
					cooldown.Start();
					 animatedSprite2D.Play("walk");

					GD.Print($"{Name}: Attack cooldowns started.");
				}
			}
		}

		public void OnAttackCooldownEnd()
		{
			IsOnAttackCoolDown = false;
			GD.Print($"{Name}: Attack cooldowns ended.");
		}
	}
}
