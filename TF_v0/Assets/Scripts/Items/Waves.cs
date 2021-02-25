using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Agua>())
        {
            other.GetComponent<Rigidbody>().AddForce(other.transform.position * Time.deltaTime);
          //  rb.AddExplosionForce(other.GetComponent<Rigidbody>().GetComponent<Rigidbody>().mass * explosionForce, transform.localPosition , explosionRadius, 1f, ForceMode.Impulse);

        }
    }
}
