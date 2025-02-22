using System.Security.Cryptography;

namespace Lab1;

public class MatrixUtilities
{
    private static Random _random = Random.Shared;
    
    public static void PrintMatrix<T>(Matrix<T> matrix)
    {
        var elements = matrix.GetElements();
        for (int i = 0; i < elements.Length; i++)
        {
            for (int j = 0; j < elements[i].Length; j++)
            {
                if(typeof(T) == typeof(bool))
                    Console.Write((bool)(object)elements[i][j] ? "1 " : "0 ");
                if(typeof(T) == typeof(float))
                {
                    float number = (float)(object)elements[i][j];
                    Console.Write((number % 1 == 0 
                        ? number.ToString("0") 
                        : number.ToString("0.###"))
                    +" ");
                }
            }
            Console.WriteLine();
        }
    }

    public static Matrix<bool> GenerateBinaryMatrix(int height, int width)
    {
        bool[][] elements = new bool[height][];
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i] = new bool[width];
            for (int j = 0; j < width; j++)
            {
                elements[i][j] = _random.Next(0, 100) > 50;
            }
        }
        
        return new Matrix<bool>(elements);
    }

    public static FloatMatrix BoolMatrixToFloat(Matrix<bool> matrix)
    {
        var elements = matrix.GetElements();
        float[][] resultElements = new float[elements.Length][];
        for (int i = 0; i < elements.Length; i++)
        {
            resultElements[i] = new float[elements[i].Length];
            for(int j = 0; j < elements[i].Length; j++)
                resultElements[i][j] = elements[i][j] ? 1 : 0;
        }

        return new FloatMatrix(resultElements);
    }

    public static FloatMatrix Addition(FloatMatrix matrix1, FloatMatrix matrix2)
    {
        var elements1 = matrix1.GetElements();
        var elements2 = matrix2.GetElements();
        
        if(elements1.Length != elements2.Length)
        {
            Console.Error.WriteLine("Матрицы не равны по размерам!");
            throw new Exception();
        }
        
        float[][] resultElements = new float[elements1.Length][];
        
        for (int i = 0; i < elements1.Length; i++)
        {
            if(elements1[i].Length != elements2[i].Length)
            {
                Console.Error.WriteLine("Матрицы не равны по размерам!");
                throw new Exception();
            }
            
            resultElements[i] = new float[elements1[i].Length];
            
            for (int j = 0; j < elements2[i].Length; j++)
            {
                resultElements[i][j] = elements1[i][j] + elements2[i][j];
            }
        }
        
        return new FloatMatrix(resultElements);
    }

    public static FloatMatrix GenerateIdentityMatrix(int size)
    {
        var resultElements = new float[size][];
        for (int i = 0; i < size; i++)
        {
            resultElements[i] = new float[size];
            resultElements[i][i] = 1;
        }
        
        return new FloatMatrix(resultElements);
    }

    public static FloatMatrix GetStochasticTransitionMatrix(Matrix<bool> matrix)
    {
        var elements = matrix.GetElements();
        float[][] resultElements = new float[elements.Length][];
        int width = elements.Length;

        for (int i = 0; i < elements.Length; i++)
        {
            resultElements[i] = new float[width];
        }
        
        for (int j = 0; j < width; j++)
        {
            float columnSum = 0;
            for(int i = 0; i < elements.Length; i++)
                if (elements[i][j])
                    columnSum += 1;

            if (columnSum == 0)
                continue;
            
            else if (columnSum == 1)
            {
                for(int i = 0; i < elements.Length; i++)
                    if(elements[i][j])
                        resultElements[i][j] = 1;
                continue;
            }
            
            for(int i = 0; i < elements.Length; i++)
                if(elements[i][j])
                    resultElements[i][j] = 1/columnSum;
        }
        
        return new FloatMatrix(resultElements);
    }

    public static void RemoveRow<T>(Matrix<T> matrix, int rowIndex)
    {
        var elements = matrix.GetElements();
        if (rowIndex >= elements.Length)
        {
            throw new Exception();
        }
        
        matrix.SetElements(elements.Where((val, index) => index != rowIndex).ToArray()); 
    }

    public static void SwapRows<T>(Matrix<T> matrix, int rowIndex1, int rowIndex2)
    {
        var elements = matrix.GetElements();
        (elements[rowIndex1], elements[rowIndex2]) = (elements[rowIndex2], elements[rowIndex1]);
    }
    
    public static void SwapColumns<T>(Matrix<T> matrix, int columnsIndex1, int columnIndex2)
    {
        var elements = matrix.GetElements();
        foreach (var row in elements)
        {
            (row[columnsIndex1], row[columnIndex2]) = (row[columnIndex2], row[columnsIndex1]);
        }
    }

    public static void AddColumn<T>(Matrix<T> matrix, T[][] newElements)
    {
        if (newElements[0].Length != 1)
        {
            Console.WriteLine("Passed more than one row!");
            throw new Exception();
        }
        var elements = matrix.GetElements();
        for (int i = 0; i < newElements.Length; i++)
        {
            var newArray = elements[i].Concat(newElements[i]).ToArray();
            elements[i] = newArray;
        }
    }
}