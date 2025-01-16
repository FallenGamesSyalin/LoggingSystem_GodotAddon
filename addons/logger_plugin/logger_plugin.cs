#if TOOLS
using Godot;
using System;

namespace Fallen.LoggerSystem
{
[Tool]
public partial class logger_plugin : EditorPlugin
{
	private Control _dock;

	public override void _EnterTree()
	{
		_dock = GD.Load<PackedScene>("res://addons/logger_plugin/scenes/interface.tscn").Instantiate<Control>();
        AddControlToDock(DockSlot.LeftBr, _dock);
	}

	public override void _ExitTree()
	{
		RemoveControlFromDocks(_dock);
        _dock.Free();
	}
}
}
#endif