using UnityEngine;
using UnityEngine.InputSystem;

public class ZonaAcierto : MonoBehaviour
{
    public InputActionReference accionTecla;

    private GameObject notaActual;

    // Evita que varios pulsos del encoder cuenten varias veces
    private bool notaYaActivada = false;

    void OnEnable()
    {
        accionTecla.action.Enable();
    }

    void OnDisable()
    {
        accionTecla.action.Disable();
    }

    void OnTriggerEnter(Collider other)
    {
        Nota nota = other.GetComponent<Nota>();

        if (nota != null)
        {
            notaActual = other.gameObject;
            notaYaActivada = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == notaActual)
        {
            Nota nota = notaActual.GetComponent<Nota>();

            if (nota != null && !nota.esProhibida && !notaYaActivada)
            {
                Debug.Log("NOTA NORMAL PERDIDA");

                GameManager.instance.NotaPerdida();
            }

            notaActual = null;
            notaYaActivada = false;
        }
    }

    void Update()
    {
        // Teclado sigue funcionando como antes
        if (accionTecla.action.WasPressedThisFrame())
        {
            ActivarZona(false);
        }
    }

    public void ActivarDesdeArduino()
    {
        ActivarZona(true);
    }

    private void ActivarZona(bool vieneDeArduino)
    {
        // Hay una nota dentro de la zona
        if (notaActual != null)
        {
            // Si Arduino ya activó esta misma nota,
            // ignoramos los demás pulsos del encoder.
            if (vieneDeArduino && notaYaActivada)
            {
                return;
            }

            Nota nota = notaActual.GetComponent<Nota>();

            if (nota != null)
            {
                if (nota.esProhibida)
                {
                    Debug.Log("ERROR: TOCASTE NOTA PROHIBIDA");

                    notaYaActivada = true;

                    GameManager.instance.TocarProhibida();
                }
                else
                {
                    Debug.Log("ACIERTO");

                    notaYaActivada = true;

                    GameManager.instance.AciertoNormal();
                }
            }

            GameObject notaParaDestruir = notaActual;

            notaActual = null;

            Destroy(notaParaDestruir);
        }
        else
        {
            // Si viene del teclado, conserva el error fuera de tiempo.
            if (!vieneDeArduino)
            {
                Debug.Log("ERROR: TECLA FUERA DE TIEMPO");

                GameManager.instance.ErrorTecla();
            }

            // Si viene del Arduino y ya giraste varias estaciones,
            // simplemente ignoramos esos pulsos extra.
        }
    }
}