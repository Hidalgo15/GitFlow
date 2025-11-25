using GitFlow_Tarea_3.Base_Entity;


namespace GitFlow_Tarea_3.Factory
{
    public static class ProductoFactory
    {
        // Este método toma los parámetros comunes y decide qué tipo crear
        public static Producto CrearProducto(int id, string tipo, string nombre, decimal precio, Dictionary<string, object> detalles)
        {
            switch (tipo.ToLower())
            {
                case "electronico":
                    // Se asume que los 'detalles' contienen la "Marca"
                    string marca = detalles.ContainsKey("Marca") ? (string)detalles["Marca"] : "N/A";
                    return new Electronico(id, nombre, precio, marca);

                case "alimento":
                    // Se asume que los 'detalles' contienen la "Caducidad"
                    DateTime caducidad = detalles.ContainsKey("Caducidad") ? (DateTime)detalles["Caducidad"] : DateTime.Today.AddDays(7);
                    return new Alimento(id, nombre, precio, caducidad);

                default:
                    throw new ArgumentException($"Tipo de producto '{tipo}' no reconocido.");
            }
        }
    }
}
