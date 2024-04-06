using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject restartTime;
    public GameObject gameOver;
    public GameObject finish;

    // Pontuação total de moedas
    public int coinScore;
    public int maxCoinScore;

    // Pontuação total de inimigos
    public int ghostScore;
    public int maxGhostScore;

    // Quantidade de vidas
    public int lifeQt;

    // Quantidade total de inimigos no cenário
    public int enemiesQt = 0;

    // Máximo de inimigos no cenário
    public int maxEnemies = 5;

    // Quantidade total de moedas no cenário
    public int coinsQt = 0;

    // Máximo de moedas no cenário
    public int maxCoins = 5;

    public TextMeshProUGUI coinScoreText;
    public TextMeshProUGUI ghostScoreText;
    public TextMeshProUGUI lifeQtScoreText;
    public TextMeshProUGUI secondsTextGameOver;
    public TextMeshProUGUI secondsTextReset;
    public TextMeshProUGUI secondsTextFinish;

    public static GameController instance;

    void Start()
    {
        instance = this;

        registerScore();
        updateScoreText();
    }

    void Update()
    {
        escButton();
    }

    // Coroutine de gerenciamento de segundos até reiniciar o cenário
    IEnumerator secondsReset()
    {
        yield return new WaitForSeconds(1);

        for (int i = 5; i >= 0; i--)
        {
            secondsTextReset.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Ao reiniciar o cenário, carrega os valores do registro e armazena nas variáveis locais
        coinScore = PlayerPrefs.GetInt("coins");
        ghostScore = PlayerPrefs.GetInt("ghosts");
        lifeQt = PlayerPrefs.GetInt("lifes");
    }

    // Coroutine de gerenciamento de segundos de Game Over
    IEnumerator secondsGameOver()
    {
        yield return new WaitForSeconds(2);

        for (int i = 5; i >= 0; i--)
        {
            secondsTextGameOver.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        // Zera a pontuação do registro e retorna ao menu principal
        resetScore();
        SceneManager.LoadScene(0);
    }

    // Coroutine de gerenciamento de segundos após a conclusão
    IEnumerator secondsFinish()
    {
        yield return new WaitForSeconds(2);

        for (int i = 5; i >= 0; i--)
        {
            secondsTextFinish.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        Debug.Log("Next Level!");
    }

    // Se apertar ESC, retorna ao menu principal
    public void escButton()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            resetScore();
            SceneManager.LoadScene(0);
        }
    }

    // Se der game over, exclui os seguintes itens
    public void removeAllObjects()
    {
        var DestroyObject1 = GameObject.FindGameObjectsWithTag("DestroyObject1");
        var DestroyObject2 = GameObject.FindGameObjectsWithTag("DestroyObject2");
        var DestroyObject3 = GameObject.FindGameObjectsWithTag("DestroyObject3");
        var DestroyObject4 = GameObject.FindGameObjectsWithTag("DestroyObject4");

        foreach (var i in DestroyObject1)
        {
            Destroy(i);
        }

        foreach (var i in DestroyObject2)
        {
            Destroy(i);
        }

        foreach (var i in DestroyObject3)
        {
            Destroy(i);
        }

        foreach (var i in DestroyObject4)
        {
            Destroy(i);
        }
    }

    // Atualiza a pontuação geral
    public void updateScoreText()
    {
        coinScoreText.text = coinScore.ToString() +" / "+ maxCoinScore.ToString();
        ghostScoreText.text = ghostScore.ToString() +" / "+ maxGhostScore.ToString();
        lifeQtScoreText.text = lifeQt.ToString() +" X ";
    }

    // Zera a pontuação geral
    public void updateScoreTextGameOver()
    {
        coinScoreText.text = " --- ";
        ghostScoreText.text = " --- ";
        lifeQtScoreText.text = " --- ";
    }

    // Armazena a pontuação nos registros para uso futuro
    public void registerScore()
    {
        // Ao iniciar, captura os valores do registro e armazena nas variáveis locais
        coinScore += PlayerPrefs.GetInt("coins");
        ghostScore += PlayerPrefs.GetInt("ghosts");

        // Se não existir o registro "lifes" atribui-lhe o valor 5, se já existir atribui o valor do registro na variável local lifeQt
        if (PlayerPrefs.HasKey("lifes"))
        {
            lifeQt = PlayerPrefs.GetInt("lifes");
        }
        else
        {
            lifeQt = 5;
        }
    }

    // Zera as pontuações de coins, ghosts e lifes do registro
    public void resetScore()
    {
        PlayerPrefs.DeleteKey("coins");
        PlayerPrefs.DeleteKey("ghosts");
        PlayerPrefs.DeleteKey("lifes");
    }

    // Reinicia o cenário
    public void restartLevel()
    {
        restartTime.SetActive(true);
        StartCoroutine(secondsReset());

        secondsReset();
    }

    // Apresenta a tela de game over
    public void gameOverScreen()
    {
        gameOver.SetActive(true);

        StartCoroutine(secondsGameOver());
        secondsGameOver();
    }

    // Apresenta a tela de conclusão
    public void finishScreen()
    {
        finish.SetActive(true);

        StartCoroutine(secondsFinish());
        secondsFinish();
    }
}
