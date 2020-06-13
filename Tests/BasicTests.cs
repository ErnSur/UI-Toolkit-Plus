using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Tests
{
    public class BasicTests
    {
        VisualElement root;
        EditorWindow window;
        
        [SetUp]
        public void Setup()
        {
            window = ScriptableObject.CreateInstance<EditorWindow>();
            root = new VisualElement();    
        }
        
        [TearDown]
        public void TearDown()
        {
            //window.Close();
            //Object.DestroyImmediate(window);
        }
        
        [Test]
        public void AttachToPanel_Is_Invoked()
        {
            var flag = false;
            root.RegisterCallback<AttachToPanelEvent>(_=>flag = true);
            
            window.rootVisualElement.Add(root);
            window.Show();
            
            Assert.IsTrue(flag);
        }

        [Test]
        public void RegisterToEvent_Is_InvokedTwice()
        {
            var flag = false;
            var foldout = new Foldout();
            var toggle = new Toggle();
            foldout.Add(toggle);
            root.Add(foldout);
            
            window.rootVisualElement.Add(root);
            window.Show();
            
            Assert.IsTrue(flag);
        }
    }
}
