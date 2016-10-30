using UnityEngine;
using UnityEngine.UI;

using GazeInput;

public class GazeDemoMenu : MonoBehaviour {
    public VREventSystem system;
    public Text text;

    void Start()
    {
        system = VREventSystem.curEventSystem;

    }
    public void ChangeStyle() {
        system.classicCursor = !system.classicCursor;
    }
    public void ChangeClickTime(float value) {
        text.text = value.ToString();
        system.autoClickTime = value;
    }

    public void AutoClick() {
        system.autoClick = !system.autoClick;
    }


}
