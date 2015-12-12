namespace Pillage
{
    public abstract class Piece
    {
        public enum Type { Knight, Peasant, None };

        private Type type;
        private int idNumber;
        private int location;
        private bool isAlive;

        public abstract int[] GetValidMoves();

        public abstract int[] GetValidAttacks();

        public abstract int[] GetValidAttacks(int tileID);


        public bool IsAlive
        {
            get
            {
                return isAlive;
            }
            set
            {
                isAlive = value;
            }
        }

        public int Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        public int IDNumber
        {
            get
            {
                return idNumber;
            }
            set
            {
                idNumber = value;
            }
        }

        public Type PieceType
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }





    }
}
