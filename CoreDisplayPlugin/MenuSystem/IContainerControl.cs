namespace MinecraftNet.MenuSystem
{
    public interface IUnalignedContainerControl: IUnalignedControl
    {
        ObservedCollection<IControl> Children { get; set; }
    }
    public interface IContainerControl: IUnalignedContainerControl, IControl { }
}
