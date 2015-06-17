using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextoCabeceraOpciones : MonoBehaviour 
{
	public Text textoCabecera;

	void OnEnable()
	{
		if (GameCenter.InstanceRef != null) 
		{
			if(GameCenter.InstanceRef.controladoraJuego.personajePulsado.DescripcionNombre != null)
				textoCabecera.text = GameCenter.InstanceRef.controladoraJuego.personajePulsado.DescripcionNombre.ToUpper();
			else
				textoCabecera.text = GameCenter.InstanceRef.controladoraJuego.objetoPulsado.DescripcionNombre.ToUpper();
		}
	}
}
