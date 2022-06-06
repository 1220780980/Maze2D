using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// using UnityEngine.UI;

public class MazeGenerator : MonoBehaviour
{
    public int row, column, wallWidth;  //row=mapWidth, col=mapHeight
    public Transform walls, Tile_regular, Tile_current;
    public Vector3 start, end;

    private Cell[,] cells;
    // private Image[,] images;

    private List<Cell> cellList;
    private Queue<Cell> cellQueue;
    // private Coroutine coroutine;

    public struct Cell 
    {
        public int x;
        public int y;
        public int direction;

        public bool isVisited;

        public bool E, S, W, N;  // added
    }

    // public class Tuple
    // {
    //     public int Item1 { get; }
    //     public int Item2 { get; }

    //     public Tuple(int first, int second)
    //     {
    //         Item1 = first;
    //         Item2 = second;
    //     }
    // }

    // public class Cell
    // {
    //     public bool Visited;
    //     public bool E, S, W, N;

    //     public Cell(bool visited, bool e, bool s, bool w, bool n)
    //     {
    //         Visited = visited;
    //         E = e; S = s; W = w; N = n;
    //     }
    // }

    private Cell[] maze;
    private int visitedCells;
    // private Stack<Tuple> stack = new Stack<Tuple>();
    private List<Transform> previousCurrent = new List<Transform>();
    System.Random rnd = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        cellList = new List<Cell>();
        cellQueue = new Queue<Cell>();
        
        maze = new Cell[row * column];

        // for (int i = 0; i < maze.Length; i++)
        // {
        //     maze[i] = new Cell(false, false, false, false, false);
        // }

        // int x = rnd.Next(row); int y = rnd.Next(column);
        // stack.Push(new Tuple(x, y));
        // maze[y * row + x].Visited = true;
        // visitedCells = 1;
        start = new Vector3((float) 0.4, (float) 0.3, (float) 0.4);
        end = new Vector3((float)(row * 2 - 2), (float) 0.1, (float) (column * 2 - 2));

        CreateAndInit();
        DoRandomizedPrim();

        // InitializeMazeStructure();

        // while (visitedCells < row * column)
        // {
        //     RB_Algorithm();
        // }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Create and initialize maze
    private void CreateAndInit()
    {
        // images = new Image[row, column];
        cells = new Cell[row, column];

        for (int i = 0; i < row; i++) {
            for (int j = 0; j < column; j++) {
                // images[i, j] = Instantiate(imagePrefab, transform);
                // images[i, j].rectTransform.anchoredPosition = new Vector2(i * imagePrefab.rectTransform.sizeDelta.x, j * imagePrefab.rectTransform.sizeDelta.y);

                cells[i, j].x = i;
                cells[i, j].y = j;
                cells[i, j].direction = -1;
                cells[i, j].isVisited = false;

                cells[i, j].E = false;   // added
                cells[i, j].S = false;
                cells[i, j].W = false;
                cells[i, j].N = false;   // added
            }
        }

    for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < column; y++)
            {
                for (int py = 0; py < wallWidth; py++)
                {
                    for (int px = 0; px < wallWidth; px++)
                    {
                        Vector3 v3 = new Vector3(x * (wallWidth + 1) + px, 0, y * (wallWidth + 1) + py);
                        Instantiate(Tile_regular, v3, Quaternion.identity);
                        Vector3 v3_1 = new Vector3(x * (wallWidth + 1) + px, 0, y * (wallWidth + 1) + wallWidth);
                        Vector3 v3_2 = new Vector3(x * (wallWidth + 1) + wallWidth, 0, y * (wallWidth + 1) + px);
                        Instantiate(walls, v3_1, Quaternion.identity);
                        Instantiate(walls, v3_2, Quaternion.identity);
                    }
                }
                Instantiate(walls, new Vector3((x + 1) * (wallWidth + 1) - 1, 0, (y + 1) * (wallWidth + 1) - 1), Quaternion.identity);
            }
        }

        for (int i = 0; i < row * (wallWidth + 1); i++)
        {
            Instantiate(walls, new Vector3(i, 0, (column - column) * (wallWidth + 1) - 1), Quaternion.identity);
        }

        for (int j = -1; j < column * (wallWidth + 1); j++)
        {
            Instantiate(walls, new Vector3((row - row) * (wallWidth + 1) - 1, 0, j), Quaternion.identity);
        }
    }

    private void DoRandomizedPrim()
    {
        int times = 0;

        int x = UnityEngine.Random.Range(0, row);
        int y = UnityEngine.Random.Range(0, column);

        cells[x, y].isVisited = true;

        AddNewCellsToList(x, y);

        cellQueue.Enqueue(cells[x, y]);

        while (cellList.Count > 0) {
            int listIndex = UnityEngine.Random.Range(0, cellList.Count);

            int newX = -1, newY = -1;
            switch (cellList[listIndex].direction)
            {
                case 0:
                    if (cellList[listIndex].y + 1 < column && !cells[cellList[listIndex].x, cellList[listIndex].y + 1].isVisited)
                    {
                        newX = cellList[listIndex].x;
                        newY = cellList[listIndex].y + 1;
                    }
                    break;
                case 1:
                    if (cellList[listIndex].y - 1 >= 0 && !cells[cellList[listIndex].x, cellList[listIndex].y - 1].isVisited)
                    {
                        newX = cellList[listIndex].x;
                        newY = cellList[listIndex].y - 1;
                    }
                    break;
                case 2:
                    if (cellList[listIndex].x - 1 >= 0 && !cells[cellList[listIndex].x - 1, cellList[listIndex].y].isVisited)
                    {
                        newX = cellList[listIndex].x - 1;
                        newY = cellList[listIndex].y;
                    }
                    break;
                case 3:
                    if (cellList[listIndex].x + 1 < row && !cells[cellList[listIndex].x + 1, cellList[listIndex].y].isVisited)
                    {
                        newX = cellList[listIndex].x + 1;
                        newY = cellList[listIndex].y;
                    }
                    break;
                default:
                    print(cellList[listIndex].x + "_" + cellList[listIndex].y + " Fail");
                    break;
            }

            if (newX != -1 && newY != -1)
            {
                times++;

                AddNewCellsToList(newX, newY);

                cellQueue.Enqueue(cells[cellList[listIndex].x, cellList[listIndex].y]);
                cellQueue.Enqueue(cells[newX, newY]);

                cells[cellList[listIndex].x, cellList[listIndex].y].isVisited = true;
                cells[newX, newY].isVisited = true;

                // added


            }

            cellList.RemoveAt(listIndex);
        }

        DrawEverything();
    }

    private void AddNewCellsToList(int x, int y)
    {
        // TODO
        if (y + 1 < column && !cells[x, y + 1].isVisited)
        {
            cells[x, y + 1].direction = 0;
            cellList.Add(cells[x, y + 1]);
        }

        if (y - 1 >= 0 && !cells[x, y - 1].isVisited)
        {
            cells[x, y - 1].direction = 1;
            cellList.Add(cells[x, y - 1]);
        }

        if (x - 1 >= 0 && !cells[x - 1, y].isVisited)
        {
            cells[x - 1, y].direction = 2;
            cellList.Add(cells[x - 1, y]);
        }

        if (x + 1  < row && !cells[x + 1, y].isVisited)
        {
            cells[x + 1, y].direction = 3;
            cellList.Add(cells[x + 1, y]);
        }
    }


    // private void InitializeMazeStructure()
    // {
    //     for (int x = 0; x < row; x++)
    //     {
    //         for (int y = 0; y < column; y++)
    //         {
    //             for (int py = 0; py < wallWidth; py++)
    //             {
    //                 for (int px = 0; px < wallWidth; px++)
    //                 {
    //                     Vector3 v3 = new Vector3(x * (wallWidth + 1) + px, 0, y * (wallWidth + 1) + py);
    //                     Instantiate(Tile_regular, v3, Quaternion.identity);
    //                     Vector3 v3_1 = new Vector3(x * (wallWidth + 1) + px, 0, y * (wallWidth + 1) + wallWidth);
    //                     Vector3 v3_2 = new Vector3(x * (wallWidth + 1) + wallWidth, 0, y * (wallWidth + 1) + px);
    //                     Instantiate(walls, v3_1, Quaternion.identity);
    //                     Instantiate(walls, v3_2, Quaternion.identity);
    //                 }
    //             }
    //             Instantiate(walls, new Vector3((x + 1) * (wallWidth + 1) - 1, 0, (y + 1) * (wallWidth + 1) - 1), Quaternion.identity);
    //         }
    //     }

    //     for (int i = 0; i < row * (wallWidth + 1); i++)
    //     {
    //         Instantiate(walls, new Vector3(i, 0, (column - column) * (wallWidth + 1) - 1), Quaternion.identity);
    //     }

    //     for (int j = -1; j < column * (wallWidth + 1); j++)
    //     {
    //         Instantiate(walls, new Vector3((row - row) * (wallWidth + 1) - 1, 0, j), Quaternion.identity);
    //     }

    // }

    private uint lookAt(int px, int py)
    {
        return (uint)((cellQueue.Peek().x + px) + (cellQueue.Peek().y + py) * row);
    }

    // void RB_Algorithm()
    // {
    //     List<int> neighbours = new List<int>();

    //     if (visitedCells < row * column)
    //     {
    //         if (stack.Peek().Item2 > 0 && maze[lookAt(0, -1)].Visited == false)
    //             neighbours.Add(0);

    //         if (stack.Peek().Item1 < row - 1 && maze[lookAt(+1, 0)].Visited == false)
    //             neighbours.Add(1);

    //         if (stack.Peek().Item2 < column - 1 && maze[lookAt(0, +1)].Visited == false)
    //             neighbours.Add(2);

    //         if (stack.Peek().Item1 > 0 && maze[lookAt(-1, 0)].Visited == false)
    //             neighbours.Add(3);

    //         if (neighbours.Count > 0)
    //         {
    //             int nextCellDir = neighbours[rnd.Next(neighbours.Count)];

    //             switch (nextCellDir)
    //             {
    //                 case 0:
    //                     maze[lookAt(0, -1)].Visited = true; maze[lookAt(0, -1)].S = true;
    //                     maze[lookAt(0, 0)].N = true;
    //                     stack.Push(new Tuple(stack.Peek().Item1 + 0, stack.Peek().Item2 - 1));
    //                     break;

    //                 case 1:
    //                     maze[lookAt(+1, 0)].Visited = true; maze[lookAt(+1, 0)].W = true;
    //                     maze[lookAt(0, 0)].E = true;
    //                     stack.Push(new Tuple(stack.Peek().Item1 + 1, stack.Peek().Item2 + 0));
    //                     break;

    //                 case 2:
    //                     maze[lookAt(0, +1)].Visited = true; maze[lookAt(0, +1)].N = true;
    //                     maze[lookAt(0, 0)].S = true;
    //                     stack.Push(new Tuple(stack.Peek().Item1 + 0, stack.Peek().Item2 + 1));
    //                     break;

    //                 case 3:
    //                     maze[lookAt(-1, 0)].Visited = true; maze[lookAt(-1, 0)].E = true;
    //                     maze[lookAt(0, 0)].W = true;
    //                     stack.Push(new Tuple(stack.Peek().Item1 - 1, stack.Peek().Item2 + 0));
    //                     break;
    //             }
    //             visitedCells++;
    //         }
    //         else
    //         {
    //             stack.Pop();
    //         }

    //         DrawEverything();
    //     }
    // }

    // void DrawEverything()
    // {
    //     for (int x = 0; x < row; x++)
    //     {
    //         for (int y = 0; y < column; y++)
    //         {
    //             for (int p = 0; p < wallWidth; p++)
    //             {
    //                 Vector3 v3 = new Vector3(x * (wallWidth + 1) + p, 0, y * (wallWidth + 1) + wallWidth);
    //                 Vector3 v3_2 = new Vector3(x * (wallWidth + 1) + wallWidth, 0, y * (wallWidth + 1) + p);

    //                 // TODO
    //                 if (y + 1 < column) 
    //                     if (cells[x, y + 1].isVisited && checkIfTilePosEmpty(v3))     //maze[y * row + x].S && 
    //                         Instantiate(Tile_regular, v3, Quaternion.identity);
                      
                        

    //                 // TODO
    //                 if (x + 1 < row)
    //                     if (cells[x + 1, y].isVisited && checkIfTilePosEmpty(v3_2))  // maze[y * row + x].E &&
    //                         Instantiate(Tile_regular, v3_2, Quaternion.identity);
    //             }
    //         }
    //     }

    //     foreach (Transform t in previousCurrent)
    //     {
    //         if (t != null)
    //             Instantiate(Tile_regular, t.position, Quaternion.identity);
    //     }

    //     for (int py = 0; py < wallWidth; py++)
    //     {
    //         for (int px = 0; px < wallWidth; px++)
    //         {
    //             // TODO
    //             Vector3 v3 = new Vector3(cellQueue.Peek().x * (wallWidth + 1) + px, 0, cellQueue.Peek().y * (wallWidth + 1) + py);

    //             if (checkIfTilePosEmpty(v3))
    //                 previousCurrent.Add(Instantiate(Tile_current, v3, Quaternion.identity));
    //         }
    //     }

    //     // added
    //     for (int i = 0; i < row * (wallWidth + 1); i++)
    //     {
    //         Debug.Log("String" + i);
    //         Instantiate(walls, new Vector3(i, 0, (column - column) * (wallWidth + 1) - 1), Quaternion.identity);
    //     }

    //     for (int j = -1; j < column * (wallWidth + 1); j++)
    //     {
    //         Debug.Log("String" + j);
    //         Instantiate(walls, new Vector3((row - row) * (wallWidth + 1) - 1, 0, j), Quaternion.identity);
    //     }
    // }

    void DrawEverything()
    {
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < column; y++)
            {
                for (int py = 0; py < wallWidth; py++)
                {
                    for (int px = 0; px < wallWidth; px++)
                    {
                        Vector3 v3 = new Vector3(x * (wallWidth + 1) + px, 0, y * (wallWidth + 1) + py);
                        Instantiate(Tile_regular, v3, Quaternion.identity);
                        Vector3 v3_1 = new Vector3(x * (wallWidth + 1) + px, 0, y * (wallWidth + 1) + wallWidth);
                        Vector3 v3_2 = new Vector3(x * (wallWidth + 1) + wallWidth, 0, y * (wallWidth + 1) + px);
                        Instantiate(walls, v3_1, Quaternion.identity);
                        Instantiate(walls, v3_2, Quaternion.identity);
                    }
                }
                Instantiate(walls, new Vector3((x + 1) * (wallWidth + 1) - 1, 0, (y + 1) * (wallWidth + 1) - 1), Quaternion.identity);
            }
        }

        for (int i = 0; i < row * (wallWidth + 1); i++)
        {
            Instantiate(walls, new Vector3(i, 0, (column - column) * (wallWidth + 1) - 1), Quaternion.identity);
        }

        for (int j = -1; j < column * (wallWidth + 1); j++)
        {
            Instantiate(walls, new Vector3((row - row) * (wallWidth + 1) - 1, 0, j), Quaternion.identity);
        }

        while (cellQueue.Count > 0) {
            Debug.Log("String" + cellQueue.Count);
            Cell cell = cellQueue.Dequeue();

            for (int py = 0; py < wallWidth; py++) {
                for (int px = 0; px < wallWidth; px++) {
                    Vector3 v3 = new Vector3(cell.x * (wallWidth + 1) + px, 0, cell.y * (wallWidth + 1) + py);

                    if (checkIfTilePosEmpty(v3))
                    previousCurrent.Add(Instantiate(Tile_current, v3, Quaternion.identity));
                }
            }
        }
    }

    private bool checkIfTilePosEmpty(Vector3 targetPos)
    {
        GameObject[] allTilings = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject t in allTilings)
        {
            if (t.transform.position == targetPos)
            {
                Destroy(t);
            }
        }
        return true;
    }
}
