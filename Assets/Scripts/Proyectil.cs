using System.Collections;
using UnityEngine;

public class Proyectil : Entidad
{
    public GameObject origen;
    float t = 0;
    void Update()
    {
        if(Jugando)
        {
            Mover(transform.position + velocidad * Time.deltaTime * transform.right, false);
            t += Time.deltaTime;
            if (t > 5)
                Destroy(gameObject);
        }
    }
    public override IEnumerator Destruir()
    {
        Destroy(gameObject);
        yield return null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Entidad e) && origen != e.gameObject && !e.indestructible)
        {
            if (e == Jugador)
                e.Vida -= 20;
            else
                e.Vida = 0;
            Destroy(gameObject);
        }
    }
}