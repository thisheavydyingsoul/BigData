using System.Diagnostics;
using Lab1;

const int SIZE = 10;

Matrix<bool> gaussMatrix = MatrixUtilities.GenerateBinaryMatrix(SIZE, SIZE);
FirstTaskClass.PageRankWithoutTeleportation(gaussMatrix);
/*FirstTaskClass.PageRankWithTeleportation(gaussMatrix);*/