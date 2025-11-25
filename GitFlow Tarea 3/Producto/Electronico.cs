

namespace GitFlow_Tarea_3.Base_Entity
{
    public class Electronico : Producto
    {
        public override string Tipo => "Electrónico"; // Implementación del tipo
        public string Marca { get; set; }

        public Electronico(int id, string nombre, decimal precio, string marca)
            : base(id, nombre, precio)
        {
            Marca = marca;
        }

        public override string ToString()
        {
            // Agregamos el campo específico de esta subclase
            return $"{base.ToString()}, Marca: {Marca}";
        }
    }

}
