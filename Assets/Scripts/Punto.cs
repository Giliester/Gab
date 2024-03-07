using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punto : MonoBehaviour
{
    private string jugadorTag = "Jugador";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(jugadorTag))
        {
            collision.gameObject.GetComponent<Jugador>().vida+= 20 * GameManager.Game.ronda/2;
            StartCoroutine(Destruir());
        }
    }
    IEnumerator Destruir()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<AudioSource>().Play();
        yield return new WaitWhile(() => GetComponent<AudioSource>().isPlaying);
        Destroy(this.gameObject);
    }
}
