/// <summary>
/// Mesh viewer camera.
/// This is a simple script to help show the screenshot script. 
/// </summary>
using UnityEngine;
using System.Collections;

public class MeshViewerCamera : MonoBehaviour {

	public Transform target;
	public float distance = 5.0f;

	public bool YLimits = false;
	public bool XLimits = false;
	public bool DistanceLimits = false;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	public float xMinLimit = -20f;
	public float xMaxLimit = 80f;

	public float distanceMin = .5f;
	public float distanceMax = 15f;
	
	float x = 0.0f;
	float y = 0.0f;

	public KeyCode RotateKey = KeyCode.Mouse1;

	// Use this for initialization
	void Start () 
	{
		distance = Vector3.Distance (target.position, transform.position);
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		

	}
	void Update(){

		//Chrome Does not support ScrollWheel. This is a simple fix
		#if UNITY_WEBGL
			float fov = Camera.main.fieldOfView;
			if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
				fov += Time.deltaTime * -220;
			} else if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
				fov += Time.deltaTime * 220;
			} else {
				
			}
			
			fov = Mathf.Clamp(fov, 5, 70);
			Camera.main.fieldOfView = fov;
			
		#endif

		#if !UNITY_WEBGL
		float fov = Camera.main.fieldOfView;
		if(DistanceLimits){
			fov += Input.GetAxis("Mouse ScrollWheel")*5;
			fov = Mathf.Clamp(fov, distanceMin, distanceMax);
		}
		else{
			fov += Input.GetAxis("Mouse ScrollWheel")*5;
			fov = Mathf.Clamp(fov, 5, 140);
		}
		Camera.main.fieldOfView = fov;
		#endif


	}
	void LateUpdate () 
	{

		if (target && Input.GetKey(RotateKey)) 
		{
			x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			if(YLimits == true){
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			}

			if(XLimits == true){
				x = ClampAngle(x, xMinLimit, xMaxLimit);
			}

			Quaternion rotation = Quaternion.Euler(y, x, 0);


			Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
			Vector3 position = rotation * (negDistance + target.position);
			
			transform.rotation = rotation;
			transform.position = position;
		}
	}
	
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}
