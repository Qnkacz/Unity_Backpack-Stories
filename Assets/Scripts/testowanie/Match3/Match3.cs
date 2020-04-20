using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Match3 : MonoBehaviour
{
    public ArrayLayout boardLayout;

    public static int tileSize = 144;

    [Header("Board Options")]
    public int width = 6;
    public int height = 6;

    [Header("Flip Back Options")]
    public bool flipBackAfterWrongMove;

    [Header("UI Elements")]
    public Sprite[] pieces;
    public RectTransform gameBoard;
    public RectTransform killedBoard;

    [Header("Prefabs")]
    public GameObject nodePiece;
    public GameObject killedPiece;

    [Header("Seed")]
    public string gameSeed = "blank";

    int[] fills;
    Node[,] board;

    List<NodePiece> update;
    List<FlippedPieces> flipped;
    List<NodePiece> dead;
    List<KilledPiece> killed;

    System.Random random;

    public NodePiece PieceToSpawn;
    public GameObject[] items;

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        List<NodePiece> finishedUpdating = new List<NodePiece>();
        for (int i = 0; i < update.Count; i++)
        {
            NodePiece piece = update[i];
            if (!piece.UpdatePiece()) finishedUpdating.Add(piece);
        }
        for (int i = 0; i < finishedUpdating.Count; i++)
        {
            NodePiece piece = finishedUpdating[i];
            FlippedPieces flip = getFlipped(piece);
            NodePiece flippedPiece = null;

            int x = (int)piece.index.x;
            fills[x] = Mathf.Clamp(fills[x] - 1, 0, width);

            List<Point> connected = isConnected(piece.index, true);
            bool wasFlipped = (flip != null);

            if (wasFlipped && flipBackAfterWrongMove) // czy bez braku matcha ma zawracac na stare miejsce czy nie
            {
                flippedPiece = flip.getOtherPiece(piece);
                AddPoints(ref connected, isConnected(flippedPiece.index, true));
            }

            if (connected.Count == 0)    // jezeli nie ma spasowania
            {
                if (wasFlipped && flipBackAfterWrongMove)
                    FlipPieces(piece.index, flippedPiece.index, false);
            }
            else
            {   // jezeli jest spasowanie
                bool counter = true;
                foreach (Point pnt in connected) // Remove the node pieces connected
                {
                    KillPiece(pnt);
                    Node node = getNodeAtPoint(pnt);
                    NodePiece nodePiece = node.getPiece();
                    if (counter == true)
                    {
                        PieceToSpawn = nodePiece;
                        counter = false;
                    }


                    if (nodePiece != null)
                    {

                        nodePiece.gameObject.SetActive(false);
                        dead.Add(nodePiece);
                    }
                    node.SetPiece(null);
                }

                //Debug.Log("usuwanie z match3");

                Spawnitem(PieceToSpawn);
                ApplyGravityToBoard();
                counter = true;
            }
            //Debug.Log("piece value"+PieceToSpawn.value);
            flipped.Remove(flip);
            update.Remove(piece);

        }
    }
    public void Spawnitem(NodePiece p)
    {
        //Debug.Log("spawnowanie itemka");
        int itemToSpawn = p.value - 1;
        switch (itemToSpawn)
        {
            case 0:
                GameObject bone = Instantiate(items[0], new Vector3(UnityEngine.Random.Range(4, 10), 6, 0), Quaternion.identity);
                bone.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            case 1:
                GameObject bottle_empty = Instantiate(items[1], new Vector3(UnityEngine.Random.Range(4, 10), 6, 0), Quaternion.identity);
                bottle_empty.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            case 2:
                GameObject bowl_empty = Instantiate(items[2], new Vector3(UnityEngine.Random.Range(4, 10), 6, 0), Quaternion.identity);
                bowl_empty.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            case 3:
                GameObject herbs = Instantiate(items[3], new Vector3(UnityEngine.Random.Range(4, 10), 6, 0), Quaternion.identity);
                herbs.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            case 4:
                GameObject metal_scrap = Instantiate(items[4], new Vector3(UnityEngine.Random.Range(4, 10), 6, 0), Quaternion.identity);
                metal_scrap.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            case 5:
                GameObject onion = Instantiate(items[5], new Vector3(UnityEngine.Random.Range(4, 10), 6, 0), Quaternion.identity);
                onion.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            case 6:
                GameObject paper = Instantiate(items[6], new Vector3(UnityEngine.Random.Range(4, 10), 6, 0), Quaternion.identity);
                paper.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            case 7:
                GameObject stick = Instantiate(items[7], new Vector3(UnityEngine.Random.Range(4, 10), 6, 0), Quaternion.identity);
                stick.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            case 8:
                GameObject stone = Instantiate(items[8], new Vector3(UnityEngine.Random.Range(4, 10), 6, 0), Quaternion.identity);
                stone.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            case 9:
                GameObject feather = Instantiate(items[9], new Vector3(UnityEngine.Random.Range(4, 10), 6, 0), Quaternion.identity);
                feather.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            case 10:
                GameObject rope = Instantiate(items[10], new Vector3(UnityEngine.Random.Range(4, 10), 6, 0), Quaternion.identity);
                rope.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            case 11:
                GameObject ink_bottle = Instantiate(items[11], new Vector3(UnityEngine.Random.Range(4, 10), 6, 0), Quaternion.identity);
                ink_bottle.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            default:
                GameObject newItem = Instantiate(items[0], new Vector3(UnityEngine.Random.Range(4, 10), 6, 0), Quaternion.identity);
                newItem.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;

        }
        BossScript.instance.itemCount++;
    }

    void ApplyGravityToBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = (height - 1); y >= 0; y--)
            {
                Point p = new Point(x, y);
                Node node = getNodeAtPoint(p);
                int val = getValueAtPoint(p);
                if (val != 0) continue; // if it is not a hole, do nothing
                for (int ny = (y - 1); ny >= -1; ny--)
                {
                    Point next = new Point(x, ny);
                    int nextVal = getValueAtPoint(next);
                    if (nextVal == 0)
                        continue;
                    if (nextVal != -1) //If we did not hit an end, but its not 0 then use this to fill the current hole
                    {
                        Node got = getNodeAtPoint(next);
                        NodePiece piece = got.getPiece();

                        //Set the hole
                        node.SetPiece(piece);
                        update.Add(piece);

                        //replace the hole
                        got.SetPiece(null);
                    }
                    else // hit an end
                    {
                        //fill in the hole
                        int newVal = fillPiece();
                        NodePiece piece;
                        Point fallPoint = new Point(x, (-1 - fills[x]));
                        if (dead.Count > 0)
                        {
                            NodePiece revived = dead[0];
                            revived.gameObject.SetActive(true);
                            piece = revived;

                            dead.RemoveAt(0);
                        }
                        else
                        {
                            GameObject obj = Instantiate(nodePiece, gameBoard);
                            NodePiece n = obj.GetComponent<NodePiece>();
                            piece = n;
                        }

                        piece.Initialize(newVal, p, pieces[newVal - 1]);

                        piece.rect.anchoredPosition = getPositionFromPoint(new Point(x, -1));

                        Node hole = getNodeAtPoint(p);
                        hole.SetPiece(piece);
                        ResetPiece(piece);
                        fills[x]++;
                    }
                    break;
                }
            }
        }
    }

    FlippedPieces getFlipped(NodePiece p)
    {
        FlippedPieces flip = null;
        for (int i = 0; i < flipped.Count; i++)
        {
            if (flipped[i].getOtherPiece(p) != null)
            {
                flip = flipped[i];
                break;
            }
        }
        return flip;
    }

    void StartGame()
    {
        fills = new int[width];
        string seed = getRandomSeed();
        random = new System.Random(seed.GetHashCode());
        update = new List<NodePiece>();
        flipped = new List<FlippedPieces>();
        dead = new List<NodePiece>();
        killed = new List<KilledPiece>();

        InitializeBoard();
        VerifyBoard();
        //LoadGame();
        InstantiateBoard();
        //SaveGame();
    }

    void InitializeBoard()
    {
        board = new Node[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                board[x, y] = new Node((boardLayout.rows[y].row[x]) ? -1 : fillPiece(), new Point(x, y));     // Jezeli nie ma dziury to wypelnia ksztaltem
            }
        }
    }

    void VerifyBoard()
    {
        List<int> remove;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Point p = new Point(x, y);
                int val = getValueAtPoint(p);
                if (val <= 0) continue;

                remove = new List<int>();
                while (isConnected(p, true).Count > 0)
                {
                    val = getValueAtPoint(p);
                    if (!remove.Contains(val))
                        remove.Add(val);
                    setValueAtPoint(p, newValue(ref remove));
                }
            }
        }
    }

    void InstantiateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Node node = getNodeAtPoint(new Point(x, y));

                //int val = board[x, y].value;
                int val = node.value;
                if (val <= 0) continue;
                GameObject p = Instantiate(nodePiece, gameBoard);
                NodePiece piece = p.GetComponent<NodePiece>();
                RectTransform rect = p.GetComponent<RectTransform>();
                //rect.anchoredPosition = new Vector2(32 + (64 * x), -32 - (64 * y));
                rect.anchoredPosition = new Vector2(tileSize / 2 + (tileSize * x), -(tileSize / 2) - (tileSize * y));
                //Debug.Log("size: " + val);
                piece.Initialize(val, new Point(x, y), pieces[val - 1]);
                node.SetPiece(piece);
            }
        }
    }

    public void SaveGame()
    {
        string fileName = "Assets/Resources/test.txt";

        string lines = "saveStart\n";

        lines += "boardWidth\n";
        lines += width.ToString() + "\n";

        lines += "boardHeight\n";
        lines += height.ToString() + "\n";

        lines += "= = = boardValuesStart = = =\n";
        lines += generateBoardStateString();
        lines += "= = = boardValuesEnd = = =\n";

        lines += "seedStart\n";
        lines += gameSeed;
        lines += "\nseedEnd\n";

        lines += "saveEnd\n";

        //System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + fileName, lines);
        System.IO.File.WriteAllText(fileName, lines);
    }

    public void LoadGame()
    {
        string path = "Assets/Resources/test.txt";

        StreamReader reader = new StreamReader(path);
        if (reader.ReadLine() == "saveStart")
        {
            if ( reader.ReadLine() != "boardWidth" || reader.ReadLine() != width.ToString() )
            {
                return;
            }
            else if (reader.ReadLine() != "boardHeight" || reader.ReadLine() != height.ToString())
            {
                return;
            }

            if (reader.ReadLine() != "= = = boardValuesStart = = =") return;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    board[i, j].value = Int32.Parse(reader.ReadLine());
                }
            }

            if (reader.ReadLine() != "= = = boardValuesEnd = = =") return;

            if (reader.ReadLine() == "seedStart")
            {
                gameSeed = reader.ReadLine();
            }
        }
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }

    public string generateBoardStateString()
    {
        string output = "";
        for ( int i = 0; i < width ; i++)
        {
            for ( int j = 0; j < height; j++)
            {
                output += board[i, j].value + "\n";
            }
        }
        
        return output;
    }

    public void ResetPiece(NodePiece piece)
    {
        piece.ResetPosition();
        update.Add(piece);
    }

    public void FlipPieces(Point one, Point two, bool main)
    {
        if (getValueAtPoint(one) < 0) return;

        Node nodeOne = getNodeAtPoint(one);
        NodePiece pieceOne = nodeOne.getPiece();
        if (getValueAtPoint(two) > 0)
        {
            Node nodeTwo = getNodeAtPoint(two);
            NodePiece pieceTwo = nodeTwo.getPiece();
            nodeOne.SetPiece(pieceTwo);
            nodeTwo.SetPiece(pieceOne);

            if(main)
                flipped.Add(new FlippedPieces(pieceOne, pieceTwo));

            update.Add(pieceOne);
            update.Add(pieceTwo);
        }
        else
            ResetPiece(pieceOne);
    }

    void KillPiece(Point p)
    {
        List<KilledPiece> available = new List<KilledPiece>();
        for (int i = 0; i < killed.Count; i++)
            if (!killed[i].falling) available.Add(killed[i]);

        KilledPiece set = null;
        if (available.Count > 0)
            set = available[0];
        else
        {
            GameObject kill = GameObject.Instantiate(killedPiece, killedBoard);
            KilledPiece kPiece = kill.GetComponent<KilledPiece>();

            killed.Add(kPiece);
        }

        int val = getValueAtPoint(p) - 1;
        if (set != null && val >= 0 && val < pieces.Length)
            set.Initialize(pieces[val], getPositionFromPoint(p));
    }
    

    List<Point> isConnected(Point p, bool main)
    {
        List<Point> connected = new List<Point>();
        int val = getValueAtPoint(p);
        Point[] directions =    // Kolejnosc kierunkow jest wazna
        {
            Point.up,
            Point.right,
            Point.down,
            Point.left
        };

        foreach (Point dir in directions) // sprawdzanie czy jest 2 lub wiecej ksztaltow w kierunkach OXOOOX
        {
            List<Point> line = new List<Point>();

            int same = 0;
            for (int i = 1; i < 3; i++)
            {
                Point check = Point.add(p, Point.mult(dir, i));
                if(getValueAtPoint(check) == val)
                {
                    line.Add(check);
                    same++;
                }
            }

            if (same > 1)   // jezeli jest wiecej niz 1 ksztalt w kierunku to wiemy ze jest to match
            {
                AddPoints(ref connected, line); // dodaj te punkty do overarching connected list
            }
        }

        for (int i = 0; i < 2; i++)  // sprawdzanie pomiedzy
        {
            List<Point> line = new List<Point>();

            int same = 0;
            Point[] check =
            {
                Point.add(p, directions[i]),
                Point.add(p, directions[i + 2])
            };

            foreach (Point next in check)   //sprawdz obie strony itemka, jezeli sa takie same to dodaj je do listy
            {
                if (getValueAtPoint(next) == val)
                {
                    line.Add(next);
                    same++;
                }
            }

            if (same > 1)
                AddPoints(ref connected, line);
        }

        for(int i = 0; i < 4; i++)  // Check for a 2x2
        {
            List<Point> square = new List<Point>();

            int same = 0;
            int next = i + 1;
            if (next >= 4)
                next -= 4;

            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[next]), Point.add(p, Point.add(directions[i], directions[next])) };
            foreach (Point pnt in check)   //sprawdz obie strony itemka, jezeli sa takie same to dodaj je do listy
            {
                if (getValueAtPoint(pnt) == val)
                {
                    square.Add(pnt);
                    same++;
                }
            }

            if (same > 2)
                AddPoints(ref connected, square);
        }

        if (main) // sprawdza za innymi obok aktualnego matcha
        {
            for (int i = 0; i < connected.Count; i++)
                AddPoints(ref connected, isConnected(connected[i], false));
        }
        
        return connected;
    }

    void AddPoints(ref List<Point> points, List<Point> add)
    {
        foreach(Point p in add)
        {
            bool doAdd = true;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Equals(p))
                {
                    doAdd = false;
                    break;
                }
            }

            if (doAdd) points.Add(p);
        }
    }

    int getValueAtPoint(Point p)
    {
        if (p.x < 0 || p.x >= width || p.y < 0 || p.y >= height) return -1;
        return board[p.x, p.y].value;
    }

    void setValueAtPoint(Point p, int v)
    {
        board[p.x, p.y].value = v;
    }

    Node getNodeAtPoint(Point p)
    {
        return board[p.x, p.y];
    }

    int newValue(ref List<int> remove)
    {
        List<int> available = new List<int>();
        for(int i = 0; i < pieces.Length; i++)
            available.Add(i + 1);
        foreach (int i in remove)
            available.Remove(i);

        if (available.Count <= 0) return 0;
        return available[random.Next(0, available.Count)];
    }

    int fillPiece()
    {
        int val = 1;
        val = (random.Next(0, 100) / (100 / pieces.Length + 1)) + 1;
        return val;
    }


    string getRandomSeed()
    {
        if (gameSeed == "blank")
        {
            string seed = "";
            string acceptableChars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890!@#$%^&*()";
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            for (int i = 0; i < 20; i++)
            {
                seed += acceptableChars[UnityEngine.Random.Range(0, acceptableChars.Length)];
            }
            seed += milliseconds.ToString();
            //Debug.Log("seed: " + seed);
            gameSeed = seed;
            return seed;
        }
        else
        {
            return gameSeed;
        }

    }

    public Vector2 getPositionFromPoint(Point p)
    {
        return new Vector2(tileSize/2 + (tileSize * p.x), -tileSize/2 - (tileSize * p.y));
    }
}


[System.Serializable]
public class Node
{
    public int value; // 0 - blank, 1 - cube, 2 - sphere, 3 - cylinder, 4 - diamond, -1 - hole
    public Point index;
    NodePiece piece;

    public Node(int v, Point i)
    {
        value = v;
        index = i;
    }

    public void SetPiece(NodePiece p)
    {
        piece = p;
        value = (piece == null) ? 0 : piece.value;
        if (piece == null) return;
        piece.SetIndex(index);
    }

    public NodePiece getPiece()
    {
        return piece;
    }
}

[System.Serializable]
public class FlippedPieces
{
    public NodePiece one;
    public NodePiece two;

    public FlippedPieces(NodePiece o, NodePiece t)
    {
        one = o; two = t;
    }

    public NodePiece getOtherPiece(NodePiece p)
    {
        if (p == one)
            return two;
        else if (p == two)
            return one;
        else
            return null;
    }
}