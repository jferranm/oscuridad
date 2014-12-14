using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextoLateralOpciones : MonoBehaviour 
{
	public Text textoCuerpo;
	
	void OnEnable()
	{
		if (GameCenter.InstanceRef != null) 
		{
			if(GameCenter.InstanceRef.controladoraJuego.objetoPulsado != null)
				textoCuerpo.text = GameCenter.InstanceRef.controladoraJuego.objetoPulsado.MostrarDescripcionBasica();
			else
				textoCuerpo.text = "";
		}
	}
}