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
		public int AttackPoints = 5;
		[Export]
		public int HitPoints = 100;
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

		public override void _Ready()
		{
			animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			if (IsInGroup("Enemies"))
			{
				animatedSprite2D.Play("idle");
			}
		}

		public virtual void TakeDamage(int damage, PlayerDirection attackSource)
		{
			HitPoints -= damage;
			MainState = PlayerState.KnockedBack;
			PushedDirection = attackSource;
		}
	}
}
