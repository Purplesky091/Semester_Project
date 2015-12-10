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