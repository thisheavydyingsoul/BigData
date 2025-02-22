using System.Runtime.CompilerServices;

namespace Lab1;

public class FirstTaskClass
{
    public static void PageRankWithoutTeleportation(Matrix<bool> taskGraph)
    {
        Console.WriteLine("1. Вычислить PageRank всех страниц в соответствии с\nматрицей используя метод Гаусса, в предположении, что телепортации нет\n");
        PrintCondition(taskGraph);
        Console.WriteLine("");
        
        PrintPageRankEquation();
        
        Console.WriteLine("P:");
        FloatMatrix PMatrix = MatrixUtilities.GetStochasticTransitionMatrix(taskGraph);
        PMatrix = PMatrix - MatrixUtilities.GenerateIdentityMatrix(PMatrix.GetElements().Length);
        MatrixUtilities.PrintMatrix(PMatrix);
        
        Console.WriteLine("\nPr = 0\n");
        for (int i = 0; i < taskGraph.GetElements().Length - 1; i++)
        {
            Console.Write($"r{i + 1} + ");
        }
        Console.Write($"r{taskGraph.GetElements().Length} = 1");
        
        var bElements = new float[taskGraph.GetElements().Length][];
        for (int i = 0; i < taskGraph.GetElements().Length; i++)
        {
            bElements[i] = new float[1];
        }
        FloatMatrix bMatrix = new FloatMatrix(bElements);
        Console.WriteLine("\nr:");
        MatrixUtilities.PrintMatrix(bMatrix);
        
        TriangulateMatrix(PMatrix, bMatrix);
        Console.WriteLine("\nПриведенная к треугольному виду матрица М:");   
        MatrixUtilities.PrintMatrix(PMatrix);
        /*Console.WriteLine("\nМатрица b:");
        MatrixUtilities.PrintMatrix(bMatrix);
        
        Console.WriteLine("\nr:");
        var r = Solution(MMatrix, bMatrix);
        MatrixUtilities.PrintMatrix(r);*/
    }

    public static void PageRankWithTeleportation(Matrix<bool> taskGraph)
    {
        Console.WriteLine("1. Вычислить PageRank всех страниц в соответствии с\nматрицей используя метод Гаусса, в предположении, что телепортация с B = 0.8\n");
        float B = 0.8f;
        PrintCondition(taskGraph);
        Console.WriteLine("");
        
        PrintPageRankEquation();
        
        Console.WriteLine("(I - BP)r = (1 - B)v\n");
        
        Console.WriteLine("I - BP\n");
        
        Console.WriteLine("P:");
        FloatMatrix PMatrix = MatrixUtilities.GetStochasticTransitionMatrix(taskGraph);
        MatrixUtilities.PrintMatrix(PMatrix);
        
        Console.WriteLine("\nM:");
        var initialMatrix = MatrixUtilities.GenerateIdentityMatrix(
            taskGraph.GetElements().Length
        );
        FloatMatrix MMatrix = initialMatrix - PMatrix * B;
        MatrixUtilities.PrintMatrix(MMatrix);
        
        Console.WriteLine("\nb = (1 - B)v");
        Console.WriteLine("v:");
        var vElements = new float[taskGraph.GetElements().Length][];
        for (int i = 0; i < taskGraph.GetElements().Length; i++)
        {
            vElements[i] = new float[1];
            vElements[i][0] = 1f/taskGraph.GetElements().Length;
        }
        var vMatrix = new FloatMatrix(vElements);
        MatrixUtilities.PrintMatrix(vMatrix);

        Console.WriteLine("\nb:");
        var bMatrix = vMatrix * (1 - B);
        MatrixUtilities.PrintMatrix(bMatrix);
        
        TriangulateMatrix(MMatrix, bMatrix);
        Console.WriteLine("\nПриведенная к треугольному виду матрица М:");   
        MatrixUtilities.PrintMatrix(MMatrix);
        Console.WriteLine("\nМатрица b:");
        MatrixUtilities.PrintMatrix(bMatrix);
        
        Console.WriteLine("\nr:");
        var r = Solution(MMatrix, bMatrix);
        MatrixUtilities.PrintMatrix(r);
    }
    
    private static void PrintCondition(Matrix<bool> initialGraph)
    {
        Console.WriteLine("Изначальная матрица:");
        MatrixUtilities.PrintMatrix(initialGraph);
    }

    private static void PrintPageRankEquation()
    {
        Console.WriteLine("Pr = r\n");
    }

    private static void TriangulateMatrix(FloatMatrix matrix, FloatMatrix bMatrix)
    {
        var bElements = bMatrix.GetElements();
        MatrixUtilities.AddColumn(matrix, bElements);
        var elements = matrix.GetElements();
        int maxWidth = elements[0].Length;
        for (int width = 0; width < maxWidth; width++)
        {
            elements[0][width] = 1;
        }
        
        MatrixUtilities.PrintMatrix(matrix);
        for (int height = 0; height < elements.Length; height++)
        {
            float multiplier = 1f ;
            float number = (float)(object)elements[height][height];
            Console.WriteLine((number % 1 == 0 
                              ? number.ToString("0") 
                              : number.ToString("0.###"))
                          + " " + height);
            elements[height][height] = 1;
            
            for(int i = height + 1; i < elements.Length; i++)
                elements[i][height] *= multiplier;
            
            for (int i = height + 1; i < elements.Length; i++)
            {
                if(elements[i][height] == 0)
                    continue;
                
                for(int k = 0; k < maxWidth; k++)
                {
                    elements[i][k] -= elements[i][k] * elements[height][k];
                }
                
                bElements[i][0] -= bElements[i][0] * bElements[height][0];
            }
        }
    }

    private static FloatMatrix Solution(FloatMatrix triangulatedMatrix, FloatMatrix bMatrix)
    {
        var elements = triangulatedMatrix.GetElements();
        var bElements = bMatrix.GetElements();
        var resultElements = new float[bElements.Length][];
        for (int i = 0; i < resultElements.Length; i++)
        {
            resultElements[i] = new float[1];
        }

        float sum = 0;
        for (int i = elements.Length - 1; i >= 0; i--)
        {
            resultElements[i][0] = bElements[i][0];
            for (int j = elements[i].Length - 1; j > i; j--)
            {
                resultElements[i][0] -= elements[i][j] * resultElements[j][0];
            }
            sum += resultElements[i][0];
        }

        var result = new FloatMatrix(resultElements);
        if (sum > 0)
        {
            result *= 1 / sum;
        }
        
        return result;
    }
}