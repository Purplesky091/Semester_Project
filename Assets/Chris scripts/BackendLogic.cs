namespace Pillage
{
    class BackendLogic
    {
        Map map;

        public BackendLogic()
        {
            map = new Map();
            
        }

        public bool IsGameRunning()
        {
            return map.GameRunning;
        }

        public int GetWinCondition()
        {
            return map.GameWinCondition;
        }

        public int[] GetKnightLocations()
        {
            return map.GetPiecesByType(Piece.Type.Knight);
        }

        public int[] GetPeasantLocations()
        {
            return map.GetPiecesByType(Piece.Type.Peasant);
        }

        public int[] GetMoveLocations(int tileID)
        {
            return map.GetValidMoveLocations(tileID);
        }

        public int[] GetAttackLocations(int tileID)
        {
            return map.GetValidAttackLocations(tileID);
        }

        public void PlaceStartingKnights(int[] TileIDs)
        {
            map.PlaceInitialKnights(TileIDs);
        }

        public void PlaceStartingPeasants(int[] TileIDs)
        {
            map.PlaceInitialPeasants(TileIDs);
        }

        public void MovePiece(int StartingTile, int DestinationTile)
        {
            if (map.GetPieceTypeAtLocation(StartingTile) == Piece.Type.Knight)
                map.MoveKnight(StartingTile, DestinationTile);
            else
                map.MovePeasant(StartingTile, DestinationTile);

            return;
        }

        public void AttackLocation(int tileID)
        {
            map.KnightAttack(tileID);
        }

        public int[] DoKnightMoveAI()
        {
            int[] output = map.CalculateAIKnightMove();

            if(output[0] != -1)
            {
                map.MoveKnight(output[0], output[1]);
                if (output[2] != -1)
                    map.KnightAttack(output[2]);
            }

            return output;
        }

        public bool BoardCleanup()
        {
            return map.UpdateGameState();
        }

        public void ResetGame()
        {
            map = new Map();
        }
    }
}
