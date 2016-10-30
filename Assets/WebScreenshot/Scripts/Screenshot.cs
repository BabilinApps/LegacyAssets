/// <summary>
/// This code takes a screenshot from the main camera and sends it to the index.html file. 
/// Look at the documentation to see how the External Call works.
/// </summary>
using UnityEngine;
using System.Collections;

public class Screenshot : MonoBehaviour {
	public bool useScreenSize = true;
	public int ScreenshotWidth = 1280; // the width of the screenshot (px)
	public int ScreenshotHeight= 720; // the height of the screenshot (px)
	public bool AddTimeStamp = false; // Add unity default time stamp to the image (png)
	public bool AddSize = false;      // add the size to the name of the image (png)
	public string RootName = "ScreenShot"; // name of the image (png)

	private Texture2D bg; //place holder for photo

	void Start(){
		if (useScreenSize == true) {
			ScreenshotWidth = Screen.currentResolution.width;
			ScreenshotHeight = Screen.currentResolution.height;
		}
		bg = new Texture2D (ScreenshotWidth, ScreenshotHeight);
	}
	void OnGUI()
	{
		if (GUI.Button (new Rect (Screen.width * 0.5f - 32, 32, 64, 32), "Save")) {
			StartCoroutine (ScreeAndSave ());
		}
	}
	//Call this if you are using Unity 4.6 GUI
	public void SaveScreen(){
		StartCoroutine (ScreeAndSave ());
	}
	
	IEnumerator ScreeAndSave()
	{
		yield return new WaitForEndOfFrame();
		
		RenderTexture rt = new RenderTexture(bg.width, bg.height, 24);
		Camera.main.targetTexture = rt;
		Texture2D screenShot = new Texture2D(bg.width, bg.height, TextureFormat.RGB24, false);
		Camera.main.Render();
		RenderTexture.active = rt;
		screenShot.ReadPixels(new Rect(0, 0, bg.width, bg.height), 0, 0);
		Camera.main.targetTexture = null;
		RenderTexture.active = null; 
		Destroy(rt);
		string filename = Name(AddTimeStamp,AddSize);
		byte[] bytes = screenShot.EncodeToPNG();
		Application.ExternalCall("Download", System.Convert.ToBase64String(bytes), filename);

	}
	
	string Name(bool Date, bool Size){
		string _name = RootName;
		string date ="";
		string size="";
		if (Date) {
			date = "_"+ System.DateTime.Today.ToString();
		}
		if (Size) {
			size = bg.width.ToString () + "x" + bg.height.ToString ();
		}
		
		_name = size+ RootName + date + ".png";
		Debug.Log(_name);
		return _name;
	}
}
