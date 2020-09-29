namespace MinecraftNetWindow.Units
{
    public interface IInterpolateable
    {
        Keyframe ToKeyframe();
        IInterpolateable ApplyKeyframe(Keyframe keyframe);
    }
}
