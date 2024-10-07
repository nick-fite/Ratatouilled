using UnityEngine;

public class ReplaceCube : MonoBehaviour
{
    [SerializeField] GameObject replacement;
    public void Replace()
    {
        Instantiate(replacement);
        Destroy(gameObject);
    }
}
