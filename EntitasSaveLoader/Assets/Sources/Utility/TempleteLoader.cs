using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface ITemplateLoader
{
    List<Tuple<string,string>> LoadSingleTemplateFile();
    List<Tuple<string, string>> LoadGroupTemplateFiles();
    Tuple<string, string> LoadSavedEntityFile(string saveFileName);
}

/// <summary>
/// load json text assets
/// </summary>
public class TemplateLoader : ITemplateLoader
{
    public List<Tuple<string, string>> LoadSingleTemplateFile()
    {
        var textAssets = Resources.LoadAll<TextAsset>("EntityTemplate/SingleEntity");

        List < Tuple<string, string> > dataTuple = new List<Tuple<string, string>>();
        foreach (var textAsset in textAssets)
        {
            dataTuple.Add(new Tuple<string, string>(textAsset.name, textAsset.text));
        }

        return dataTuple;
    }

    public List<Tuple<string, string>> LoadGroupTemplateFiles()
    {
        var textAssets = Resources.LoadAll<TextAsset>("EntityTemplate/GroupEntity");

        List<Tuple<string, string>> dataTuple = new List<Tuple<string, string>>();
        foreach (var textAsset in textAssets)
        {
            dataTuple.Add(new Tuple<string, string>(textAsset.name, textAsset.text));
        }

        return dataTuple;
    }

    public Tuple<string, string> LoadSavedEntityFile(string saveFileName)
    {
        var textAssets = Resources.Load<TextAsset>($"EntityTemplate/SaveFile/{saveFileName}");
        
        if (textAssets == null)
        {
            throw new Exception($"no save file : {saveFileName}");
        }

        return new Tuple<string, string>(textAssets.name, textAssets.text);
    }
}
