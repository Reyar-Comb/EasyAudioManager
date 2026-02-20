using Godot;

[GlobalClass]
public partial class AudioClip : Resource
{
    [Export] public string Name;

    [Export(PropertyHint.ResourceType, "AudioStream,CustomStream")] 
    public Resource Stream;
}