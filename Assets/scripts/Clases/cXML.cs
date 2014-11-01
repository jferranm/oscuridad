using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class cXML
{
    private string raiz;
    public string Raiz { get; set; }
    
    private Encoding codificacion = Encoding.UTF8;
    public Encoding Codificacion { get; set; }

    private string fichero;
    public string Fichero { get; set; }

    private XmlDocument doc = null;
    private XmlElement root = null;

    //constructores con varios parámetros
    //-----------------------------------
    public cXML()
    {
        raiz = "oscuridad";
        codificacion = Encoding.UTF8;
    }

    public cXML(string Raiz)
    {
        raiz = Raiz;
        codificacion = Encoding.UTF8;
    }

    public cXML(string Raiz, Encoding Codificacion)
    {
        raiz = Raiz;
        codificacion = Codificacion;
    }
    //-----------------------------------

    //Crear fichero XML 
    public bool Crear(string Fichero, bool Sobreescribir)
    {
        fichero = Fichero;
        return Crear(Sobreescribir);
    }

    public bool Crear(bool Sobreescribir) {
        if (fichero == "") { return false; }
        if (File.Exists(fichero) && !Sobreescribir) { return false; }

        try
        {
            XmlTextWriter f = new XmlTextWriter(fichero, codificacion);
            f.Formatting = Formatting.Indented;
            f.WriteStartDocument();
            f.WriteStartElement(raiz);
            f.WriteEndElement();
            f.Close();
        }
        catch
        {
            return false;
        }

        return true;
    }

    //Abrir un fichero existente
    //public bool Abrir(string Fichero)
	public bool Abrir(Stream streamAbrir)
    {
        doc = new XmlDocument();
   
        try {
            doc.Load(streamAbrir);
            root = doc.DocumentElement;
        } catch {
            return false;
        }
       
        return true;
    }   

	public bool Abrir(string fichero) {
		this.fichero = fichero;
		if (!File.Exists(fichero)) { return false; }
		doc = new XmlDocument();
		
		try {
			doc.Load(fichero);
			root = doc.DocumentElement;
			raiz = root.Name;
		} catch {
			return false;
		}
		
		return true;
	}
	
    public bool Abrir() {
        if (fichero == "") { return false; }
        if (!File.Exists(fichero)) { return false; }
        doc = new XmlDocument();
    
        try {
            doc.Load(fichero);
            root = doc.DocumentElement;
        } catch {
            return false;
        }
        
        return true;
    }

    //Cerrar (comprobar si guarda)
    public bool Cerrar() {
        root = null;
        doc = null;
        return true;
    }

    //Grabar Cambios
    public bool Grabar() {
        try {
			doc.Save(this.fichero);
        } catch {
            return false;
        }

        return true;
    }

    //diferentes maneras de crear un elemento
    //Path, atributos = crea una rama para contener nuevos valores
    public XmlNode CrearElemento(string Path, string[] Atributos)
    {
        XmlNode Nodo = root.SelectSingleNode(Path);
        if (Nodo == null) {
            try {
                XmlElement elemento = doc.CreateElement(Path);
                foreach (string atributo in Atributos)
                {
                    XmlAttribute attr = doc.CreateAttribute(atributo.Split(':')[0]);
                    attr.Value = atributo.Split(':')[1];
                    elemento.Attributes.Append(attr);
                }
                Nodo = root.AppendChild(elemento);
            }
            catch
            {
                return null;
            }
        }

        return Nodo;
    }

    //Path, Elemento, valor = crea un valor dentro de una rama
    public XmlNode CrearElemento(string Path, string Elemento, string Valor)
    {
        XmlNode Nodo = root.SelectSingleNode(Path);
        if (Nodo == null) {
           return null;
        }

        try {
            XmlElement elemento = doc.CreateElement(Path+"/"+Elemento);
            elemento.InnerText = Valor;    
            Nodo.AppendChild(elemento);
        } catch {
            return null;
        }
        return Nodo;
    }

    //Path, Elemento, valor = crea un valor dentro de una rama
    public XmlNode CrearElemento(string Path, string Elemento, string[] Atributos)
    {
        XmlNode Nodo = root.SelectSingleNode(Path);
        if (Nodo == null) { return null; }

        try
        {
            XmlElement elemento = doc.CreateElement(Elemento);
            foreach (string atributo in Atributos)
            {
                XmlAttribute attr = doc.CreateAttribute(atributo.Split(':')[0]);
                attr.Value = atributo.Split(':')[1];
                elemento.Attributes.Append(attr);
            }
            Nodo.AppendChild(elemento);
        }
        catch
        {
            return null;
        }
        return Nodo;
    }

	public XmlNode CrearElemento(string Path, int Elemento, string Valor, string[] Atributos) {
		return CrearElemento(Path, Elemento.ToString(), Valor, Atributos);
	}

    //Path, Elemento, valor, atributos = crea un valor dentro de una rama, con atributos
    public XmlNode CrearElemento(string Path, string Elemento, string Valor, string[] Atributos)
    {
        XmlNode Nodo = CrearElemento(Path, Elemento, Valor);
        if (Nodo == null) {
           return null;
        }
        Nodo = DevolverElementos(Path + "//" + Elemento)[0];

        try {
            foreach (string atributo in Atributos)
            {
                XmlAttribute attr = doc.CreateAttribute(atributo.Split(':')[0]);
                attr.Value = atributo.Split(':')[1];
                Nodo.Attributes.Append(attr);
            }
        } catch {
            return null;
        }

        return Nodo;
    }
	
	public bool SetValor(string ruta, string elemento, string valor)
	{
		ruta = ruta + "/" + elemento;
		Console.WriteLine(ruta);
		XmlNode nodo = null;
		try {
			nodo = root.SelectSingleNode(ruta);
		} catch {
		}
		if (nodo == null)  return false;
		nodo.InnerText = valor;
		return true;
	}

	public bool SetValor(string Path, int Elemento, string Valor)
	{
		return SetValor(Path, Elemento.ToString(), Valor);
	}

	public bool SetAtributo(string path, int elemento, string atributo, string valor) {
		return SetAtributo(path, elemento.ToString(), atributo, valor);
	}

	public bool SetAtributo(string path, string elemento, string atributo, string valor) {
		XmlNode Nodo = DevolverElementos(path + "//" + elemento)[0];
		if (Nodo == null) {
			return false;
		}

		XmlAttribute attr = Nodo.Attributes[atributo];
		if (attr == null) {
			attr = doc.CreateAttribute(atributo);
			attr.Value = valor;
			Nodo.Attributes.Append(attr);
		} else {
			attr.Value = valor;
		}
		return true;
	}

    //devuelve una lista de elementos dentro de una rama
    public XmlNodeList DevolverElementos(string path) {
       return root.SelectNodes(path);
    }

    //devuelve un dato de un elemento dado 
    public string DevolverValorElemento (string Path) {
        string result = "";

        try
        {
            XmlNode elemento = root.SelectSingleNode(Path);
            result = elemento.InnerText;
        }
        catch { }

        return result;
    }

    //elimina todos los elementos que coinciden con la Path
    public int EliminarElementos(string Path) {
        int cont = 0;
        XmlNodeList elementos = root.SelectNodes(Path);
        foreach (XmlNode elemento in elementos)
            try
            {
                elemento.ParentNode.RemoveChild(elemento);
                cont += 1;
            }
            catch
            {
            }

        return cont;
    }

    //Serializar Clases
    //Ejemplo

    /* ClaseASerializar cs = new ClaseASerializar();
     * cs.uno = 43;
     * cs.dos = 31;
     * Guardar_Clase_Serializable<ClaseASerializar>(@"c:\walter\pepe.xml", cs);
     * 
     *  [Serializable()]
     *  public class ClaseASerializar
     *  {
     *  public int uno { get; set; }
     *  public int dos { get; set; }
     *  }
     */
    public void Guardar_Clase_Serializable<T>(string ruta, T objetoClase)
    {
        XmlSerializer objetoSerializado = new XmlSerializer(typeof(T));

        // Creamos un nuevo FileStream para serializar el objeto en un fichero
        TextWriter GrabarFileStream = new StreamWriter(ruta, false);
        objetoSerializado.Serialize(GrabarFileStream, objetoClase);

        GrabarFileStream.Close();
    }

    public T Cargar_Clase_Serializable<T>(string ruta, T objetoClase)
    {
        XmlSerializer objetoSerializado = new XmlSerializer(typeof(T));

        // creamos un nuevo FileStream para leer el archivo XML
        FileStream CargarFileStream = new FileStream(ruta, FileMode.Open, FileAccess.Read, FileShare.Read);

        // Cargamos el objeto cargado usando la funcion Deserialize
        T objetoCargado = (T)objetoSerializado.Deserialize(CargarFileStream);

        CargarFileStream.Close();

        return objetoCargado;
    }
}
