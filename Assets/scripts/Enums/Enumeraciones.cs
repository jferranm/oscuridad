namespace Oscuridad.Enumeraciones
{
    /// <summary>
    /// tipo de interaccion con el objeto
    /// </summary>
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
    public enum Caracteristicas
    {
        Fuerza,
        Constitucion,
        Tamanyo,
        Destreza,
        Aparicencia,
        Cordura,
        Inteligencia,
        Poder,
        Educacion,
        Idea,
        Suerte,
        Conocimiento,
        Vida,
        BonificacionDanyo
    }

    /// <summary>
    /// Habilidades del usuario
    /// </summary>
    public enum Habilidades
    {
        Ninguna,
        Antropologia,
        Arqueologia,
        Astronomia,
        BuscarLibros,
        CienciasOcultas,
        Conducir,
        Cerrajeria,
        Charlataneria,
        Credito,
        Derecho,
        Descubrir,
        Disfrazarse,
        Discrecion,
        Escuchar,
        Equitacion,
        Esquivar,
        Fotografia,
        HabilidadArtistica,
        Historia,
        HistoriaNatural,
        LenguaPropia,
        Mecanica,
        MitosDeCthulhu,
        Nadar,
        Ocultar,
        Ocultarse,
        OtraLenguaFrances,
		OtraLenguaAleman,
		OtraLenguaItaliano,
        OtraLenguaLatin,
        Persuasion,
        PrimerosAuxilios,
        Psicologia,
        Quimica,
        Regatear,
        Saltar,
        Examinar,
        Fallo
    }

    /// <summary>
    /// Nombre de las escenas
    /// </summary>
    public enum Escenas
    {
        ninguna,
		MenuPrincipal,
		Escena1,
		Escena2,
		Escena3,
        WardCasa,
        WardJardin,
        WardRecibidor,
        WardSalon,
        WardPasillo,
        WardHabitacionPadres,
        WardHabitacionCharles,
        WardPuertaDesvan,
        WardDesvan,
		EscenaWardExterior,
		EscenaWardInteriorPlantaBaja,
		EscenaWardInteriorPlantaSuperior
    }

    /// <summary>
    /// Nombre de los objetos
    /// </summary>
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
    public enum Personaje : int
    {
        RobertDuncan,
        MarlaGibbs,
        WarrenBedford
    }

    /// <summary>
    /// Datos personales del personaje jugador
    /// </summary>
    public enum DatosPersonales : int
    {
        Nombre,
        Profesion,
        Edad,
    }

    /// <summary>
    /// Armas del personaje jugador
    /// </summary>
    public enum Armas : int
    {
        ArmaCorta,
        Fusil,
        Escopeta
    }

    /// <summary>
    /// Estados del personaje jugador
    /// </summary>
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

	/// <summary>
	/// Estados del jugador
	/// </summary>
	public enum EstadosJugador
	{
		enMenus,
		enEspera,
		enZoomIn,
		enZoomOut,
		enZoomEspera,
		enInventario
	};

	/// <summary>
	/// Enumeracion para la copia de xml al dispositivo
	/// </summary>
	public enum XmlDatosEscenas
	{
		EscenaWardExterior,
		EscenaWardInteriorPlantaBaja,
		EscenaWardInteriorPlantaSuperior
	};

	/// <summary>
	/// Enumeracion para el idioma del juego
	/// </summary>
	public enum Idioma
	{
		spanish,
		english,
		french
	}
}
