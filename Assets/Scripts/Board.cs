using UnityEngine;

public class Board
{
    public GameObject[,] boardArray;
    public int width;
    public int height;

    public Board(int width, int height)
    {
        this.width = width;
        this.height = height;
        boardArray = new GameObject[height, width];
    }

    public GameObject GetTileAt(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return boardArray[y, x];
        }
        return null;
    }

    public void SetTileAt(int x, int y, GameObject tile)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            boardArray[y, x] = tile;
        }
    }
}
