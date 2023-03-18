using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using NUnit.Framework;
using QuickEye.UxmlBridgeGen;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Tests
{
    public class InlineSettingsTests
    {
        const string ResourcesDir = "uxml-bridge-tests/";

        static readonly Dictionary<UxmlFileType, string> Uxmls = new Dictionary<UxmlFileType, string>()
        {
            { UxmlFileType.Default, LoadUxml("Default") },
            { UxmlFileType.NoRootNamespace, LoadUxml("NoRootNamespace") },
            { UxmlFileType.WithCustomSettings, LoadUxml("WithCustomSettings") }
        };

        static readonly InlineSettings CustomSettings = new InlineSettings()
        {
            CsNamespace = "test.test",
            GenCsGuid = "guid",
            PrivateFieldPrefix = "pf-prefix",
            PrivateFieldSuffix = "pf-suffix",
            PrivateFieldStyle = CaseStyle.LowerCamelCase.ToString(),
            ClassPrefix = "c-prefix",
            ClassSuffix = "c-suffix",
            ClassStyle = CaseStyle.UpperCamelCase.ToString(),
        };

        [TestCase(UxmlFileType.Default, false)]
        [TestCase(UxmlFileType.NoRootNamespace, false)]
        [TestCase(UxmlFileType.WithCustomSettings, true)]
        public void Deserialize_Settings(UxmlFileType fileType, bool hasCustomSettings)
        {
            var uxml = Uxmls[fileType];

            var settings = InlineSettings.FromXml(uxml);

            Assert.IsNotNull(settings);
            if (hasCustomSettings)
            {
                AssertAreSettingsEqual(CustomSettings, settings);
            }
        }

        [TestCase]
        public void Serialize_Settings()
        {
            var uxml = Uxmls[UxmlFileType.Default];
            var root = XDocument.Parse(uxml).Root;
            var settings = InlineSettings.FromXml(uxml);

            AssertAreSettingsEqual(new InlineSettings(), settings);
            CustomSettings.AddTo(root);
            settings = InlineSettings.FromXml(root.ToString());
            AssertAreSettingsEqual(CustomSettings, settings);
        }

        private void AssertAreSettingsEqual(InlineSettings expected, InlineSettings actual)
        {
            var json = JsonUtility.ToJson(actual);
            var expectedJson = JsonUtility.ToJson(expected);
            Assert.AreEqual(expectedJson, json);
        }

        private static string LoadUxml(string fileName)
        {
            var asset = Resources.Load<VisualTreeAsset>(ResourcesDir + fileName);
            var path = AssetDatabase.GetAssetPath(asset);
            return File.ReadAllText(path);
        }

        public enum UxmlFileType
        {
            Default,
            NoRootNamespace,
            WithCustomSettings
        }
    }
}