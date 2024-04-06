using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public float JumpForce;
    public float DoubleJumpForce;

    private bool isJumping = true;
    private bool doubleJump = false;
    private bool isAlive = true;
    private bool heHasFallen;

    private float IdleSeconds = 0;
    private float delayShot = 0;

    // Valores que controlam os limites do cenário
    private const float limitX = 8.6f;
    private const float limitYDown = 5.6f;
    private const float limitYUp = 4.5f;

    private Rigidbody2D rig;
    private Animator anim;
    private CapsuleCollider2D coll;

    public GameObject projectile;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();

        // Animação ao nascer
        anim.SetBool("Jumping", true);
    }

    void Update()
    {
        if (isAlive)
        {
            Move();
            Jump();
            Idle2();
            Shot();
            RestrictMovement();
            ScoreIsMatch();
        }
    }

    // Movimentação
    void Move() 
    {
        // Gerencia as teclas correspondentes --------------------------------------------------------------------
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
            transform.position += movement * Time.deltaTime * (Speed * 2);
        }
        else
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
            transform.position += movement * Time.deltaTime * Speed;
        }

        // Gerencia animações -------------------------------------------------------------------------------------
        if (Input.GetAxis("Horizontal") > 0f)
        {
            anim.SetBool("Moving", true);
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }

        if (Input.GetAxis("Horizontal") < 0f)
        {
            anim.SetBool("Moving", true);
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        if (Input.GetAxis("Horizontal") == 0f)
        {
            anim.SetBool("Moving", false);
        }
    }

    // Gerencia restrição de movimentos nos limites do cenário
    void RestrictMovement()
    {
        float xRestrict;

        // Impede o movimento do jogador nos limites horizontais
        if (transform.position.x <= -limitX || transform.position.x >= limitX)
        {
            xRestrict = Mathf.Clamp(transform.position.x, -limitX, limitX);
            transform.position = new Vector3(xRestrict, transform.position.y, transform.position.z);
        }

        // Se o jogador cair no buraco, renasce na parte superior
        if (transform.position.y <= -limitYDown)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Abs(-limitYDown), transform.position.z);
            rig.velocity = Vector3.zero;

            // (heHasFallen) Ele caiu no buraco? Sim
            heHasFallen = true;
        }

        // Se o jogador não CAIU no buraco e está tentando ultrapassar o limite superior da tela, impeça-o
        if (transform.position.y >= limitYUp && heHasFallen == false)
        {
            transform.position = new Vector3(transform.position.x, limitYUp, transform.position.z);
        }

        // Se o jogador caiu no buraco e já renasceu na parte superior, altere o valor de heHasFallen para false
        if (transform.position.y <= limitYUp)
        {
            heHasFallen = false;
        }
    }

    // Gerencia animação de Idle 2
    void Idle2()
    {
        IdleSeconds += 1f * Time.deltaTime;

        if (IdleSeconds > 30f)
        {
            anim.SetBool("Inactive", true);
        }
        
        if (Input.anyKey)
        {
            IdleSeconds = 0;
            anim.SetBool("Inactive", false);
        }
    }

    // Pulos
    void Jump() 
    {
        if (Input.GetButtonDown("Jump")) 
        {
            anim.SetBool("Jumping", true);

            if (!isJumping)
            {
                rig.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
                doubleJump = true;
            }
            else
            {
                if (doubleJump)
                {
                    rig.AddForce(new Vector2(0f, DoubleJumpForce), ForceMode2D.Impulse);
                    doubleJump = false;
                }
            }
        }
    }

    // Atirar projéteis
    void Shot()
    {
        if (delayShot > 0f)
            delayShot -= Time.deltaTime;

        if (Input.GetKey(KeyCode.E) && delayShot <= 0)
        {
            Instantiate(projectile, transform.position, transform.rotation);
            delayShot = .5f;
        }
    }

    // Teste de pontuação
    void ScoreIsMatch()
    {
        if (GameController.instance.coinScore == GameController.instance.maxCoinScore && GameController.instance.ghostScore == GameController.instance.maxGhostScore)
        {
            Destroy(gameObject);

            GameController.instance.removeAllObjects();
            GameController.instance.resetScore();
            GameController.instance.finishScreen();
        }
    }

    void OnCollisionEnter2D(Collision2D coll2D)
    {
        switch (coll2D.gameObject.layer)
        {
            case 6:
                isJumping = false;
                anim.SetBool("Jumping", false);
                break;
            case 7:
                isAlive = false;
                coll.enabled = false;
                anim.SetBool("Inactive", true);
                rig.gravityScale = 6f;
                rig.AddForce(transform.up * 15f, ForceMode2D.Impulse);

                GameController.instance.lifeQt -= 1;
                GameController.instance.updateScoreText();

                // Se a quantidade de vidas for maior que 0, reinicia o nível. Se a quantidade de vidas for igual a 0, apresenta a tela de game over
                if (GameController.instance.lifeQt != 0)
                {
                    GameController.instance.restartLevel();

                    // Ao morrer, armazena as pontuações de coins, ghosts e lifes para uso futuro
                    PlayerPrefs.SetInt("coins", GameController.instance.coinScore);
                    PlayerPrefs.SetInt("ghosts", GameController.instance.ghostScore);
                    PlayerPrefs.SetInt("lifes", GameController.instance.lifeQt);
                }
                else
                {
                    GameController.instance.removeAllObjects();
                    GameController.instance.updateScoreTextGameOver();
                    GameController.instance.gameOverScreen();
                    GameController.instance.resetScore();
                }

                Destroy(gameObject, 1f);
                break;
            case 8:
                isAlive = false;
                coll.enabled = false;
                anim.SetBool("Inactive", true);
                rig.gravityScale = 6f;
                rig.AddForce(transform.up * 15f, ForceMode2D.Impulse);

                GameController.instance.lifeQt -= 1;
                GameController.instance.updateScoreText();

                // Se a quantidade de vidas for maior que 0, reinicia o nível. Se a quantidade de vidas for igual a 0, apresenta a tela de game over
                if (GameController.instance.lifeQt != 0)
                {
                    GameController.instance.restartLevel();

                    // Ao morrer, armazena as pontuações de coins, ghosts e lifes para uso futuro
                    PlayerPrefs.SetInt("coins", GameController.instance.coinScore);
                    PlayerPrefs.SetInt("ghosts", GameController.instance.ghostScore);
                    PlayerPrefs.SetInt("lifes", GameController.instance.lifeQt);
                }
                else
                {
                    GameController.instance.removeAllObjects();
                    GameController.instance.updateScoreTextGameOver();
                    GameController.instance.gameOverScreen();
                    GameController.instance.resetScore();
                }

                Destroy(gameObject, 1f);
                break;
        }
    }

    void OnCollisionExit2D(Collision2D coll2D) 
    {
        if (coll2D.gameObject.layer == 6)
        {
            isJumping = true;
        }
    }
}
