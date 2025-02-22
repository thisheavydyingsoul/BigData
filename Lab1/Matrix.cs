namespace Lab1;

public class Matrix<T>
{
    protected T[][] _elements;

    public Matrix(T[][] elements)
    {
        _elements = elements;
    }
    
    public Type GetElementType() => typeof(T);
    
    public T[][] GetElements() => _elements;
    
    public void SetElements(T[][] elements) => _elements = elements;
}