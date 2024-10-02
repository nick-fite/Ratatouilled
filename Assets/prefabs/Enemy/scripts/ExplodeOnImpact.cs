using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ExplodeOnImpact : MonoBehaviour
{
    public bool Explode;
    public float ExplosiveForce;
    public float ExplosiveRadius;
    public GameObject enemyObj;
    
    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.name);
        if(col.gameObject.tag == "Explodable" && Explode)
        {
            Collider[] colliders = Physics.OverlapSphere(col.transform.position, ExplosiveRadius);
            foreach(Collider collider in colliders)
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if(rb != null){
                rb.AddExplosionForce(ExplosiveForce, transform.position, ExplosiveRadius);
                }
                Enemy enemy = collider.GetComponent<Enemy>();
                enemy.AddHealth(-100, this.transform, ExplosiveForce);
            }
        }
        if(col.gameObject.tag == "Explodable")
        {
            StartCoroutine(WaitThenDestroy());
        }
    }

    IEnumerator WaitThenDestroy()
    {

        yield return new WaitForSeconds(3.0f);
        Destroy(enemyObj);
    }
}
