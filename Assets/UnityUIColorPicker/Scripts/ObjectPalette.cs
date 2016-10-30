using UnityEngine;
using System.Collections;
public class ObjectPalette : MonoBehaviour {
	public Color value;
	public LayerMask _Layer;
	void Start() {

	}
	void Update() {
		if (!Input.GetMouseButton(0))
			return;
		
		RaycastHit hit;
		if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit,Mathf.Infinity,_Layer))
			return;
		
		Renderer rend = hit.transform.GetComponent<Renderer>();
		if (rend == null) {
			return;
		} else if (rend.material.mainTexture) {
			if(!hit.collider.gameObject.GetComponent<MeshCollider>()){
				Debug.LogWarning("The Object that is being hit does not have a MeshCollider.");
			}
			Texture2D tex = rend.material.mainTexture as Texture2D;
			Vector2 pixelUV = hit.textureCoord;
			pixelUV.x *= tex.width;
			pixelUV.y *= tex.height;
			try {
				Color _Get = tex.GetPixel ((int)pixelUV.x, (int)pixelUV.y);
				_Get.a = 1;
				value = _Get;

				
			} catch (UnityException) {
				
			}
		} else {
			value = rend.material.color;
		}
		

	}
}
