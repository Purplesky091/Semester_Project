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
        PEASANT_SELECTED
    }

    public enum HighlightType { Attack, Move }
}
