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
}
