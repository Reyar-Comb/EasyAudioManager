#if TOOLS
using Godot;
using System;

[Tool]
public partial class EasyAudioManager : EditorPlugin
{
    private const string SingletonName = "AudioManager";
	private const string SingletonPath = "res://addons/EasyAudioManager/core/AudioManager.cs";
	
    private EditorDock _dock = new();

	public override void _EnterTree()
	{
        AddAutoloadSingleton(SingletonName, SingletonPath);

        var dockScene = GD.Load<PackedScene>("res://addons/EasyAudioManager/gui/AudioManagerDock.tscn");
        _dock.Title = "Easy Audio Manager";
        _dock.DefaultSlot = EditorDock.DockSlot.LeftUl;
        _dock.AddChild(dockScene.Instantiate());
        AddDock(_dock);
        

		SetupBuses();
		GD.Print("Plugin loaded: EasyAudioManager");
		// Initialization of the plugin goes here.
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
        RemoveAutoloadSingleton(SingletonName);
        RemoveDock(_dock);
		GD.Print("Plugin unloaded: EasyAudioManager");

	}

	public void SetupBuses()
    {
        if (AudioServer.GetBusIndex("SFX") == -1)
        {
            AudioServer.AddBus();
            int index = AudioServer.GetBusCount() - 1;
            AudioServer.SetBusName(index, "SFX");
            AudioServer.SetBusSend(index, "Master");
        }
        if (AudioServer.GetBusIndex("BGM") == -1)
        {
            AudioServer.AddBus();
            int index = AudioServer.GetBusCount() - 1;
            AudioServer.SetBusName(index, "BGM");
            AudioServer.SetBusSend(index, "Master");

        }
    }

    public void SetupSettings()
    {
        if (!ProjectSettings.HasSetting("audio/easy_audio_manager/pool_size"))
        {
            ProjectSettings.SetSetting("audio/easy_audio_manager/pool_size", 20);
            ProjectSettings.Save();
        }

        if (!ProjectSettings.HasSetting("audio/easy_audio_manager/2d_player_enabled"))
        {
            ProjectSettings.SetSetting("audio/easy_audio_manager/2d_player_enabled", true);
            ProjectSettings.Save();
        }

        if (!ProjectSettings.HasSetting("audio/easy_audio_manager/3d_player_enabled"))
        {
            ProjectSettings.SetSetting("audio/easy_audio_manager/3d_player_enabled", true);
            ProjectSettings.Save();
        }
    }


}
#endif
