using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    float t = 0;
    public bool propio;
    public bool contrario;
    public GameObject enemigoExplosion;
    public string enemigoTag = "Enemigo";
    public string jugadorTag = "Jugador";
    void Update()
    {
        if(GameManager.Game.enJuego)
        {
            transform.Translate(Vector3.right * 5 * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            t += Time.deltaTime;
            if (t > 5)
            {
                Destroy(this.gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(enemigoTag) || collision.gameObject.CompareTag(jugadorTag))
        {
            StartCoroutine(Destruir(collision));
        }
    }
    public IEnumerator Destruir(Collider2D collision)
    {
        Atacante a;
        Enemigo e;
        Jugador j;
        if (this.gameObject != null && collision != null)
        {
            if (collision.TryGetComponent(out a) && !contrario)
            {
                yield return StartCoroutine(a.Destruir());
                Destroy(this.gameObject);
            }
            else
            {
                if (collision.TryGetComponent(out e))
                {
                    yield return StartCoroutine(e.Destruir());
                    Destroy(this.gameObject);
                }
                else
                {
                    if (collision.TryGetComponent(out j) && !propio)
                    {
                        j.vida -= 20;
                        Destroy(this.gameObject);
                    }
                }
            }
        }
        yield return null;
    }
}
