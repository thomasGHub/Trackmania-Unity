using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> gameObjects = new List<GameObject> ();

    public void Outline()
    {
        foreach(GameObject go in gameObjects)
        {
            if(go.GetComponent<Material>() != null)
            {
                print("ok");
                go.GetComponent<Material>().EnableKeyword("_EMISSION");
            }
            
        }
    }
}
