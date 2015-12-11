using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public enum GameState
    {
        KNIGHT_INIT,
        PEASANT_INIT,
        KNIGHT,
        PEASANT
    }

    public enum HighlightType{ Attack, Move }
}
