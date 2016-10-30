using UnityEngine;
using System.Collections.Generic;



public class GizmoSelection : MonoBehaviour {
	[Tooltip("layer that the gizmos on.")]
	public LayerMask DynamicLayer;
    private StatePatternGizmo _gizmo;
    [HideInInspector]
    public GameObject SelectedGameObject;

    // Use this for initialization
    void Start () {


			_gizmo = gameObject.GetComponent<StatePatternGizmo>();


	}
	
	// Update is called once per frame
	void LateUpdate () {

       
        


        if (!_gizmo)
        {
            Debug.LogError("No 'DynamicTransformGizmos' was found on object. Selection set back to Non-Dynamic.");

        }
        else {
            Dynamically();
        }


        }
	void Dynamically(){
		Ray CameraRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

        if (Input.GetMouseButtonDown (0)) {
           



			if (Physics.Raycast (CameraRay, out hit, Mathf.Infinity, DynamicLayer)) {
				if(_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && hit.collider.gameObject != SelectedGameObject){
                  
                    _gizmo.ObjectSelected = false;
                    SelectedGameObject = null;


                    _gizmo.Item = SelectedGameObject = hit.collider.gameObject;
                    _gizmo.setTransform();
                    _gizmo.ObjectSelected = true;

			}
			}
            else if (SelectedGameObject != null && _gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE)
                {

                    _gizmo.ObjectSelected = false;
                    SelectedGameObject = null;

                }

		
		}





	}

















}
