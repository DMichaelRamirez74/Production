using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Common
{
    public sealed class FactoryInstance
    {

        private static readonly object padlock = new object();
        private static  FactoryInstance _factoryInstance=null;
        public static FactoryInstance Instance { get {


                if(_factoryInstance==null)
                {
                    lock(padlock)
                    {
                        if(_factoryInstance == null)
                        {
                            _factoryInstance = new FactoryInstance();
                        }

                    }
                }

                return _factoryInstance;

            } }




        public T CreateInstance<T>() where T : class
        {
            return (T)Activator.CreateInstance(typeof(T));

        }

        public T CreateInstance<T>(params object[] obj) where T:class
        {
            return (T)Activator.CreateInstance(typeof(T), obj);
        }

       //public T CreateInstance<T>(string dbNamed) where

     
    }
}
