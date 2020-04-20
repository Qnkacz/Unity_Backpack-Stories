using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public static Grid instance;
    public AudioSource audiosouce;


    public enum PieceType
	{
		EMPTY,
		NORMAL,
		BUBBLE,
		COUNT,
	};

	[System.Serializable]
	public struct PiecePrefab
	{
		public PieceType type;
		public GameObject prefab;
	};

	public int xDim;
	public int yDim;
	public float fillTime;
	public int counter = 0; // for score

	public PiecePrefab[] piecePrefabs;
	public GameObject backgroundPrefab;
	public GameObject[] items;
	private GameObject newItem;

	private Dictionary<PieceType, GameObject> piecePrefabDict;

	private GamePiece[,] pieces;

	private bool inverse = false;

	private GamePiece pressedPiece;
	private GamePiece enteredPiece;

	// Use this for initialization
	void Start () {
        AudioSource audiosouce = this.gameObject.GetComponent<AudioSource>();
        instance = this;
		piecePrefabDict = new Dictionary<PieceType, GameObject> ();

		for (int i = 0; i < piecePrefabs.Length; i++) {
			if (!piecePrefabDict.ContainsKey (piecePrefabs [i].type)) {
				piecePrefabDict.Add (piecePrefabs [i].type, piecePrefabs [i].prefab);
			}
		}

		for (int x = 0; x < xDim; x++) {
			for (int y = 0; y < yDim; y++) {
				GameObject background = (GameObject)Instantiate (backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
				background.transform.parent = transform;
			}
		}

		pieces = new GamePiece[xDim, yDim];
		for (int x = 0; x < xDim; x++) {
			for (int y = 0; y < yDim; y++) {
				SpawnNewPiece (x, y, PieceType.EMPTY);
			}
		}

		

		StartCoroutine(Fill ());
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public IEnumerator Fill()
	{
		bool needsRefill = true;

		while (needsRefill) {
			yield return new WaitForSeconds (fillTime);

			while (FillStep ()) {
				inverse = !inverse;
				yield return new WaitForSeconds (fillTime);
			}

			needsRefill = ClearAllValidMatches ();
		}
	}

	public bool FillStep()
	{
		bool movedPiece = false;

		for (int y = yDim-2; y >= 0; y--)
		{
			for (int loopX = 0; loopX < xDim; loopX++)
			{
				int x = loopX;

				if (inverse) {
					x = xDim - 1 - loopX;
				}

				GamePiece piece = pieces [x, y];

				if (piece.IsMovable ())
				{
					GamePiece pieceBelow = pieces [x, y + 1];

					if (pieceBelow.Type == PieceType.EMPTY) {
						Destroy (pieceBelow.gameObject);
						piece.MovableComponent.Move (x, y + 1, fillTime);
						pieces [x, y + 1] = piece;
						SpawnNewPiece (x, y, PieceType.EMPTY);
						movedPiece = true;
					} else {
						for (int diag = -1; diag <= 1; diag++)
						{
							if (diag != 0)
							{
								int diagX = x + diag;

								if (inverse)
								{
									diagX = x - diag;
								}

								if (diagX >= 0 && diagX < xDim)
								{
									GamePiece diagonalPiece = pieces [diagX, y + 1];

									if (diagonalPiece.Type == PieceType.EMPTY)
									{
										bool hasPieceAbove = true;

										for (int aboveY = y; aboveY >= 0; aboveY--)
										{
											GamePiece pieceAbove = pieces [diagX, aboveY];

											if (pieceAbove.IsMovable ())
											{
												break;
											}
											else if(!pieceAbove.IsMovable() && pieceAbove.Type != PieceType.EMPTY)
											{
												hasPieceAbove = false;
												break;
											}
										}

										if (!hasPieceAbove)
										{
											Destroy (diagonalPiece.gameObject);
											piece.MovableComponent.Move (diagX, y + 1, fillTime);
											pieces [diagX, y + 1] = piece;
											SpawnNewPiece (x, y, PieceType.EMPTY);
											movedPiece = true;
											break;
										}
									} 
								}
							}
						}
					}
				}
			}
		}

		for (int x = 0; x < xDim; x++)
		{
			GamePiece pieceBelow = pieces [x, 0];

			if (pieceBelow.Type == PieceType.EMPTY)
			{
				Destroy (pieceBelow.gameObject);
				GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity);
				newPiece.transform.parent = transform;

				pieces [x, 0] = newPiece.GetComponent<GamePiece> ();
				pieces [x, 0].Init (x, -1, this, PieceType.NORMAL);
				pieces [x, 0].MovableComponent.Move (x, 0, fillTime);
				pieces [x, 0].ColorComponent.SetColor ((ColorPiece.ColorType)Random.Range (0, pieces [x, 0].ColorComponent.NumColors));
				movedPiece = true;
			}
		}

		return movedPiece;
	}

	public Vector2 GetWorldPosition(int x, int y)
	{
		return new Vector2 (transform.position.x - xDim / 2.0f + x,
			transform.position.y + yDim / 2.0f - y);
	}

	public GamePiece SpawnNewPiece(int x, int y, PieceType type)
	{
		GameObject newPiece = (GameObject)Instantiate (piecePrefabDict [type], GetWorldPosition (x, y), Quaternion.identity);
		newPiece.transform.parent = transform;

		pieces [x, y] = newPiece.GetComponent<GamePiece> ();
		pieces [x, y].Init (x, y, this, type);
		gameObject.layer = 8;

		return pieces [x, y];
	}

	public bool IsAdjacent(GamePiece piece1, GamePiece piece2)
	{
		return (piece1.X == piece2.X && (int)Mathf.Abs (piece1.Y - piece2.Y) == 1)
		|| (piece1.Y == piece2.Y && (int)Mathf.Abs (piece1.X - piece2.X) == 1);
	}

	public void SwapPieces(GamePiece piece1, GamePiece piece2)
	{
		if (piece1.IsMovable () && piece2.IsMovable ()) {
			pieces [piece1.X, piece1.Y] = piece2;
			pieces [piece2.X, piece2.Y] = piece1;

			

				int piece1X = piece1.X;
				int piece1Y = piece1.Y;

				piece1.MovableComponent.Move (piece2.X, piece2.Y, fillTime);
				piece2.MovableComponent.Move (piece1X, piece1Y, fillTime);

				ClearAllValidMatches ();

				StartCoroutine (Fill ());
			
		}
	}

	public void PressPiece(GamePiece piece) // on mouseDown : OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider.
    {
		pressedPiece = piece;
	}

	public void EnterPiece(GamePiece piece) // on mouseEnter : Called when the mouse enters the GUIElement or Collider.
    {
        if (IsAdjacent(pressedPiece, piece)) enteredPiece = piece;
	}

	public void ReleasePiece()
	{
		
		SwapPieces (pressedPiece, enteredPiece);
		
	}

	public List<GamePiece> GetMatch(GamePiece piece, int newX, int newY)
	{
		if (piece.IsColored ()) {
			ColorPiece.ColorType color = piece.ColorComponent.Color;
			List<GamePiece> horizontalPieces = new List<GamePiece> ();
			List<GamePiece> verticalPieces = new List<GamePiece> ();
			List<GamePiece> matchingPieces = new List<GamePiece> ();

			// First check horizontal
			horizontalPieces.Add(piece);

			for (int dir = 0; dir <= 1; dir++) {
				for (int xOffset = 1; xOffset < xDim; xOffset++) {
					int x;

					if (dir == 0) { // Left
						x = newX - xOffset;
					} else { // Right
						x = newX + xOffset;
					}

					if (x < 0 || x >= xDim) {
						break;
					}

					if (pieces [x, newY].IsColored () && pieces [x, newY].ColorComponent.Color == color) {
						horizontalPieces.Add (pieces [x, newY]);
					} else {
						break;
					}
				}
			}

			if (horizontalPieces.Count >= 3) {
				for (int i = 0; i < horizontalPieces.Count; i++) {
					matchingPieces.Add (horizontalPieces [i]);
				}
			}


            // Traverse vertically if we found a match (for L and T shapes)
            //if (horizontalPieces.Count >= 3)
            //{
            //	for (int i = 0; i < horizontalPieces.Count; i++) {
            //		for (int dir = 0; dir <= 1; dir++) {
            //			for (int yOffset = 1; yOffset < yDim; yOffset++) {
            //				int y;

            //				if (dir == 0) { // Up
            //					y = newY - yOffset;
            //				} else { // Down
            //					y = newY + yOffset;
            //				}

            //				if (y < 0 || y >= yDim) {
            //					break;
            //				}

            //				if (pieces [horizontalPieces [i].X, y].IsColored () && pieces [horizontalPieces [i].X, y].ColorComponent.Color == color) {
            //					verticalPieces.Add (pieces [horizontalPieces [i].X, y]);
            //				} else {
            //					break;
            //				}
            //			}
            //		}

            //		if (horizontalPieces.Count < 2) {
            //			horizontalPieces.Clear ();
            //		} else {
            //			for (int j = 0; j < horizontalPieces.Count; j++) {
            //				matchingPieces.Add (horizontalPieces[j]);
            //			}

            //			break;
            //		}
            //	}
            //}

            //if (matchingPieces.Count >= 5)
            //{
            //    Spawnitem(matchingPieces);
            //    Spawnitem(matchingPieces);
            //    Spawnitem(matchingPieces);
            //    AudioSource audiosouce = this.gameObject.GetComponent<AudioSource>();
            //    audiosouce.Play();
            //    return matchingPieces;
            //}
            //else if (matchingPieces.Count == 4)
            //{
            //    Spawnitem(matchingPieces);
            //    Spawnitem(matchingPieces);
            //    AudioSource audiosouce = this.gameObject.GetComponent<AudioSource>();
            //    audiosouce.Play();
            //    return matchingPieces;
            //}
            //else if (matchingPieces.Count == 3)
            //{
            //    Spawnitem(matchingPieces);
            //    AudioSource audiosouce = this.gameObject.GetComponent<AudioSource>();
            //    audiosouce.Play();
            //    return matchingPieces;
            //}



            // Didn't find anything going horizontally first,
            // so now check vertically
            horizontalPieces.Clear();
			verticalPieces.Clear();
			verticalPieces.Add(piece);

			for (int dir = 0; dir <= 1; dir++) {
				for (int yOffset = 1; yOffset < yDim; yOffset++) {
					int y;

					if (dir == 0) { // Up
						y = newY - yOffset;
					} else { // Down
						y = newY + yOffset;
					}

					if (y < 0 || y >= yDim) {
						break;
					}

					if (pieces [newX, y].IsColored () && pieces [newX, y].ColorComponent.Color == color) {
						verticalPieces.Add (pieces [newX, y]);
					} else {
						break;
					}
				}
			}

			if (verticalPieces.Count >= 3) {
				for (int i = 0; i < verticalPieces.Count; i++) {
					matchingPieces.Add (verticalPieces [i]);
				}
			}

            //// Traverse horizontally if we found a match (for L and T shapes)
            //if (verticalPieces.Count >= 3)
            //{
            //	for (int i = 0; i < verticalPieces.Count; i++) {
            //		for (int dir = 0; dir <= 1; dir++) {
            //			for (int xOffset = 1; xOffset < xDim; xOffset++) {
            //				int x;

            //				if (dir == 0) { // Left
            //					x = newX - xOffset;
            //				} else { // Right
            //					x = newX + xOffset;
            //				}

            //				if (x < 0 || x >= xDim) {
            //					break;
            //				}

            //				if (pieces [x, verticalPieces[i].Y].IsColored () && pieces [x, verticalPieces[i].Y].ColorComponent.Color == color) {
            //					horizontalPieces.Add (pieces [x, verticalPieces[i].Y]);
            //				} else {
            //					break;
            //				}
            //			}
            //		}

            //		if (horizontalPieces.Count < 2) {
            //			horizontalPieces.Clear ();
            //		} else {
            //			for (int j = 0; j < horizontalPieces.Count; j++) {
            //				matchingPieces.Add (horizontalPieces [j]);
            //			}

            //			break;
            //		}
            //	}
            //}

            if (matchingPieces.Count >= 5)
            {
                Spawnitem(matchingPieces);
                Spawnitem(matchingPieces);
                Spawnitem(matchingPieces);
                AudioSource audiosouce = this.gameObject.GetComponent<AudioSource>();
                audiosouce.Play();
                return matchingPieces;
            }
            else if (matchingPieces.Count == 4)
            {
                Spawnitem(matchingPieces);
                Spawnitem(matchingPieces);
                AudioSource audiosouce = this.gameObject.GetComponent<AudioSource>();
                audiosouce.Play();
                return matchingPieces;
            }
            else if (matchingPieces.Count == 3)
            {
                Spawnitem(matchingPieces);
                AudioSource audiosouce = this.gameObject.GetComponent<AudioSource>();
                audiosouce.Play();
                return matchingPieces;
            }
        }

		return null;
	}

	public bool ClearAllValidMatches()
	{
		bool needsRefill = false;

		for (int y = 0; y < yDim; y++) {
			for (int x = 0; x < xDim; x++) {
				if (pieces [x, y].IsClearable ()) {
					List<GamePiece> match = GetMatch (pieces [x, y], x, y);

					if (match != null) {
						for (int i = 0; i < match.Count; i++) {
							if (ClearPiece (match [i].X, match [i].Y)) {
								needsRefill = true;
							}
						}
					}
				}
			}
		}

		return needsRefill;
	}

	public bool ClearPiece(int x, int y)
	{
		if (pieces [x, y].IsClearable () && !pieces [x, y].ClearableComponent.IsBeingCleared) {
			pieces [x, y].ClearableComponent.Clear ();
			counter += 1; // +1 to score
			SpawnNewPiece (x, y, PieceType.EMPTY);

			return true;
		}

		return false;
	}
	public void Spawnitem(List<GamePiece> MatchingItems )
	{
		GamePiece needed = MatchingItems[1];
		ColorPiece.ColorType color = needed.ColorComponent.Color;
		int coolor = (int)color;
		switch(coolor)
		{
			case 0:
			GameObject newItem = Instantiate(items[2], new Vector3(Random.Range(4, 10), 6, 0), Quaternion.identity);
			break;
			case 1:
			GameObject bone = Instantiate(items[0], new Vector3(Random.Range(4, 10), 6, 0), Quaternion.identity);
			break;
			case 2:
			GameObject bottle = Instantiate(items[1], new Vector3(Random.Range(4, 10), 6, 0), Quaternion.identity);
			break;
			case 3:
			GameObject spring = Instantiate(items[4], new Vector3(Random.Range(4, 10), 6, 0), Quaternion.identity);
			break;
			case 4:
			GameObject scroll = Instantiate(items[3], new Vector3(Random.Range(4, 10), 6, 0), Quaternion.identity);
			break;
			case 5:
			GameObject grymuar = Instantiate(items[5], new Vector3(Random.Range(4, 10), 6, 0), Quaternion.identity);
			break;
			default:
				GameObject cos = Instantiate(items[5], new Vector3(Random.Range(4, 10), 6, 0), Quaternion.identity);
				break;
		}

		//GameObject newItem = Instantiate(items, new Vector3(Random.Range(4, 10), 6, 0), Quaternion.identity);
		BackPack.instance.ItemCount++;        
	}
}
