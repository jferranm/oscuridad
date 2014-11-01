using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oscuridad.Enumeraciones
{
    /// <summary>
    /// tipo de interaccion con el objeto
    /// </summary>
	[Serializable]
    public enum opcionObjeto
    {
        Coger,
        Hablar,
        Observar,
        Ninguna
    }

    /// <summary>
    /// Enumeracion para la Acciones en el juego que generan eventos.
    /// </summary>
	[Serializable]
    public enum Acciones
    {
        Ninguna,
        AbrirPuertaDesvanFallo,
        AbrirPuertaDesvanAcierto,
        TiradaCienciasOcultasFigura,
        TiradaMitosCthulhuFigura
    }

    /// <summary>
    /// Caracteristicas del usuario
    /// </summary>
	[Serializable]
    public enum Caracteristicas : int
    {
        Fuerza = 1,
        Constitucion = 2,
        Tamanyo = 3,
        Destreza = 4,
        Aparicencia = 5,
        Cordura = 6,
        Inteligencia = 7,
        Poder = 8,
        Educacion = 9,
        Idea = 10,
        Suerte = 11,
        Conocimiento = 12,
        Vida = 13,
        BonificacionDanyo = 14
    }

    /// <summary>
    /// Habilidades del usuario
    /// </summary>
	[Serializable]
    public enum Habilidades : int
    {
        Ninguna = 0,
        Antropologia = 1,
        Arqueologia = 2,
        Astronomia = 3,
        BuscarLibros = 4,
        CienciasOcultas = 5,
        Conducir = 6,
        Cerrajeria = 7,
        Charlataneria = 8,
        Credito = 9,
        Derecho = 10,
        Descubrir = 11,
        Disfrazarse = 12,
        Discrecion = 13,
        Escuchar = 14,
        Equitacion = 15,
        Esquivar = 16,
        Fotografia = 17,
        HabilidadArtistica = 18,
        Historia = 19,
        HistoriaNatural = 20,
        LenguaPropia = 21,
        Mecanica = 22,
        MitosDeCthulhu = 23,
        Nadar = 24,
        Ocultar = 25,
        Ocultarse = 26,
        OtraLenguaFrances = 27,
        OtraLenguaLatin = 28,
        Persuasion = 29,
        PrimerosAuxilios = 30,
        Psicologia = 31,
        Quimica = 32,
        Regatear = 33,
        Saltar = 34,
        Examinar = 35,
        Fallo = 666
    }

    /// <summary>
    /// Nombre de las escenas
    /// </summary>
	[Serializable]
    public enum Escenas
    {
        ninguna,
        Escena10,
        Escena11,
        Escena12,
        Escena13,
        Escena14,
        Escena15,
        Escena16,
        Escena17,
        Escena18,
        Escena19
    }

    /// <summary>
    /// Nombre de los objetos
    /// </summary>
	[Serializable]
    public enum Objetos
    {
        Ninguno,
        PeriodicoSalon,
        CuadroSalon,
        Figura,
        CuadrosPrimeraPlanta1,
        CuadrosPrimeraPlanta2,
        PeriodicosHabitacionCharles,
        LibrosHabitacionCharles,
        Mesita,
        Puerta,
        Pala,
        CuadroDesvan,
        LibrosDesvan,
        LibroMesa,
        CirculoInvocacion,
        ObjetosAlquimia,
        HojasPared
    }

    /// <summary>
    /// Nombre del personaje jugador
    /// </summary>
	[Serializable]
    public enum Personaje : int
    {
        RobertDuncan = 1,
        MarlaGibbs = 2,
        WarrenBedford = 3
    }

    /// <summary>
    /// Datos personales del personaje jugador
    /// </summary>
	[Serializable]
    public enum DatosPersonales : int
    {
        Nombre = 1,
        Profesion = 2,
        Edad = 3,
        Foto = 4
    }

    /// <summary>
    /// Armas del personaje jugador
    /// </summary>
	[Serializable]
    public enum Armas : int
    {
        ArmaCorta = 1,
        Fusil = 2,
        Escopeta = 3
    }

    /// <summary>
    /// Estados del personaje jugador
    /// </summary>
	[Serializable]
    public enum estadosJugador
    {
        enMenus,
        enEspera,
        enZoomIn,
        enZoomOut,
        enZoomEspera,
        enInventario
    }

    /// <summary>
    /// Localizaciones para el mapa
    /// </summary>
	[Serializable]
    public enum Localizaciones
    {
        Ninguna,
        CasaFamiliarWard,
        CementerioArkham,
        LibreriaArkham,
        UniversidadArkham,
        ArkhamAdvertiser,
        ManicomioArkham,
        PuertoArkham,
        MansionPawtuxet,
        AlrededoresMansionPawtuxet,
        EntradaSecretaMansion
    }
}
