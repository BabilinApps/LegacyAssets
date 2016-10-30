
using UnityEngine;
[RequireComponent(typeof(GizmoSelection))]
public class StatePatternGizmo : MonoBehaviour {
    /// <summary>
    /// The Interface that controles all the changes in the scripts 'RotateAction' 'TransformAction' 'ScaleActon'. 
    /// Before changing anyhting in the Interfaces, send us an email and we can help you contact@babilinapps.com.
    /// </summary>
    
    [Tooltip("Scale of the gizmo.")]
    public float Spread = .5f;
    public enum MOVETYPE { NONE, X, X2, Y, Y2, Z, Z2, XZ, YZ, XY, RX, RY, RZ, TX, TY, TZ, TX2, TY2, TZ2, TXY, TXZ, TYZ }; // all the ways an object can be altered
    [Tooltip("how close your mouse has to be in order to turn the object")]
    public float Sencsitivity = 1.0f; //sencsitivity
    [Tooltip("How fast the Item turns.")]
    public int RotationSpeed = 40; // how fast the object rotates around in the scene
    [Tooltip("Rotation by using the mouse axies instead of contact with the gizmo")]
    public bool NeedContactToTurn = true;
    [Tooltip("The gizmo to be fully round")]
    public bool FullcircleGizmo = false;
    [Tooltip("highlight color")]
    public Color SelectedColor = new Color(1, 1, 0, 0.5f);
    [Tooltip("Use the Global Vector to be the reference for the gizmo. AKA “Alter in global space”.")]
    public bool GlobalLocation = false;
    [Tooltip("When pressing the arrow keys how many unites do you want the object to move")]
    public float MovementUnit = 1;

    [Tooltip("When pressing the arrow keys how many unites do you want the object to rotate")]
    public float RotationUnit = 25;

    [Tooltip("When pressing the arrow keys how many unites do you want the object to Scale")]
    public float ScaleUnit = 1;
    [Tooltip("Multiply the scale factor")]
    public float ScaleMultiplier = 1;
    [Tooltip("Key to press to activate Transformation Option.")]
    public KeyCode TransformationOption = KeyCode.Alpha1;
    [Tooltip("Key to press to activate Rotation Option.")]
    public KeyCode RotationOption = KeyCode.Alpha2;
    [Tooltip("Key to press to activate Scale Option.")]
    public KeyCode ScaleOption = KeyCode.Alpha3;
    [Tooltip("Key to toggle Contact to Turn.")]
    public KeyCode ToggleContactToTurn = KeyCode.Q;

    [Tooltip("Key to toggle Full Cirlce Gizmo")]
    public KeyCode ToggleFullCirlce= KeyCode.C;
    private Plane planeXZ, planeYZ, planeXY; // all form of rotation planes
    private float enterX, enterY, enterZ;

    [HideInInspector]
    public GameObject Item; // The item that is being modified.
    [HideInInspector]
    public MOVETYPE SelectedType; // current alteration
    [HideInInspector]
    public IGizmoState currentState; // The action that the object is in.
    [HideInInspector]
    public TransformAction transformAction;
    [HideInInspector]
    public RotateAction rotateAction;
    [HideInInspector]
    public ScaleAction scaleAction;


    [HideInInspector]
    public float distanceMovement, SetSpread; //Used to ajust the gizmos with distance.
    [HideInInspector]
    public  Vector3 CurrentScale, hitX, hitY, hitZ; //The vectors for scale and for the rotation planes.
    [HideInInspector]
    public Matrix4x4 rotationMatrix;  // draw the rotation cercal

    [HideInInspector]
    public float rlensX, rlensY, rlensZ = 0; // clips the planes into circle

    [HideInInspector]
    public DrawAction _Draw = new DrawAction(); //calls the drawing script
    [HideInInspector]
    public Camera cam; // refrence to the main camera. Change this if you want to use a diffrent camera.

    [HideInInspector]
    public Material lineMaterial; // calls for the gizmo shader in the resources folder

    [HideInInspector]
    public Matrix4x4 locklocalToWorldMatrix; // the matrax befor the alterantion (Used in the other states
    [HideInInspector]
    public float StartSencsitivity; // the sencitivity before it is changed for the rotation second option affect
    [HideInInspector]
    public bool SecondAction = false; // Holding down the shift keys
    [HideInInspector]
    public bool ObjectSelected = false; // used to turn off the gizmo Called by the selection script
    [HideInInspector]
    public Vector3 matScal = new Vector3(1, 1, 1); // scale of gizmo
    private float TimerdelayForArrows = .3f;
    private float TimerForArrows;
    void Awake() {
        _Draw._Cam = Camera.main;
        lineMaterial = new Material(Shader.Find("Custom/GizmoShader"));
        
        transformAction = new TransformAction(this);
       rotateAction = new RotateAction(this);
       scaleAction = new ScaleAction(this);
        Item = gameObject;
        CurrentScale = Item.transform.localScale;



        TimerForArrows = TimerdelayForArrows;

    }

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        
        _Draw.NothingSelected();
        currentState = transformAction;
        currentState.Awake(Item);
        StartSencsitivity = Sencsitivity;
    }
    // the second action keys and the arrow key actions
    void KeyBindings() {
        if (currentState == rotateAction) {
            if (Input.GetKeyDown(ToggleContactToTurn))
                NeedContactToTurn = !NeedContactToTurn;

            if (Input.GetKeyDown(ToggleFullCirlce))
                FullcircleGizmo = !FullcircleGizmo;
        }

        if (currentState == transformAction)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                Item.transform.position += (Vector3.right * MovementUnit);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Item.transform.position += (Vector3.right * -MovementUnit);

            if (Input.GetKeyDown(KeyCode.UpArrow))
                Item.transform.position += (Vector3.forward * MovementUnit);

            if (Input.GetKeyDown(KeyCode.DownArrow))
                Item.transform.position += (Vector3.forward * -MovementUnit);


            if (Input.GetKey(KeyCode.RightArrow)) {
                TimerForArrows = TimerForArrows - Time.deltaTime;
                if (TimerForArrows < 0)
                {
                    Item.transform.position += (Vector3.right * MovementUnit);
                    TimerForArrows = TimerdelayForArrows;
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow)) {
                TimerForArrows = TimerForArrows - Time.deltaTime;
                if (TimerForArrows < 0)
                {
                    Item.transform.position += (Vector3.right * -MovementUnit);
                        TimerForArrows = TimerdelayForArrows;
                    }
                }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                TimerForArrows = TimerForArrows - Time.deltaTime;
                if (TimerForArrows < 0)
                {
                    Item.transform.position += (Vector3.forward * MovementUnit);
                            TimerForArrows = TimerdelayForArrows;
                        }
                    }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                TimerForArrows = TimerForArrows - Time.deltaTime;
                if (TimerForArrows < 0)
                {
                    Item.transform.position += (Vector3.forward * -MovementUnit);
                    TimerForArrows = TimerdelayForArrows;
                }
            }
        }

        if (currentState == rotateAction)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                Item.transform.Rotate(0, -RotationUnit, 0);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Item.transform.Rotate(0, RotationUnit, 0);

            if (Input.GetKeyDown(KeyCode.UpArrow))
                Item.transform.Rotate(-RotationUnit, 0, 0);

            if (Input.GetKeyDown(KeyCode.DownArrow))
                Item.transform.Rotate(RotationUnit, 0, 0);

            if (Input.GetKey(KeyCode.RightArrow))
            {
                TimerForArrows = TimerForArrows - Time.deltaTime;
                if (TimerForArrows < 0)
                {
                    Item.transform.Rotate(0, -RotationUnit, 0);
                    TimerForArrows = TimerdelayForArrows;
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                TimerForArrows = TimerForArrows - Time.deltaTime;
                if (TimerForArrows < 0)
                {
                    Item.transform.Rotate(0, RotationUnit, 0);
                    TimerForArrows = TimerdelayForArrows;
                }
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                TimerForArrows = TimerForArrows - Time.deltaTime;
                if (TimerForArrows < 0)
                {
                    Item.transform.Rotate(-RotationUnit, 0, 0);
                    TimerForArrows = TimerdelayForArrows;
                }
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                TimerForArrows = TimerForArrows - Time.deltaTime;
                if (TimerForArrows < 0)
                {
                    Item.transform.Rotate(RotationUnit, 0, 0);
                    TimerForArrows = TimerdelayForArrows;
                }
            }
        }

        if (currentState == scaleAction)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                Item.transform.localScale += new Vector3(ScaleUnit, 0, 0);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Item.transform.localScale += new Vector3(-ScaleUnit, 0, 0);

            if (Input.GetKeyDown(KeyCode.UpArrow))
                Item.transform.localScale += new Vector3(0, ScaleUnit, 0);

            if (Input.GetKeyDown(KeyCode.DownArrow))
                Item.transform.localScale += new Vector3(0, -ScaleUnit, 0);

            if (Input.GetKey(KeyCode.RightArrow)) {
                Item.transform.localScale = CurrentScale;
                TimerForArrows = TimerForArrows - Time.deltaTime;
                if (TimerForArrows < 0)
                {
                    Item.transform.localScale += new Vector3(ScaleUnit, 0, 0);
                    TimerForArrows = TimerdelayForArrows;
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow)) {
                Item.transform.localScale = CurrentScale;
                TimerForArrows = TimerForArrows - Time.deltaTime;
                    if (TimerForArrows < 0)
                    {
                        Item.transform.localScale += new Vector3(-ScaleUnit, 0, 0);
                    TimerForArrows = TimerdelayForArrows;
                }
                    }
                    if (Input.GetKey(KeyCode.UpArrow)) {
                Item.transform.localScale = CurrentScale;
                TimerForArrows = TimerForArrows - Time.deltaTime;
                        if (TimerForArrows < 0)
                        {
                          Item.transform.localScale += new Vector3(0, ScaleUnit, 0);
                    TimerForArrows = TimerdelayForArrows;
                }
                    }
                    if (Input.GetKey(KeyCode.DownArrow)) {
                Item.transform.localScale = CurrentScale;
                TimerForArrows = TimerForArrows - Time.deltaTime;
                            if (TimerForArrows < 0)
                            {
                     Item.transform.localScale += new Vector3(0, -ScaleUnit, 0);
                    TimerForArrows = TimerdelayForArrows;
                }
                    }
                }

        

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            GlobalLocation = !GlobalLocation;
            rotationMatrix = Matrix4x4.TRS(Item.transform.position, cam.transform.rotation, matScal);
            locklocalToWorldMatrix = rotationMatrix;
        }
        else
        {
            Sencsitivity = StartSencsitivity;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            SecondAction = true;
        else
            SecondAction = false;

        if (Input.GetKeyDown(TransformationOption))
        {
            setTransform();
        }

        if (Input.GetKeyDown(RotationOption))
        {
            setRotation();
        }

        if (Input.GetKeyDown(ScaleOption))
        {
            setScale();
        }



    }
    // Update is called once per frame
    void Update()
    {
        _Draw.FullcircleGizmo = FullcircleGizmo;
        _Draw.Global = GlobalLocation;
  
    }

   
   

    void PlaneActions()
    {
        if (GlobalLocation)
        {
            planeXZ.SetNormalAndPosition(Vector3.up, Item.transform.position);
            planeXY.SetNormalAndPosition(new Vector3(0,0,1), Item.transform.position);
            planeYZ.SetNormalAndPosition(new Vector3(1, .2f, 0), Item.transform.position);
        }
        else {
            planeXZ.SetNormalAndPosition(Item.transform.up, Item.transform.position);
            planeXY.SetNormalAndPosition(Item.transform.forward, Item.transform.position);
            planeYZ.SetNormalAndPosition(Item.transform.right, Item.transform.position);

        }


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      

        if (currentState == scaleAction || currentState == transformAction)
        {
            planeXZ.Raycast(ray, out enterX);
            hitX = ray.GetPoint(enterX);
            planeXY.Raycast(ray, out enterY);
            hitY = ray.GetPoint(enterY);
            planeYZ.Raycast(ray, out enterZ);
            hitZ = ray.GetPoint(enterZ);

            hitX = rotationMatrix.inverse.MultiplyPoint(hitX);
            hitZ = rotationMatrix.inverse.MultiplyPoint(hitZ);
            hitY = rotationMatrix.inverse.MultiplyPoint(hitY);
        }

        if (currentState == rotateAction) {

            planeYZ.Raycast(ray, out enterX);
            planeXY.Raycast(ray, out enterZ);
            planeXZ.Raycast(ray, out enterY);
            hitX = ray.GetPoint(enterX);
            hitY = ray.GetPoint(enterY);
            hitZ = ray.GetPoint(enterZ);
            if (SelectedType == StatePatternGizmo.MOVETYPE.RX)
            {
                hitX = locklocalToWorldMatrix.inverse.MultiplyPoint(hitX);
            }
            else
                hitX = rotationMatrix.inverse.MultiplyPoint(hitX);

            if (SelectedType == StatePatternGizmo.MOVETYPE.RY)
            {
                hitY = locklocalToWorldMatrix.inverse.MultiplyPoint(hitY);
            }
            else { hitY = rotationMatrix.inverse.MultiplyPoint(hitY); }

            if (SelectedType == StatePatternGizmo.MOVETYPE.RZ)
            {
                hitZ = locklocalToWorldMatrix.inverse.MultiplyPoint(hitZ);
            }
            else
                hitZ = rotationMatrix.inverse.MultiplyPoint(hitZ);

            rlensX = (Mathf.Sqrt(hitX.z * hitX.z + hitX.y * hitX.y)/3) * distanceMovement * SetSpread;
            rlensZ = (Mathf.Sqrt(hitZ.x * hitZ.x + hitZ.y * hitZ.y)/3) * distanceMovement * SetSpread;
            rlensY = (Mathf.Sqrt(hitY.x * hitY.x + hitY.z * hitY.z)/3) * distanceMovement * SetSpread;
        }





    }


    void LateUpdate()
    {
        
        if (Item)
        {
            KeyBindings();
            PlaneActions();
            

            if (Input.GetMouseButtonUp(0))
            {

                CurrentScale = Item.transform.localScale;
            }

            if (Input.GetMouseButtonDown(1))
            {

                Item.transform.localScale = CurrentScale;
            }



            float DistanceToCam = Vector3.Distance(Camera.main.transform.position, Item.transform.position) / 10;
            SetSpread = Mathf.Clamp(DistanceToCam, Spread, Spread * 2);


            distanceMovement = Vector3.Distance(Camera.main.transform.position, Item.transform.position) / 2;
        }

    }

    void OnPostRender() {
        if(ObjectSelected)
            currentState.OnRenderObject();
    }

    public void setTransform() {
        currentState = transformAction;
        currentState.Awake(Item);
        _Draw.NothingSelected();
        CurrentScale = Item.transform.localScale;
        locklocalToWorldMatrix = rotationMatrix;
    }

    public void setRotation()
    {
        currentState = rotateAction;
        currentState.Awake(Item);
       
        _Draw.NothingSelected();
        CurrentScale = Item.transform.localScale;
        locklocalToWorldMatrix = rotationMatrix;
    }

    public void setScale()
    {
        currentState = scaleAction;
        currentState.Awake(Item);
        _Draw.NothingSelected();
        CurrentScale = Item.transform.localScale;
        locklocalToWorldMatrix = rotationMatrix;
    }
}
