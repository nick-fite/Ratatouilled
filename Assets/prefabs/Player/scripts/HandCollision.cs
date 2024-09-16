using UnityEngine;

public class HandCollision : MonoBehaviour
{
    [SerializeField] int Damage = 10;
    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.name);
        if(col.gameObject.tag == "Enemy")
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            enemy.AddHealth(-Damage);
        }
    }
}
