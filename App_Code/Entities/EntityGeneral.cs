using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using EntityModel;
/// <summary>
/// Descripción breve de General
/// </summary>
namespace EntityModel
{
    public abstract class EntityGeneral
    {
        public static TransparenciaEntities Entity = new TransparenciaEntities();
        private static DbContextTransaction DbTransaction { get; set; }

        public static void BeginTransaction()
        {
            try
            {
                DbTransaction = Entity.Database.BeginTransaction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CommitTransaction()
        {
            try
            {
                if (DbTransaction != null)
                {
                    DbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DisposeTransaction()
        {
            try
            {
                if (DbTransaction != null)
                {
                    DbTransaction.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void RollBackTransaction()
        {
            try
            {
                if (DbTransaction != null)
                {
                    DbTransaction.Rollback();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}