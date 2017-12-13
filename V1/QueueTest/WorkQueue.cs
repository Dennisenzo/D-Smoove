using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueTest
{
    public class WorkQueue<T>
    {
        private ConcurrentQueue<T> _innerQueue;



        public WorkQueue()
        {
            _innerQueue = new ConcurrentQueue<T>();
        }

        public void Enqueue()
        {

        }
    }
}
