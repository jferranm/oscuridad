namespace Oscuridad.Enumeraciones
{
	/// <summary>
    /// tipo de interaccion con el interactuable
    /// </summary>
    public enum OpcionInteractuable
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
		PerdidaCordura,
        AbrirPuertaDesvanFallo,
        AbrirPuertaDesvanAcierto,
		AbrirDesvan,
        TiradaMitosCthulhuFigura,
		TiradaCienciasOcultasFigura,
		ExaminarAltar,
		EncontrarLlaveAltar
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
        HabilidadArtisticaPintar,
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
    /// Nombre de los Interactuables
    /// </summary>
    public enum Interactuables
    {
        Ninguno,
        PeriodicoSalon,
        CuadroSalon,
        Figura,
        CuadrosPrimeraPlanta1,
        CuadrosPrimeraPlanta2,
        PeriodicosHabitacionCharles,
        LibrosHabitacionCharles,
        FotosFamiliares,
        Puerta,
        Pala,
        CuadroDesvan,
        LibrosDesvan,
        LibroMesa,
        CirculoInvocacion,
        RecipientesAlquimia,
        HojasPared,
		Altar,
		AmaDeLlaves,
		Llave
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
		spa,
		eng,
		fr
	}

	/// <summary>
	/// Enumeracion para el control del estado del usuario
	/// </summary>
	public enum estadoUsuario
	{
		enHojaDePersonaje,
		enInventario,
		enMapa,
		enOpciones,
		enJuego
	};
}
