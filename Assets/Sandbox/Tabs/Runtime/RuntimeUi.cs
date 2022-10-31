#if UNITY_2021_1_OR_NEWER
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class RuntimeUi : MonoBehaviour
{
    private void Start()
    {
        var uiDoc = GetComponent<UIDocument>();
    }
}
#endif