using System;

namespace PuertoSur
{
    public class Vehiculo
    {
        // Propiedades públicas con get y set (Encapsulamiento)
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Patente { get; set; }
        public int AnioFabricacion { get; set; }
        public string TipoCombustible { get; set; }
        public float PrecioBasePoliza { get; set; }

        // Constructor completo: recibe todos los atributos incluyendo la póliza
        public Vehiculo(string marca, string modelo, string patente, int anioFabricacion, string tipoCombustible, float precioBasePoliza)
        {
            Marca = marca;
            Modelo = modelo;
            Patente = patente;
            AnioFabricacion = anioFabricacion;
            TipoCombustible = tipoCombustible;
            PrecioBasePoliza = precioBasePoliza;
        }

        // Constructor reducido: omite PrecioBasePoliza y delega en el completo asignando tarifa preventiva de USD 1500
        public Vehiculo(string marca, string modelo, string patente, int anioFabricacion, string tipoCombustible)
            : this(marca, modelo, patente, anioFabricacion, tipoCombustible, 1500.0f)
        {
        }

        // Calcula el costo de la póliza aplicando un impuesto del 3% sobre el precio base
        public virtual float CalcularCostoPoliza()
        {
            return PrecioBasePoliza * 1.03f;
        }

        // Muestra la información general del vehículo y el costo calculado de su póliza
        public virtual void MostrarInfo()
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Patente: {Patente} | {Marca} {Modelo} ({AnioFabricacion})");
            Console.WriteLine($"Combustible: {TipoCombustible}");
            Console.WriteLine($"Precio Base Póliza: USD {PrecioBasePoliza:F2}");
            Console.WriteLine($"Costo Final Póliza (con imp. 3%): USD {CalcularCostoPoliza():F2}");
            Console.WriteLine("--------------------------------------------------");
        }
    }
}
