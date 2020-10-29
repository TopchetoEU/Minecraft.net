namespace NetGL
{
    public interface ISubtractable<InT, OutT>
    {
        public OutT Subtract(InT obj);
        public static OutT operator -(ISubtractable<InT, OutT> a, InT b)
        {
            return a.Subtract(b);
        }
    }
}
