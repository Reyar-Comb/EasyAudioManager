using Godot;

[GlobalClass]
public partial class AudioEmitter3D : AudioEmitterBase
{
    protected override void ExecutePlay(AudioClip clip)
    {
        GD.Print("AudioEmitter3D Play: " + clip.Name);
    }
}