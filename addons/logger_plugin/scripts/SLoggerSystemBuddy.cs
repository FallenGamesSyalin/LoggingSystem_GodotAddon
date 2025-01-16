using Godot;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fallen.LoggerSystem
{
public partial class SLoggerSystemBuddy : Node
{
    public static SLoggerSystemBuddy Instance {get; private set;}
    public Dictionary<string, TagProfile> s_jsonMap = new Dictionary<string, TagProfile>();

    long lastTicks = 0;

    public override void _Ready()
    {
        Instance = this;
        s_jsonMap = LogTagProfiler.GetProfiles();
        lastTicks = File.GetLastAccessTime("addons/logger_plugin/log_tag_profiles.json").Ticks;
    }

    public override void _Process(double delta)
    {
        if(lastTicks != File.GetLastAccessTime("addons/logger_plugin/log_tag_profiles.json").Ticks)
        {
            OnChange();
            lastTicks = File.GetLastAccessTime("addons/logger_plugin/log_tag_profiles.json").Ticks;
        }

    }

    private void OnChange()
    {
        GD.Print("Changed");
        s_jsonMap = LogTagProfiler.FromJson();
    }

}
}
