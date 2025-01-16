#if TOOLS
using Godot;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fallen.LoggerSystem

{
[Tool]
public partial class TagInput : Control
{
	LineEdit lineEdit;
	Button enterButton;
	TabContainer tabContainer;

	PackedScene logTagTable = GD.Load<PackedScene>("res://addons/logger_plugin/scenes/log_tag_table.tscn");

    public void Init()
	{
		lineEdit = GetNode<LineEdit>("./LineEdit");
		enterButton = GetNode<Button>("./EnterButton");
		tabContainer = GetNode<TabContainer>("./TagTabs");

		enterButton.Pressed += CreateTag;

		LoadJson();
	}

    public override void _ExitTree()
    {
        enterButton.Pressed -= CreateTag;
    }

    public void LoadJson()
	{
        foreach (var kvp in LogTagProfiler.GetProfiles())
		{
			TagProfile p = new TagProfile();
			p.enabled = kvp.Value.enabled;
			p.logToFile = kvp.Value.logToFile;

			var table = logTagTable.Instantiate();
			table.Name = kvp.Key;
			tabContainer.AddChild(table);
		}
	}

	private void CreateTag()
	{
		if(lineEdit.Text == null || lineEdit.Text == "")
			{
				GD.PrintErr("Invalid Tag Name");
				return;
			}
		LogTagProfiler.CreateProfile(lineEdit.Text);
		var table = logTagTable.Instantiate();
		table.Name = lineEdit.Text;
		tabContainer.AddChild(table);

		foreach(var kvp in LogTagProfiler.GetProfiles())
		{
			GD.Print(kvp.Key);
		}
	}

	
}

}
#endif