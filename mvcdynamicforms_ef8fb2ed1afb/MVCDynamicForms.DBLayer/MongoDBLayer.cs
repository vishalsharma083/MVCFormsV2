using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MVCDynamicForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MVCDynamicForms.DBLayer
{
    public class MongoDBLayer : IDBLayer
    {
        static string _databaseName = ConfigurationManager.AppSettings["mongoDatabase"];
        static MongoServer _server = null;
        static MongoDatabase _db = null;
        static MongoDBLayer()
        {
            _server = GetConnection();
            BsonClassMap.RegisterClassMap<ContentBase>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                cm.IdMemberMap.SetIdGenerator(BsonObjectIdGenerator.Instance);
                cm.MapMember(x => x.ContentId);
            });

            BsonClassMap.RegisterClassMap<Form>(cm =>
            {
                cm.AutoMap();
                cm.UnmapProperty(x => x.InputFields);
                cm.MapProperty(x => x.Fields);
                cm.MapProperty(x => x.FieldPrefix);
                cm.MapProperty(x => x.Serialize);
                cm.MapProperty(x => x.Template);                
            });


            BsonClassMap.RegisterClassMap<FormData>(cm =>
            {
                cm.AutoMap();
                cm.MapProperty(x => x.Content);
            });

            BsonClassMap.RegisterClassMap<Site>(cm =>
            {
                cm.AutoMap();
            });

            _db = _server.GetDatabase(_databaseName);
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

        }

        public void Save<T>(T val_) where T : ContentBase
        {
            var coll = _db.GetCollection<T>(typeof(T).ToString());
            var writeConcernResult = coll.Save<T>(val_);
        }

        public T Get<T>(Guid id_) where T : ContentBase
        {
            var result = _db.GetCollection<T>(typeof(T).ToString()).Find(Query.EQ("ContentId", id_));
            if (result != null && result.Count() > 0)
            {
                return result.Cast<T>().FirstOrDefault<T>();
            }
            else
            {
                return null;
            }
        }

        public void Delete<T>(Guid id_) where T : ContentBase
        {
            var writeConcernResult = _db.GetCollection<T>(typeof(T).ToString()).Remove(Query.EQ("ContentId", id_));
        }


        public List<T> GetByTag<T>(string tag_) where T : ContentBase
        {
            return GetByTagAndContentId<T>(Guid.Empty, tag_);
        }

        public List<T> GetByTagAndContentId<T>(Guid id_, string tag_) where T : ContentBase
        {
            var collection = _db.GetCollection<T>(typeof(T).ToString());
            CommandDocument textSearchCommand;
            if (id_ != Guid.Empty)
            {
                textSearchCommand = new CommandDocument
                {
                    {"text",collection.Name},
                    {"search",tag_},
                    {"filter",BsonValue.Create(Query.EQ("ContentId", id_))}
                };
            }
            else
            {
                textSearchCommand = new CommandDocument
                {
                    {"text",collection.Name},
                    {"search",tag_},
                };
            }

            var commandResult = collection.Database.RunCommand(textSearchCommand);
            // TODO : Check for commandresult here before looping on response

            List<T> results = new List<T>();
            foreach (BsonDocument doc in commandResult.Response["results"].AsBsonArray)
            {
                results.Add(BsonSerializer.Deserialize<T>(doc["obj"] as BsonDocument));
            }

            return results;
        }


        public List<T> GetAll<T>() where T : ContentBase
        {
            List<T> retVal = new List<T>();
            MongoCollection collection = _db.GetCollection<T>(typeof(T).ToString());
            MongoCursor<T> mongoCursor = collection.FindAllAs<T>();
            using (var enumerator = mongoCursor.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    retVal.Add(enumerator.Current);
                }
            }
            return retVal;
        }
    }

}
