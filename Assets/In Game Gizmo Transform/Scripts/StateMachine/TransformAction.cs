using UnityEngine;
using System.Collections;

public class TransformAction : IGizmoState
{
    private readonly StatePatternGizmo _gizmo;
    public TransformAction(StatePatternGizmo State)
    {
        _gizmo = State;
    }

    
    private float SelectedSencsitivity = 0.1f; //sencsitivity

    
    private Vector3 MouseDown = Vector3.zero;
    private Vector3 Drag = Vector3.zero;
    private Vector3 Dir = Vector3.zero; //The current derections of the Gizmos and object
   




    public GameObject Item;
    public void Awake(GameObject gameObject)
    {
        Item = gameObject;
        _gizmo.rotationMatrix= Matrix4x4.TRS(Item.transform.position, Item.transform.localRotation, _gizmo.matScal);

        if (_gizmo.GlobalLocation)
        {

            _gizmo.rotationMatrix = Matrix4x4.TRS(Item.transform.position, Quaternion.identity, _gizmo.matScal);
            _gizmo.locklocalToWorldMatrix = _gizmo.rotationMatrix;

        }

    }



    void X() {
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitX.x > 0f && _gizmo.hitX.x <= 0.3f * _gizmo.SetSpread && _gizmo.hitX.z > 0 && _gizmo.hitX.z <= 0.3f * _gizmo.SetSpread)
        {
            
            _gizmo._Draw.panY = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.XZ;
            MouseDown = _gizmo.hitX;
        }

        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.XZ && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {
            Drag = _gizmo.hitX - MouseDown;
            Drag.y = 0;
            if (!_gizmo.GlobalLocation)
                Item.transform.position += Item.transform.localRotation * Drag;
            else
                Item.transform.position += Drag;
        }

        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitX.x > 0f && _gizmo.hitX.x <= _gizmo.SetSpread && Mathf.Abs(_gizmo.hitX.z) < SelectedSencsitivity)
        { //case x
            _gizmo._Draw.selX = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.X;
            MouseDown = _gizmo.hitX;

        }
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.X && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {
            Drag = _gizmo.hitX - MouseDown;
            Drag.y = 0;
            Drag.z = 0;
            if (!_gizmo.GlobalLocation)
                Item.transform.position += Item.transform.localRotation * Drag;
            else
                Item.transform.position += Drag;

        }
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitX.z > 0f && _gizmo.hitX.z <= _gizmo.SetSpread && Mathf.Abs(_gizmo.hitX.x) < SelectedSencsitivity)
        {//case  z
            _gizmo._Draw.selZ = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.Z;
            MouseDown = _gizmo.hitX;
        }
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.Z && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {
            Drag = _gizmo.hitX - MouseDown;
            Drag.y = 0;
            Drag.x = 0;
            if (!_gizmo.GlobalLocation)
                Item.transform.position += Item.transform.localRotation * Drag;
            else
                Item.transform.position +=Drag;

        }
    }
    void Y() {
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitY.x > 0f && _gizmo.hitY.x <= 0.3f * _gizmo.SetSpread && _gizmo.hitY.y > 0 && _gizmo.hitY.y <= 0.3f * _gizmo.SetSpread)
        {
            _gizmo._Draw.panZ = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.XY;
            MouseDown = _gizmo.hitY;
        }
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.XY)
        {

            Drag = _gizmo.hitY - MouseDown;


            Drag.z = 0;
            if (!_gizmo.GlobalLocation)
                Item.transform.position += Item.transform.localRotation * Drag;
            else
                Item.transform.position += Drag;



        }
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitY.x > 0f && _gizmo.hitY.x <= _gizmo.SetSpread && Mathf.Abs(_gizmo.hitY.y) < SelectedSencsitivity)
        { //case x
            _gizmo._Draw.selX = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.X2;

            MouseDown = _gizmo.hitY;


        }
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.X2 && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {

            Drag = _gizmo.hitY - MouseDown;
            Drag.y = 0;
            Drag.z = 0;
            if (!_gizmo.GlobalLocation)
                Item.transform.position += Item.transform.localRotation * Drag;
            else
                Item.transform.position += Drag;

        }
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitY.y > 0f && _gizmo.hitY.y <= _gizmo.SetSpread && Mathf.Abs(_gizmo.hitY.x) < SelectedSencsitivity)
        { //case y
            _gizmo._Draw.selY = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.Y;
            MouseDown = _gizmo.hitY;
        }
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.Y && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {

            Drag = _gizmo.hitY - MouseDown;
            Drag.z = 0;
            Drag.x = 0;
            if (!_gizmo.GlobalLocation)
                Item.transform.position += Item.transform.localRotation * Drag;
            else
                Item.transform.position +=  Drag;
        }
    }
    void Z() {
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitZ.z > 0 && _gizmo.hitZ.z <= 0.3f * _gizmo.SetSpread && _gizmo.hitZ.y > 0 && _gizmo.hitZ.y <= 0.3f * _gizmo.SetSpread)
        {
            _gizmo._Draw.panX = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.YZ;
            MouseDown = _gizmo.hitZ;
        }
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.YZ)
        {

            Drag = _gizmo.hitZ - MouseDown;


            Drag.x = 0;
            if (!_gizmo.GlobalLocation)
                Item.transform.position += Item.transform.localRotation * Drag;
            else
                Item.transform.position +=  Drag;

        }
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitZ.z > 0f && _gizmo.hitZ.z <= _gizmo.SetSpread && Mathf.Abs(_gizmo.hitZ.y) < SelectedSencsitivity * _gizmo.SetSpread)
        {//case  z
            _gizmo._Draw.selZ = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.Z2;
            MouseDown = _gizmo.hitZ;
        }
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.Z2 && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {

            Drag = _gizmo.hitZ - MouseDown;
            Drag.y = 0;
            Drag.x = 0;
            if (!_gizmo.GlobalLocation)
                Item.transform.position += Item.transform.localRotation * Drag;
            else
                Item.transform.position += Drag;

        }
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitZ.y > 0f && _gizmo.hitZ.y <= _gizmo.SetSpread && Mathf.Abs(_gizmo.hitZ.z) < SelectedSencsitivity * _gizmo.SetSpread)
        { //case y
            _gizmo._Draw.selY = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.Y2;
            MouseDown = _gizmo.hitZ;
        }
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.Y2 && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {

            Drag = _gizmo.hitZ - MouseDown;
            Drag.z = 0;
            Drag.x = 0;
            if (!_gizmo.GlobalLocation)
                Item.transform.position += Item.transform.localRotation * Drag;
            else
                Item.transform.position +=  Drag;
        }
    }


    public void OnRenderObject()
    {

        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && !Input.GetMouseButton(0))
        {
            _gizmo._Draw.NothingSelected();
        }



        if (!Input.GetMouseButton(0))
        {
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.NONE;

            if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE)
            {
            }
        }

        X();
        Y();
        Z();

        RenderGizmos();
    }

    public void RenderGizmos()
    {

        Dir = _gizmo.cam.transform.position - Item.transform.position;

        Dir.Normalize();

        GL.PushMatrix();
        if(!_gizmo.GlobalLocation)
        _gizmo.rotationMatrix = Item.transform.localToWorldMatrix;

        if (!_gizmo.GlobalLocation)
            _gizmo.rotationMatrix = Matrix4x4.TRS(Item.transform.position, Item.transform.localRotation, _gizmo.matScal);
        GL.MultMatrix(_gizmo.rotationMatrix );
        _gizmo.lineMaterial.SetPass(0);

        if (_gizmo.GlobalLocation)
        {
            _gizmo.rotationMatrix = Matrix4x4.TRS(Item.transform.position, Quaternion.identity, _gizmo.matScal);
            _gizmo.locklocalToWorldMatrix = _gizmo.rotationMatrix;
        }


            _gizmo._Draw.DrawAxis(Vector3.right * _gizmo.SetSpread, Vector3.up, Vector3.forward, 0.04f * _gizmo.SetSpread, 0.9f, _gizmo._Draw.selX);
            _gizmo._Draw.DrawAxis(Vector3.up * _gizmo.SetSpread, Vector3.right, Vector3.forward, 0.04f * _gizmo.SetSpread, 0.9f, _gizmo._Draw.selY);
            _gizmo._Draw.DrawAxis(Vector3.forward * _gizmo.SetSpread, Vector3.right, Vector3.up, 0.04f * _gizmo.SetSpread, 0.9f, _gizmo._Draw.selZ);


            _gizmo._Draw.DrawQuad(0.3f * _gizmo.SetSpread, false, Vector3.forward, Vector3.up, _gizmo._Draw.panX);
            _gizmo._Draw.DrawQuad(0.3f * _gizmo.SetSpread, false, Vector3.right, Vector3.forward, _gizmo._Draw.panY);
            _gizmo._Draw.DrawQuad(0.3f * _gizmo.SetSpread, false, Vector3.right, Vector3.up, _gizmo._Draw.panZ);


        



        GL.PopMatrix();

    }



















}
