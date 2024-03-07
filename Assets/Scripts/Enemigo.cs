using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    private Transform objectivo;
    private string proyectilTag = "Proyectil";
    public GameObject enemigoExplocion;
    public bool Destrullendo = false;
    void Update()
    {
        if (GameManager.Game.enJuego && !Destrullendo)
        {
            objectivo = Jugador.JugadorIns.gameObject.transform;
            Vector3 direccion = objectivo.position - transform.position;
            if (direccion.magnitude > 0)
            {
                float distancia = Mathf.Min(7 * Time.deltaTime, direccion.magnitude);
                transform.position = Vector3.Lerp(transform.position, objectivo.position, distancia / direccion.magnitude);
            }
        }
    }
    public IEnumerator Destruir()
    {
        if (this.gameObject != null)
        {
            Destrullendo = true;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<AudioSource>().Play();
            Instantiate(enemigoExplocion, transform.position, transform.rotation);
            yield return new WaitWhile(() => GetComponent<AudioSource>().isPlaying);
            if (this.gameObject != null)
                Destroy(this.gameObject);
        }
        yield return null;
    }
}
