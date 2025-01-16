#if TOOLS
using Godot;
using System;
using System.Diagnostics;

namespace Fallen.LoggerSystem
{
[Tool]
public partial class Interface : Control
{
	
	private CheckBox logFileCheck;

	private TagInput tagInput;

	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
	{
		GD.Print("Interface");
		tagInput = GetNode<TagInput>("./TagInput");
		logFileCheck = GetNode<CheckBox>("Control/Check");

		LogTagProfiler.Init();
		tagInput.Init();
	}

    public override void _ExitTree()
    {
		LogTagProfiler.Shutdown();
    }
    }
}
#endif