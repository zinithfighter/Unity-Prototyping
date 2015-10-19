using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Ability : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("set parent");
        GameObject parent = FindObjectOfType<UIAbilitites>().gameObject as GameObject;        
        transform.parent = parent.transform;
    }
}
