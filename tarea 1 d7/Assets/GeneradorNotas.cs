using UnityEngine;

public class GeneradorNotas : MonoBehaviour
{
    public GameObject notaNormal;
    public GameObject notaProhibida;

    public Transform spawnA;
    public Transform spawnW;
    public Transform spawnS;
    public Transform spawnD;

    private float tiempoJuego = 0f;
    private float temporizador = 0f;

    void Update()
    {
        tiempoJuego += Time.deltaTime;
        temporizador += Time.deltaTime;

        float intervalo;

        // FASE 1: 0 - 15 segundos
        if (tiempoJuego < 15f)
        {
            intervalo = 2.5f;
        }
        // FASE 2: 15 - 30 segundos
        else if (tiempoJuego < 30f)
        {
            intervalo = 1.8f;
        }
        // FASE 3: después de 30 segundos
        else
        {
            intervalo = 1.4f;
        }

        if (temporizador >= intervalo)
        {
            // Fases 1 y 2: una nota a la vez
            if (tiempoJuego < 30f)
            {
                CrearUnaNota();
            }
            // Fase 3: puede haber una o dos notas
            else
            {
                CrearNotasFase3();
            }

            temporizador = 0f;
        }
    }

    void CrearUnaNota()
    {
        Transform[] puntos = { spawnA, spawnW, spawnS, spawnD };

        int carril = Random.Range(0, puntos.Length);

        GameObject tipoNota = ElegirTipoNota();

        Instantiate(
            tipoNota,
            puntos[carril].position,
            puntos[carril].rotation
        );
    }

    void CrearNotasFase3()
    {
        Transform[] puntos = { spawnA, spawnW, spawnS, spawnD };

        // 50% una nota
        // 50% dos notas
        int cantidadNotas = Random.Range(1, 3);

        if (cantidadNotas == 1)
        {
            CrearUnaNota();
        }
        else
        {
            int carril1 = Random.Range(0, puntos.Length);
            int carril2;

            do
            {
                carril2 = Random.Range(0, puntos.Length);
            }
            while (carril2 == carril1);

            GameObject tipoNota1 = ElegirTipoNota();
            GameObject tipoNota2 = ElegirTipoNota();

            Instantiate(
                tipoNota1,
                puntos[carril1].position,
                puntos[carril1].rotation
            );

            Instantiate(
                tipoNota2,
                puntos[carril2].position,
                puntos[carril2].rotation
            );
        }
    }

    GameObject ElegirTipoNota()
    {
        float random = Random.value;

        // FASE 1:
        // 100% notas normales
        if (tiempoJuego < 15f)
        {
            return notaNormal;
        }

        // FASE 2:
        // 75% normales
        // 25% prohibidas
        if (tiempoJuego < 30f)
        {
            if (random < 0.25f)
            {
                return notaProhibida;
            }

            return notaNormal;
        }

        // FASE 3:
        // 70% normales
        // 30% prohibidas
        if (random < 0.30f)
        {
            return notaProhibida;
        }

        return notaNormal;
    }
}