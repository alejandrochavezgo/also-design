using data.providerData.providers;

namespace data.providerData.components
{
    internal sealed class ManagerComponents
    {
        #region Variable para objeto Singleton
        public static CommonConsumer GenCon = null;
        #endregion

        #region Varibles para historico de url's de conexion
        public static string OlderUrlCommonConn = string.Empty;
        #endregion

        private ManagerComponents() {}

        /// <summary>
        /// Referencia a nulo de un componente Generico indicado por un Proveedor
        /// </summary>
        public static void DisposeComponent()
        {
            if (ManagerComponents.GenCon != null)
            {
                ManagerComponents.GenCon = null;
                ManagerComponents.OlderUrlCommonConn = string.Empty;
            }
            else
                throw new Exception("No se puede destruir el objeto \"DataConsumer\" ya que no se ha creado una instancia del mismo."
                                    + " Y su estado estado actual es nulo.");
        }
    }
}
