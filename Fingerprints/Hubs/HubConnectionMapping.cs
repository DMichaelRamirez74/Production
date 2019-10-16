using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FingerprintsModel;
using System.Collections.Concurrent;

namespace Fingerprints.Hubs
{
    public class HubConnectionMapping<T>
    {

        private readonly ConcurrentDictionary<T, HashSet<AppUserState>> _connections =
            new ConcurrentDictionary<T, HashSet<AppUserState>>();


        private static readonly object padlock = new object();
        private static HubConnectionMapping<T> _connectionMappingInstance = new HubConnectionMapping<T>();
        public static HubConnectionMapping<T> Instance
        {
            get
            {
                //if (_connectionMappingInstance == null)
                //{
                //    lock (padlock)
                //    {
                //        if (_connectionMappingInstance == null)
                //        {
                //            _connectionMappingInstance = new HubConnectionMapping<T>();
                //        }

                //    }
                //}

                return _connectionMappingInstance;

            }
        }

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(T key,AppUserState appUserState)
        {
            lock (_connections)
            {
                HashSet<AppUserState> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<AppUserState>();
                    _connections.TryAdd(key, connections);
                }

                lock (connections)
                {
                    connections.Add(appUserState);
                }
            }
        }

        public IEnumerable<AppUserState> GetConnections(T key)
        {
            //HashSet<AppUserState> connections;
            //if (_connections.TryGetValue(key, out connections))
            //{
            //    return connections;
            //}

            //return Enumerable.Empty<string>();

            HashSet<AppUserState> connections;

            if(_connections.TryGetValue(key, out connections))
            {
                return connections;
            }
            else
            {
                
            }
            return new HashSet<AppUserState>();


        }

        public IEnumerable<AppUserState> GetAllConnections()
        {
            HashSet<AppUserState> appUserState = new HashSet<AppUserState>();


            if (_connections.Keys.Count > 0)
            {
                foreach (var key in _connections.Keys)
                {
                    HashSet<AppUserState> connections;

                    if (_connections.TryGetValue(key, out connections))
                    {

                        appUserState.UnionWith(connections);
                    }

                }

            }

            return appUserState;
        }

        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<AppUserState> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    // connections.Remove(appUsersate);

                    var appUserState =connections.Where(x => x.ConnectionId == connectionId).FirstOrDefault();
                    connections.Remove(appUserState);

                    if (connections.Count == 0)
                    {
                        HashSet<AppUserState> appuser=new HashSet<AppUserState>();
                        _connections.TryRemove(key,out appuser);
                    }
                }
            }
        }
    }

}