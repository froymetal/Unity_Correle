using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState { Idle, Playing, Ended, Ready };

public class GameController : MonoBehaviour
{
    [Range (0f,0.20f)]
    public float parallaxSpeed = 0.02f;
    public RawImage background;
    public RawImage platform;
    public GameObject uiIdle;
    public GameObject uiScore;

    public GameState gameState = GameState.Idle;

    public GameObject player;
    //variables del enemigo
    public GameObject enemyGenerator;
    //variable de la musica
    private AudioSource musicPlayer;
    //Dificultad del juego
    public float scaleTime = 6f;
    public float scaleInc = .25f;
    //Puntos del jugador
    private int points = 0;
    public Text pointsText;
    public Text recordText;
    
    // Start is called before the first frame update
    void Start()
    {
        //musicPlayer.GetComponent<AudioSource>();
        //Muestra el mejor puntaje
        recordText.text = "Mejor Puntuación : " + GetMaxScore().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Empieza el juego 
        if (gameState == GameState.Idle && (Input.GetKeyDown("up") || Input.GetMouseButtonDown(0)))
        {
            gameState = GameState.Playing;
            uiIdle.SetActive(false);
            uiScore.SetActive(true);
            player.SendMessage("UpdateState","PlayerRun");
            //Inicia enemigos
            enemyGenerator.SendMessage("StartGenerator");
            //Inicia Musica
            //musicPlayer.Play();
            //PAra aumentar dificultad del juego
            InvokeRepeating("GameTimeScale", scaleTime, scaleTime);
        }
        //Juego en marcha
        else if (gameState == GameState.Playing)
        {
            Parallax();
        }
        //Juego finalizado
        else if (gameState == GameState.Ready)
        {
            if (Input.GetKeyDown("up") || Input.GetMouseButtonDown(0))
            {
                RestartGame();
            }
        }

    }

    void Parallax()
    {
        float finalSpeed = parallaxSpeed = Time.deltaTime;
        background.uvRect = new Rect(background.uvRect.x + finalSpeed, 0f, 1f, 1f);
        platform.uvRect = new Rect(platform.uvRect.x + finalSpeed * 4, 0f, 1f, 1f);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    //Funcion para incrementar dificultad del juego
    void GameTimeScale()
    {
        Time.timeScale += scaleInc;
        //Debug.Log();
    }

    public void ResetTimeScale()
    {
        CancelInvoke("GameTimeScale");
        Time.timeScale = 1f;
    }
    //Método para incrementarpuntos del jugador
    void IncreasePoints()
    {
        points++;
        pointsText.text = points.ToString();
        if (points > GetMaxScore())
        {
            recordText.text = "Mejor Puntuación : " + points.ToString();
            SaveScore(points);
        }
    }

    //Métodos para almacenar los puntajes
    public int GetMaxScore()
    {
        return PlayerPrefs.GetInt("Max Points", 0);
    }

    public void SaveScore(int currentPoints)
    {
        PlayerPrefs.SetInt("Max Points", currentPoints);
    }

    
}
