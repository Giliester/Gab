using UnityEngine;

public class Punto : Entidad
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Jugador>() == Jugador)
        {
            Jugador.Vida+= 20 * GameManager.Juego.ronda/2;
            StartCoroutine(Destruir());
        }
    }
}