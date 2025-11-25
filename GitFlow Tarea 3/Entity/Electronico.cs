
using GitFlow_Tarea_3.Base_Entity;
namespace GitFlow_Tarea_3.Entity
{
    public class Electronico : Producto
    {
        public override string Tipo => "Electrónico"; // Implementación del tipo
        public string Marca { get; set; }
        private const decimal ImpuestoTasa = 0.15m;


        public Electronico(int id, string nombre, decimal precio, string marca)
            : base(id, nombre, precio)
        {
            Marca = marca;
        }

        public override decimal CalcularPrecioFinal()
        {
            // Precio base + (Precio base * Impuesto)
            return Precio * (1 + ImpuestoTasa);
        }
        public override string ToString()
        {
            // Agregamos el campo específico de esta subclase
            return $"{base.ToString()}, Marca: {Marca}";
        }
    }

}
