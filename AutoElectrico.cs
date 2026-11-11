using System;

namespace PuertoSur
{
    // Clase derivada que hereda de Vehiculo.
    // Aplica: Herencia, Polimorfismo (override), Encapsulamiento y Sobrecarga de métodos.
    public class AutoElectrico : Vehiculo
    {
        // Propiedades propias de AutoElectrico con get y set (Encapsulamiento)
        public float CapacidadKWh { get; set; }
        public float PorcentajeCarga { get; set; }
        public bool EsAptoParaEmbarque { get; set; }

        // Constructor completo: recibe todos los datos + póliza + datos eléctricos.
        // Fija TipoCombustible = "ELECTRICO" automáticamente vía base(...).
        // EsAptoParaEmbarque arranca en false hasta que se realice la inspección.
        public AutoElectrico(string marca, string modelo, string patente, int anioFabricacion, float precioBasePoliza, float capacidadKWh, float porcentajeCarga)
            : base(marca, modelo, patente, anioFabricacion, "ELECTRICO", precioBasePoliza)
        {
            CapacidadKWh = capacidadKWh;
            PorcentajeCarga = porcentajeCarga;
            EsAptoParaEmbarque = false;
        }

        // Constructor reducido: sin póliza declarada, aplica tarifa preventiva de USD 1500 delegando a base(...)
        public AutoElectrico(string marca, string modelo, string patente, int anioFabricacion, float capacidadKWh, float porcentajeCarga)
            : base(marca, modelo, patente, anioFabricacion, "ELECTRICO")
        {
            CapacidadKWh = capacidadKWh;
            PorcentajeCarga = porcentajeCarga;
            EsAptoParaEmbarque = false;
        }

        // Evalúa si la carga actual es menor o igual al 30% (condición de seguridad).
        // Actualiza y retorna EsAptoParaEmbarque.
        public bool InspeccionarRiesgoBateria()
        {
            EsAptoParaEmbarque = (PorcentajeCarga <= 30.0f);
            return EsAptoParaEmbarque;
        }

        // Sobrecarga: actualiza PorcentajeCarga con una nueva lectura y vuelve a evaluar el riesgo
        public bool InspeccionarRiesgoBateria(float nuevoPorcentaje)
        {
            PorcentajeCarga = nuevoPorcentaje;
            return InspeccionarRiesgoBateria();
        }

        // Retorna el dictamen textual de la inspección
        public string InformarEstadoInspeccion()
        {
            if (EsAptoParaEmbarque)
                return "APROBADO PARA EMBARQUE";
            else
                return "RECHAZADO POR RIESGO TERMICO";
        }

        // Override: descuento ambiental del 30% sobre el precio base.
        // Los eléctricos no pagan el impuesto del 3%.
        public override float CalcularCostoPoliza()
        {
            return PrecioBasePoliza * 0.70f;
        }

        // Override: muestra datos generales + datos eléctricos + dictamen de inspección.
        // El costo de póliza con descuento solo se muestra si el vehículo está aprobado,
        // ya que el descuento ambiental aplica únicamente a los que superan la inspección.
        public override void MostrarInfo()
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Patente: {Patente} | {Marca} {Modelo} ({AnioFabricacion}) [ELECTRICO]");
            Console.WriteLine($"Capacidad Batería: {CapacidadKWh} kWh | Carga Actual: {PorcentajeCarga}%");
            Console.WriteLine($"Estado Inspección: {InformarEstadoInspeccion()}");
            Console.WriteLine($"Precio Base Póliza: USD {PrecioBasePoliza:F2}");

            if (EsAptoParaEmbarque)
                Console.WriteLine($"Costo Final Póliza (con desc. 30% ambiental): USD {CalcularCostoPoliza():F2}");
            else
                Console.WriteLine("Costo de Póliza: NO CORRESPONDE (vehículo retenido por riesgo térmico)");

            Console.WriteLine("--------------------------------------------------");
        }
    }
}
