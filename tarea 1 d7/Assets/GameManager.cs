using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Slider barraPublico;

    public TextMeshProUGUI textoFinal;
    public TextMeshProUGUI textoPuntos;
    public TextMeshProUGUI textoFeedback;

    public GameObject fondoFinal;

    public int progreso = 0;

    private float tiempoJuego = 0f;

    private bool juegoTerminado = false;
    private bool yaHuboProgreso = false;

    private Coroutine animacionPuntos;
    private Coroutine animacionFeedback;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;

        progreso = 0;

        barraPublico.minValue = 0;
        barraPublico.maxValue = 100;
        barraPublico.value = progreso;

        textoFinal.gameObject.SetActive(false);
        textoPuntos.gameObject.SetActive(false);
        textoFeedback.gameObject.SetActive(false);

        if (fondoFinal != null)
        {
            fondoFinal.SetActive(false);
        }
    }

    void Update()
    {
        if (!juegoTerminado)
        {
            tiempoJuego += Time.deltaTime;
        }
    }

    public int ObtenerFase()
    {
        if (tiempoJuego < 15f)
        {
            return 1;
        }

        if (tiempoJuego < 30f)
        {
            return 2;
        }

        return 3;
    }

    // -------------------------------------------------------
    // ACIERTO NORMAL
    // -------------------------------------------------------

    public void AciertoNormal()
    {
        if (juegoTerminado)
            return;

        int fase = ObtenerFase();
        int puntosGanados;

        if (fase == 1)
        {
            puntosGanados = 3;
        }
        else if (fase == 2)
        {
            puntosGanados = 4;
        }
        else
        {
            puntosGanados = 6;
        }

        progreso += puntosGanados;

        yaHuboProgreso = true;

        MostrarPuntos("+" + puntosGanados);

        MostrarFeedbackCorrecto();

        Debug.Log(
            "ACIERTO | Fase " + fase +
            " | +" + puntosGanados +
            " | Progreso: " + progreso
        );

        ActualizarBarra();
    }

    // -------------------------------------------------------
    // NOTA PERDIDA
    // -------------------------------------------------------

    public void NotaPerdida()
    {
        if (juegoTerminado)
            return;

        int fase = ObtenerFase();
        int puntosPerdidos;

        if (fase == 1)
        {
            puntosPerdidos = 3;
        }
        else if (fase == 2)
        {
            puntosPerdidos = 4;
        }
        else
        {
            puntosPerdidos = 6;
        }

        progreso -= puntosPerdidos;

        MostrarPuntos("-" + puntosPerdidos);

        MostrarFeedbackError();

        Debug.Log(
            "NOTA PERDIDA | -" + puntosPerdidos +
            " | Progreso: " + progreso
        );

        ActualizarBarra();
    }

    // -------------------------------------------------------
    // TECLA FUERA DE TIEMPO
    // -------------------------------------------------------

    public void ErrorTecla()
    {
        if (juegoTerminado)
            return;

        int puntosPerdidos = 3;

        progreso -= puntosPerdidos;

        MostrarPuntos("-" + puntosPerdidos);

        MostrarFeedbackError();

        Debug.Log(
            "TECLA FUERA DE TIEMPO | -" +
            puntosPerdidos +
            " | Progreso: " + progreso
        );

        ActualizarBarra();
    }

    // -------------------------------------------------------
    // NOTA PROHIBIDA
    // -------------------------------------------------------

    public void TocarProhibida()
    {
        if (juegoTerminado)
            return;

        Debug.Log(
            "TOCASTE UNA NOTA PROHIBIDA - GAME OVER"
        );

        Perder();
    }

    // -------------------------------------------------------
    // BONUS ESPECIAL
    // -------------------------------------------------------

    public void BonusEspecial()
    {
        if (juegoTerminado)
            return;

        int bonus = 15;

        progreso += bonus;

        yaHuboProgreso = true;

        Debug.Log(
            "RAISE YOUR HAND +" + bonus +
            " | Progreso: " + progreso
        );

        ActualizarBarra();
    }

    // -------------------------------------------------------
    // TEXTO DE PUNTOS
    // -------------------------------------------------------

    void MostrarPuntos(string mensaje)
    {
        if (animacionPuntos != null)
        {
            StopCoroutine(animacionPuntos);
        }

        animacionPuntos =
            StartCoroutine(
                AnimarPuntos(mensaje)
            );
    }

    IEnumerator AnimarPuntos(string mensaje)
    {
        textoPuntos.gameObject.SetActive(true);

        textoPuntos.text = mensaje;

        RectTransform rect =
            textoPuntos.GetComponent<RectTransform>();

        rect.localScale =
            Vector3.one * 0.8f;

        float tiempo = 0f;
        float duracion = 0.15f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;

            float escala =
                Mathf.Lerp(
                    0.8f,
                    1.15f,
                    tiempo / duracion
                );

            rect.localScale =
                Vector3.one * escala;

            yield return null;
        }

        yield return new WaitForSeconds(0.55f);

        textoPuntos.gameObject.SetActive(false);

        rect.localScale = Vector3.one;

        animacionPuntos = null;
    }

    // -------------------------------------------------------
    // FEEDBACK CORRECTO
    // -------------------------------------------------------

    void MostrarFeedbackCorrecto()
    {
        string[] mensajes =
        {
            "OK",
            "GOOD",
            "PERFECT"
        };

        int indice =
            Random.Range(
                0,
                mensajes.Length
            );

        if (animacionFeedback != null)
        {
            StopCoroutine(
                animacionFeedback
            );
        }

        animacionFeedback =
            StartCoroutine(
                AnimarFeedback(
                    mensajes[indice]
                )
            );
    }

    // -------------------------------------------------------
    // FEEDBACK ERROR
    // -------------------------------------------------------

    void MostrarFeedbackError()
    {
        if (animacionFeedback != null)
        {
            StopCoroutine(
                animacionFeedback
            );
        }

        animacionFeedback =
            StartCoroutine(
                AnimarFeedback("X")
            );
    }

    // -------------------------------------------------------
    // ANIMACIÓN FEEDBACK
    // -------------------------------------------------------

    IEnumerator AnimarFeedback(string mensaje)
    {
        textoFeedback.gameObject.SetActive(true);

        textoFeedback.text = mensaje;

        RectTransform rect =
            textoFeedback.GetComponent<RectTransform>();

        rect.localScale =
            Vector3.one * 0.7f;

        float tiempo = 0f;
        float duracion = 0.15f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;

            float escala =
                Mathf.Lerp(
                    0.7f,
                    1.2f,
                    tiempo / duracion
                );

            rect.localScale =
                Vector3.one * escala;

            yield return null;
        }

        yield return new WaitForSeconds(0.45f);

        textoFeedback.gameObject.SetActive(false);

        rect.localScale = Vector3.one;

        animacionFeedback = null;
    }

    // -------------------------------------------------------
    // ACTUALIZAR BARRA
    // -------------------------------------------------------

    void ActualizarBarra()
    {
        progreso =
            Mathf.Clamp(
                progreso,
                0,
                100
            );

        barraPublico.value = progreso;

        Debug.Log(
            "BARRA: " + progreso
        );

        if (progreso >= 100)
        {
            Ganar();
        }
        else if (
            progreso <= 0 &&
            yaHuboProgreso
        )
        {
            Perder();
        }
    }

    // -------------------------------------------------------
    // GANAR
    // -------------------------------------------------------

    void Ganar()
    {
        if (juegoTerminado)
            return;

        juegoTerminado = true;

        if (fondoFinal != null)
        {
            fondoFinal.SetActive(true);
        }

        textoFinal.text =
            "¡FIESTA LLENA!";

        textoFinal.gameObject.SetActive(true);

        Debug.Log("GANASTE");

        Time.timeScale = 0f;
    }

    // -------------------------------------------------------
    // GAME OVER
    // -------------------------------------------------------

    void Perder()
    {
        if (juegoTerminado)
            return;

        juegoTerminado = true;

        if (fondoFinal != null)
        {
            fondoFinal.SetActive(true);
        }

        textoFinal.text =
            "GAME OVER";

        textoFinal.gameObject.SetActive(true);

        Debug.Log("PERDISTE");

        StartCoroutine(
            ReiniciarDespuesDeGameOver()
        );
    }

    // -------------------------------------------------------
    // REINICIO DESPUÉS DE GAME OVER
    // -------------------------------------------------------

    IEnumerator ReiniciarDespuesDeGameOver()
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(6f);

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager
                .GetActiveScene()
                .buildIndex
        );
    }
}