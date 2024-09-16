using UnityEngine;

public class HandCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.name);
    }
}
