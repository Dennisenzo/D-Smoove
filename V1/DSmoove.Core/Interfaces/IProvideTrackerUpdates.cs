using DSmoove.Core.Entities;
using DSmoove.Core.Helpers;
using DSmoove.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Interfaces
{
    public interface IProvideTrackerUpdates
    {
        AsyncSubscription<TrackerData, IProvideTrackerUpdates> UpdateSubscription { get; }
    }
}
