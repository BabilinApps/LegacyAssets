using UnityEngine;
using System.Collections;

public class RotateAction : IGizmoState
{
    private readonly StatePatternGizmo _gizmo;

    public RotateAction(StatePatternGizmo State) {

        _gizmo = State;
    }


    

    private float rotaCal; // how the roation is calculated

    private Vector3 MouseDown = Vector3.zero;
    private Vector3 Drag = Vector3.zero;
    public GameObject Item;
    public float addErrorFiled = 3;


    float Max;
    float Min;

    
     public void Awake( GameObject gameObject)
    {
        Item = gameObject;
        //Creating the material for the Gizmo


        //Start with all things cleared
        _gizmo.rotationMatrix = Matrix4x4.TRS(Item.transform.position, Item.transform.localRotation, _gizmo.matScal);
        _gizmo.locklocalToWorldMatrix = _gizmo.rotationMatrix;




        if (_gizmo.GlobalLocation) {

            _gizmo.rotationMatrix = Matrix4x4.TRS(Item.transform.position, Quaternion.identity, _gizmo.matScal);
            _gizmo.locklocalToWorldMatrix = _gizmo.rotationMatrix;

        }
    }

    void Start()
    {
        _gizmo._Draw.NothingSelected();
    }






   public void CheckType()
    {
        if (!_gizmo.GlobalLocation)
        {
            _gizmo.rotationMatrix = Matrix4x4.TRS(Item.transform.position, Item.transform.localRotation, _gizmo.matScal);
            _gizmo.locklocalToWorldMatrix = _gizmo.rotationMatrix;

            
        }

        Max = (_gizmo.SetSpread + _gizmo.Sencsitivity*3) / (Mathf.Pow(_gizmo.distanceMovement, -1)*10);
        Min = (_gizmo.SetSpread - _gizmo.Sencsitivity*3) / (Mathf.Pow(_gizmo.distanceMovement,-1)*10);
        if (Vector3.Dot(_gizmo.hitX, CurVectors.rForward) >0)
            if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.rlensX > Min && _gizmo.rlensX < Max)
            {
            
                _gizmo._Draw.circleY = Color.green;
                _gizmo._Draw.circleZ = Color.blue;
                _gizmo._Draw.circleX = _gizmo.SelectedColor;
                _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.RX;
                MouseDown = _gizmo.hitX;

                rotaCal = 0.0f;

            }

        if (Vector3.Dot(_gizmo.hitY, CurVectors.uForward) > 0)
            if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.rlensY > Min && _gizmo.rlensY < Max)
            {
              
                _gizmo._Draw.circleX = Color.red;
                _gizmo._Draw.circleZ = Color.blue;
                _gizmo._Draw.circleY = _gizmo.SelectedColor;
                _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.RY;
                MouseDown = _gizmo.hitY;




                rotaCal = 0.0f;

            }

        if (Vector3.Dot(_gizmo.hitZ, CurVectors.fForward) > 0)
            if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.rlensZ > Min && _gizmo.rlensZ < Max)
            {
           
                _gizmo._Draw.circleX = Color.red;
                _gizmo._Draw.circleY = Color.green;
                _gizmo._Draw.circleZ = _gizmo.SelectedColor;
                _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.RZ;
                MouseDown = _gizmo.hitZ;

                rotaCal = 0.0f;
            }

    }

    void RotationX()
    {

        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.RX)
        {
            Drag = _gizmo.hitX;


            float angledown = Vector3.Dot(MouseDown.normalized, Vector3.forward);
            float anglemove = Vector3.Dot(Drag.normalized, Vector3.forward);
            float angledeta = Vector3.Dot(MouseDown.normalized, Drag.normalized);

            angledown = SnapDot(angledown, MouseDown.z, MouseDown.y);

            anglemove = SnapDot(anglemove, Drag.z, Drag.y);

            angledeta = Mathf.Acos(angledeta);

            if (angledown == anglemove)
            {
                if (!_gizmo.GlobalLocation)
                    _gizmo.rotationMatrix = Item.transform.localToWorldMatrix;

            }
            else
            {

                float judge = angledown + Mathf.PI;

                if (judge > 2 * Mathf.PI)
                {
                    if (!(anglemove >= 0f && anglemove <= judge - 2 * Mathf.PI || anglemove > angledown))
                        angledeta = -angledeta;


                }
                else if (!(anglemove >= angledown && anglemove <= judge))
                {
                    angledeta = -angledeta;


                }

                rotaCal = angledeta - rotaCal;
                if (!_gizmo.GlobalLocation)
                    _gizmo.rotationMatrix = _gizmo.locklocalToWorldMatrix;
      
                RotateItem(OnX: true, angledeta: angledeta);
            }
        }
    }


    void RotationZ()
    {


        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.RZ)
        {
            Drag = _gizmo.hitZ;


            float angledown = Vector3.Dot(MouseDown.normalized, Vector3.up);
            float anglemove = Vector3.Dot(Drag.normalized, Vector3.up);
            float angledeta = Vector3.Dot(MouseDown.normalized, Drag.normalized);

            angledown = SnapDot(angledown, MouseDown.y, MouseDown.x);

            anglemove = SnapDot(anglemove, Drag.y, Drag.x);

            angledeta = Mathf.Acos(angledeta);

            if (angledown == anglemove)
            {
                if (!_gizmo.GlobalLocation)
                    _gizmo.rotationMatrix = Item.transform.localToWorldMatrix;
                return;
            }

            float judge = angledown + Mathf.PI;

            if (judge > 2 * Mathf.PI)
            {
                if (!(anglemove >= 0f && anglemove <= judge - 2 * Mathf.PI || anglemove > angledown))
                    angledeta = -angledeta;


            }
            else if (!(anglemove >= angledown && anglemove <= judge))
            {
                angledeta = -angledeta;


            }

            rotaCal = angledeta - rotaCal;
            if (!_gizmo.GlobalLocation)
                _gizmo.rotationMatrix = _gizmo.locklocalToWorldMatrix;
            RotateItem(OnZ: true, angledeta: angledeta);
        }

    }



    void RotationY()
    {


        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.RY)
        {

            Drag = _gizmo.hitY;


            float angledown = Vector3.Dot(MouseDown.normalized, Vector3.right);
            float anglemove = Vector3.Dot(Drag.normalized, Vector3.right);
            float angledeta = Vector3.Dot(MouseDown.normalized, Drag.normalized);

            angledown = SnapDot(angledown, MouseDown.x, MouseDown.z);

            anglemove = SnapDot(anglemove, Drag.x, Drag.z);

            angledeta = Mathf.Acos(angledeta);

            if (angledown == anglemove)
            {
                if (!_gizmo.GlobalLocation)
                    _gizmo.rotationMatrix = Item.transform.localToWorldMatrix;
                return;
            }

            float judge = angledown + Mathf.PI;

            if (judge > 2 * Mathf.PI)
            {
                if (!(anglemove >= 0f && anglemove <= judge - 2 * Mathf.PI || anglemove > angledown))
                    angledeta = -angledeta;


            }
            else if (!(anglemove >= angledown && anglemove <= judge))
            {
                angledeta = -angledeta;


            }

            rotaCal = angledeta - rotaCal;


            if (!_gizmo.GlobalLocation)
                _gizmo.rotationMatrix = _gizmo.locklocalToWorldMatrix;


            RotateItem(OnY:true, angledeta:angledeta);


        }
    }

    public void OnRenderObject()
    {
        Max = 3 + _gizmo.Sencsitivity * 5;
        Min = _gizmo.distanceMovement + (-_gizmo.Sencsitivity * 5);

        addErrorFiled = 2 * (_gizmo.SetSpread) * (_gizmo.Sencsitivity / 2);
        if (!Input.GetMouseButton(0))
        {

            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.NONE;

            if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE)
            {

                
                _gizmo._Draw.NothingSelected();
            }
        }


        RotationY();
        RotationZ();
        RotationX();
        CheckType();


        _gizmo._Draw.RenderGizmos(Item, _gizmo.SetSpread, _gizmo.rotationMatrix, _gizmo.matScal);

    }


    float SnapDot(float dot, float x, float y)
    {
        float SnapSpot = 0.0f;
        if (dot >= 0)
        {

            if (y > 0)
                SnapSpot = Mathf.Acos(dot);
            else
                SnapSpot = 2 * Mathf.PI - Mathf.Acos(dot);
        }
        else
        {
            if (y > 0)
                SnapSpot = Mathf.Acos(dot);
            else
                SnapSpot = 2 * Mathf.PI - Mathf.Acos(dot);


        }
        return SnapSpot;
    }

    void RotateItem(bool OnX = false, bool OnY = false, bool OnZ = false, float angledeta = 0)
    {

         if (_gizmo.GlobalLocation == false)
        {
            if (_gizmo.NeedContactToTurn == true)
            {
                if (OnX)
                    Item.transform.Rotate(new Vector3(-rotaCal, 0, 0) * _gizmo.RotationSpeed);

                if (OnY)
                    Item.transform.Rotate(new Vector3(0, -rotaCal, 0) * _gizmo.RotationSpeed);

                if (OnZ)
                    Item.transform.Rotate(new Vector3(0, 0, -rotaCal) * _gizmo.RotationSpeed);

            }
            else
            {
               
                if (OnX) {
                    
                    float angle =1;
                    if (Vector3.Dot(Item.transform.right, Vector3.right) < -.6f) {

                        angle = -1;
                    }
                        Item.transform.Rotate(Vector3.right, Input.GetAxis("Mouse Y") *
                           _gizmo.RotationSpeed * angle * 1);
                  

                }
                if (OnY)
                {

                    float angle = 1;
                    if (Vector3.Dot(Item.transform.up, Vector3.up) < -.6f)
                    {

                        angle = -1;
                    }
                    
                        Item.transform.Rotate(Vector3.up,  Input.GetAxis("Mouse X") *
                         -_gizmo.RotationSpeed  * angle * 1);



                }
                if (OnZ)
                {
                    float angle = 1;
                    if (Vector3.Dot(Item.transform.forward, Vector3.back) < -.6f)
                    {

                        angle = -1;
                    }
                   
                    
                        Item.transform.Rotate(Vector3.forward,  -Input.GetAxis("Mouse X") *
                         _gizmo.RotationSpeed * angle*-1);


                }


            }
        }
        else {
            _gizmo.rotationMatrix = Matrix4x4.TRS(Item.transform.position, Quaternion.identity, _gizmo.matScal);
            _gizmo.locklocalToWorldMatrix = _gizmo.rotationMatrix;

            if (OnX)
                Item.transform.Rotate(Vector3.right, -Mathf.Deg2Rad * Input.GetAxis("Mouse Y") * 105 * _gizmo.RotationSpeed, Space.World);

            if (OnY)
                Item.transform.Rotate(Vector3.up, -Mathf.Deg2Rad * Input.GetAxis("Mouse X") * 105 * _gizmo.RotationSpeed, Space.World);

            if (OnZ)
                Item.transform.Rotate(Vector3.forward, Mathf.Deg2Rad * (Input.GetAxis("Mouse X")+ Input.GetAxis("Mouse Y")) * 105 * _gizmo.RotationSpeed, Space.World);





        }

        rotaCal = angledeta;

    }








}