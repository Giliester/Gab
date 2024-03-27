using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Entidad : MonoBehaviour
{
    public GameObject explocionPrefab;
    public float velocidad;
    public bool indestructible;
    protected Jugador Jugador => GameManager.Juego.Jugador;
    protected bool Jugando => GameManager.Juego.enJuego;
    protected bool destrullendo = false;

    float vida = 100;
    public float Vida
    {
        set {
            vida = Mathf.Clamp(value, 0, 100);

            if (vida <= 0 && !destrullendo)
            {
                StartCoroutine(Destruir());
                vida = 0;
            }
        } get { return vida; }
    }
    protected virtual void Mover(Vector3 objetivo, bool impulso, float r = 0, float Velocidad = 0)
    {
        if (Jugando && !destrullendo)
        {
            if (Velocidad == 0)
                Velocidad = velocidad;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector2 direccion = objetivo - transform.position;
            if (direccion.magnitude > r)
            {
                float distancia = Mathf.Min(Velocidad * Time.deltaTime, direccion.magnitude);
                if (impulso)
                    rb.AddForce(Velocidad * Time.deltaTime * direccion.normalized, ForceMode2D.Impulse);
                else
                    transform.position = Vector3.Lerp(transform.position, objetivo, distancia / direccion.magnitude);
            }
        }
    }
    protected virtual void Rotar(Vector3 objetivo, float rotacion = 15)
    {
        if (Jugando && !destrullendo)
        {
            Vector2 direccion = objetivo - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg), rotacion * Time.deltaTime);
        }
    }
    public virtual IEnumerator Destruir()
    {
        destrullendo = true;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<AudioSource>().Play();
        Instantiate(explocionPrefab, transform.position, transform.rotation);
        yield return new WaitWhile(() => GetComponent<AudioSource>().isPlaying);
        Destroy(this.gameObject);
    }
}