using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Oscuridad.Enumeraciones;


public class OpcionesMapaJuego : MonoBehaviour 
{
    public List<GameObject> localizacionesMapa = new List<GameObject>();

    void OnEnable()
    {
        if(GameCenter.InstanceRef != null)
        {
            foreach(Localizaciones localizacion in GameCenter.InstanceRef.controladoraJuego.jugadorActual.LocalizacionesDescubiertas)
            {
                localizacionesMapa.Find(x => x.name == localizacion.ToString()).SetActive(true);
            }
        }
    }
}
