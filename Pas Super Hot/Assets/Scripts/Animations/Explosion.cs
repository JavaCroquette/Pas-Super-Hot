using UnityEngine;

public class Explosion : MonoBehaviour
{
    #region Vars
    [SerializeField]
    private float minRadius = 1.0F;
    [SerializeField]
    private float maxRadius = 5.0F;
    [SerializeField]
    private float minPower = 10.0F;
    [SerializeField]
    private float maxPower = 50.0F;
    #endregion

    #region BuildIn
    void Start()
    {
        Vector3 explosionPos = transform.position;
        float radius = Random.Range(minRadius, maxRadius);
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            //Debug.Log(hit.name);
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(Random.Range(minPower, maxPower), explosionPos, radius, Random.Range(-6f, 6f));
        }
    }
    #endregion

}
