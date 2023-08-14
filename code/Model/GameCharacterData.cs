using Godot;

namespace RPGTutorial.Model
{
	public struct GameCharacterData
	{
		public string ExistsOnScene { get; set; }
		public string CharacterId { get; set; }
		public float SpeedModifier { get; set; }
		public float Speed { get; set; }
		public int AttackPoints { get; set; }
		public float MaxHitPoints { get; set; }
		public float AttackDiff { get; set; }
		public int KnockBackVelocity { get; set; }
		public PlayerDirection CurrentDirection { get; set; } = PlayerDirection.Down;
		public PlayerDirection PushedDirection { get; set; } = PlayerDirection.None;
		public PlayerState CurrentState { get; set; } = PlayerState.Idle;
		public PlayerState MainState { get; set; } = PlayerState.Idle;
		public GameCharacterType CharacterType { get; set; }
		public string CharacterName { get; set; }
		public float HitPoints { get; set; }
		public Vector2 Position {get; set; }

		public GameCharacterData(string existsOnScene, string characterId, float speedModifier, float speed, int attackPoints, float maxHP, float attackDiff, int knockBackVelocity, GameCharacterType type, string name, float hp, Vector2 position)
		{
			ExistsOnScene = existsOnScene;
			CharacterId = characterId;
			SpeedModifier = speedModifier;
			Speed = speed;
			AttackPoints = attackPoints;
			MaxHitPoints = maxHP;
			AttackDiff = attackDiff;
			KnockBackVelocity = knockBackVelocity;
			CharacterType = type;
			CharacterName = name;
			HitPoints = hp;
			Position = position;
		}
	}
}