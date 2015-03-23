using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Helpers
{
   public class AsyncSubscription<Data, Source>
    {
       private List<Action<Data, Source>> _subscriptions;

       public AsyncSubscription()
       {
           _subscriptions = new List<Action<Data, Source>>();
       }

       public void Subscribe(Action<Data, Source> action)
       {
           _subscriptions.Add(action);
       }

       public void Unsubscribe(Action<Data, Source> action)
       {
           _subscriptions.Remove(action);
       }

       public void Trigger(Data data, Source source)
       {
           foreach (var subscription in _subscriptions)
           {
               subscription.Invoke(data, source);
           }
       }

       public Task TriggerAsync(Data data, Source source)
       {
           return Task.Factory.StartNew(() =>
           {
               Trigger(data, source);
           });
       }
    }
}
