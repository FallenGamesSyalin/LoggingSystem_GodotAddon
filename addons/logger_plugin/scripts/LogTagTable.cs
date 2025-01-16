#if TOOLS
using Godot;
using System;
using System.IO;

namespace Fallen.LoggerSystem
{
[Tool]
public partial class LogTagTable : Control
{
	CheckBox enableCheck;
	CheckBox loggabeCheck;
	CheckBox verboseCheck;
	Button deleteButton;


	public override void _EnterTree()
	{
		GD.Print(Name + " Tag Created");
		enableCheck = GetNode<CheckBox>("Control/BoxContainer/EnableCheck");
		loggabeCheck =  GetNode<CheckBox>("Control/BoxContainer/LoggableCheck");
		verboseCheck =  GetNode<CheckBox>("Control/BoxContainer/VerboseCheck");

		deleteButton = GetNode<Button>("Control/DeleteButton");

		deleteButton.Pressed += DeleteThisObject;

		enableCheck.Toggled += UpdateProfile;
		loggabeCheck.Toggled += UpdateProfile;
		verboseCheck.Toggled += UpdateProfile;


		enableCheck.ButtonPressed = LogTagProfiler.GetProfile(Name).enabled;
		loggabeCheck.ButtonPressed = LogTagProfiler.GetProfile(Name).logToFile;
		verboseCheck.ButtonPressed = LogTagProfiler.GetProfile(Name).verbose;

		CreateLogTag(Name);
	}

	private void UpdateProfile(bool val)
	{
		TagProfile p = new TagProfile();
		p.enabled = enableCheck.ButtonPressed;
		p.logToFile = loggabeCheck.ButtonPressed;
		p.verbose = verboseCheck.ButtonPressed;
		LogTagProfiler.UpdateJson(Name, p);
	}

	public  void DeleteThisObject()
	{
		deleteButton.Pressed -= DeleteThisObject;

		enableCheck.Toggled -= UpdateProfile;
		loggabeCheck.Toggled -= UpdateProfile;
		verboseCheck.Toggled -= UpdateProfile;

		LogTagProfiler.DeleteProfile(Name);
		DeleteLogTag(Name);
		GetParent().RemoveChild(this);

		QueueFree();
	}

	private void CreateLogTag(string tagName)
	{
		string data = File.ReadAllText("addons/logger_plugin/scripts/LogTags.cs");
		string newData = data.Replace("\n}", $"\npublic static string {tagName} = \"{tagName}\"; \n}}");
		if(data.Contains($"\npublic static string {tagName} = \"{tagName}\";"))
			return;
		File.WriteAllText("addons/logger_plugin/scripts/LogTags.cs", newData);
	}

	private void DeleteLogTag(string tagName)
	{
		string data = File.ReadAllText("addons/logger_plugin/scripts/LogTags.cs");
		string newData = data.Replace($"public static string {tagName} = \"{tagName}\";", "");
		File.WriteAllText("addons/logger_plugin/scripts/LogTags.cs", newData);
	}
}
}
#endif