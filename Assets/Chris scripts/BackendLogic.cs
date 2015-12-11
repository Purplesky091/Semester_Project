namespace Pillage
{
    class BackendLogic
    {
        Map map;

        public BackendLogic()
        {
            map = new Map();
            
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
    }
}
