namespace NetGL
{
    public interface IMultipilable<InT, OutT>
    {
        public OutT Multiply(InT obj);
        public static OutT operator *(IMultipilable<InT, OutT> a, InT b)
        {
            return a.Multiply(b);
        }
    }
}
