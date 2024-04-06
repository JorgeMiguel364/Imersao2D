using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoin : MonoBehaviour
{
    private SpriteRenderer sr;
    private CircleCollider2D circle;

    private float lifeTime = 0;

    public GameObject collected;

    // Pontuação adicional
    private int score = 1;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        circle = GetComponent<CircleCollider2D>();

        GameController.instance.coinsQt += 1;
    }

    void Update()
    {
        lifeTime += Time.deltaTime;

        if (lifeTime >= 30)
        {
            Destroy(gameObject);

            GameController.instance.coinsQt -= 1;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            sr.enabled = false;
            circle.enabled = false;
            collected.SetActive(true);

            if (GameController.instance.coinScore >= GameController.instance.maxCoinScore)
            {
                GameController.instance.coinScore = GameController.instance.maxCoinScore;
            }
            else
            {
                GameController.instance.coinScore += score;
            }

            GameController.instance.coinsQt -= 1;
            GameController.instance.updateScoreText();

            Destroy(gameObject, .5f);
        }
    }
}
