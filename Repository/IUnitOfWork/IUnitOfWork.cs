using System;
using System.Data;

namespace Repository.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();

        void Dispose(bool disposing);

        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);

        bool Commit();

        void Rollback();

        void ResetObjectState();
    }
}