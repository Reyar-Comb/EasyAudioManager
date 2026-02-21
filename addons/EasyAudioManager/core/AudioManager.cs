using Godot;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

public partial class AudioManager : Node
{
    public static AudioManager Instance { get; private set; }
    private readonly Queue<AudioStreamPlayer> _pool = new ();

    private readonly AudioStreamPlayer[] _bgmPlayers = new AudioStreamPlayer[2];
    private int _activeBgmIndex = 0;

    private AudioStreamPlayer CurrentBgmPlayer => _bgmPlayers[_activeBgmIndex];
    private AudioStreamPlayer NextBgmPlayer => _bgmPlayers[1 - _activeBgmIndex];

    private bool _is2DEnabled => (bool)ProjectSettings.GetSetting("audio/easy_audio_manager/2d_player_enabled", true);
    private bool _is3DEnabled => (bool)ProjectSettings.GetSetting("audio/easy_audio_manager/3d_player_enabled", true);

    public override void _Ready()
    {
        Instance = this;
        InitPlayers();
    }
    private void InitPlayers()
    {
        int playerCount = (int)ProjectSettings.GetSetting("audio/easy_audio_manager/pool_size", 20);
        for (int i = 0; i < playerCount; i++)
        {
            var player = new AudioStreamPlayer();
            AddChild(player);
            _pool.Enqueue(player);
            player.Bus = "SFX";
        }

        for (int i = 0; i < _bgmPlayers.Length; i++)
        {
            var bgmPlayer = new AudioStreamPlayer();
            AddChild(bgmPlayer);
            _bgmPlayers[i] = bgmPlayer;
            bgmPlayer.Bus = "BGM";
        }
    }



}