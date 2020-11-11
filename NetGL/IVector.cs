namespace NetGL
{
    public interface IVector<T> : 
        IAddable<IVector<T>, IVector<T>>,      ISubtractable<IVector<T>, IVector<T>>,
        IMultipilable<float, IVector<T>>,      IDivideable<float, IVector<T>> where T: unmanaged
    {
        T this[int index] { get; set; }

        float Length { get; set; }
        float LengthSquared { get; set; }

        T Dot(IVector<T> vector);

        T[] Flattern();
    }
}
