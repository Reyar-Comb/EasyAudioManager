using Godot;

[GlobalClass]
public partial class AudioEmitter : AudioEmitterBase
{
    protected override void ExecutePlay(AudioClip clip)
    {
        GD.Print("AudioEmitter Play: " + clip.Name);
    }
}