using UnityEngine;
using System.Collections;

public class ScaleAction : IGizmoState
{
    private readonly StatePatternGizmo _gizmo;
    public ScaleAction(StatePatternGizmo State)
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
        _gizmo.rotationMatrix = Matrix4x4.TRS(Item.transform.position, Item.transform.localRotation, _gizmo.matScal);



            _gizmo.rotationMatrix = Matrix4x4.TRS(Item.transform.position, Quaternion.identity, _gizmo.matScal);
            _gizmo.locklocalToWorldMatrix = _gizmo.rotationMatrix;



    }



    public void CheckType()
    {
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitX.x > 0f && _gizmo.hitX.x <= 0.3f *
            _gizmo.SetSpread && _gizmo.hitX.z > 0 && _gizmo.hitX.z <= 0.3f * _gizmo.SetSpread && _gizmo.hitX.x + _gizmo.hitX.z - 0.3f * _gizmo.SetSpread <= 0f)
        {

            _gizmo._Draw.panY = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.TXZ;
            MouseDown = _gizmo.hitX;


        }

        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitX.x > 0f && _gizmo.hitX.x <= _gizmo.SetSpread && Mathf.Abs(_gizmo.hitX.z) < SelectedSencsitivity)
        { //case x
            _gizmo._Draw.selX = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.TX;
            MouseDown = _gizmo.hitX;

        }

        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitX.z > 0f && _gizmo.hitX.z <= _gizmo.SetSpread && Mathf.Abs(_gizmo.hitX.x) < SelectedSencsitivity)
        {//case  z
            _gizmo._Draw.selZ = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.TZ;
            MouseDown = _gizmo.hitX;

        }

        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitY.x > 0f && _gizmo.hitY.x <= 0.3f *
            _gizmo.SetSpread && _gizmo.hitY.y > 0 && _gizmo.hitY.y <= 0.3f * _gizmo.SetSpread && _gizmo.hitY.x + _gizmo.hitY.y - 0.3f * _gizmo.SetSpread <= 0f)
        {
            _gizmo._Draw.panZ = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.TXY;
            MouseDown = _gizmo.hitY;

        }

        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitY.x > 0f && _gizmo.hitY.x <= _gizmo.SetSpread && Mathf.Abs(_gizmo.hitY.y) < SelectedSencsitivity)
        { //case x
            _gizmo._Draw.selX = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.TX2;
            MouseDown = _gizmo.hitY;

        }


        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitY.y > 0f && _gizmo.hitY.y <= _gizmo.SetSpread && Mathf.Abs(_gizmo.hitY.x) < SelectedSencsitivity)
        { //case y
            _gizmo._Draw.selY = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.TY;
            MouseDown = _gizmo.hitY;
        }

        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitZ.z > 0 && _gizmo.hitZ.z <= 0.3f *
            _gizmo.SetSpread && _gizmo.hitZ.y > 0 && _gizmo.hitZ.y <= 0.3f * _gizmo.SetSpread && _gizmo.hitZ.y + _gizmo.hitZ.z - 0.3f * _gizmo.SetSpread <= 0f)
        {
            _gizmo._Draw.panX = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.TYZ;
            MouseDown = _gizmo.hitZ;
        }

        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitZ.z > 0f && _gizmo.hitZ.z <= _gizmo.SetSpread && Mathf.Abs(_gizmo.hitZ.y) < SelectedSencsitivity * _gizmo.SetSpread)
        {//case  z
            _gizmo._Draw.selZ = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.TZ2;
            MouseDown = _gizmo.hitZ;
        }


        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE && _gizmo.hitZ.y > 0f && _gizmo.hitZ.y <= _gizmo.SetSpread && Mathf.Abs(_gizmo.hitZ.z) < SelectedSencsitivity * _gizmo.SetSpread)
        { //case y
            _gizmo._Draw.selY = _gizmo.SelectedColor;
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.TY2;
            MouseDown = _gizmo.hitZ;
        }

    }

    void X() {
       
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.TXZ && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {

            Drag = _gizmo.hitX - MouseDown;
            Drag.y = 0;
            float dm = Drag.x + Drag.z;
            float dchg = (int)((dm) * 10) / 10f;
            if(_gizmo.SecondAction)
            {
                _gizmo._Draw.panX = _gizmo.SelectedColor;
                _gizmo._Draw.panY = _gizmo.SelectedColor;
                _gizmo._Draw.panZ = _gizmo.SelectedColor;
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(dchg, dchg, dchg);

            }
            else
            {
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(dchg, 0, dchg);
            }

        }


        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.TX && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {

            Drag = _gizmo.hitX - MouseDown;
            Drag.y = 0;
            Drag.z = 0;

            float dm = Drag.x;
            float dchg = (int)((dm) * 10) / 10f;


            if(_gizmo.SecondAction)
            {
                _gizmo._Draw.selX = _gizmo.SelectedColor;
                _gizmo._Draw.selY = _gizmo.SelectedColor;
                _gizmo._Draw.selZ = _gizmo.SelectedColor;
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier);
            }
            else
            {
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(dchg * _gizmo.ScaleMultiplier, 0, 0);
            }

        }

        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.TZ && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {

            Drag = _gizmo.hitX - MouseDown;
            Drag.y = 0;
            Drag.x = 0;

            float dm = Drag.z;
            float dchg = (int)((dm) * 10) / 10f;

            if(_gizmo.SecondAction)
            {
                _gizmo._Draw.selX = _gizmo.SelectedColor;
                _gizmo._Draw.selY = _gizmo.SelectedColor;
                _gizmo._Draw.selZ = _gizmo.SelectedColor;
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier);
            }
            else
            {
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(0, 0, dchg * _gizmo.ScaleMultiplier);
            }



        }
    }
    void Y() {

       
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.TXY)
        {

            Drag = _gizmo.hitY - MouseDown;


            Drag.z = 0;


            float dm = Drag.x + Drag.y;
            float dchg = (int)((dm) * 10) / 10f;


            if(_gizmo.SecondAction)
            {
                _gizmo._Draw.panX = _gizmo.SelectedColor;
                _gizmo._Draw.panY = _gizmo.SelectedColor;
                _gizmo._Draw.panZ = _gizmo.SelectedColor;
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier);
            }
            else
            {
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier, 0f);
            }



        }

        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.TX2 && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {

            Drag = _gizmo.hitY - MouseDown;
            Drag.y = 0;
            Drag.z = 0;

            float dm = Drag.x;
            float dchg = (int)((dm) * 10) / 10f;
            if(_gizmo.SecondAction)
            {
                _gizmo._Draw.selX = _gizmo.SelectedColor;
                _gizmo._Draw.selY = _gizmo.SelectedColor;
                _gizmo._Draw.selZ = _gizmo.SelectedColor;
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier);
            }
            else
            {
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(dchg * _gizmo.ScaleMultiplier, 0, 0);
            }


        }

        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.TY && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {

            Drag = _gizmo.hitY - MouseDown;
            Drag.z = 0;
            Drag.x = 0;

            float dm = Drag.y;
            float dchg = (int)((dm) * 10) / 10f;
            if(_gizmo.SecondAction)
            {
                _gizmo._Draw.selZ = _gizmo.SelectedColor;
                _gizmo._Draw.selX = _gizmo.SelectedColor;
                _gizmo._Draw.selY = _gizmo.SelectedColor;
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier);
            }
            else
            {
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(0, dchg * _gizmo.ScaleMultiplier, 0);
            }


        }
    }
    void Z() {
       
        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.TYZ)
        {

            Drag = _gizmo.hitZ - MouseDown;


            Drag.x = 0;


            float dm = Drag.z + Drag.y;
            float dchg = (int)((dm) * 10) / 10f;
            if(_gizmo.SecondAction)
            {
                _gizmo._Draw.panX = _gizmo.SelectedColor;
                _gizmo._Draw.panY = _gizmo.SelectedColor;
                _gizmo._Draw.panZ = _gizmo.SelectedColor;
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier);
            }
            else
            {
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(0, dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier);
            }




        }

        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.TZ2 && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {

            Drag = _gizmo.hitZ - MouseDown;
            Drag.y = 0;
            Drag.x = 0;

            float dm = Drag.z;
            float dchg = (int)((dm) * 10) / 10f;

            if(_gizmo.SecondAction)
            {
                _gizmo._Draw.selX = _gizmo.SelectedColor;
                _gizmo._Draw.selY = _gizmo.SelectedColor;
                _gizmo._Draw.selZ = _gizmo.SelectedColor;
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier);
            }
            else
            {
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(0, 0, dchg * _gizmo.ScaleMultiplier);
            }



        }

        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.TY2 && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {

            Drag = _gizmo.hitZ - MouseDown;
            Drag.z = 0;
            Drag.x = 0;

            float dm = Drag.y;
            float dchg = (int)((dm) * 10) / 10f;


            if(_gizmo.SecondAction)
            {
                _gizmo._Draw.selX = _gizmo.SelectedColor;
                _gizmo._Draw.selY = _gizmo.SelectedColor;
                _gizmo._Draw.selZ = _gizmo.SelectedColor;
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier);
            }
            else
            {
                Item.transform.localScale = _gizmo.CurrentScale + new Vector3(0 * _gizmo.ScaleMultiplier, dchg * _gizmo.ScaleMultiplier, 0 * _gizmo.ScaleMultiplier);
            }



        }

    }

    public void OnRenderObject()
    {


      

        if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE)
        {
            _gizmo._Draw.NothingSelected();
        }
        if (!Input.GetMouseButton(0))
        {
            _gizmo.SelectedType = StatePatternGizmo.MOVETYPE.NONE;

            if (_gizmo.SelectedType == StatePatternGizmo.MOVETYPE.NONE)
            {
                RenderGizmos();
            }
        }


        CheckType();
        X();
        Y();
        Z();
        RenderGizmos();

    }

    void RenderGizmos()
    {

        Dir = Camera.main.transform.position - Item.transform.position;

        Dir.Normalize();


        GL.PushMatrix();
        _gizmo.rotationMatrix = Item.transform.localToWorldMatrix;

        _gizmo.rotationMatrix = Matrix4x4.TRS(Item.transform.position, Item.transform.localRotation, _gizmo.matScal);
        GL.MultMatrix(_gizmo.rotationMatrix);

        _gizmo.lineMaterial.SetPass(0);







            _gizmo._Draw.DrawShpere(Vector3.right * _gizmo.SetSpread, Vector3.up, Vector3.forward, 0.04f * _gizmo.SetSpread, 0.9f, _gizmo._Draw.selX);
            _gizmo._Draw.DrawShpere(Vector3.up * _gizmo.SetSpread, Vector3.right, Vector3.forward, 0.04f * _gizmo.SetSpread, 0.9f, _gizmo._Draw.selY);
            _gizmo._Draw.DrawShpere(Vector3.forward * _gizmo.SetSpread, Vector3.right, Vector3.up, 0.04f * _gizmo.SetSpread, 0.9f, _gizmo._Draw.selZ);


            _gizmo._Draw.DrawScale(0.3f * _gizmo.SetSpread, false, Vector3.forward, Vector3.up, _gizmo._Draw.panX);
            _gizmo._Draw.DrawScale(0.3f * _gizmo.SetSpread, false, Vector3.right, Vector3.forward, _gizmo._Draw.panY);
            _gizmo._Draw.DrawScale(0.3f * _gizmo.SetSpread, false, Vector3.right, Vector3.up, _gizmo._Draw.panZ);
        

        GL.PopMatrix();

    }

















}
