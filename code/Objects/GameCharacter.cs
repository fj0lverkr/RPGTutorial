using System;
using Godot;
using System.Collections.Generic;

namespace RPGTutorial.Objects
{

	using Model;
	public abstract partial class GameCharacter : CharacterBody2D
	{
		[Export]
		public float SpeedModifier = 50.0f;
		[Export]
		public float Speed = 100.0f;
		[Export]
		public int AttackPoints = 3;
		[Export]
		public float MaxHitPoints = 100.0f;
		[Export]
		public float AttackDiff = 10.0f;
		[Export]
		public int KnockBackVelocity = 200;


		[Signal]
		public delegate void CharacterDefeatedEventHandler();

		public PlayerDirection CurrentDirection = PlayerDirection.Down;
		public PlayerDirection PushedDirection;
		public PlayerState CurrentState = PlayerState.Idle;
		public PlayerState MainState = PlayerState.Idle;
		public AnimatedSprite2D animatedSprite2D;
		public List<Node2D> EnemiesInMeleeRange = new();
		public bool IsOnAttackCoolDown = false;
		public GpuParticles2D HitEffect;
		public ProgressBar HealthBar;
		public string CharacterId = Guid.NewGuid().ToString();
		public GameCharacterType CharacterType;
		public string CharacterName;
		public float HitPoints = 0.0f;


		public override void _Ready()
		{
			ZIndex = 1;
			animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			HealthBar = GetNode<ProgressBar>("HealthBar");
			HealthBar.MaxValue = MaxHitPoints;
			HitPoints = HitPoints == 0.0f ? MaxHitPoints : HitPoints;

			if (IsInGroup("Enemies"))
			{
				HitEffect = GetNode<GpuParticles2D>("HitEffectParticles2D");
				animatedSprite2D.Play("idle");
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			HealthBar.Value = HitPoints;
			HealthBar.Visible = HealthBar.Value < MaxHitPoints;
		}

		public virtual void TakeDamage(int damage, float modifier, PlayerDirection attackSource)
		{
			modifier = modifier < 0.05 || modifier == 0 ? 0.05f : modifier; // prevent over-damage and division by zero
			modifier = modifier > 5 ? 5 : modifier; //prevent under-damage
			HitPoints -= damage / modifier;
			GD.Print($"{Name} is hit for {damage}/{modifier} = {damage / modifier} damage, new HP: {HitPoints}/{MaxHitPoints}.");
			MainState = PlayerState.KnockedBack;
			PushedDirection = attackSource;
		}

		public void BecomeDefeated()
		{
			void defeatHandler()
			{
				EmitSignal(SignalName.CharacterDefeated);
				QueueFree();
				animatedSprite2D.AnimationFinished -= defeatHandler;
			}

			if (IsInGroup("Enemies"))
			{
				RemoveFromGroup("Enemies");
				animatedSprite2D.Play("defeat");
				animatedSprite2D.AnimationFinished += defeatHandler;
			}
		}
	}
}
