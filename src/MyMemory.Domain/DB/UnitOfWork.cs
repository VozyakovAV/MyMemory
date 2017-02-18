using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class UnitOfWork : IDisposable
    {
        internal MemoryDbContext _context; // TODO: сделать по другому

        public UnitOfWork()
        {
            _context = new MemoryDbContext();
            _disposed = false;
            //SERIALIZE WILL FAIL WITH PROXIED ENTITIES
            //_db.Configuration.ProxyCreationEnabled = false;
            //ENABLING COULD CAUSE ENDLESS LOOPS AND PERFORMANCE PROBLEMS
            //_db.Configuration.LazyLoadingEnabled = false;
        }

        public IDbSet<MemoryGroup> GetGroups()
        {
            return _context.Groups;
        }

        public IDbSet<MemoryItem> GetItems()
        {
            return _context.Items;
        }

        public void Commit()
        {
             _context.SaveChanges();
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_context != null) _context.Dispose();
                }
                _context = null;
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
