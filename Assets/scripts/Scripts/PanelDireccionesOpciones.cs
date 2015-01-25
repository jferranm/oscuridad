using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Oscuridad.Enumeraciones;

public class PanelDireccionesOpciones : MonoBehaviour 
{
	public GameObject botonNorte;
	public GameObject botonSur;
	public GameObject botonEste;
	public GameObject botonOeste;

	void OnEnable()
	{
		if (GameCenter.InstanceRef != null) 
		{
			Reiniciar_Direcciones();
		}
	}

	private void Reorganizar_Direcciones()
	{
		if (GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaNorte.Equals("ninguna")) 
			Desactivar (botonNorte);

		if (GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaSur.Equals("ninguna"))
			Desactivar (botonSur);

		if (GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaEste.Equals("ninguna"))
			Desactivar (botonEste);
		
		if (GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaOeste.Equals("ninguna"))
			Desactivar (botonOeste);
	}

	private void Normalizar_Botones()
	{
		botonNorte.GetComponent<Image> ().color = new Color (255, 255, 255);
		botonSur.GetComponent<Image> ().color = new Color (255, 255, 255);
		botonEste.GetComponent<Image> ().color = new Color (255, 255, 255);
		botonOeste.GetComponent<Image> ().color = new Color (255, 255, 255);
	}

	private void Desactivar (GameObject objeto)
	{
		Image imagenBoton = objeto.GetComponent<Image> ();

		imagenBoton.color = new Color (255,0,0);
	}

	public void Reiniciar_Direcciones()
	{
		Normalizar_Botones();
		Reorganizar_Direcciones ();
	}
}
