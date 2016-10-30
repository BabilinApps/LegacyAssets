using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ColorPickerUnityUI : MonoBehaviour {
	[Tooltip("Is the image a circle")]
	public bool circular;
	[Tooltip("Picture to use")]
	public Image colorPalette;
	[Tooltip("Thumb to use")]
	public Image thumb;
	[Tooltip("Output Color.")]
	public Color value;
    public float Offset = 0;
    [HideInInspector]
    public bool WasClicked = false;
	private Vector2 SpectrumXY; // the size of the palette
	/// <summary>
	/// Tis is to make the code more clean down below. We store the values most often used.
	/// </summary>
	private Bounds PictureBounds; // the bounds of the palette
    private Vector3 Max; // max bounds
    private Vector3 Min; // min bounds

    private CanvasScaler myScale;
	//////////////////	

	private void Start(){


        myScale = colorPalette.canvas.transform.GetComponent<CanvasScaler>();


        SpectrumXY = new Vector2 (colorPalette.GetComponent<RectTransform> ().rect.width* myScale.transform.localScale.x, colorPalette.GetComponent<RectTransform> ().rect.height*myScale.transform.localScale.y);
		PictureBounds = colorPalette.GetComponent<Collider2D> ().bounds ;
		Max =PictureBounds.max;
        Min = PictureBounds.min;

 

	}

    public static Vector3 MultiplyVectors(Vector3 V1, Vector3 V2) {
        float[] X = { V1.x, V2.x };
        float[] Y = { V1.y, V2.y };
        float[] Z = { V1.z, V2.z };
        return new Vector3(X[0] * X[1], Y[0] * Y[1], Z[0] * Z[1]);

    }

    public void ResetTumb() {
        thumb.GetComponent<RectTransform>().localPosition = Vector3.zero;

    }

	// called by event on object
	public void OnPress(){
			UpdateThumbPosition ();
        WasClicked = true;
	}
	// called by event on object
	public void OnDrag(){
			UpdateThumbPosition ();
        WasClicked = true;

    }
	

	//get color of mouse point
	private Color GetColor(){
		Vector2 spectrumScreenPosition = colorPalette.transform.position;
		Vector2 thumbScreenPosition = thumb.transform.position;
		Vector2 position=thumbScreenPosition-spectrumScreenPosition+SpectrumXY*0.5f;
		Texture2D texture = colorPalette.mainTexture as Texture2D;
		if (circular) {

            myScale = colorPalette.canvas.transform.GetComponent<CanvasScaler>();


            SpectrumXY = new Vector2(colorPalette.GetComponent<RectTransform>().rect.width * myScale.transform.localScale.x, colorPalette.GetComponent<RectTransform>().rect.height * myScale.transform.localScale.y);
            PictureBounds = colorPalette.GetComponent<Collider2D>().bounds;
            Max = PictureBounds.max;
            Min = PictureBounds.min;

            position = new Vector2 ((position.x / (colorPalette.GetComponent<RectTransform> ().rect.width)),
			                        (position.y / (colorPalette.GetComponent<RectTransform> ().rect.height)));
            Color circularSelectedColor = texture.GetPixelBilinear(position.x / myScale.transform.localScale.x, position.y / myScale.transform.localScale.y);
            circularSelectedColor.a = 1;
            return circularSelectedColor;

        } else {
			position = new Vector2 ((position.x/colorPalette.GetComponent<RectTransform>().rect.width), (position.y / colorPalette.GetComponent<RectTransform>().rect.height) );
		}

		Color SelectedColor = texture.GetPixelBilinear (position.x / myScale.transform.localScale.x, position.y / myScale.transform.localScale.y);
		SelectedColor.a = 1;
		return SelectedColor;
	}
	//move the object only where the picture is
	private void UpdateThumbPosition(){
	
		if (circular && colorPalette.GetComponent<CircleCollider2D> ()) {
			Vector3 center = transform.position;
			Vector3 position = Input.mousePosition;
			Vector3 offset = position - center;
			Vector3 Set = Vector3.ClampMagnitude (offset, (colorPalette.GetComponent<CircleCollider2D> ().radius * myScale.transform.localScale.x));
			Vector3 newPos = center + Set;
			if (thumb.transform.position != newPos) {
				thumb.transform.position = newPos;
				value = GetColor ();
			}

		} else {
			if (circular) {
				Debug.LogError ("No 'CircleCollider2D' found on object. Please add a CircleCollider or turn off 'circular'.");
			}
        SpectrumXY = new Vector2 (colorPalette.GetComponent<RectTransform> ().rect.width* myScale.transform.localScale.x, colorPalette.GetComponent<RectTransform> ().rect.height*myScale.transform.localScale.y);
		PictureBounds = colorPalette.GetComponent<Collider2D> ().bounds ;
		Max =PictureBounds.max;
        Min = PictureBounds.min;

            float x = Mathf.Clamp (Input.mousePosition.x, Min.x, Max.x + 1);
			float y = Mathf.Clamp (Input.mousePosition.y, Min.y, Max.y);
			Vector3 newPos = new Vector3 (x, y, transform.position.z);
			if (thumb.transform.position != newPos) {
				thumb.transform.position = newPos;
				value = GetColor ();
			}
		}
	}
	
	
}
