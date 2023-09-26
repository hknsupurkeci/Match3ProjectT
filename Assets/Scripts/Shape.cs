using UnityEngine;

[CreateAssetMenu(fileName = "NewShape", menuName = "Shape", order = 1)]
public class Shape : ScriptableObject
{
    public int width;
    public int height;
    public int[] shapeArray;

    public int[,] GetShapeArray()
    {
        int[,] twoDArray = new int[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                twoDArray[i, j] = shapeArray[i * width + j];
            }
        }
        return twoDArray;
    }
}
