#if TOOLS
using Godot;
using System;

[Tool]
public partial class AudioManagerDock : Control
{

	private SpinBox _poolSizeSpinBox;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_poolSizeSpinBox = GetNode<SpinBox>("VBoxContainer/HBoxContainer/SpinBox");
		_poolSizeSpinBox.ValueChanged += OnPoolSizeChanged;
		LoadSettings();
	}

	private void LoadSettings()
	{
		int currentSize = (int)ProjectSettings.GetSetting("audio/easy_audio_manager/pool_size", 20);
		_poolSizeSpinBox.Value = currentSize;
	}

	private void OnPoolSizeChanged(double value)
	{
		int intValue = (int)value;
		ProjectSettings.SetSetting("audio/easy_audio_manager/pool_size", intValue);
		ProjectSettings.Save();
		GD.Print($"Pool size updated to: {intValue}");
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

#endif
