namespace Lab1;

public class FloatMatrix : Matrix<float>
{
    public FloatMatrix(float[][] elements) : base(elements)
    {
    }

    public static FloatMatrix operator +(FloatMatrix a, FloatMatrix b)
    {
        return MatrixUtilities.Addition(a, b);
    }

    public static FloatMatrix operator -(FloatMatrix a, FloatMatrix b)
    {
        return MatrixUtilities.Addition(a, b * -1);
    }

    public static FloatMatrix operator *(FloatMatrix matrix, float multiplier)
    {
        var resultElements = new float[matrix._elements.Length][];
        
        for (int i = 0; i < matrix._elements.Length; i++)
        {
            resultElements[i] = new float[matrix._elements[i].Length];
            for(int j = 0; j < matrix._elements[i].Length; j++)
                resultElements[i][j] = matrix._elements[i][j] * multiplier;
        }
        
        return new FloatMatrix(resultElements);
    }
}