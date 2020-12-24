namespace MinecraftNet.MenuSystem
{
    public interface IMenu: IMouseCatcher, IKeyboardCatcher, IStaticRenderable
    {
        IUnalignedContainerControl InnerContainer { get; set; }
    }   
}
