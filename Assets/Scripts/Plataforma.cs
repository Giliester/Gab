using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Plataforma : MonoBehaviour
{
    private bool enPlataforma;
    private string jugadorTag = "Jugador";
    public GameObject jugadorExplocion;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(jugadorTag))
        {
            enPlataforma = false;
            StartCoroutine(Salio());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(jugadorTag))
        {
            enPlataforma = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(jugadorTag))
        {
            enPlataforma = true;
        }
    }
    public IEnumerator Salio()
    {
        float t = 3;
        while (t >= 0 && !enPlataforma)
        {
            Camera.main.backgroundColor = GameManager.Game.FondoCritico;
            PostProcess.Vignette.color.value = GameManager.Game.ViñetaCritico;
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(0.5f);
            Camera.main.backgroundColor = GameManager.Game.FondoNormal;
            PostProcess.Vignette.color.value = GameManager.Game.ViñetaNormal;
            yield return new WaitForSeconds(0.5f);
            t -=0.75f;
            if (enPlataforma)
                break;
        }
        if (t <= 0 && Jugador.JugadorIns != null)
        {
            GameManager.Game.StartCoroutine(GameManager.Game.FinPartida());
            Camera.main.backgroundColor = GameManager.Game.FondoCritico;
            PostProcess.Vignette.color.value = GameManager.Game.ViñetaCritico;
            Jugador.JugadorIns.GetComponent<Collider2D>().enabled = false;
            Jugador.JugadorIns.GetComponent<SpriteRenderer>().enabled = false;
            Jugador.JugadorIns.GetComponent<AudioSource>().Play();
            GameManager.Game.enJuego = false;
            Instantiate(jugadorExplocion, Jugador.JugadorIns.gameObject.transform.position, Jugador.JugadorIns.gameObject.transform.rotation);
            Jugador.Desenlazar();
            yield return new WaitWhile(() => Jugador.JugadorIns.GetComponent<AudioSource>().isPlaying);
            Destroy(Jugador.JugadorIns.gameObject);
        }
        yield return null;
    }
}
