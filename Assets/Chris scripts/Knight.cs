using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private int[] GetThreeAhead()
        {
            int[] output = new int[3];

            output[0] = (Location % 10 != 0) ? Location + 9 : -1;
            output[1] = Location + 10;
            output[2] = (Location % 10 != 7) ? Location + 11 : -1;

            return output;
        }
    }
}
