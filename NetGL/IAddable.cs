namespace NetGL
{
    public interface IAddable<InT, OutT>
    {
        public OutT Add(InT obj);

        public static OutT operator +(IAddable<InT, OutT> a, InT b)
        {
            return a.Add(b);
        }
    }
}
