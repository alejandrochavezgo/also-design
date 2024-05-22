/*
 * Copyright© 2017 Cross Border Xpress
 * All rights reserved.
 * Total or partial distribution is prohibited.
*/

namespace data
{
     internal abstract class BaseMethod<C, E> where C : BaseMethod<C, E>, new()
    {
        private static C NewObject;

        private static object locker = new object();

        private static void Init()
        {
            lock (locker)
            {
                if (NewObject == null)
                    NewObject = new C();
            }
        }

        public static E Get(System.Data.IDataReader dr)
        {
            try
            {
                E entity;

                if (!dr.Read())
                    return default(E);

                entity = GetNotClose(dr);

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

        public static E GetNotRead(System.Data.IDataReader dr)
        {
            try
            {
                E entity = GetNotClose(dr);

                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<E> GetList(System.Data.IDataReader dr)
        {
            List<E> list = new List<E>();

            while (dr.Read())
                list.Add(GetNotClose(dr));

            dr.Close();

            return list;
        }

        protected static E GetNotClose(System.Data.IDataReader dr)
        {
            Init();

            E entity = NewObject._GetEntity(dr);

            return entity;
        }

        protected abstract E _GetEntity(System.Data.IDataReader dr);
    }
}