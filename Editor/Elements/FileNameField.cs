using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;

namespace QuickEye.UIToolkit
{
    public class FileNameField : TextValueField<string>
    {
        public FileNameField() : base(null, -1, new FileNameInput()) { }

        public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, string startValue) { }

        protected override string StringToValue(string str) => str;

        protected override string ValueToString(string value) => value;

        class FileNameInput : TextValueInput
        {
            protected override string allowedCharacters { get; }

            public FileNameInput()
            {
                allowedCharacters = new string(Enumerable.Range(char.MinValue, char.MaxValue)
                    .Select(c => (char)c)
                    .Where(c => !char.IsControl(c))
                    .Except(Path.GetInvalidFileNameChars())
                    .ToArray());
            }

            public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, string startValue) { }

            protected override string ValueToString(string value) => value;

            protected override string StringToValue(string str) => str;
        }

        public new class UxmlFactory : UxmlFactory<FileNameField, UxmlTraits> { }
    }
}