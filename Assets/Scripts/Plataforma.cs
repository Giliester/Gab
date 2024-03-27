using System.Collections;
using TMPro;
using UnityEngine;

public class Plataforma : Entidad
{
    public TMP_Text texto;
    public string PlataformaTexto { set { texto.text = value; } }
    public AudioSource Tick;
    public TMP_Text rondaTexto;
    string RondaMaxTexto { set { rondaTexto.text = value; } }
    bool enPlataforma;
    void Update()
    {
        RondaMaxTexto = GameManager.Juego.rondaMax.ToString();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Entidad e) && e == Jugador)
        {
            enPlataforma = false;
            StartCoroutine(Salio());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Entidad e) && e == Jugador)
            enPlataforma = true;
    }
    public IEnumerator Cuenta()
    {
        PlataformaTexto = "Ronda: " + GameManager.Juego.ronda;
        yield return new WaitForSeconds(3);
        PlataformaTexto = "3";
        Tick.Play();
        yield return new WaitForSeconds(1);
        PlataformaTexto = "2";
        Tick.Play();
        yield return new WaitForSeconds(1);
        PlataformaTexto = "1";
        Tick.Play();
        yield return new WaitForSeconds(1);
    }
    public IEnumerator Salio()
    {
        float t = 3;
        while (t >= 0 && !enPlataforma)
        {
            Camera.main.backgroundColor = GameManager.Juego.FondoCritico;
            PostProcess.Vignette.color.value = GameManager.Juego.ViñetaCritico;
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(0.5f);
            Camera.main.backgroundColor = GameManager.Juego.FondoNormal;
            PostProcess.Vignette.color.value = GameManager.Juego.ViñetaNormal;
            yield return new WaitForSeconds(0.5f);
            t -=0.75f;
            if (enPlataforma)
                break;
        }
        if (t <= 0 && GameManager.Juego.Jugador != null)
            GameManager.Juego.Jugador.Vida = 0;
        yield return null;
    }
    public override IEnumerator Destruir()
    {
        yield return null;
    }
}