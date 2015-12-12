namespace Pillage
{
    class Knight:Piece
    {

        public Knight():this(-1)
        {

        }

        public Knight(int idNumber):this(idNumber, -1)
        {
        }

        public Knight(int idNumber, int location)
        {
            IDNumber = idNumber;
            Location = location;
            PieceType = Piece.Type.Knight;
        }

        public override int[] GetValidAttacks()
        {
            return GetThreeAhead();
        }

        public override int[] GetValidMoves()
        {
            return GetThreeAhead();
        }

        public override int[] GetValidAttacks(int tileID)
        {
            return GetThreeAhead(tileID);
        }

        private int[] GetThreeAhead()
        {
            int[] output = new int[] { -1, -1, -1 };

            if (Location / 10 != 7)
            {
                output[0] = (Location % 10 != 0) ? Location + 9 : -1;
                output[1] = Location + 10;
                output[2] = (Location % 10 != 7) ? Location + 11 : -1;
            }

            return output;
        }

        private int[] GetThreeAhead(int tileID)
        {
            int[] output = new int[] { -1, -1, -1 };

            if (Location / 10 != 7)
            {
                output[0] = (tileID % 10 != 0) ? tileID + 9 : -1;
                output[1] = tileID + 10;
                output[2] = (tileID % 10 != 7) ? tileID + 11 : -1;
            }

            return output;
        }

    }
}
