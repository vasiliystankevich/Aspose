using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Dal
{
    public class LocalDbSet<T> : DbSet<T>, IDbSet<T> where T : class
    {
        public LocalDbSet()
        {
            lock (LockObject)
            {
                if (Elements != null)
                {
                    Elements.Clear();
                    Elements = null;
                }   
                Elements = new List<T>(CountLocalElements);
            }
        }

        public override T Find(params object[] keyValues)
        {
            throw new NotImplementedException("Derive from MockDbSet<T> and override Find");
        }

        public override T Add(T item)
        {
            lock (LockObject)
            {
                Elements.Add(item);
            }
            return item;
        }

        public override T Remove(T item)
        {
            lock (LockObject)
            {
                Elements.Remove(item);
            }
            return item;
        }

        public override T Attach(T item)
        {
            lock (LockObject)
            {
                Elements.Add(item);
            }
            return item;
        }

        public T Detach(T item)
        {
            lock (LockObject)
            {
                Elements.Remove(item);
            }
            return item;
        }

        public override T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public new TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public override IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            lock (LockObject)
            {
                Elements.AddRange(entities);
            }
            return this;
        }

        public override IEnumerable<T> RemoveRange(IEnumerable<T> entities)
        {
            entities.ToList().ForEach(el =>
            {
                lock (LockObject)
                {
                    if (!Elements.Contains(el))
                        Remove(el);
                }
            });
            return this;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (LockObject)
            {
                return Elements.GetEnumerator();
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            lock (LockObject)
            {
                return Elements.GetEnumerator();
            }
        }

        public List<T> GetLocal()
        {
            return Enumerable.Empty<T>().ToList();
        }

        Type IQueryable.ElementType => Elements.AsQueryable().ElementType;

        Expression IQueryable.Expression => Elements.AsQueryable().Expression;

        IQueryProvider IQueryable.Provider => Elements.AsQueryable().Provider;
        private static readonly int CountLocalElements = 65535;
        private List<T> Elements { get; }
        private static readonly object LockObject = new object();
    }
}
