using System;
using System.Linq;

namespace Pillage
{
    class Map
    {
        Tile[][] map = new Tile[8][];
        int[] KnightLocations = new int[4];
        int[] PeasantLocations = new int[16];
        private int pieceID = 0;
        private int[] StoneLocations; 
        private bool gameRunning = true;
        private int winCondition = 0;           /* 0 = Game still running
                                                 * 1 = Peasants win
                                                 * 2 = Knight wins by killing peasants
                                                 * 3 = Knight wins by pillaging village
                                                 */
                                      
        

        public Map()
        {
            for (int i = 0; i < 8; i++)
            {
                map[i] = new Tile[8];
                for (int j = 0; j < 8; j++)
                    map[i][j] = new Tile((i * 10) + j);
            }

            pieceID = 0;
        }

        public int[] GetPiecesByType(Piece.Type t)
        {
            return (t == Piece.Type.Knight) ? RemoveFromArray(KnightLocations, -1) : RemoveFromArray(PeasantLocations, -1);
        }

        public bool GameRunning
        {
            get
            {
                return gameRunning;
            }
        }

        public int GameWinCondition
        {
            get
            {
                return winCondition;
            }
        }

        public bool UpdateGameState()
        {
            bool output = KillSurroundedKnights();

            if (GetSurvivingKnightCount() < 3)
            {
                gameRunning = false;
                winCondition = 1;
                //Do Peasant Win Logic Here
            }
            else if (GetSurvivingPeasantCount() < 4)
            {
                gameRunning = false;
                winCondition = 2;
                //Do Knight Win by death Logic Here
            }
            else if (CountPiecesOfTypeInRow(Piece.Type.Knight, 7) > 2)
            {
                gameRunning = false;
                winCondition = 3;
                //Do Knight wins by pillaging logic here
            }

            return output;
        }

        public void KnightAttack(int i)
        {
            int j = 0;

            try
            {
                while (PeasantLocations[j] != i)
                    j++;

                PeasantLocations[j] = -1;
            }
            catch (Exception) { }

            map[RowFromID(i)][ColFromID(i)].PieceOnTile = null;
        }

        public void SetStones(int numberOfStones)
        {
            if (numberOfStones > 0 && numberOfStones < 7)
            {
                Random r = new Random();
                StoneLocations = new int[numberOfStones];

                for (int i = 0; i < numberOfStones; i++)
                {
                    int row = r.Next(0, 7);
                    int col = r.Next(0, 7);

                    StoneLocations[i] = TileID(row, col);
                    map[row][col].SetStone();
                }
            }
        }

        private int CountPiecesOfTypeInRow(Piece.Type t, int row)
        {
            int[] holder = (t == Piece.Type.Knight) ? KnightLocations : PeasantLocations;
            int output = 0;

            foreach (int i in holder)
                if (RowFromID(i) == row)
                    output++;

            return output;
        }

        private int GetSurvivingKnightCount()
        {
            return RemoveFromArray(KnightLocations, -1).Length;
        }

        private int GetSurvivingPeasantCount()
        {
            return RemoveFromArray(PeasantLocations, -1).Length;
        }

        public int[] GetValidAttackLocations(int TileID)
        {
            int[] output = map[RowFromID(TileID)][ColFromID(TileID)].PieceOnTile.GetValidAttacks();

            for (int i = 0; i < output.Length; i++)
                if (map[RowFromID(output[i])][ColFromID(output[i])].PieceOnTile == null)
                    output[i] = -1;
                else
                    if(map[RowFromID(output[i])][ColFromID(output[i])].PieceOnTile.PieceType != Piece.Type.Peasant)
                        output[i] = -1;

            output = RemoveFromArray(output, -1);

            return output;
        }

        public void PlaceInitialKnights(int[] startingLocations)
        {
            for (int i = 0; i < startingLocations.Length; i++)
            {
                KnightLocations[i] = startingLocations[i];
                map[RowFromID(startingLocations[i])][ColFromID(startingLocations[i])].PieceOnTile = new Knight(pieceID++, startingLocations[i]);
            }
        }

        public void PlaceInitialPeasants(int[] startingLocations)
        {
            for(int i = 0; i < startingLocations.Length; i++)
            {
                PeasantLocations[i] = startingLocations[i];
                map[RowFromID(startingLocations[i])][ColFromID(startingLocations[i])].PieceOnTile = new Peasant(pieceID++, startingLocations[i]);
            }
        }

        public Piece.Type GetPieceTypeAtLocation(int tileID)
        {
            return map[RowFromID(tileID)][ColFromID(tileID)].PieceOnTile.PieceType;
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

            UpdateGameState();
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

        public bool KillSurroundedKnights()
        {
            bool output = false;

            for (int i = 0; i < 4; i++)
            {
                if (KnightLocations[i] == -1) continue;
                int k = KnightLocations[i], up = TileAbove(k), down = TileBelow(k), left = TileLeft(k), right = TileRight(k);
                if(up != -1)
                    if (map[RowFromID(up)][ColFromID(up)].Passable)
                        continue;
                if(down != -1)
                    if (map[RowFromID(down)][ColFromID(down)].Passable)
                        continue;
                if(left != -1)
                    if (map[RowFromID(left)][ColFromID(left)].Passable)
                        continue;
                if(right != -1)
                    if (map[RowFromID(right)][ColFromID(right)].Passable)
                        continue;

                KillKnight(k);
                output = true;
            }


            return output;


        }

        private void KillKnight(int i)
        {
            map[RowFromID(i)][ColFromID(i)].PieceOnTile = null;
            map[RowFromID(i)][ColFromID(i)].Passable = true;
            for (int j = 0; j < 4; j++)
                if (KnightLocations[j] == i)
                    KnightLocations[j] = -1;
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

        private int[] RemoveFromArray(int[] array, int value)
        {
            return array.Where(val => val != value).ToArray();
        }

        private int TileID(int row, int column) { return ((row * 10) + column); }
        private int RowFromID(int id) { return id / 10; }
        private int ColFromID(int id) { return id % 10; }
        private int TileAbove(int id) { return (id / 10 == 0) ? -1 : id - 10; }
        private int TileBelow(int id) { return (id / 10 == 7) ? -1 : id + 10; }
        private int TileLeft(int id) { return (id % 10 == 0) ? -1 : id - 1; }
        private int TileRight(int id) { return (id % 10 == 7) ? -1 : id + 1; }


        #region AI Move Calculators

        public int[] CalculateAIKnightMove()
        {
                                    //Move From, Move to, Attack
            int[] output = new int[] { -1, -1, -1 };
            bool[] tested = new bool[] { false, false, false, false };
            Random r = new Random();

            int i = r.Next(3);

            while (tested.Contains<bool>(false))
            {
                if (!tested[i])
                    if (KnightLocations[i] != -1)
                    {
                        int[] posMoves = map[RowFromID(i)][ColFromID(i)].PieceOnTile.GetValidMoves();
                        posMoves = RemoveFromArray(posMoves, -1);
                        if (posMoves.Length > 0)
                        {
                            output[0] = KnightLocations[i];
                            output[1] = posMoves[r.Next(posMoves.Length)];
                            int[] posAttacks = map[RowFromID(i)][ColFromID(i)].PieceOnTile.GetValidAttacks(output[1]);
                            posAttacks = RemoveFromArray(posAttacks, -1);
                            if (posAttacks.Length > 0)
                                output[2] = posAttacks[r.Next(posAttacks.Length)];

                            return output;
                        }
                    }
            }

            return output;
        }
        /* Smart move technology for the knight. NOT YET IMPLIMENTED
        private bool CalculateTileSafetyForKnight(int tileID)
        {
            bool output = false;

            int[][] alg = new int[][] { new int[] { -1, 10000, -1 }, new int[] { -1, 10000, -1 }, new int[] { -1, 100000, -1 }, new int[] { -1, 10000, -1 } };

            alg[0][0] = TileAbove(tileID);
            alg[1][0] = TileBelow(tileID);
            alg[2][0] = TileRight(tileID);
            alg[3][0] = TileLeft(tileID);

            for (int i = 0; i < 4; i++)
            {
                if (alg[i][0] == -1)
                    alg[i][1] = 0;

                //If the tile does exist but is already flagged as impassible
                if (!map[RowFromID(i)][ColFromID(i)].Passable)
                    alg[i][1] = 0;
            }

            foreach (int i in PeasantLocations)
                if (i != -1)
                {
                    

                    int[][] distMatrix = new int[4][];

                    //Populate the distance matrix. 
                    for (int j = 0; i < 4; j++)
                    {
                        distMatrix[j][] = new int[2];
                        distMatrix[j][0] = alg[j][0];
                        distMatrix[j][1] = (distMatrix[j][0] != -1) ? Distance(i, distMatrix[j][0]) : 1000;
                    }


                }
            
            return output;
        }

        private int Distance(int tileA, int tileB)
        {
            int i = 0;
            int rowA, colA, rowB, colB;
            rowA = RowFromID(tileA);
            rowB = RowFromID(tileB);
            colA = ColFromID(tileA);
            colB = ColFromID(tileB);

            i += Math.Abs(rowA - rowB);
            i += Math.Abs(colA - colB);            

            return i;
        }
        */

        #endregion
    }


    class Tile
    {
        private int id = -1;
        private Piece piece = null;
        private bool passable = true;
        private bool hasStone = false;

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

        public void SetStone()
        {
            hasStone = true;
            passable = false;
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


