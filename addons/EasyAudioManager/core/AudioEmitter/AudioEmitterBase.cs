using Godot;

public abstract partial class AudioEmitterBase : Node
{
	[Export] public Godot.Collections.Dictionary<string, AudioClip> AudioDictionary = new ();

	protected abstract void ExecutePlay(AudioClip clip);

	public void Play(string name)
	{
		if (AudioDictionary.TryGetValue(name, out var clip))
		{
			ExecutePlay(clip);
		}
		else
		{
			GD.PrintErr($"AudioEmitter: AudioClip with name '{name}' not found in AudioDictionary.");
		}
	}

}
