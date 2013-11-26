using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MvcDynamicForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MVCDynamicForms.DBLayer
{
    public class MongoDBLayer : IDBLayer
    {
        static string databaseName = ConfigurationManager.AppSettings["mongoDatabase"];
        static MongoServer server = null;
        MongoDatabase db = null;
        static MongoDBLayer()
        {
            server = GetConnection();

            BsonClassMap.RegisterClassMap<Form>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(x => x.ContentId);
                cm.MapProperty(x => x.InputFields);
                cm.MapProperty(x => x.Fields);
                cm.MapProperty(x => x.FieldPrefix);
                cm.MapProperty(x => x.Serialize);
                cm.MapProperty(x => x.Template);
            });

            BsonClassMap.RegisterClassMap<FormData>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(x => x.ContentId);
                cm.MapProperty(x => x.Content);
            });
        }
        private static MongoServer GetConnection()
        {
            MongoServerSettings settings = new MongoServerSettings();
            settings.MaxConnectionLifeTime = new TimeSpan(0, 0, 600);
            settings.ConnectTimeout = new TimeSpan(0, 0, 600);
            settings.MaxConnectionIdleTime = new TimeSpan(0, 0, 600);
            settings.WaitQueueTimeout = new TimeSpan(0, 0, 600);
            settings.SocketTimeout = new TimeSpan(0, 0, 600);
            settings.WriteConcern = WriteConcern.Acknowledged;
            settings.ReadPreference = ReadPreference.PrimaryPreferred;
            settings.ConnectionMode = ConnectionMode.Direct;
            settings.Server = new MongoServerAddress(ConfigurationManager.AppSettings["mongoServer"], 27017);
            settings.MinConnectionPoolSize = 1;
            settings.MaxConnectionPoolSize = 500;

            MongoServer server = new MongoServer(settings);

            return server;
        }
        public MongoDBLayer()
        {
            db = server.GetDatabase(databaseName);
        }

        public void Save<T>(T val_) where T : ContentBase
        {
            var coll = db.GetCollection<T>(typeof(T).ToString());
            var writeConcernResult = coll.Save<T>(val_);
        }

        public T Get<T>(Guid id_) where T : ContentBase
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(Guid id_) where T : ContentBase
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T val_) where T : ContentBase
        {
            throw new NotImplementedException();
        }
    }
}
