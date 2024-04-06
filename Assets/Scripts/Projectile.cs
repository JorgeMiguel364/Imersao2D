using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Velocidade de movimento do projétil
    public float sp = 8;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * sp);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D coll2D)
    {
        switch (coll2D.gameObject.tag)
        {
            case "SolidBlock":
                //Destroy(gameObject);
                break;
        }
    }
}
