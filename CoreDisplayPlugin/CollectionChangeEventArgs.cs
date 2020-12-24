namespace MinecraftNet
{
    public delegate void CollectionChangeEventHandler<T>(object sender, CollectionChangeEventArgs<T> e);
    public class CollectionChangeEventArgs<T>
    {
        public int Index { get; }
        public T Element { get; }

        public CollectionChangeEventArgs(int index, T element)
        {
            Index = index;
            Element = element;
        }
    }
}
