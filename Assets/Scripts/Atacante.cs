using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Atacante : MonoBehaviour
{
    public float speed;
    public float Distance;
    public GameObject enemigoExplocion;
    private string proyectilTag = "Proyectil";
    public GameObject proyectil;
    public bool Destrullendo = false;
    void Start()
    {
        StartCoroutine(Disparar());
    }
    void Update()
    {
        if(GameManager.Game.enJuego && !Destrullendo)
        {
            Vector2 direccion = (Jugador.JugadorIns.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg), 15 * Time.deltaTime);


            float distance = Vector2.Distance(transform.position, Jugador.JugadorIns.transform.position);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (distance > Distance)
            {
                Vector2 targetPosition = Vector2.Lerp(rb.position, rb.position + direccion * speed * Time.deltaTime, 0.5f);
                rb.MovePosition(targetPosition);
            }
        }
    }
    IEnumerator Disparar()
    {
        while (GameManager.Game.enJuego)
        {
            if (!Destrullendo)
            {
                yield return new WaitForSeconds(3);
                Instantiate(proyectil, transform.position + Vector3.forward, transform.rotation).GetComponent<Proyectil>().contrario = true;
            }
            yield return null;
        }
        yield return null;
    }
    public IEnumerator Destruir()
    {
        Destrullendo = true;
        if (this.gameObject != null)
        {
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
