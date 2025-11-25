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
                  
                    case "3":
                            ActualizarProducto(); 
                            break;
                        
                            case "4":
                            EliminarProducto();
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

          private static void ActualizarProducto()
  {
      Console.WriteLine("--- ACTUALIZAR PRODUCTO ---");
      var lista = manager.ListaProductos;

      if (lista.Count == 0)
      {
          Console.WriteLine("No hay productos para actualizar.");
          return;
      }

      Console.Write("Ingrese el ID del producto a actualizar: ");
      if (!int.TryParse(Console.ReadLine(), out int id))
      {
          Console.WriteLine("❌ Error: ID no válido.");
          return;
      }

      Producto productoAActualizar = manager.ObtenerProductoPorId(id);

      if (productoAActualizar == null)
      {
          Console.WriteLine($"❌ Error: Producto con ID {id} no encontrado.");
          return;
      }

      Console.WriteLine($"\nProducto actual: {productoAActualizar.ToString()}");
      Console.WriteLine("-----------------------------------");

      // --- 1. Actualizar Nombre ---
      Console.Write($"Ingrese el NUEVO Nombre ({productoAActualizar.Nombre}): ");
      string nuevoNombre = Console.ReadLine();
      if (!string.IsNullOrWhiteSpace(nuevoNombre))
      {
          productoAActualizar.Nombre = nuevoNombre;
      }

      // --- 2. Actualizar Precio ---
      Console.Write($"Ingrese el NUEVO Precio ({productoAActualizar.Precio:C}): ");
      string inputPrecio = Console.ReadLine();
      if (!string.IsNullOrWhiteSpace(inputPrecio))
      {
          if (decimal.TryParse(inputPrecio, out decimal nuevoPrecio) && nuevoPrecio > 0)
          {
              productoAActualizar.Precio = nuevoPrecio;
          }
          else
          {
              Console.WriteLine("⚠️ Advertencia: El precio ingresado no es válido. Se mantuvo el precio anterior.");
          }
      }

      // --- 3. Actualizar Campos Específicos (Polimorfismo) ---
      // Usamos 'is' para verificar el tipo y 'as' para hacer el casting

      if (productoAActualizar is Electronico electronico) // Verifica si es Electrónico
      {
          Console.Write($"Ingrese la NUEVA Marca ({electronico.Marca}): ");
          string nuevaMarca = Console.ReadLine();
          if (!string.IsNullOrWhiteSpace(nuevaMarca))
          {
              electronico.Marca = nuevaMarca;
          }
      }
      else if (productoAActualizar is Alimento alimento) // Verifica si es Alimento
      {
          Console.Write($"Ingrese NUEVA Caducidad ({alimento.FechaCaducidad.ToShortDateString()} - yyyy-MM-dd): ");
          string inputCaducidad = Console.ReadLine();
          if (!string.IsNullOrWhiteSpace(inputCaducidad))
          {
              if (DateTime.TryParse(inputCaducidad, out DateTime nuevaCaducidad))
              {
                  alimento.FechaCaducidad = nuevaCaducidad;
              }
              else
              {
                  Console.WriteLine("⚠️ Advertencia: Formato de fecha no válido. Se mantuvo la fecha anterior.");
              }
          }
      }

      Console.WriteLine($"✅ Producto actualizado con éxito. Nuevo estado: {productoAActualizar.ToString()}");
  }




 private static void EliminarProducto()
 {
     Console.WriteLine("--- ELIMINAR PRODUCTO ---");
     var lista = manager.ListaProductos;

     if (lista.Count == 0)
     {
         Console.WriteLine("No hay productos para eliminar.");
         return;
     }

     Console.Write("Ingrese el ID del producto a eliminar: ");
     if (!int.TryParse(Console.ReadLine(), out int idAEliminar))
     {
         Console.WriteLine("❌ Error: ID no válido.");
         return;
     }

     // Usamos el método RemoveAll de List<T> que es muy eficiente
     // y devuelve la cantidad de elementos eliminados.
     int productosEliminados = lista.RemoveAll(p => p.Id == idAEliminar);

     if (productosEliminados > 0)
     {
         Console.WriteLine($"✅ Producto con ID {idAEliminar} eliminado con éxito.");

         // Nota: En una aplicación real, deberías reasignar IDs 
         // o asegurar que el NextId no se reutilice, pero para un CRUD básico está bien.
     }
     else
     {
         Console.WriteLine($"❌ Error: Producto con ID {idAEliminar} no encontrado.");
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
            Console.WriteLine("3. Actualizar Producto");
            Console.WriteLine("4. Eliminar Producto");
            Console.WriteLine("5. Mostrar Resumen de Precios Finales 💰"); // NUEVA LÍNEA
            Console.WriteLine("6. Salir"); // OPCIÓN MOVEMOS A 6
            Console.Write("Seleccione una opción: ");
            Console.Write("Seleccione una opción: ");
        }
    }

}
