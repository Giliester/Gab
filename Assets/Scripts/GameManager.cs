using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Game;
    [Header("Colores")]
    public List<Color> Colores = new List<Color>();
    public Color FondoNormal;
    public Color FondoCritico;
    public Color ViñetaNormal;
    public Color ViñetaCritico;
    public AudioSource Audio;
    public AudioSource Tick;
    public AudioClip AudioFondo;
    public AudioClip AudioMenu;
    public AudioClip AudioTuto;
    [Header("UI")]
    public Image Panel;
    public TMP_Text RondaMax;
    public GameObject Menu;
    public GameObject Info;
    public bool interactuable;
    [Header("Enemigos")]
    public List<GameObject> Enemigos;
    [Header("Referencias")]
    public GameObject Plataforma;
    public TMP_Text PlataformaTexto;
    public GameObject JugadorInstancia;
    public bool enJuego;
    public bool gano;
    public float ronda = 1;
    public int rondaMax;
    private int tiempo;

    void Awake()
    {
        if (Game == null)
            Game = this;
        else
            Destroy(this.gameObject);
    }
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("Ronda"))
        {
            rondaMax = PlayerPrefs.GetInt("Ronda");
        }
        else
        {
            rondaMax = 0;
            PlayerPrefs.SetInt("Ronda",rondaMax);
        }
        StartCoroutine(MostrarMenu());
    }

    void Update()
    {
        RondaMax.text = rondaMax.ToString();
    }

    IEnumerator MostrarMenu()
    {
        UIMenu();
        enJuego = false;
        Audio.clip = AudioMenu;
        Audio.Play();
        Panel.gameObject.SetActive(true);
        PostProcess.Activate();
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            Panel.transform.GetComponentInParent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, t / 1);
            PostProcess.Depth.focalLength.value = Mathf.Lerp(0, 300, t/1);
            Audio.volume = Mathf.Clamp01(t/1);
            yield return null;
        }
        interactuable = true;
    }
    IEnumerator OcultarMenu()
    {
        interactuable = false;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            Panel.transform.GetComponentInParent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, t / 1);
            Audio.volume = 1 - Mathf.Clamp01(t / 1);
            yield return null;
        }
        Menu.SetActive(false);
        Info.SetActive(false);
        yield return null;
    }

    public void BotonJugar()
    {
        if(interactuable)
        StartCoroutine(Jugar());
    }

    IEnumerator Jugar()
    {
        yield return StartCoroutine(OcultarMenu());
        Camera.main.orthographic = false;
        PostProcess.Depth.focalLength.value = 300;
        PlataformaTexto.text = "";
        Audio.clip = AudioFondo;
        Audio.Play();
        Camera.main.backgroundColor = FondoNormal;
        PostProcess.Vignette.color.value = ViñetaNormal;
        float t = 0;
        while (t < 3)
        {
            t += Time.deltaTime;
            Plataforma.transform.position = new Vector3(0, 0, Mathf.Lerp(-10, 0, t));
            PostProcess.Depth.focalLength.value = Mathf.Lerp(300, 0, t/2);
            Audio.volume = Mathf.Clamp01(t / 2);
            yield return null;

        }
        ronda = 1;
        Camera.main.orthographic = true;
        StartCoroutine(Oleada());
    }
    public IEnumerator FinPartida()
    {
        yield return StartCoroutine(MostrarMenu());
        yield return null;
    }
    IEnumerator Oleada()
    {
        foreach (var a in GameObject.FindObjectsOfType<Atacante>())
        {
            StartCoroutine(a.GetComponent<Atacante>().Destruir());
        }
        foreach (var e in GameObject.FindObjectsOfType<Enemigo>())
        {
            StartCoroutine(e.GetComponent<Enemigo>().Destruir());
        }
        foreach (var p in GameObject.FindObjectsOfType<Proyectil>())
        {
            Destroy(p.gameObject);
        }
        foreach (var p in GameObject.FindObjectsOfType<Punto>())
        {
            Destroy(p.gameObject);
        }
        PlataformaTexto.text = "Ronda: " + ronda;
        yield return new WaitForSeconds(3);
        PlataformaTexto.text = "3";
        Tick.Play();
        yield return new WaitForSeconds(1);
        PlataformaTexto.text = "2";
        Tick.Play();
        yield return new WaitForSeconds(1);
        PlataformaTexto.text = "1";
        Tick.Play();
        yield return new WaitForSeconds(1);
        enJuego = true;
        gano = false;
        Instantiate(JugadorInstancia);
        Jugador.JugadorIns.vida = 100;
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
            PlataformaTexto.text = tiempo.ToString();
            yield return new WaitForSeconds(0.5f);
            tiempo--;
            if(Jugador.JugadorIns.vida <= 0)
            {
                StartCoroutine(FinPartida());
                Camera.main.backgroundColor = FondoCritico;
                PostProcess.Vignette.color.value = ViñetaCritico;
                Jugador.JugadorIns.GetComponent<Collider2D>().enabled = false;
                Jugador.JugadorIns.GetComponent<SpriteRenderer>().enabled = false;
                Jugador.JugadorIns.GetComponent<AudioSource>().Play();
                enJuego = false;
                Instantiate(Plataforma.GetComponent<Plataforma>().jugadorExplocion, Jugador.JugadorIns.gameObject.transform.position, Jugador.JugadorIns.gameObject.transform.rotation);
                Jugador.Desenlazar();
                yield return new WaitWhile(() => Jugador.JugadorIns.GetComponent<AudioSource>().isPlaying);
                Destroy(Jugador.JugadorIns.gameObject);
            }
        }
        if (tiempo <= 0)
        {
            gano = true;
            ronda++;
            if(ronda > rondaMax)
            {
                rondaMax = (int)ronda;
                PlayerPrefs.SetInt("Ronda", rondaMax);
            }
            StartCoroutine(Oleada());
        }
        yield return null;
    }
    public void Salir()
    {
        if (interactuable)
        {
            StopAllCoroutines();
            Application.Quit();
        }
    }
    public void UIMenu()
    {
        Menu.SetActive(true);
        Info.SetActive(false);
    }
    public void UIInstrucciones()
    {
        Menu.SetActive(false);
        Info.SetActive(true);
    }
    public Vector3 Pos(int x, int y, int r)
    {
        Vector2 random = new Vector2(Random.Range(-x / 2, x / 2), Random.Range(-y / 2, y / 2));
        if(random.magnitude < r)
            return Pos(x, y, r);
        return new Vector3(random.x, random.y, 0);
    }
    public Vector3 Pos(int x, int y)
    {
        Vector2 random = new Vector2(Random.Range(-x / 2, x / 2), Random.Range(-y / 2, y / 2));
        return new Vector3(random.x, random.y, 0);
    }
}
