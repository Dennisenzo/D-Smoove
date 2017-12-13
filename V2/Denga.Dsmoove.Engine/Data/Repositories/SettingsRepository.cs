using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Denga.Dsmoove.Engine.Data.Entities;
using LiteDB;

namespace Denga.Dsmoove.Engine.Data.Repositories
{
    public class SettingsRepository : BaseRepository<Settings>
    {
        public SettingsRepository() : base()
        {
        }

        protected override void Initialize()
        {

        }

        public static Settings Get()
        {
            using (var repo = new SettingsRepository())
            {
                return repo.Load();
            }
        }

        public Settings Load()
        {
            return GetCollection().FindAll().FirstOrDefault() ?? new Settings();
        }

        public override void Save(Settings settings)
        {
            GetCollection().Delete(Query.All());
            GetCollection().Insert(settings);
        }
    }
}