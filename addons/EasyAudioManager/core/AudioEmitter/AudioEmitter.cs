using Godot;

[GlobalClass]
public partial class AudioEmitter : AudioEmitterBase
{
    protected override async void ExecutePlay(AudioClip clip)
    {
        GD.Print("AudioEmitter Play: " + clip.Name);
        var player = AudioManager.Instance.HandlePlaySFX<AudioStreamPlayer>(clip);
        if (player == null)
        {
            GD.PrintErr("AudioEmitter: Failed to play SFX '" + clip.Name + "'");
            return;
        }
        AddChild(player);
        player.Play();

        await ToSignal(player, "finished");
    }
}