using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private Animator animator;
    public GameObject game;
    public GameObject enemyGenerator;

    //Sonidos del jugador
    public AudioClip jumpClip;
    public AudioClip dieClip;
    private AudioSource audioPlayer;
    private float startY;
    public AudioClip pointClip;
    //efecto de polvo del personaje
    public ParticleSystem dust;
   


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //Recupero el audio
        audioPlayer = GetComponent<AudioSource>();
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        bool gamePlaying = game.GetComponent<GameController>().gameState == GameState.Playing;
        bool isGrounded = transform.position.y == startY;
        if (isGrounded && gamePlaying && (Input.GetKeyDown("up") || Input.GetMouseButtonDown(0)))
        {
        UpdateState("PlayerJump");
        //Sonidos
        audioPlayer.clip = jumpClip;
        audioPlayer.Play();
        }
        
    }

    public void UpdateState(string state = null) {
        if (state != null)
        {
            animator.Play(state);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        { 
            //Destroy(gameObject); 
            UpdateState("PlayerDie");
            game.GetComponent<GameController>().gameState = GameState.Ended;
            enemyGenerator.SendMessage("CancelGenerator");
            //Reseteo de dificultad del juego
            game.SendMessage("ResetTimeScale");
            
            //Sonidos
            game.GetComponent<AudioSource>().Stop();
            audioPlayer.clip = dieClip;
            audioPlayer.Play();
        }
        else if (other.gameObject.tag == "NuevoPunto")
        {
            game.SendMessage("IncreasePoints");
            audioPlayer.clip = pointClip;
            audioPlayer.Play();
        }
    }
    //Metodo para indicar que el jugador ha iniciado
    void GameReady()
    {
        game.GetComponent<GameController>().gameState = GameState.Ready;
    }
    //Metodo para el efecto de polvo
    void DustPLay()
    {
        dust.Play();
    }
    void DustStop()
    {
        dust.Stop();
    }
    
}
