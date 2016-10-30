using UnityEngine;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour {
    // Use this for initialization


    public Toggle ShowGlobal;
    public Toggle ShowContactNeeded;
    public StatePatternGizmo _gizmoScript;
    public void LoadNextLevel(string SceneToLoad) {
        Application.LoadLevel(SceneToLoad);
    }

    void Start() {

        if (_gizmoScript) {
            if (ShowGlobal) {
                ShowGlobal.isOn = _gizmoScript.GlobalLocation;

            }

            if (ShowContactNeeded)
            {
                ShowContactNeeded.isOn = _gizmoScript.NeedContactToTurn;

            }

        }

    }

    public void toggleGlobal() {
        _gizmoScript.GlobalLocation = !_gizmoScript.GlobalLocation;
    }

    public void toggleContact()
    {
        _gizmoScript.NeedContactToTurn = !_gizmoScript.NeedContactToTurn;
    }

    public void changeMultiplier(string value) {
        float num;
        if (float.TryParse(value, out num)) {
            _gizmoScript.ScaleMultiplier = num;
        }
    }
}
