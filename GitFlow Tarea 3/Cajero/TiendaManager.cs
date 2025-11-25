using GitFlow_Tarea_3.Base_Entity;
using GitFlow_Tarea_3.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitFlow_Tarea_3.Cajero
{
    public sealed class TiendaManager
    {
        // 1. Instancia estática (el único objeto permitido)
        private static readonly TiendaManager instance = new TiendaManager();

        // 2. Propiedad pública para acceder a la instancia
        public static TiendaManager Instance
        {
            get { return instance; }
        }

        // 3. Constructor privado para evitar instanciación externa
        private TiendaManager()
        {
            // Inicialización de la "base de datos"
            ListaProductos = new List<Producto>();
            NextId = 1;

            // Datos de prueba
            ListaProductos.Add(ProductoFactory.CrearProducto(NextId++, "Electronico", "Smartphone X", 799.00m, new Dictionary<string, object> { { "Marca", "TechCo" } }));
            ListaProductos.Add(ProductoFactory.CrearProducto(NextId++, "Alimento", "Pan Integral", 2.50m, new Dictionary<string, object> { { "Caducidad", DateTime.Today.AddDays(10) } }));
        }

        // --- Lógica de la Tienda (Inventario y Contador) ---
        public List<Producto> ListaProductos { get; private set; }
        public int NextId { get; private set; }

        public void AgregarProducto(Producto producto)
        {
            ListaProductos.Add(producto);
            NextId++; // Incrementa el contador global de IDs
        }

        public Producto ObtenerProductoPorId(int id)
        {
            return ListaProductos.FirstOrDefault(p => p.Id == id);
        }

        // Aquí puedes añadir más lógica de negocio, como calcular impuestos,
        // o manejar ventas.
    }
}
