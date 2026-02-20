using Godot;
using System.Collections.Generic;

public partial class AudioManager : Node
{
    public static AudioManager Instance { get; private set; }
    private readonly Queue<AudioStreamPlayer> _pool = new ();
    public override void _Ready()
    {
        Instance = this;
    }
}