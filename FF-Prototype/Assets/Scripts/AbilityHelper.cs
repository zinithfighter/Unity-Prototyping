using UnityEngine;
using System.Collections;

public class AbilityHelper : MonoBehaviour
{
    
    public Animator anim;
    public Transform target;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            StartCoroutine("Move");
            
    }
    IEnumerator Move()
    {
        while (transform.position != target.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, .01f);
            yield return null;
        }
    }
}
