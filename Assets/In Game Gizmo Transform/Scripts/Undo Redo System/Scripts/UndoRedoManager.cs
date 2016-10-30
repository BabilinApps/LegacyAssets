using UnityEngine;
using System.Collections;

public class UndoRedoManager : MonoBehaviour {
	private int steps = 0;
	public ObjectSystem _system = new ObjectSystem();

    [Tooltip("The key to use when you press to undo.")]
    public KeyCode UndoKey = KeyCode.Z;
    [Tooltip("The key to use when you press to redo.")]
    public KeyCode RedoKey = KeyCode.R;
    [Tooltip("The key to use when you press to Clear.")]
    public KeyCode ClearKey = KeyCode.Escape;
    [Tooltip("What Layer gets affected.")]
    public LayerMask ActiveLayer;
    [Tooltip("Object being tracked")]
    private GameObject _currentObj;
    [Tooltip("Delay Before Mass Undo Or Redo")]
    public float DelayTime = .5f;
    private float CurTime;
    private float setTimer;
    // Use this for initialization
    void Start () {
		_system = new ObjectSystem ();
        CurTime = DelayTime;
        setTimer = DelayTime / 4;

        if (gameObject.GetComponent<GizmoSelection>())
        {

            ActiveLayer = gameObject.GetComponent<GizmoSelection>().DynamicLayer;
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ActiveLayer)) {
                _currentObj = hit.collider.gameObject;
                _system.Store(_currentObj, steps);
                steps = _system._spot;
            }
        }
        if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(0) ||
            Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) ||
            Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)) && _currentObj != null) {
			_system.Store(_currentObj, steps);
            steps = _system._spot;
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
            steps = _system._spot = 0;
            _system.StoredObj.Clear();
		}

        if (Input.GetKeyDown(KeyCode.Z)) {
            if (_system._spot > 1)
            {
                steps--;
               _system._spot--;
                _system.Call(_system._spot - 1);
            }

        }

        if (Input.GetKeyDown(KeyCode.R)) {
            if (_system._spot < _system.StoredObj.Count) {
                steps++;
                _system._spot++;
                _system.Call(_system._spot - 1);
            }
        }


        if (Input.GetKey(KeyCode.Z))
        {
            CurTime = CurTime - Time.deltaTime;
            if (_system._spot > 1 && CurTime < 0)
            {
                setTimer = setTimer - Time.deltaTime;
                if (setTimer < 0)
                {
                    steps--;
                    _system._spot--;
                    _system.Call(_system._spot - 1);
                    setTimer = DelayTime / 4;
                }
            }

        }

        if (Input.GetKey(KeyCode.R))
        {
            CurTime = CurTime - Time.deltaTime;
            if (_system._spot < _system.StoredObj.Count && CurTime < 0)
            {
                setTimer = setTimer - Time.deltaTime;
                if (setTimer < 0)
                {
                    steps++;
                    _system._spot++;
                    _system.Call(_system._spot - 1);
                    setTimer = DelayTime / 4;
                }
            }
        }

        if (!Input.GetKey(KeyCode.R) && !Input.GetKey(KeyCode.Z)) {
            CurTime = DelayTime;
        }

    }


}
