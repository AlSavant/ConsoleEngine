using DataModel.Math.Structures;

namespace ConsoleEngine.Editor.Services.Util
{
    internal interface IMatrixOperationsService : IService
    {
        T[] FlipMatrixHorizontally<T>(T[] matrix, int matrixWidth, int matrixHeight);
        T[] FlipMatrixVertically<T>(T[] matrix, int matrixWidth, int matrixHeight);
        T[] RotateMatrix180<T>(T[] matrix, int matrixWidth, int matrixHeight);
        T[] RotateMatrix90CW<T>(T[] matrix, int matrixWidth, int matrixHeight);
        T[] RotateMatrix90CCW<T>(T[] matrix, int matrixWidth, int matrixHeight);
        T[] ResizeMatrix<T>(T[] matrix, int matrixWidth, int matrixHeight, int newWidth, int newHeight, Vector2Int normalizedPivot);
    }
}
