#if TOOLS
using Godot;
using System;

[Tool]
public partial class EasyAudioManager : EditorPlugin
{
	public override void _EnterTree()
	{
		SetupBuses();
		GD.Print("Plugin loaded: EasyAudioManager");
		// Initialization of the plugin goes here.
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.

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
}
#endif
