using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pillage
{
    class Map
    {
        Tile[][] map = new Tile[8][];
        int[] KnightLocations = new int[4];
        int[] PeasantLocations = new int[16];
        private int pieceID = 0;

        public Map()
        {

        }

        private void CreateMap()
        {
            for(int i = 0; i < 8; i++)
            {
                map[i] = new Tile[8];
                for (int j = 0; j < 8; j++)
                    map[i][j] = new Tile((i * 10) + j);
            }

            pieceID = 0;
        }

        public void PlaceInitialPieces(int[] KnightStarts, int[] PeasantStarts)
        {
            PlaceInitialKnights(KnightStarts);
            PlaceInitialPeasants(PeasantStarts);
        }

        private void PlaceInitialKnights(int[] startingLocations)
        {
            for (int i = 0; i < startingLocations.Length; i++)
            {
                KnightLocations[i] = startingLocations[i];
                map[RowFromID(startingLocations[i])][ColFromID(startingLocations[i])].PieceOnTile = new Knight(pieceID++, startingLocations[i]);
            }
        }

        private void PlaceInitialPeasants(int[] startingLocations)
        {
            for(int i = 0; i < startingLocations.Length; i++)
            {
                PeasantLocations[i] = startingLocations[i];
                map[RowFromID(startingLocations[i])][ColFromID(startingLocations[i])].PieceOnTile = new Peasant(pieceID++, startingLocations[i]);
            }
        }

        public void MoveKnight(int currentTile, int destinationTile)
        {
            if (!ValidateTileID(currentTile) || !ValidateTileID(destinationTile))
                return;

            int cRow = RowFromID(currentTile), cCol = ColFromID(currentTile);
            int dRow = RowFromID(destinationTile), dCol = ColFromID(destinationTile);

            if (map[cRow][cCol].GetPieceType() != Piece.Type.Knight)
                return;

            if (map[dRow][dCol].GetPieceType() != Piece.Type.None)
                return;

            if (UpdateKnightLocations(currentTile, destinationTile))
                MovePiece(currentTile, destinationTile);
        }

        public void MovePeasant(int currentTile, int destinationTile)
        {
            if (!ValidateTileID(currentTile) || !ValidateTileID(destinationTile))
                return;

            int cRow = RowFromID(currentTile), cCol = ColFromID(currentTile);
            int dRow = RowFromID(destinationTile), dCol = ColFromID(destinationTile);

            if (map[cRow][cCol].GetPieceType() != Piece.Type.Peasant)
                return;

            if (map[dRow][dCol].GetPieceType() != Piece.Type.None)
                return;

            if (UpdatePeasantLocations(currentTile, destinationTile))
                MovePiece(currentTile, destinationTile);
        }

        private void MovePiece(int currentTile, int destinationTile)
        {
            int cRow = RowFromID(currentTile), cCol = ColFromID(currentTile);
            int dRow = RowFromID(destinationTile), dCol = ColFromID(destinationTile);

            map[dRow][dCol].PieceOnTile = map[cRow][cCol].PieceOnTile;
            map[dRow][dCol].PieceOnTile.Location = destinationTile;
            map[dRow][dCol].Passable = false;
            map[cRow][cCol].PieceOnTile = null;
            map[cRow][cCol].Passable = true;
        }

        private bool UpdateKnightLocations(int current, int destination)
        {
            int i = 0;
            try
            {
                while (KnightLocations[i] != current)
                    i++;
            }catch(IndexOutOfRangeException)
            {
                return false;
            }

            KnightLocations[i] = destination;
            return true;
        }

        private bool UpdatePeasantLocations(int current, int destination)
        {
            int i = 0;
            try
            {
                while (PeasantLocations[i] != current)
                    i++;
            }
            catch(IndexOutOfRangeException)
            {
                return false;
            }

            PeasantLocations[i] = destination;

            return true;
        }

        public int[] GetValidMoveLocations(int currentTile)
        {
            int[] output;

            int Row = RowFromID(currentTile), Col = ColFromID(currentTile);

            Piece p = map[Row][Col].PieceOnTile;

            if (p == null)
                return new int[] { -1 };

            output = p.GetValidMoves();

            for (int i = 0; i < output.Length; i++)
                if (i != -1 && !map[RowFromID(i)][ColFromID(i)].Passable)
                    output[i] = -1;

            return output;
        }

        private bool ValidateTileID(int id)
        {
            bool output;

            output = (id < 0) ? false : true;
            output = (id > 79) ? false : output;
            output = (id % 10 > 7) ? false : output;    

            return output;
        }

        public int[] KillSurroundedKnights()
        {
            int[] output = new int[4];

            for (int i = 0; i < 4; i++)
            {
                if (KnightLocations[i] == -1) continue;
                int Row = RowFromID(KnightLocations[i]), Col = ColFromID(KnightLocations[i]);

            }


            return output;


        }

        public bool PlayerHasMoveAvailable(Piece.Type Type)
        {
            switch(Type)
            {
                case Piece.Type.Knight:
                    foreach (int i in KnightLocations)
                        if (HasMove(map[RowFromID(i)][ColFromID(i)].PieceOnTile))
                            return true;
                    break;
                case Piece.Type.Peasant:
                    foreach (int i in PeasantLocations)
                        if (!IsSurrounded(i))
                            return true;
                    break;
            }


            return false;
        }

        private bool IsSurrounded(int id)
        {
            if (IsPassable(TileAbove(id)))
                return false;

            if (IsPassable(TileBelow(id)))
                return false;

            if (IsPassable(TileRight(id)))
                return false;

            if (IsPassable(TileLeft(id)))
                return false;

            return true;
        }

        private bool HasMove(Piece p)
        {
            int[] moves = p.GetValidMoves();

            foreach (int i in moves)
                if (i != -1)
                    return true;

            return false;
        }

        private bool IsPassable(int tileID)
        {
            if(tileID != -1)
                return map[RowFromID(tileID)][ColFromID(tileID)].Passable;

            return false;
        }



        private int TileID(int row, int column) { return ((row * 10) + column); }
        private int RowFromID(int id) { return id / 10; }
        private int ColFromID(int id) { return id % 10; }
        private int TileAbove(int id) { return (id / 10 == 0) ? -1 : id - 10; }
        private int TileBelow(int id) { return (id / 10 == 7) ? -1 : id + 10; }
        private int TileLeft(int id) { return (id % 10 == 0) ? -1 : id - 1; }
        private int TileRight(int id) { return (id % 10 == 7) ? -1 : id + 1; }



    }


    class Tile
    {
        private int id = -1;
        private Piece piece = null;
        private bool passable = true;

        public Tile()
        {

        }

        public Tile(int idNumber)
        {
            id = idNumber;
        }

        public Tile (int idNumber, Piece piece)
        {
            id = idNumber;
            this.piece = piece;
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public Piece PieceOnTile
        {
            get
            {
                return piece;
            }
            set
            {
                piece = value;
            }
        }

        public bool Passable
        {
            get
            {
                return passable;
            }
            set
            {
                passable = value;
            }
        }

        public Piece.Type GetPieceType()
        {
            return (piece == null) ? Piece.Type.None : piece.PieceType;
        }
    }
}


