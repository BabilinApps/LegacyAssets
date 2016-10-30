using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[DisallowMultipleComponent]
[HelpURL("http://www.babilinapps.com/contact/")]
public class MaterialManager : MonoBehaviour {
    [Header("GUI Elements")]
    [Tooltip("GUI Panel that will have the buttons")]
    [SerializeField]
    private GameObject GUIPanel;

    [Tooltip("Button type to use")]
    [SerializeField]
    private GameObject Button;

    [Tooltip("selection indicator type to use.")]
    [SerializeField]
    private GameObject SelectionIndicator;

    [Header("Advanced Settings")]
    [Tooltip("The color for the indicator to use when an object is selected.")]
    [SerializeField]
    private Color SelectedColor = Color.cyan;
    [Tooltip("The color for the indicator to use when an object is deselected")]
    [SerializeField]
    private Color DeselectedColor = Color.white;

    [Tooltip("The Current object's stats that are selected")]
    private MaterialChanger CurrentStats;
    [Tooltip("The Image of the 'SelectionIndicator' object.")]
    private Image Indicator;

    [Tooltip("The target of the 'SelectionIndicator'.")]
    private GameObject SelectedObject;
    [Tooltip("List of all the current stats being changed")]
    private List<ObjectStats> AllObjectStats = new List<ObjectStats>();

    private Vector3 ObjCenter; 
    // Use this for initialization
    void Start () {
        if (!EventSystem.current)
        {
            gameObject.AddComponent<EventSystem>();
            gameObject.AddComponent<StandaloneInputModule>();
        }

        if (!Button)
        {
            Button = Resources.Load("Button") as GameObject;
        }

        if (!SelectionIndicator)
        {
            SelectionIndicator = Resources.Load("SelectionIndicator") as GameObject;
            SelectionIndicator.transform.SetParent(transform);
        }


        Indicator = SelectionIndicator.GetComponent<Image>();
        SelectionIndicator.SetActive(false);

    }

    /// <summary>
    /// Changes the color of the indicator if there is an object selected
    /// </summary>
    void UpdateSelectionIndicator() {
        if (CurrentStats != null)
        {
            Indicator.color = SelectedColor;
            SelectionIndicator.transform.position = Camera.main.WorldToScreenPoint(SelectedObject.GetComponent<Collider>().bounds.center);
        }
        else
        {
            Indicator.color = DeselectedColor;
        }

    }
	
	// Update is called once per frame
	void Update () {
        UpdateSelectionIndicator();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            CheckForOptions(hit.transform.gameObject);

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                GetMaterialObjects(hit.transform.gameObject);


            }
        }
        else if (CurrentStats == null)
        {
            SelectionIndicator.SetActive(false);
        }

    }
    /// <summary>
    /// Checks if the object has the ability to change materials
    /// </summary>
    /// <param name="_obj"> What object to check</param>
    public void CheckForOptions(GameObject _obj)
    {
        if (CurrentStats != null)
            return;

        if (_obj.GetComponent<MaterialChanger>()) 
        {
            ObjCenter = _obj.GetComponent<Collider>().bounds.center;
            SelectionIndicator.transform.position = Camera.main.WorldToScreenPoint(ObjCenter);
            SelectionIndicator.SetActive(true);

        }
        else
        {
            SelectionIndicator.SetActive(false);
        }
    }
    /// <summary>
    ///Checks the tag of a given object and grabs the correct MaterialObjects.
    ///if there is a material option with the given tag, it will get the stats of the object. (The ObjStats)
    ///It will then create buttons with all of the material options
    /// </summary>
    /// <param name="_obj"> What object to check</param>
    public void GetMaterialObjects(GameObject _obj)
    {
        //clear all buttons
        foreach (Transform child in GUIPanel.transform)
        {
            Destroy(child.gameObject);
        }
        CurrentStats = null;
        SelectedObject = null;
        AllObjectStats.Clear();
        if (_obj.GetComponent<MaterialChanger>())
        {
            ObjCenter = _obj.GetComponent<Collider>().bounds.center;
            SelectionIndicator.transform.position = Camera.main.WorldToScreenPoint(ObjCenter);
            SelectionIndicator.SetActive(true);
            CurrentStats = _obj.GetComponent<MaterialChanger>();
            if (CurrentStats.ChangeByTag)
            {
                AllObjectStats = CurrentStats.TaggedStats(GameObject.FindGameObjectsWithTag(_obj.tag));
            }
            else {
                AllObjectStats.Add(CurrentStats.Stats);
            }
            SelectedObject = _obj;
            ButtonCreator();

        }

    }

    /// <summary>
    ///Creates a button with all of the material texture options.
    /// </summary>
    public void ButtonCreator()
    {
        foreach (ComplexMaterial diffrentMaterial in CurrentStats._Materials)
        {
            if (!diffrentMaterial._Material)
            {
                Debug.LogError("Game object '"+CurrentStats.gameObject.name + "' does not have a material option set. Consider removing the option or assigning a material.");
                return;
            }
                
            GameObject newButton = Instantiate(Button, Vector3.zero, Quaternion.identity) as GameObject;
            newButton.transform.SetParent(GUIPanel.transform);


            Button _b = newButton.GetComponent<Button>();
            newButton.GetComponent<RawImage>().texture = diffrentMaterial._Material.mainTexture;
            newButton.GetComponent<RawImage>().color = diffrentMaterial._Material.color;

            Color highlightColor = diffrentMaterial._Material.color;
            highlightColor = Color.clear;
            ColorBlock cb = _b.colors;
            cb.highlightedColor = highlightColor;
            _b.colors = cb;

            Material StoreMaterial = diffrentMaterial._Material;

            AddListener(_b, StoreMaterial, diffrentMaterial.MaterialIndex);
        }

    }
    /// <summary>
    /// adds a listener to the button that calls the void 'Call Material'
    /// </summary>
    /// <param name="b"> The button that will be assigned</param>
    /// <param name="value"> the material value that it will call when clicked</param>
    void AddListener(Button b, Material value, int Index)
    {
        b.onClick.AddListener(() => CallMaterial(value, Index));
    }

    /// <summary>
    /// sets the material of the object.
    /// </summary>
    /// <param name="setMaterial"> The material to set</param>
    public void CallMaterial(Material setMaterial, int Index = 0)
    {
        if (!setMaterial) {
            Debug.LogError("You have not set a material option for this button. Please make sure all of your material options have a material selected.");
        }
        

        foreach (ObjectStats _stats in AllObjectStats)
        {
            //sets the material of the object in the correct spot or hierarchy
            _stats.StartMaterials[Index] = setMaterial;
            // sets all the materials. this has to be done so that the hierarchy is not messed up
            _stats._ObjRenderer.materials = _stats.StartMaterials;
        }



        if (CurrentStats.ChangeChildObjects)
        {
            foreach (ObjectStats _stats in AllObjectStats)
            {
                Renderer[] Children = _stats._ObjRenderer.gameObject.GetComponentsInChildren<Renderer>();
                foreach (Renderer _Rend in Children)
                {
                    //check if the child object has the same about of materials as the parent. If not, set the first material
                    if (_Rend.materials.Length - 1 < Index) {
                        _Rend.material = setMaterial;
                    }
                    else
                    _Rend.materials = _stats.StartMaterials;
                }
            }

        }
    }
}
