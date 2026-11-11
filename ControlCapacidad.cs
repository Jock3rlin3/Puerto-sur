namespace PuertoSur
{
    // Clase estática: centraliza las reglas de negocio sobre la capacidad de bodega.
    // No requiere instanciación para usarse.
    public static class ControlCapacidad
    {
        // Límite máximo de vehículos por viaje (aprobados + rechazados)
        public const int CapacidadMaxima = 15;

        // Devuelve true si todavía hay cupo disponible en bodega
        public static bool HayCupoDisponible(int cantidadActual)
        {
            return cantidadActual < CapacidadMaxima;
        }

        // Genera el reporte textual de ocupación y plazas libres
        public static string ObtenerReporteCupo(int cantidadActual)
        {
            int plazasLibres = CapacidadMaxima - cantidadActual;
            if (plazasLibres < 0) plazasLibres = 0;
            return $"Ocupación actual: {cantidadActual}/{CapacidadMaxima} | Plazas libres: {plazasLibres}";
        }
    }
}
