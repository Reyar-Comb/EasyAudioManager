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
    private readonly Queue<AudioStreamPlayer2D> _pool2D = new ();
    private readonly Queue<AudioStreamPlayer3D> _pool3D = new ();

    private readonly AudioStreamPlayer[] _bgmPlayers = new AudioStreamPlayer[2];
    private int _activeBgmIndex = 0;

    private AudioStreamPlayer CurrentBgmPlayer => _bgmPlayers[_activeBgmIndex];
    private AudioStreamPlayer NextBgmPlayer => _bgmPlayers[1 - _activeBgmIndex];

    public bool Is2DEnabled => (bool)ProjectSettings.GetSetting("audio/easy_audio_manager/2d_player_enabled", true);
    public bool Is3DEnabled => (bool)ProjectSettings.GetSetting("audio/easy_audio_manager/3d_player_enabled", true);
    public int MaxSFXPolyphony => (int)ProjectSettings.GetSetting("audio/easy_audio_manager/max_sfx_polyphony", 10);

    public const string SingletonName = "AudioManager";

    public Dictionary<string, List<object>> ActivePlayers = new ();

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

        if (Is2DEnabled)
        {
            for (int i = 0; i < playerCount; i++)
            {
                var player2D = new AudioStreamPlayer2D();
                AddChild(player2D);
                _pool2D.Enqueue(player2D);
                player2D.Bus = "SFX";
            }
        }

        if (Is3DEnabled)
        {
            for (int i = 0; i < playerCount; i++)
            {
                var player3D = new AudioStreamPlayer3D();
                AddChild(player3D);
                _pool3D.Enqueue(player3D);
                player3D.Bus = "SFX";
            }
        }

        for (int i = 0; i < _bgmPlayers.Length; i++)
        {
            var bgmPlayer = new AudioStreamPlayer();
            AddChild(bgmPlayer);
            _bgmPlayers[i] = bgmPlayer;
            bgmPlayer.Bus = "BGM";
        }
    }

    // Methods getting players from pool
    private AudioStreamPlayer GetPlayer()
    {
        if (_pool.Count > 0)
            return _pool.Dequeue();
        else
        {
            GD.PrintErr("AudioManager: No available AudioStreamPlayer in pool, consider increasing pool size.");
            return null;            
        }
    }

    private AudioStreamPlayer2D GetPlayer2D()
    {
        if (!Is2DEnabled)
        {
            GD.PrintErr("AudioManager: 2D AudioStreamPlayers are disabled in settings.");
            return null;
        }

        if (_pool2D.Count > 0)
            return _pool2D.Dequeue();
        else
        {
            GD.PrintErr("AudioManager: No available AudioStreamPlayer2D in pool, consider increasing pool size.");
            return null;            
        }
    }

    private AudioStreamPlayer3D GetPlayer3D()
    {
        if (!Is3DEnabled)
        {
            GD.PrintErr("AudioManager: 3D AudioStreamPlayers are disabled in settings.");
            return null;
        }

        if (_pool3D.Count > 0)
            return _pool3D.Dequeue();
        else
        {
            GD.PrintErr("AudioManager: No available AudioStreamPlayer3D in pool, consider increasing pool size.");
            return null;            
        }
    }

    private void ConfigurePlayer(object player, AudioClip clip)
    {
        if (clip.Stream is CustomStream)
        {
            GD.PrintErr("AudioManager: CustomStream type is not supported yet.");
            return;
        }
        else if (clip.Stream is AudioStream stream)
        {
            if (player is AudioStreamPlayer p)
                p.Stream = stream;
            else if (player is AudioStreamPlayer2D p2D)
                p2D.Stream = stream;
            else if (player is AudioStreamPlayer3D p3D)
                p3D.Stream = stream;
        }
        else
        {
            GD.PrintErr("AudioManager: Unsupported AudioStream type in AudioClip.");
        }
    }

    public T HandlePlaySFX<T>(AudioClip clip) where T : Node
    {
        // Get player based on emitter
        Node player = null;
        if (typeof(T) == typeof(AudioStreamPlayer))
            player = GetPlayer();
        else if (typeof(T) == typeof(AudioStreamPlayer2D))
            player = GetPlayer2D();
        else if (typeof(T) == typeof(AudioStreamPlayer3D))
            player = GetPlayer3D();
        else
        {
            GD.PrintErr("AudioManager: Unsupported player type requested in HandlePlaySFX.");
            return null;
        }

        if (player == null)
        {
            GD.PrintErr("AudioManager: No available player for SFX, cannot play.");
            return null;
        }

        // Handle clip polyphony logic
        if (clip.PlayMode == AudioClip.PlayModeEnum.OneShot)
        {
            if (ActivePlayers.ContainsKey(clip.Name) && ActivePlayers[clip.Name].Count > 0)
            {
                GD.Print($"AudioManager: SFX '{clip.Name}' is already playing and marked as PlayOnce, skipping.");
                return null;   
            }
            else
            {
                if (ActivePlayers.ContainsKey(clip.Name))
                    ActivePlayers[clip.Name].Add(player);
                else
                    ActivePlayers[clip.Name] = new List<object> { player };
            }
        }
        else
        {
            if (!PlayingSFX.ContainsKey(clip.Name))
                PlayingSFX[clip.Name] = 1;
            else
            {
                if (PlayingSFX[clip.Name] < MaxSFXPolyphony)
                    PlayingSFX[clip.Name]++;
                else
                {
                    GD.Print($"AudioManager: SFX '{clip.Name}' has reached max polyphony limit, skipping.");
                    return null;
                }
            }
        }

        

        // Configure player
        ConfigurePlayer(player, clip);
        return (T)player;
    }

    public void StopPlayer

}