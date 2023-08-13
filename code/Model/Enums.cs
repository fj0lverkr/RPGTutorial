namespace RPGTutorial.Model
{
    public enum PlayerDirection
    {
        None,
        Up,
        Left,
        Right,
        Down
    }

    public enum PlayerState {
        None,
        Idle,
        Walking,
        Chasing,
        Attacking,
        KnockedBack,
        Dazed,
        Defeated
    }

    public enum GameCharacterType {
        Player,
        Slime,
        Skeleton
    }
}