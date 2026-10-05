using UnityEngine;
using System.IO.Ports;

public class ArduinoInput : MonoBehaviour
{
    private SerialPort puerto;

    public ZonaAcierto zonaControlada;

    void Start()
    {
        puerto = new SerialPort("COM3", 9600);
        puerto.ReadTimeout = 20;

        try
        {
            puerto.Open();
            Debug.Log("Arduino conectado correctamente");
        }
        catch
        {
            Debug.Log("No se pudo conectar Arduino");
        }
    }

    void Update()
    {
        if (puerto == null || !puerto.IsOpen)
            return;

        try
        {
            string mensaje = puerto.ReadLine().Trim();

            if (mensaje == "HIT")
            {
                zonaControlada.ActivarDesdeArduino();
            }
        }
        catch
        {
            // No llegó información nueva
        }
    }

    void OnApplicationQuit()
    {
        if (puerto != null && puerto.IsOpen)
        {
            puerto.Close();
        }
    }
}