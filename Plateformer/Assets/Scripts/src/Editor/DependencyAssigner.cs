using System;
using System.IO;
using System.Linq;
using src.Log;
using UnityEditor;
using UnityEngine;

namespace src.Editor
{
    #if UNITY_EDITOR && !UniTask
    
    public class DependencyAssigner : AssetPostprocessor
    {
        void OnPreprocessAsset()
        {
            var path = Application.dataPath;
            path = path.Replace(path.Substring(path.LastIndexOf("/", StringComparison.Ordinal)), string.Empty);
            path += "/Packages/manifest.json";
            
            try
            {
                var file = File.ReadLines(path);
                var lines = file as string[] ?? file.ToArray();

                foreach (var line in lines)
                {
                    if (line.Contains("com.cysharp.unitask"))
                    {
                        Logs.Log("Successfully added UniTask to dependencies.. you may need to go click outside of unity and come back :)", LogType.Log, Logs.LogColors.Green);
                        ScriptingSymbols.AddScriptingSymbol("UniTask");
                        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                        EditorUtility.RequestScriptReload();
                        return;
                    }
                }

                var text = lines.ToList();

                for (int l = 0; l < text.Count; l++)
                {
                    if (text[l].Contains("dependencies"))
                    {
                        text[l] += "\"com.cysharp.unitask\": \"https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask#2.3.3\",";
                    }
                }

                File.WriteAllLines(path, text);
                Logs.Log("Successfully added UniTask to dependencies.. you may need to go click outside of unity and come back :)", LogType.Log, Logs.LogColors.Green);
                ScriptingSymbols.AddScriptingSymbol("UniTask");
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                EditorUtility.RequestScriptReload();
            }
            catch
            {
                throw new DependencyAssignerException(path);
            }
        }
    }
    
    #endif
}