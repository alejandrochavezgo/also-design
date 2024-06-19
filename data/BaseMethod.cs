namespace data;

internal abstract class baseMethod<C, E> where C : baseMethod<C, E>, new()
{
    private static C newObject;

    private static object locker = new object();

    private static void init()
    {
        lock (locker)
        {
            if (newObject == null)
                newObject = new C();
        }
    }

    public static E get(System.Data.IDataReader dr)
    {
        try
        {
            E entity;

            if (!dr.Read())
                return default(E);

            entity = getNotClose(dr);

            return entity;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            dr.Close();
        }
    }

    public static E getNotRead(System.Data.IDataReader dr)
    {
        try
        {
            E entity = getNotClose(dr);

            return entity;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static List<E> getList(System.Data.IDataReader dr)
    {
        List<E> list = new List<E>();

        while (dr.Read())
            list.Add(getNotClose(dr));

        dr.Close();

        return list;
    }

    protected static E getNotClose(System.Data.IDataReader dr)
    {
        init();

        E entity = newObject._getEntity(dr);

        return entity;
    }

    protected abstract E _getEntity(System.Data.IDataReader dr);
}