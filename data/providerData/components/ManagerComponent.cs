namespace data.providerData.components;

using data.providerData.providers;

internal sealed class managerComponents
{
    #region Variable para objeto Singleton
    public static commonConsumer genCon = null;
    #endregion

    #region Varibles para historico de url's de conexion
    public static string olderUrlCommonConn = string.Empty;
    #endregion

    private managerComponents() {}

    /// <summary>
    /// Referencia a nulo de un componente Generico indicado por un Proveedor
    /// </summary>
    public static void disposeComponent()
    {
        if (managerComponents.genCon != null)
        {
            managerComponents.genCon = null;
            managerComponents.olderUrlCommonConn = string.Empty;
        }
        else
        {
            throw new Exception("No se puede destruir el objeto \"DataConsumer\" ya que no se ha creado una instancia del mismo. Y su estado estado actual es nulo.");
        }
    }
}