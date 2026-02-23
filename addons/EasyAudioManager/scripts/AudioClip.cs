using Godot;

[GlobalClass]
public partial class AudioClip : Resource
{
    public enum TypeEnum
    {
        SFX,
        BGM
    }

    public enum PlayModeEnum
    {
        Normal,
        Loop,
        OneShot,
        OneShotInstance
    }
    [Export] 
    public string Name;

    [Export]
    public TypeEnum ClipType = TypeEnum.SFX;

    [Export(PropertyHint.ResourceType, "AudioStream,CustomStream")] 
    public Resource Stream;

    [Export]
    public PlayModeEnum PlayMode = PlayModeEnum.Normal;



    public string ResourceType => Stream.GetType().Name;
}