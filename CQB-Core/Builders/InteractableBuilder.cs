using EFT.Interactive;
using UnityEngine;

namespace CQB.Builders;

public class InteractableBuilder<T> where T : InteractableObject
{
    
    private static string _name;
    private static Vector3 _position;
    private static Vector3 _scale;
    private static Transform _parent;
    private static bool _debug;
    
    public static GameObject Build(string name, Vector3 position, Vector3 scale, Transform parent, bool debug)
    {
        _name = name;
        _position = position;
        _scale = scale;
        _parent = parent;
        _debug = debug;

        if (_debug)
        {
            Plugin.LogSource.LogDebug("InteractableBuilder<" + typeof(T) + "> created");  
            
        }
        return CreateGameObject();
    }

    private static GameObject CreateGameObject()
    { 
       GameObject interactableObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
       interactableObject.name = _name;
       interactableObject.transform.position = _position;
       interactableObject.transform.localScale = _scale;
       interactableObject.transform.SetParent(_parent, false);
       interactableObject.AddComponent<T>();
       interactableObject.GetComponent<BoxCollider>().enabled = false;
       interactableObject.GetComponent<MeshRenderer>().enabled = _debug;
       interactableObject.SetActive(true);
       
       return interactableObject;
    }
    
}