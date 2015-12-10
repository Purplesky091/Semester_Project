using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pillage
{
    class Peasant:Piece
    {

        public Peasant():this(-1)
        {

        }

        public Peasant(int idNumber):this(idNumber, -1)
        {
        }

        public Peasant(int idNumber, int location)
        {
            IDNumber = idNumber;
            Location = location;
            PieceType = Piece.Type.Peasant;
        }


        public override int[] GetValidAttacks()
        {
            return new int[] {-1};
        }

        public override int[] GetValidMoves()
        {
            int[] output = new int[4];

            //Moving upwards to the top of the board
            output[0] = (Location / 10 != 0) ? Location - 10 : -1;

            //Moving down towards the bottom of the board
            output[1] = (Location / 10 != 7) ? Location + 10 : -1;

            //Moving left
            output[2] = (Location % 10 != 0) ? Location - 1 : -1;

            //Moving right
            output[3] = (Location % 10 != 7) ? Location + 1 : -1;

            return output;
        }

    }
}
