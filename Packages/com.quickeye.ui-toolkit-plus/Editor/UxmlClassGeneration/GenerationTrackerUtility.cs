using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using System;

namespace QuickEye.UIToolkit.Editor
{
    internal static class GenerationTrackerUtility
    {
        [DataContract]
        public class GenerationTracker
        {
            [DataMember]
            public string UxmlGUID { get; set; }

            [DataMember]
            public string ScriptPath { get; set; }
            [DataMember]
            public string ScriptGUID { get; set; }
        }

        [DataContract]
        public class GenerationTrackerContainer
        {
            [DataMember]
            public List<GenerationTracker> Trackers = new List<GenerationTracker>();

            public string GetFixedPath(string assetGuid)
            {
                for (int i = 0; i < Trackers.Count; i++)
                {
                    if (Trackers[i].UxmlGUID == assetGuid)
                    {
                        if (string.IsNullOrEmpty(Trackers[i].ScriptGUID))
                        {
                            return Trackers[i].ScriptPath;
                        }
                        return AssetDatabase.GUIDToAssetPath(Trackers[i].ScriptGUID);
                    }
                }
                return "";
            }

            public bool HasTracker(string assetGuid)
            {
                for (int i = 0; i < Trackers.Count; i++)
                {
                    if (Trackers[i].UxmlGUID == assetGuid)
                    {
                        return true;
                    }
                }
                return false;
            }

            internal void AddTracker(string assetGuid, string scriptPath)
            {
                if (HasTracker(assetGuid) == true) return;

                Trackers.Add(new GenerationTracker()
                {
                    UxmlGUID = assetGuid,
                    ScriptPath = scriptPath
                });
            }

            internal void RemoveTracker(string assetGuid)
            {
                if (HasTracker(assetGuid) == false) return;

                for (int i = 0; i < Trackers.Count; i++)
                {
                    if (Trackers[i].UxmlGUID == assetGuid)
                    {
                        Trackers.RemoveAt(i);
                    }
                }
            }

            internal void Refresh()
            {
                //Clean Up Trackers
                for (int i = 0; i < Trackers.Count; i++)
                {
                    string path = AssetDatabase.GUIDToAssetPath(Trackers[i].UxmlGUID);
                    string guid = AssetDatabase.AssetPathToGUID(path, AssetPathToGUIDOptions.OnlyExistingAssets);

                    if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(guid))
                    {
                        //Delete Script
                        if (string.IsNullOrEmpty(Trackers[i].ScriptPath) == false)
                        {
                            AssetDatabase.DeleteAsset(Trackers[i].ScriptPath);
                        }

                        //Remove Tracker
                        Trackers.RemoveAt(i);
                    }
                }

                //Update GUID
                for (int i = 0; i < Trackers.Count; i++)
                {
                    if (string.IsNullOrEmpty(Trackers[i].ScriptGUID))
                    {
                        string guid = AssetDatabase.AssetPathToGUID(Trackers[i].ScriptPath, AssetPathToGUIDOptions.OnlyExistingAssets);
                        if (string.IsNullOrEmpty(guid) == false)
                        {
                            Trackers[i].ScriptGUID = guid;
                        }
                    }
                    else
                    {
                        string path = AssetDatabase.GUIDToAssetPath(Trackers[i].ScriptGUID);
                        if (string.IsNullOrEmpty(path) == false)
                        {
                            Trackers[i].ScriptPath = path;
                        }
                    }
                }
            }
        }

        public static string GenerationTrackPath
        {
            get
            {
                return Application.dataPath + "/../QuickEye";
            }
        }

        public static string GenerationTrackJsonPath
        {
            get
            {
                return GenerationTrackPath + "/" + "Trackers.json";
            }
        }

        private static string JsonSerialize(GenerationTrackerContainer container)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(GenerationTrackerContainer));
            using (MemoryStream ms = new MemoryStream())
            {
                js.WriteObject(ms, container);
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms, System.Text.Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private static GenerationTrackerContainer JsonDeserialize(string json)
        {
            using (var ms = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(json)))
            {
                DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(GenerationTrackerContainer));
                return (GenerationTrackerContainer)deseralizer.ReadObject(ms);
            }
        }

        private static GenerationTrackerContainer ReadGenerationTrackJsonFile()
        {

            if (Directory.Exists(GenerationTrackPath) == false)
            {
                Directory.CreateDirectory(GenerationTrackPath);
            }

            if (File.Exists(GenerationTrackJsonPath) == false)
            {
                WriteGenerationTrackJsonFile(new GenerationTrackerContainer());
            }

            using (StreamReader sr = new StreamReader(GenerationTrackJsonPath, System.Text.Encoding.UTF8))
            {
                return JsonDeserialize(sr.ReadToEnd());
            }
        }

        private static void WriteGenerationTrackJsonFile(GenerationTrackerContainer container)
        {
            if (Directory.Exists(GenerationTrackPath) == false)
            {
                Directory.CreateDirectory(GenerationTrackPath);
            }

            string json = JsonSerialize(container);
            using (StreamWriter sw = new StreamWriter(GenerationTrackJsonPath, false, System.Text.Encoding.UTF8))
            {
                sw.Write(json);
            }
        }

        internal static void Refresh()
        {
            var container = ReadGenerationTrackJsonFile();
            container.Refresh();
            WriteGenerationTrackJsonFile(container);
        }

        internal static string GetFixedPath(string assetGuid, string path)
        {
            if (HasTracker(assetGuid) == false)
            {
                AddTracker(assetGuid, path);
                return path;
            }

            var container = ReadGenerationTrackJsonFile();

            string fixedPath = container.GetFixedPath(assetGuid);

            if (string.IsNullOrEmpty(fixedPath))
                return path;

            return fixedPath;
        }

        private static bool HasTracker(string assetGuid)
        {
            var container = ReadGenerationTrackJsonFile();
            return container.HasTracker(assetGuid);
        }

        private static void AddTracker(string assetGuid, string path)
        {
            var container = ReadGenerationTrackJsonFile();
            container.AddTracker(assetGuid, path);
            WriteGenerationTrackJsonFile(container);
        }

    }
}
