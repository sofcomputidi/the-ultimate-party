using UnityEngine;
using System.Collections;

public class EventoEspecial : MonoBehaviour
{
    public GameObject textoEspecial;
    public GameObject textoBonusEspecial;

    public float primerEvento = 22f;
    public float intervaloEvento = 16f;

    private float tiempoJuego = 0f;
    private float proximoEvento;

    private bool eventoActivo = false;

    void Awake()
    {
        if (textoEspecial != null)
        {
            textoEspecial.SetActive(false);
        }

        if (textoBonusEspecial != null)
        {
            textoBonusEspecial.SetActive(false);
        }
    }

    void Start()
    {
        proximoEvento = primerEvento;

        if (textoEspecial != null)
        {
            textoEspecial.SetActive(false);
        }

        if (textoBonusEspecial != null)
        {
            textoBonusEspecial.SetActive(false);
        }
    }

    void Update()
    {
        tiempoJuego += Time.deltaTime;

        if (!eventoActivo && tiempoJuego >= proximoEvento)
        {
            StartCoroutine(ActivarEventoEspecial());

            proximoEvento += intervaloEvento;
        }
    }

    IEnumerator ActivarEventoEspecial()
    {
        eventoActivo = true;

        // =========================================
        // 1. APARECE RAISE YOUR HAND
        // =========================================

        textoEspecial.SetActive(true);
        textoBonusEspecial.SetActive(false);

        RectTransform rectEspecial =
            textoEspecial.GetComponent<RectTransform>();

        rectEspecial.localScale = Vector3.one;

        Debug.Log("RAISE YOUR HAND");

        // =========================================
        // 2. PALPITACIÓN SUAVE
        // =========================================

        float tiempoPalpitando = 0f;
        float duracionPalpitando = 2.5f;

        while (tiempoPalpitando < duracionPalpitando)
        {
            tiempoPalpitando += Time.deltaTime;

            float pulso =
                1f +
                Mathf.Sin(
                    tiempoPalpitando * 5f
                ) * 0.05f;

            rectEspecial.localScale =
                Vector3.one * pulso;

            yield return null;
        }

        rectEspecial.localScale = Vector3.one;

        // =========================================
        // 3. DESAPARECE RAISE YOUR HAND
        // =========================================

        textoEspecial.SetActive(false);

        yield return new WaitForSeconds(0.25f);

        // =========================================
        // 4. APARECE +15 BONUS
        // =========================================

        textoBonusEspecial.SetActive(true);

        RectTransform rectBonus =
            textoBonusEspecial.GetComponent<RectTransform>();

        rectBonus.localScale =
            Vector3.one * 0.85f;

        // =========================================
        // 5. SUMA LOS 15 PUNTOS
        // =========================================

        if (GameManager.instance != null)
        {
            GameManager.instance.BonusEspecial();

            Debug.Log(
                "BONUS ESPECIAL APLICADO +15"
            );
        }

        // =========================================
        // 6. CRECIMIENTO SUAVE
        // =========================================

        float tiempoAnimacion = 0f;
        float duracionAnimacion = 0.3f;

        while (tiempoAnimacion < duracionAnimacion)
        {
            tiempoAnimacion += Time.deltaTime;

            float escala =
                Mathf.Lerp(
                    0.85f,
                    1.15f,
                    tiempoAnimacion /
                    duracionAnimacion
                );

            rectBonus.localScale =
                Vector3.one * escala;

            yield return null;
        }

        // =========================================
        // 7. VUELVE SUAVEMENTE A TAMAÑO NORMAL
        // =========================================

        tiempoAnimacion = 0f;
        duracionAnimacion = 0.2f;

        while (tiempoAnimacion < duracionAnimacion)
        {
            tiempoAnimacion += Time.deltaTime;

            float escala =
                Mathf.Lerp(
                    1.15f,
                    1f,
                    tiempoAnimacion /
                    duracionAnimacion
                );

            rectBonus.localScale =
                Vector3.one * escala;

            yield return null;
        }

        // =========================================
        // 8. SE MANTIENE VISIBLE
        // =========================================

        yield return new WaitForSeconds(1.1f);

        // =========================================
        // 9. DESAPARECE
        // =========================================

        textoBonusEspecial.SetActive(false);

        rectBonus.localScale = Vector3.one;

        eventoActivo = false;
    }
}