using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class ObjectSystem : System.Object{
	public List<ObjectInfo> StoredObj = new List<ObjectInfo>();
    private bool NewObj;
    public int _spot = 1;


    GameObject CreateObj (int Slot){
		GameObject _obj = new GameObject ();
		if(!string.IsNullOrEmpty(StoredObj[Slot]._Name))
		_obj.name = StoredObj[Slot]._Name;

		_obj.transform.position = StoredObj[Slot]._Position;

		_obj.transform.rotation = StoredObj[Slot]._Rotation;

		_obj.transform.localScale = StoredObj[Slot]._Scale;

		_obj.AddComponent<Renderer> ();
		if(StoredObj[Slot]._Material)
		_obj.GetComponent<Renderer> ().material = StoredObj[Slot]._Material;
		if (StoredObj [Slot]._Mesh) {
			_obj.AddComponent<MeshFilter> ();
			_obj.GetComponent<MeshFilter> ().mesh = StoredObj [Slot]._Mesh;
		}
		return _obj;
	}

	public void Store(GameObject gameObj, int spot){
        NewObj = false;
        ObjectInfo obj = new ObjectInfo ();
		obj._Object = gameObj;
		obj._Name = gameObj.name;
		obj._Position = gameObj.transform.position;
		obj._Scale = gameObj.transform.localScale;
		obj._Rotation = gameObj.transform.rotation;

		if(gameObj.GetComponent<Renderer> ())
		obj._Material = gameObj.GetComponent<Renderer> ().material;

		if(gameObj.GetComponent<MeshFilter>())
		obj._Mesh = gameObj.GetComponent<MeshFilter>().mesh;
        
        NewObj = IsNew(obj,_spot);

        if (NewObj == true)
        {
            _spot = SetSpot(spot);
            StoredObj.Insert(_spot - 1, obj);
       
        }
	}

    public int SetSpot(int set) {
        if (set< StoredObj.Count)
        {
            set = set + 1;
            return set++;

        }
        else
        {
            return StoredObj.Count +1 ;
        }

        
    }
    // Edit this if you add or want to use other parameters to check for new objects
    bool IsNew (ObjectInfo obj, int spot) {
        if(StoredObj.Count <=0)
            return true;

        ObjectInfo LastObj = StoredObj[spot - 1];
        if (LastObj._Object == obj._Object)
        {
            if (LastObj._Name.Equals(obj._Name) &&
                LastObj._Position == obj._Position &&
                LastObj._Rotation == obj._Rotation &&
                LastObj._Material == obj._Material) {
                return false;
            }
            else
                return true;
        }
        else {
            return true;
        }
    }

	public void Call(int Slot){
		GameObject _obj = StoredObj [Slot]._Object;
		if(!string.IsNullOrEmpty(StoredObj[Slot]._Name))
		_obj.name = StoredObj[Slot]._Name;
		
		_obj.transform.position = StoredObj[Slot]._Position;
		
		_obj.transform.rotation = StoredObj[Slot]._Rotation;
		
		_obj.transform.localScale = StoredObj[Slot]._Scale;
	
		if(StoredObj[Slot]._Material)
			_obj.GetComponent<Renderer> ().material = StoredObj[Slot]._Material;
	}



}
