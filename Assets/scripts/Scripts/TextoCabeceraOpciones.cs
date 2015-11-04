using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextoCabeceraOpciones : MonoBehaviour 
{
	public Text textoCabecera;

	void OnEnable()
	{
		if (GameCenter.InstanceRef != null) 
			textoCabecera.text = GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.Descripcion.ToUpper();
	}
}
