

namespace GitFlow_Tarea_3.Base_Entity
{
    public class Alimento : Producto
    {
        public override string Tipo => "Alimento"; // Implementación del tipo
        public DateTime FechaCaducidad { get; set; }

        public Alimento(int id, string nombre, decimal precio, DateTime caducidad)
            : base(id, nombre, precio)
        {
            FechaCaducidad = caducidad;
        }

        public override string ToString()
        {
            // Agregamos el campo específico de esta subclase
            return $"{base.ToString()}, Caduca: {FechaCaducidad.ToShortDateString()}";
        }
    }
}
