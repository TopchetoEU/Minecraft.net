using System;
using System.Collections.Generic;
using System.Text;

namespace NetGL.GraphicsAPI
{
    public interface IMatrix<T>: 
        IAddable<IMatrix<T>, IMatrix<T>>,
        ISubtractable<IMatrix<T>, IMatrix<T>>,
        IMultipilable<IMatrix<T>, IMatrix<T>>,
        IMultipilable<IVector<T>, IVector<T>>,
        IMultipilable<T, IMatrix<T>>,
        IDivideable<T, IMatrix<T>> where T : unmanaged
    {
        IMatrix<T> Inverse();
        T Determinant { get; }

        T[] Flattern();
    }
}
