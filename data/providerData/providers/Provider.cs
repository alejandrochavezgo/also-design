using System.Data;
using System.Data.Common;

namespace data.providerData.providers
{
    /// <summary>
    /// Proporciona al proveedor de objetos del tipo de manejador de base de datos indicado
    /// </summary>
    ///  <Author>Juan C. Escobar Hernández</Author>
    internal sealed class Provider
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        private Provider() {; }

        /// <summary>
        /// Provee un objeto del tipo DbProviderFactory con el nombre del proveedor indicado.
        /// </summary>
        /// <returns>Un objeto del tipo DbProviderFactory</returns>
        internal static DbProviderFactory GetDbFactory(string ProviderName)
        {
            try
            {
                return DbProviderFactories.GetFactory(ProviderName);
            }
            catch (DbException)
            {
                throw new Exception(@"Ocurrio una excepción al intentar crear la fabrica del proveedor de BD. 
                                      Revise si el nombre de proveedor(ProviderName) especificado es correcto.");
            }
        }
        /// <summary>
        /// Provee un listado con los nombre de Proveedores soportados por el Framework .NET
        /// </summary>
        /// <returns>System.Data.DataTable object</returns>
        internal static DataTable GetProviders()
        {
            DataTable dtProviders = DbProviderFactories.GetFactoryClasses();

            if (dtProviders.Rows.Count == 0)
            {
                throw new Exception("No hay proveedores de datos referenciados en el FrameWork .NET"
                                    + Environment.NewLine + Environment.NewLine +
                                    "No se puede implementar la clase abstracta DbProviderFactory"
                                    + Environment.NewLine + "del Espacio de nombres System.Data.Common");
            }

            return dtProviders;
        }
    }
}