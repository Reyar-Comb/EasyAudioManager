using Godot;

[GlobalClass]
public partial class AudioEmitter : AudioEmitterBase
{
    protected override void ExecutePlay(Resource resource)
    {
        GD.Print("AudioEmitter Play");
    }
}