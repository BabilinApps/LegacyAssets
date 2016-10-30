using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
public class ObjectStats
{
    [Tooltip("collects all the materials that the object has")]
    public Material[] StartMaterials;
    [Tooltip("stored renderer of object.")]
    public Renderer _ObjRenderer;


    /// <summary>
    /// Creates an ObjStats class. This stores the renderer, materials, and sets what material will be changing
    /// </summary>
    ///  <param name="_obj">The renderer that you want to make a stat for.</param>
    public ObjectStats(Renderer _obj = null)
    {
        if (!_obj)
            return;

        StartMaterials = _obj.materials;
        _ObjRenderer = _obj;

    }
}

public static class CopyClass
{
    /// <summary>
    /// Creates a copy of a Component
    /// </summary>
    /// <typeparam name="T"> The component</typeparam>
    /// <param name="comp"> the component to copy the information too</param>
    /// <param name="other"> The component to copy</param>
    /// <returns>returns a copy of a component</returns>
    public static T GetCopyOf<T>(this Component comp, T other) where  T: Component
    {
        Type type = comp.GetType();
        if (type != other.GetType()) return default(T); // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return comp as T;
    }

    /// <summary>
    ///  Add a component directly to GameObject
    /// </summary>
    /// <typeparam name="T">The component</typeparam>
    /// <param name="go"> the object to add it to</param>
    /// <param name="toAdd"> the component to add</param>
    /// <returns></returns>
    public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
    {
        return go.AddComponent<T>().GetCopyOf(toAdd) as T;
    }
}

[Serializable]
public class ComplexMaterial {
    [Tooltip("Material to store.")]
    public Material _Material;
    [Tooltip("Material Index to replace.")]
    public int MaterialIndex = 0;
}
[AddComponentMenu("Material Changer/Material Change Options")]
[DisallowMultipleComponent]
[HelpURL("http://www.babilinapps.com/contact/")]
public class MaterialChanger : MonoBehaviour {
    [Header("Material Options")]
    [Tooltip("List of material options for the object")]
    public List<ComplexMaterial> _Materials = new List<ComplexMaterial>();
    [Header("Advanced Options")]
    [Tooltip("Change all objects with this objects tag at the same time")]
    public bool ChangeByTag =false;
    [Tooltip("Change all of the objects children")]
    public bool ChangeChildObjects = false;
    public ObjectStats Stats;

    /// <summary>
    /// Creates a list of 'ObjectStats' based on the [] of GameObjects passed
    /// </summary>
    /// <param name="otherObjects"> Array of GameObjects to be Changed to List<ObjectStats> </param>
    /// <returns></returns>
    public List<ObjectStats> TaggedStats(GameObject[] otherObjects) {

        List<ObjectStats> StatsList = new List<ObjectStats>();
        foreach (GameObject obj in otherObjects)
        {
            MaterialChanger objstat = obj.GetComponent<MaterialChanger>();

            if (objstat)
            {
                StatsList.Add(objstat.Stats);
            }
        }
        return StatsList;
    }
	// Use this for initialization
	void Start () {
        ObjectStats objstat = new ObjectStats(gameObject.GetComponent<Renderer>());
        Stats = objstat;

        if (ChangeByTag) {
            foreach (GameObject otherObject in GameObject.FindGameObjectsWithTag(gameObject.tag)) {
                if (!otherObject.GetComponent<MaterialChanger>()) {
                    otherObject.AddComponent(this);
                }
                

            }
        }
	}
    /// <summary>
    /// Adds an object to the Materials list.
    /// </summary>
    [ContextMenu("Add A Material Option")]
    void AddOption()
    {
        ComplexMaterial NewOption = new ComplexMaterial();
        _Materials.Add(NewOption);
        Debug.Log("Added Material Option");
    }
    /// <summary>
    /// Clears all materials from material list
    /// </summary>
    [ContextMenu("Clear All Material Options")]
    void ClearOptions()
    {
        _Materials.Clear();
        Debug.Log("Cleared Material Options");
    }


}
