

using GitFlow_Tarea_3.Base_Entity;

namespace GitFlow_Tarea_3.Entity

{
    public class Alimento : Producto
    {
        public override string Tipo => "Alimento"; // Implementación del tipo
        public DateTime FechaCaducidad { get; set; }

        private const decimal DescuentoTasa = 0.10m; // 10% de descuento
        public Alimento(int id, string nombre, decimal precio, DateTime caducidad)
            : base(id, nombre, precio)
        {
            FechaCaducidad = caducidad;
        }

        // Implementación del método abstracto: Aplica el descuento
        public override decimal CalcularPrecioFinal()
        {
            // Precio base - (Precio base * Descuento)
            return Precio * (1 - DescuentoTasa);
        }
        public override string ToString()
        {
            // Agregamos el campo específico de esta subclase
            return $"{base.ToString()}, Caduca: {FechaCaducidad.ToShortDateString()}";
        }
    }
}
