#if TOOLS
using Godot;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fallen.LoggerSystem
{

public struct TagProfile
{
	public bool enabled;
	public bool logToFile;
	public bool verbose;

	
}

[Tool]
public static class LogTagProfiler
{
	private const string pathToJson = "addons/logger_plugin/log_tag_profiles.json";

	private static Dictionary<string, TagProfile> s_jsonMap = new Dictionary<string, TagProfile>();

	public static void Init()
	{
		s_jsonMap = FromJson();
	}

	public static void Shutdown()
	{
		s_jsonMap.Clear();
	}

	public static void CreateProfile(string profileName)
	{
		GD.Print("Starting Profile");
		s_jsonMap = FromJson();
		if(s_jsonMap.ContainsKey(profileName))
			return;

		KeyValuePair<string, TagProfile> tagProfile = new KeyValuePair<string, TagProfile>(profileName, new TagProfile());
		s_jsonMap.Add(tagProfile.Key, tagProfile.Value);

		GD.Print("Profile Created");

		ToJson();
	}

	public static void DeleteProfile(string callerProfileName)
	{
		s_jsonMap.Remove(callerProfileName);

		var gdDict = new Godot.Collections.Dictionary<string, Godot.Collections.Dictionary<string, bool>>();

		foreach(var kvp in s_jsonMap)
		{
			gdDict.Add(kvp.Key, ToDict(kvp.Value.enabled, kvp.Value.logToFile, kvp.Value.verbose));
		}
		string data = Json.Stringify(gdDict);
		File.WriteAllText(pathToJson, data);
	}

	private static Godot.Collections.Dictionary<string, bool> ToDict(bool _enabled, bool _logToFile, bool _verbose)
	{
		return new Godot.Collections.Dictionary<string, bool>{
			  	{ "enabled", _enabled },
                { "log_to_file", _logToFile },
				{"verbose", _verbose}
		 };
	}

	public static TagProfile GetProfile(string profileName)
	{
		if(s_jsonMap.Count < 1)
			Init();
			
		return s_jsonMap[profileName];
	}

	public static Dictionary<string, TagProfile> GetProfiles()
	{
		if(s_jsonMap.Count < 1)
			Init();
			
		return s_jsonMap;
	}

	
	private static void ToJson()
	{
		if(IsFileLocked(new FileInfo(pathToJson)))
		{
			GD.Print("FILE IN USE");
			return;
		}

		var gdDict = new Godot.Collections.Dictionary<string, Godot.Collections.Dictionary<string, bool>>();

		foreach(var kvp in s_jsonMap)
		{
			gdDict.Add(kvp.Key, ToDict(kvp.Value.enabled, kvp.Value.logToFile, kvp.Value.verbose));
		}
		string data = Json.Stringify(gdDict);
		File.WriteAllText(pathToJson, data);
	}

	public static Dictionary<string, TagProfile> FromJson()
	{
		if(!File.Exists(pathToJson))
		{
			GD.Print("could not locate 'log_tag_profiles.json' at: " + pathToJson);
			return null;
		}

		string textData = File.ReadAllText(pathToJson);
		var json = Json.ParseString(textData);
		Dictionary<string, TagProfile> l = new Dictionary<string, TagProfile>();

        foreach (var kvp in json.AsGodotDictionary())
		{
			TagProfile p = new TagProfile();
			p.enabled = kvp.Value.AsGodotDictionary()["enabled"].AsBool();
			p.logToFile = kvp.Value.AsGodotDictionary()["log_to_file"].AsBool();
			p.verbose = kvp.Value.AsGodotDictionary()["verbose"].AsBool();
			l.Add(kvp.Key.AsString(), p);
		}
		return l;
	}

	public static void UpdateJson(string callerProfileName, TagProfile update)
	{
		s_jsonMap = FromJson();

		s_jsonMap[callerProfileName] = update;

		ToJson();
	}

	private static bool IsFileLocked(FileInfo file)
{
    try
    {
        using(FileStream stream = file.Open(FileMode.Open, System.IO.FileAccess.Read, FileShare.None))
        {
            stream.Close();
        }
    }
    catch (IOException)
    {
        return true;
    }

    //file is not locked
    return false;
}
}

}
#endif