using Godot;

[GlobalClass]
public partial class AudioEmitter2D : AudioEmitterBase
{
    protected override void ExecutePlay(AudioClip clip)
    {
        GD.Print("AudioEmitter2D Play: " + clip.Name);
    }
}