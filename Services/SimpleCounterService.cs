using ASPNetCore_MongoIdentity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetCore_MongoIdentity.Services
{
    public interface ISimpleCounterService
    {
        int GetCounter();


    }

    public static class SimpleCounterService
    {
        private static SimpleCounter simpleCounter = new SimpleCounter();
        public static int GetCounter()
        {
            if (simpleCounter == null)
            {
                simpleCounter = new SimpleCounter();
            }
            return (simpleCounter.counter++);
        }
    }
}
