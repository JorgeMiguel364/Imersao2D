using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBlock : MonoBehaviour
{
    // Valores que controlam os limites do cenário
    private const float limitX = 8.6f;
    private const float limitY = 5.6f;

    private Rigidbody2D rig;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RestrictMovement();
    }

    // Gerencia restrição de movimentos nos limites do cenário
    void RestrictMovement()
    {
        float xRestrict;

        if (transform.position.x <= -limitX || transform.position.x >= limitX)
        {
            xRestrict = Mathf.Clamp(transform.position.x, -limitX, limitX);
            transform.position = new Vector3(xRestrict, transform.position.y, transform.position.z);
        }

        if (transform.position.y <= -limitY)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Abs(-limitY), transform.position.z);
            rig.velocity = Vector3.zero;
        }
    }
}
