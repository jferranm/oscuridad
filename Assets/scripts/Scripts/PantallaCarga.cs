using UnityEngine;
using System.Collections;

public class PantallaCarga : MonoBehaviour 
{
	private float fadeSpeed = 0.6f;
	private Transform objetoTextura;
	private Color colorInicial = new Color();

	public bool comenzarFade = false;

	void Awake()
	{
		DontDestroyOnLoad(this);

		foreach (Transform objetoHijo in this.transform) 
		{
			objetoHijo.guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
			objetoTextura = objetoHijo;
		}

		colorInicial = objetoTextura.guiTexture.color;
		this.gameObject.SetActive(false);
	}
	
	void OnEnable()
	{
		objetoTextura.guiTexture.color = colorInicial;
		comenzarFade = false;
	}

	void Update()
	{
		if(comenzarFade)
			Hacer_Fade();
	}

	private void Hacer_Fade()
	{
		//Hacer Fade del la imagen a imagen vacia
		FadeToClear();
		
		//Si la imagen esta casi clara
		if(objetoTextura.guiTexture.color.a <= 0.05f)
		{
			//hacemos que el color sea claro y desactivamos el objeto
			objetoTextura.guiTexture.color = Color.clear;
			comenzarFade = false;
			GameCenter.InstanceRef.controladoraGUI.Activar_Opciones_Basicas();
			this.gameObject.SetActive(false);
		}
	}

	void FadeToClear ()
	{
		objetoTextura.guiTexture.color = Color.Lerp(objetoTextura.guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
}
