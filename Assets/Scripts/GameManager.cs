using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Juego;
    [Header("Colores")]
    public Color FondoNormal;
    public Color FondoCritico;
    public Color ViñetaNormal;
    public Color ViñetaCritico;
    public AudioSource Audio;
    public AudioClip AudioFondo;
    [Header("Enemigos")]
    public List<GameObject> Enemigos;
    [Header("Referencias")]
    public Menu menu;
    public Plataforma plataforma;
    public GameObject JugadorInstancia;
    public Jugador Jugador;
    public bool enJuego;
    public bool gano;
    public float ronda = 1;
    public int rondaMax;
    private int tiempo;

    void Awake()
    {
        if (Juego == null)
            Juego = this;
        else
            Destroy(this.gameObject);
    }
    void Start()
    {
        ObtenerDatos();
        StartCoroutine(menu.MostrarMenu());
    }

    public IEnumerator Jugar()
    {
        yield return StartCoroutine(menu.OcultarMenu());
        Camera.main.orthographic = false;
        PostProcess.Depth.focalLength.value = 300;
        plataforma.PlataformaTexto = "";
        Audio.clip = AudioFondo;
        Audio.Play();
        Camera.main.backgroundColor = FondoNormal;
        PostProcess.Vignette.color.value = ViñetaNormal;
        float t = 0;
        while (t < 3)
        {
            t += Time.deltaTime;
            plataforma.transform.position = new Vector3(0, 0, Mathf.Lerp(-10, 0, t));
            PostProcess.Depth.focalLength.value = Mathf.Lerp(300, 0, t/2);
            Audio.volume = Mathf.Clamp01(t / 2);
            yield return null;
        }
        ronda = 1;
        Camera.main.orthographic = true;
        StartCoroutine(Oleada());
    }
    IEnumerator Oleada()
    {
        foreach (var e in FindObjectsOfType<Entidad>())
        {
            if(!(e is Jugador))
                StartCoroutine(e.Destruir());
        }

        yield return StartCoroutine(plataforma.Cuenta());

        enJuego = true;
        gano = false;

        if(Jugador == null)
            Jugador = Instantiate(JugadorInstancia).GetComponent<Jugador>();

        Jugador.Vida = 100;

        Camera.main.backgroundColor = FondoNormal;
        PostProcess.Vignette.color.value = ViñetaNormal;
        tiempo = (int)(60 * (ronda / 2));
        int EnemigosNum = (int)(20 * (ronda / 2));
        int NumEnemig = (int)(tiempo / EnemigosNum);
        int PuntosNum = (int)EnemigosNum / 5;
        int NumPuntos = (int)(tiempo / PuntosNum);

        while (tiempo > 0 && enJuego)
        {
            int e = 0;
            if (ronda< Enemigos.Count)
                e= (int)Random.Range(0,ronda);
            else
                e = (int)Random.Range(0, Enemigos.Count);

            if (EnemigosNum > 0)
            {
                if (NumEnemig * EnemigosNum == tiempo)
                {
                    Instantiate(Enemigos[e], Pos(20, 20, 5), Quaternion.identity);
                    EnemigosNum--;
                }
            }
            if (PuntosNum > 0)
            {
                if (NumPuntos * PuntosNum == tiempo)
                {
                    Instantiate(Resources.Load("Prefabs/Punto") as GameObject, Pos(7, 7), Quaternion.identity);
                    PuntosNum--;
                }
            }
            plataforma.PlataformaTexto = tiempo.ToString();
            yield return new WaitForSeconds(0.5f);
            tiempo--;
        }
        if (tiempo <= 0)
        {
            Gano();
        }
    }
    private void ObtenerDatos()
    {
        if (PlayerPrefs.HasKey("Ronda"))
            rondaMax = PlayerPrefs.GetInt("Ronda");
        else
        {
            rondaMax = 0;
            PlayerPrefs.SetInt("Ronda", rondaMax);
        }
    }
    private void Gano()
    {
        gano = true;
        ronda++;
        if (ronda > rondaMax)
        {
            rondaMax = (int)ronda;
            PlayerPrefs.SetInt("Ronda", rondaMax);
        }
        StartCoroutine(Oleada());
    }
    public IEnumerator FinPartida()
    {
        yield return StartCoroutine(menu.MostrarMenu());
    }
    public Vector3 Pos(int x, int y, int r = 0)
    {
        Vector2 random = new(Random.Range(-x / 2, x / 2), Random.Range(-y / 2, y / 2));
        if(random.magnitude < r)
            return Pos(x, y, r);
        return new(random.x, random.y, 0);
    }
}