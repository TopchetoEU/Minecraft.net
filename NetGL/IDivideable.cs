namespace NetGL
{
    public interface IDivideable<InT, OutT>
    {
        public OutT Divide(InT obj);
        public static OutT operator /(IDivideable<InT, OutT> a, InT b)
        {
            return a.Divide(b);
        }
    }
}
