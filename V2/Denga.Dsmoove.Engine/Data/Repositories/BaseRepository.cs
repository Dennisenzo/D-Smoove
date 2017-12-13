using System;
using System.IO;
using Denga.Dsmoove.Engine.Data.Entities;
using LiteDB;

namespace Denga.Dsmoove.Engine.Data.Repositories
{
    public abstract class BaseRepository<T> : IDisposable where T: BaseEntity
    {
        private bool isDisposed = false;

        protected readonly LiteDatabase db;

        protected BaseRepository()
        {
            Directory.CreateDirectory(StaticSettings.BaseDataPath);
            db = new LiteDatabase(Path.Combine(StaticSettings.BaseDataPath, typeof(T).Name + ".dat"));
        }

        protected abstract void Initialize();

        protected LiteDatabase GetDb()
        {
            return db;
        }

        protected LiteCollection<T> GetCollection()
        {
            return db.GetCollection<T>();
        }

        public virtual void Save(T item)
        {
            GetCollection().Upsert(item);
        }

        public T ById(int id)
        {
           return GetCollection().FindOne(i => i.Id == id);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }
            if (disposing)
            {
                db.Dispose();
            }
            isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}