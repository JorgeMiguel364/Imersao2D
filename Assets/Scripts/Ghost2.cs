using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost2 : MonoBehaviour
{
    public float spd;

    private Animator anm;
    private Rigidbody2D rig;

    // Posição do jogador
    private GameObject player;

    // Pontuação adicional
    private int score = 1;

    void Start()
    {
        anm = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();

        // Captura a posição do jogador
        player = GameObject.FindWithTag("Player");

        GameController.instance.enemiesQt += 1;
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        // Se movimenta em direção ao player
        if (player != null)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, spd * Time.deltaTime);
        else
            transform.position = Vector2.MoveTowards(transform.position, Vector2.zero, spd * Time.deltaTime);

        // Controla a orientação do sprite
        if (player != null)
        {
            if (player.transform.position.x > transform.position.x)
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            else
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else
        {
            if (transform.position.x > 0)
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            else
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
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
