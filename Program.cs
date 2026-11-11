using System;
using System.Collections.Generic;
using System.Globalization;

namespace PuertoSur
{
    class Program
    {
        static void Main(string[] args)
        {
            // Cultura invariante: acepta punto decimal al ingresar valores float
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            // Colecciones requeridas por el enunciado
            List<Vehiculo> listaAprobados  = new List<Vehiculo>();
            List<Vehiculo> listaRechazados = new List<Vehiculo>();

            int opcion = 0;

            // Bucle principal do-while: repite hasta que el usuario elija Salir (opción 5)
            do
            {
                // La ocupación real incluye aprobados + rechazados (todos ocupan bodega)
                int totalEnBodega = listaAprobados.Count + listaRechazados.Count;

                Console.WriteLine();
                Console.WriteLine("================================================================================");
                Console.WriteLine("  SISTEMA DE CONTROL DE EMBARQUE - PUERTO SUR");
                Console.WriteLine("================================================================================");
                Console.WriteLine($"  {ControlCapacidad.ObtenerReporteCupo(totalEnBodega)}");
                Console.WriteLine("================================================================================");
                Console.WriteLine("  1. Cargar Vehículo (Ingreso e Inspección)");
                Console.WriteLine("  2. Modificar Lista de Aprobados (Eliminar por Patente)");
                Console.WriteLine("  3. Ver Lista de Vehículos APROBADOS");
                Console.WriteLine("  4. Ver Lista de Vehículos RECHAZADOS");
                Console.WriteLine("  5. Salir del Sistema");
                Console.WriteLine("================================================================================");
                Console.Write("  Ingrese opción: ");

                string entradaOpcion = Console.ReadLine().Trim();
                if (!int.TryParse(entradaOpcion, out opcion))
                {
                    opcion = -1;
                }

                Console.WriteLine();

                switch (opcion)
                {
                    // -----------------------------------------------------------------
                    // OPCIÓN 1: Cargar Vehículo
                    // -----------------------------------------------------------------
                    case 1:
                        Console.WriteLine("--- CARGA E INSPECCIÓN DE VEHÍCULO ---");

                        // Verificar cupo antes de registrar (aprobados + rechazados)
                        if (!ControlCapacidad.HayCupoDisponible(listaAprobados.Count + listaRechazados.Count))
                        {
                            Console.WriteLine(">> ATENCIÓN: No hay cupo disponible en bodega. Se alcanzó la capacidad máxima (15 vehículos).");
                            break;
                        }

                        // Determinar tipo de vehículo
                        Console.Write("¿El vehículo es eléctrico? (S/N): ");
                        string esElec = Console.ReadLine().Trim().ToUpper();
                        bool esElectrico = (esElec == "S" || esElec == "SI");

                        // Datos generales (comunes a ambos tipos)
                        Console.Write("Marca: ");
                        string marca = Console.ReadLine().Trim();

                        Console.Write("Modelo: ");
                        string modelo = Console.ReadLine().Trim();

                        Console.Write("Patente: ");
                        string patente = Console.ReadLine().Trim().ToUpper();

                        Console.Write("Año de fabricación: ");
                        int anio;
                        while (!int.TryParse(Console.ReadLine(), out anio) || anio < 1900 || anio > DateTime.Now.Year + 1)
                        {
                            Console.Write("Año inválido. Ingrese nuevamente: ");
                        }

                        if (!esElectrico)
                        {
                            // --- Vehículo a Combustión ---
                            Console.Write("Tipo de combustible (Nafta/Diesel/GNC/Híbrido): ");
                            string combustible = Console.ReadLine().Trim();
                            if (string.IsNullOrWhiteSpace(combustible)) combustible = "Nafta";

                            Console.Write("Precio base de póliza (USD, Enter o 0 = tarifa preventiva $1500): ");
                            string strPoliza = Console.ReadLine().Trim();
                            float poliza;
                            bool ingresoPoliza = float.TryParse(strPoliza, NumberStyles.Float, CultureInfo.InvariantCulture, out poliza) && poliza > 0;

                            Vehiculo vehiculoCombustion;
                            if (ingresoPoliza)
                                vehiculoCombustion = new Vehiculo(marca, modelo, patente, anio, combustible, poliza);
                            else
                                vehiculoCombustion = new Vehiculo(marca, modelo, patente, anio, combustible); // usa tarifa preventiva

                            listaAprobados.Add(vehiculoCombustion);
                            Console.WriteLine("\n>> Vehículo a combustión registrado con éxito.");
                            Console.WriteLine(">> Clasificado en: APROBADOS PARA EMBARQUE.");
                            Console.WriteLine($">> Costo de póliza (con impuesto del 3%): USD {vehiculoCombustion.CalcularCostoPoliza():F2}");
                        }
                        else
                        {
                            // --- Vehículo Eléctrico ---
                            Console.Write("Capacidad de batería (kWh, entre 30 y 100): ");
                            float kwh;
                            while (!float.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out kwh) || kwh < 30 || kwh > 100)
                            {
                                Console.Write("Valor fuera de rango (30-100 kWh). Ingrese nuevamente: ");
                            }

                            Console.Write("Porcentaje de carga actual (%, entre 0 y 100): ");
                            float carga;
                            while (!float.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out carga) || carga < 0 || carga > 100)
                            {
                                Console.Write("Porcentaje fuera de rango (0-100%). Ingrese nuevamente: ");
                            }

                            Console.Write("Precio base de póliza (USD, Enter o 0 = tarifa preventiva $1500): ");
                            string strPolizaElec = Console.ReadLine().Trim();
                            float polizaElec;
                            bool ingresoPolizaElec = float.TryParse(strPolizaElec, NumberStyles.Float, CultureInfo.InvariantCulture, out polizaElec) && polizaElec > 0;

                            AutoElectrico autoElectrico;
                            if (ingresoPolizaElec)
                                autoElectrico = new AutoElectrico(marca, modelo, patente, anio, polizaElec, kwh, carga);
                            else
                                autoElectrico = new AutoElectrico(marca, modelo, patente, anio, kwh, carga); // usa tarifa preventiva

                            // Ofrecer re-medición de la carga en bodega
                            Console.Write("¿Desea re-medir la carga en bodega? (S/N): ");
                            string remedir = Console.ReadLine().Trim().ToUpper();

                            if (remedir == "S" || remedir == "SI")
                            {
                                Console.Write("Ingrese nueva lectura de carga (%): ");
                                float nuevaCarga;
                                while (!float.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out nuevaCarga) || nuevaCarga < 0 || nuevaCarga > 100)
                                {
                                    Console.Write("Porcentaje fuera de rango (0-100%). Ingrese nuevamente: ");
                                }
                                autoElectrico.InspeccionarRiesgoBateria(nuevaCarga); // sobrecarga con nueva lectura
                            }
                            else
                            {
                                autoElectrico.InspeccionarRiesgoBateria(); // usa lectura inicial
                            }

                            Console.WriteLine($"\n>> DICTAMEN: {autoElectrico.InformarEstadoInspeccion()}");

                            if (autoElectrico.EsAptoParaEmbarque)
                            {
                                listaAprobados.Add(autoElectrico);
                                Console.WriteLine(">> Clasificado en: APROBADOS PARA EMBARQUE.");
                                Console.WriteLine($">> Costo de póliza (con descuento ambiental del 30%): USD {autoElectrico.CalcularCostoPoliza():F2}");
                            }
                            else
                            {
                                listaRechazados.Add(autoElectrico);
                                Console.WriteLine(">> El vehículo queda retenido por superar el límite del 30% de carga.");
                                Console.WriteLine(">> Clasificado en: RECHAZADOS.");
                            }
                        }
                        break;

                    // -----------------------------------------------------------------
                    // OPCIÓN 2: Modificar Aprobados (Eliminar por Patente)
                    // -----------------------------------------------------------------
                    case 2:
                        Console.WriteLine("--- MODIFICAR LISTA DE APROBADOS (ELIMINAR POR PATENTE) ---");

                        if (listaAprobados.Count == 0)
                        {
                            Console.WriteLine(">> La lista de aprobados está vacía.");
                            break;
                        }

                        Console.Write("Ingrese la patente del vehículo a eliminar: ");
                        string patenteBuscada = Console.ReadLine().Trim().ToUpper();

                        int indiceEliminar = listaAprobados.FindIndex(v => v.Patente.Equals(patenteBuscada, StringComparison.OrdinalIgnoreCase));

                        if (indiceEliminar != -1)
                        {
                            Vehiculo eliminado = listaAprobados[indiceEliminar];
                            listaAprobados.RemoveAt(indiceEliminar);
                            Console.WriteLine($">> Vehículo [{eliminado.Patente}] {eliminado.Marca} {eliminado.Modelo} eliminado de APROBADOS.");
                        }
                        else
                        {
                            Console.WriteLine($">> No se encontró ningún vehículo con la patente [{patenteBuscada}].");
                        }
                        break;

                    // -----------------------------------------------------------------
                    // OPCIÓN 3: Ver Lista de Vehículos APROBADOS (Polimorfismo)
                    // -----------------------------------------------------------------
                    case 3:
                        Console.WriteLine("--- LISTA DE VEHÍCULOS APROBADOS PARA EMBARQUE ---");

                        if (listaAprobados.Count == 0)
                        {
                            Console.WriteLine(">> No hay vehículos aprobados registrados.");
                        }
                        else
                        {
                            Console.WriteLine($"Total aprobados: {listaAprobados.Count}");
                            for (int i = 0; i < listaAprobados.Count; i++)
                            {
                                Console.WriteLine($"\n[Vehículo #{i + 1}]");
                                // Polimorfismo: C# llama MostrarInfo() de Vehiculo o AutoElectrico según el tipo real
                                listaAprobados[i].MostrarInfo();
                            }
                        }
                        break;

                    // -----------------------------------------------------------------
                    // OPCIÓN 4: Ver Lista de Vehículos RECHAZADOS
                    // -----------------------------------------------------------------
                    case 4:
                        Console.WriteLine("--- LISTA DE VEHÍCULOS RECHAZADOS POR RIESGO TÉRMICO ---");

                        if (listaRechazados.Count == 0)
                        {
                            Console.WriteLine(">> No hay vehículos retenidos en bodega.");
                        }
                        else
                        {
                            Console.WriteLine($"Total retenidos: {listaRechazados.Count}");
                            for (int i = 0; i < listaRechazados.Count; i++)
                            {
                                Console.WriteLine($"\n[Vehículo Retenido #{i + 1}]");
                                listaRechazados[i].MostrarInfo();
                            }
                        }
                        break;

                    // -----------------------------------------------------------------
                    // OPCIÓN 5: Salir
                    // -----------------------------------------------------------------
                    case 5:
                        Console.WriteLine(">> Programa finalizado. Hasta pronto.");
                        break;

                    default:
                        Console.WriteLine(">> Opción inválida. Seleccione un número entre 1 y 5.");
                        break;
                }

                if (opcion != 5)
                {
                    Console.WriteLine("\nPresione ENTER para continuar...");
                    Console.ReadLine();
                }

            } while (opcion != 5);
        }
    }
}
