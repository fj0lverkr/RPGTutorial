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
        Idle,
        Walking,
        Chasing,
        Attacking,
        KnockedBack,
        Dazed,
        Defeated
    }
}