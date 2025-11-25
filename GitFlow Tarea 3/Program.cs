using GitFlow_Tarea_3.Base_Entity;
using GitFlow_Tarea_3.Cajero;
using GitFlow_Tarea_3.Entity;
using GitFlow_Tarea_3.Factory;

namespace GitFlow_Tarea_3
{
    public class Program
    {
        // Simulación de la base de datos: una lista estática de productos
        private static List<Producto> listaProductos = new List<Producto>();
        private static int nextId = 1; // Contador para asignar IDs únicos
        private static readonly TiendaManager manager = TiendaManager.Instance;
        static void Main(string[] args)
        {
            // Rellenar con algunos datos iniciales (opcional)
            manager.AgregarProducto(ProductoFactory.CrearProducto(
            manager.NextId, "Electronico", "Laptop", 1200.50m,
            new Dictionary<string, object> { { "Marca", "TechBrand" } }));

            manager.AgregarProducto(ProductoFactory.CrearProducto(
            manager.NextId, "Electronico", "Mouse Gamer", 45.99m,
            new Dictionary<string, object> { { "Marca", "GamingCo" } }));

            manager.AgregarProducto(ProductoFactory.CrearProducto(
            manager.NextId, "Alimento", "Pan Integral", 2.50m,
            new Dictionary<string, object> { { "Caducidad", DateTime.Today.AddDays(10) } }));


            bool running = true;
            while (running)
            {
                MostrarMenu();
                string opcion = Console.ReadLine();
                Console.WriteLine("-----------------------------------");

                switch (opcion)
                {
                    case "1":
                        CrearProducto(); // CREATE
                        break;
                    case "2":
                        LeerProductos(); // READ
                        break;
                    // ... (Otras opciones 3, 4, 5)
                    case "3":
                        break;

                        case "4":
                            break;


                    case "5": // NUEVA OPCIÓN
                        MostrarResumenPrecios();
                        break;
                    case "6": // OPCIÓN DE SALIR MOVIDA
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Intente de nuevo.");
                        break;
                }
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
            Console.WriteLine("Programa finalizado. ¡Hasta luego!");
        }

        // --- Métodos de Operación Modificados ---

        private static void CrearProducto()
        {
            Console.WriteLine("--- CREAR NUEVO PRODUCTO ---");

            Console.Write("Ingrese el Tipo (Electronico/Alimento): ");
            string tipo = Console.ReadLine();

            Console.Write("Ingrese el Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Ingrese el Precio: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal precio) || precio <= 0)
            {
                Console.WriteLine("❌ Error: Precio no válido.");
                return;
            }

            // Recopilar detalles específicos para el Factory
            var detalles = new Dictionary<string, object>();

            try
            {
                if (tipo.ToLower() == "electronico")
                {
                    Console.Write("Ingrese la Marca: ");
                    detalles.Add("Marca", Console.ReadLine());
                }
                else if (tipo.ToLower() == "alimento")
                {
                    Console.Write("Ingrese Fecha de Caducidad (yyyy-MM-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime caducidad))
                    {
                        detalles.Add("Caducidad", caducidad);
                    }
                    else
                    {
                        Console.WriteLine("⚠️ Advertencia: Formato de fecha no válido. Usando fecha por defecto.");
                    }
                }

                // Llamada al Factory usando el ID del Singleton
                Producto nuevoProducto = ProductoFactory.CrearProducto(manager.NextId, tipo, nombre, precio, detalles);
                manager.AgregarProducto(nuevoProducto);

                Console.WriteLine($"✅ Producto creado con éxito: {nuevoProducto.ToString()}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"❌ Error al crear producto: {ex.Message}");
            }
        }

        private static void LeerProductos()
        {
            var lista = manager.ListaProductos;
            Console.WriteLine($"--- LISTA DE PRODUCTOS ({lista.Count} en total) ---");
            if (lista.Count == 0)
            {
                Console.WriteLine("¡No hay productos registrados!");
                return;
            }

            foreach (var producto in lista)
            {
                Console.WriteLine(producto.ToString());
            }
        }




        // --- NUEVO MÉTODO ---
        private static void MostrarResumenPrecios()
        {
            var lista = manager.ListaProductos;
            Console.WriteLine("--- RESUMEN DE PRECIOS FINALES (Impuestos/Descuentos) ---");
            if (lista.Count == 0)
            {
                Console.WriteLine("¡No hay productos registrados!");
                return;
            }

            foreach (var producto in lista)
            {
                // La magia del Polimorfismo: llama al método específico de Electronico o Alimento
                decimal precioFinal = producto.CalcularPrecioFinal();

                Console.WriteLine($"Producto: {producto.Nombre} ({producto.Tipo})");
                Console.WriteLine($"  Precio Base: {producto.Precio:C}");
                Console.WriteLine($"  Precio Final: {precioFinal:C}");
                Console.WriteLine("-----------------------------------");
            }
        }


        // Los métodos ActualizarProducto y EliminarProducto deberían
        // modificarse para usar manager.ObtenerProductoPorId() y operar
        // directamente en manager.ListaProductos.
        // ...
        private static void MostrarMenu()
        {
            Console.WriteLine("===== 🛍️ Gestor de Productos CRUD (Factory + Singleton) =====");
            Console.WriteLine("1. Crear Producto");
            Console.WriteLine("2. Listar Productos");
            Console.WriteLine("3. Actualizar Producto (PENDIENTE)");
            Console.WriteLine("4. Eliminar Producto (PENDIENTE)");
            Console.WriteLine("5. Mostrar Resumen de Precios Finales 💰"); // NUEVA LÍNEA
            Console.WriteLine("6. Salir"); // OPCIÓN MOVEMOS A 6
            Console.Write("Seleccione una opción: ");
            Console.Write("Seleccione una opción: ");
        }
    }
}