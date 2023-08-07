using Godot;
using System.Collections.Generic;

namespace RPGTutorial.Objects
{
    using System;
    using Model;
	public abstract partial class GameCharacter : CharacterBody2D
	{
		[Export]
		public float SpeedModifier = 50.0f;
		[Export]
		public float Speed = 100.0f;
		[Export]
		public int AttackPoints = 5;
		[Export]
		public float HitPoints = 100.0f;
		[Export]
		public float AttackDiff = 10.0f;
		[Export]
		public int KnockBackVelocity = 200;

		public PlayerDirection CurrentDirection = PlayerDirection.Down;
		public PlayerDirection PushedDirection;
		public PlayerState CurrentState = PlayerState.Idle;
		public PlayerState MainState = PlayerState.Idle;
		public AnimatedSprite2D animatedSprite2D;
		public List<Node2D> EnemiesInMeleeRange = new();
		public bool IsOnAttackCoolDown = false;

		public string CharacterName;
		public float StartingHitPoints = 0.0f;

		public override void _Ready()
		{
			animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			if (IsInGroup("Enemies"))
			{
				StartingHitPoints = HitPoints;
				animatedSprite2D.Play("idle");
			}
		}

		public virtual void TakeDamage(int damage, float modifier, PlayerDirection attackSource)
		{
			HitPoints -= damage / modifier;
			GD.Print($"{Name} is hit for {damage / modifier} damage, new HP: {HitPoints}/{StartingHitPoints}.");
			MainState = PlayerState.KnockedBack;
			PushedDirection = attackSource;
		}

		public void BecomeDefeated()
		{
			void defeatHandler() {
				QueueFree();
					animatedSprite2D.AnimationFinished -= defeatHandler;
			}

			if (IsInGroup("Enemies"))
			{	RemoveFromGroup("Enemies");
                animatedSprite2D.Play("defeat");
                animatedSprite2D.AnimationFinished += defeatHandler;
			}
		}
    }
}
