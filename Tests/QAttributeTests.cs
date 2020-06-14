using System;
using NUnit.Framework;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Tests
{
    public class QAttributeTests
    {
        const string LabelName = "label";
        const string LabelClass = "first"; 

        VisualElement root;
        BasePoco poco;
        readonly Label label = new Label { name = LabelName };
        readonly Button button = new Button();

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            label.AddToClassList(LabelClass);
            
            root = new VisualElement();
            root.Add(new Label());
            root.Add(label);
            root.Add(button);
        }

        [TearDown]
        public void TearDown()
        {
            poco.Dispose();
        }
        
        [Test]
        public void AssignMembers_By_Name()
        {
            poco = new BasePoco();
            
            root.AssignQueryResults(poco);
            
            Assert.AreEqual(root.Q(LabelName), poco.elementByName);
        }
        
        [Test]
        public void AssignMembers_By_Class()
        {
            poco = new BasePoco();
            
            root.AssignQueryResults(poco);
            
            Assert.AreEqual(root.Q(null,LabelClass), poco.elementByClass);
        }
        
        [Test]
        public void AssignMembers_By_Type()
        {
            poco = new BasePoco();
            
            root.AssignQueryResults(poco);
            
            Assert.AreEqual(root.Q<Button>(), poco.elementByType);
        }
        
        [Test]
        public void AssignMembers_By_NameAndClass()
        {
            poco = new BasePoco();
            
            root.AssignQueryResults(poco);
            
            Assert.AreEqual(root.Q(LabelName,LabelClass), poco.elementByNameAndClass);
        }
        
        [Test]
        public void Fields_AreAssigned_InBaseClass()
        {
            poco = new BasePoco();
            
            root.AssignQueryResults(poco);
            
            Assert.AreEqual(label, BasePoco.b_internal_static_field);
            poco.AssertBasePrivateStaticField(label);
            
            Assert.AreEqual(label, poco.b_public_field);
            Assert.AreEqual(label, poco.b_internal_field);
            poco.AssertBaseProtectedField(label);
            poco.AssertBasePrivateField(label);
        }
        
        [Test]
        public void Properties_AreAssigned_InBaseClass()
        {
            poco = new BasePoco();
            
            root.AssignQueryResults(poco);
            
            Assert.AreEqual(label, BasePoco.b_internal_static_prop);
            poco.AssertBasePrivateStaticProp(label);
            
            Assert.AreEqual(label, poco.b_public_prop);
            Assert.AreEqual(label, poco.b_internal_prop);
            poco.AssertBaseProtectedProp(label);
            poco.AssertBasePrivateProp(label);
        }
        
        [Test]
        public void Inherited_Fields_AreAssigned_InDerivedClass()
        {
            poco = new DerivedPoco();
            
            root.AssignQueryResults(poco);
            
            Assert.AreEqual(label, BasePoco.b_internal_static_field);
            poco.AssertBasePrivateStaticField(null);
            
            Assert.AreEqual(label, poco.b_public_field);
            Assert.AreEqual(label, poco.b_internal_field);
            poco.AssertBaseProtectedField(label);
            poco.AssertBasePrivateField(null);
        }
        
        [Test]
        public void Inherited_Properties_AreAssigned_InDerivedClass()
        {
            poco = new DerivedPoco();
            
            root.AssignQueryResults(poco);
            
            Assert.AreEqual(label, BasePoco.b_internal_static_prop);
            poco.AssertBasePrivateStaticProp(null);
            
            Assert.AreEqual(label, poco.b_public_prop);
            Assert.AreEqual(label, poco.b_internal_prop);
            poco.AssertBaseProtectedProp(label);
            poco.AssertBasePrivateProp(null);
        }

        class BasePoco : IDisposable
        {
            [Q(LabelName)] internal static Label b_internal_static_field;
            [Q(LabelName)] private static Label b_private_static_field;
            [Q(LabelName)] public Label b_public_field;
            [Q(LabelName)] internal Label b_internal_field;
            [Q(LabelName)] protected Label b_protected_field;
            [Q(LabelName)] private Label b_private_field;

            [Q(LabelName)] internal static Label b_internal_static_prop { get; set; }
            [Q(LabelName)] private static Label b_private_static_prop { get; set; }
            [Q(LabelName)] public Label b_public_prop { get; set; }
            [Q(LabelName)] internal Label b_internal_prop { get; set; }
            [Q(LabelName)] protected Label b_protected_prop { get; set; }
            [Q(LabelName)] private Label b_private_prop { get; set; }

            [Q]
            public Button elementByType;
            
            [Q(LabelName)]
            public VisualElement elementByName;

            [Q(null, LabelClass)]
            public VisualElement elementByClass;
            
            [Q(LabelName, LabelClass)]
            public VisualElement elementByNameAndClass;

            public void AssertBasePrivateField(object expectedValue)=>
                Assert.AreEqual(expectedValue, b_private_field);
            
            public void AssertBasePrivateStaticField(object expectedValue)=>
                Assert.AreEqual(expectedValue, b_private_static_field);
            
            public void AssertBaseProtectedProp(object expectedValue)=>
                Assert.AreEqual(expectedValue, b_protected_prop);
            
            public void AssertBasePrivateProp(object expectedValue)=>
                Assert.AreEqual(expectedValue, b_private_prop);
            
            public void AssertBasePrivateStaticProp(object expectedValue)=>
                Assert.AreEqual(expectedValue, b_private_static_prop);
            
            public void AssertBaseProtectedField(object expectedValue)=>
                Assert.AreEqual(expectedValue, b_protected_prop);

            public void Dispose()
            {
                b_private_static_prop =
                b_internal_static_prop =
                b_private_static_field =
                b_internal_static_field = null;
            }
        }

        class DerivedPoco : BasePoco { }
    }
}