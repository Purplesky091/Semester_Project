namespace Assets.Scripts
{
    public enum GameState
    {
        KNIGHT_INIT,
        PEASANT_INIT,
        KNIGHT_UNSELECTED,
        KNIGHT_SELECTED,
        KNIGHT_ATTACK,
        PEASANT_UNSELECTED,
        PEASANT_SELECTED,
        GAME_OVER
    }

    public enum HighlightType { Attack, Move }
}
