using UnityEngine;
using System.Collections;

public class ChangeColor : MonoBehaviour {
    [Tooltip("What Color Picker Code Will Affect The Color.")]
    public GameObject ColorPicker;
    [Tooltip("material index to change")]
    public int MaterialIndex = 0;
	private ColorPickerUnityUI ColorPickerToUse;
	public ObjectPalette ObjectPaletteToUse;
	private bool isCamera;
	private bool isLight;
	// Use this for initialization
	void Start () {
        if (!ObjectPaletteToUse)
        {

            ColorPickerToUse = ColorPicker.GetComponent<ColorPickerUnityUI>();
            if (!ColorPickerToUse)
            {
                Debug.LogError("The color picker object assigned to '" + gameObject.name + "' does not have the 'ColorPickerUnityUI' component.");
            }
        }
		if (gameObject.GetComponent<Camera> ()) {
						isCamera = true;
				} else {
			isCamera = false;		
		}

		if (gameObject.GetComponent<Light> ()) {
						isLight = true;

				} else {
			isLight = false;		
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (ColorPickerToUse) {
			if (!isCamera && !isLight && gameObject.GetComponent<Renderer> ().material) {
                if (ColorPickerToUse.value.a < 1)
                    return;
				gameObject.GetComponent<Renderer> ().materials[MaterialIndex].color = ColorPickerToUse.value;
			}
			if (isCamera) {
                if (ColorPickerToUse.value.a < 1)
                    return;
                gameObject.GetComponent<Camera> ().backgroundColor = ColorPickerToUse.value;
			}

			if (isLight) {
                if (ColorPickerToUse.value.a < 1)
                    return;
                gameObject.GetComponent<Light> ().color = ColorPickerToUse.value;
			}
		} else if (ObjectPaletteToUse) {
			if (!isCamera && !isLight && gameObject.GetComponent<Renderer> ().material) {
				gameObject.GetComponent<Renderer> ().materials[MaterialIndex].color = ObjectPaletteToUse.value;
			}
			if (isCamera) {
				gameObject.GetComponent<Camera> ().backgroundColor = ObjectPaletteToUse.value;
			}
			
			if (isLight) {
				gameObject.GetComponent<Light> ().color = ObjectPaletteToUse.value;
			}
		}
		else {
			Debug.LogWarning("No 'ColorPickerToUse' or 'ObjectPaletteToUse' was found assigned to '" + gameObject.name+"'.");
			return;
		}
	}
}
