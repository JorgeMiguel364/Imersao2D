using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost1 : MonoBehaviour
{
    public float spd;
    public Transform rightColl;
    public Transform leftColl;
    public LayerMask layer;

    // Valores que controlam os limites do cenário
    private const float limitX = 8.6f;
    private const float limitY = 5.6f;

    // Pontuação adicional
    private int score = 1;

    private bool colliding;

    private Animator anm;
    private Rigidbody2D rig;

    void Start()
    {
        anm = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();

        GameController.instance.enemiesQt += 1;
    }

    void Update()
    {
        Movement();
        RestrictMovement();
    }

    // Gerencia movimentação no cenário
    void Movement()
    {
        transform.position += Vector3.left * Time.deltaTime * spd;

        colliding = Physics2D.Linecast(rightColl.position, leftColl.position, layer);

        if (colliding || transform.position.x <= -limitX || transform.position.x >= limitX)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);
            spd *= -1f;
        }
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

    void OnTriggerEnter2D(Collider2D coll2D)
    {
        switch (coll2D.gameObject.tag) 
        {
            case "Project":
                spd = 0;
                Destroy(GetComponent<BoxCollider2D>());
                Destroy(GetComponent<Rigidbody2D>());
                Destroy(coll2D.gameObject);

                GameController.instance.enemiesQt -= 1;

                if (GameController.instance.ghostScore >= GameController.instance.maxGhostScore)
                {
                    GameController.instance.ghostScore = GameController.instance.maxGhostScore;
                }
                else
                {
                    GameController.instance.ghostScore += score;
                }

                GameController.instance.updateScoreText();

                if (Random.Range(0, 2) == 0) 
                {
                    anm.SetTrigger("die1");
                    Destroy(gameObject, .5f);
                }
                else
                {
                    anm.SetTrigger("die2");
                    Destroy(gameObject, .7f);
                }
                break;
        }
    }
}
