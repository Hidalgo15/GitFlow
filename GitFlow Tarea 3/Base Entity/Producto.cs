
namespace GitFlow_Tarea_3.Base_Entity
{
    public abstract class Producto
    {
        public int Id { get; protected set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public  abstract string Tipo { get; } // Propiedad abstracta

        protected Producto(int id, string nombre, decimal precio)
        {
            Id = id;
            Nombre = nombre;
            Precio = precio;
        }

        public abstract decimal CalcularPrecioFinal(); // <--- NUEVO MÉTODO
        public override string ToString()
        {
            return $"ID: {Id}, Tipo: {Tipo}, Nombre: {Nombre}, Precio: {Precio:C}";
        }
    }
}
