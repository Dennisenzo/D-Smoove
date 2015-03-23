using DSmoove.Core.Entities;
using DSmoove.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Interfaces
{
public interface IProvideTrackerUpdates
    {
    AsyncSubscription<TrackerData, IProvideTrackerUpdates> TrackerUpdateSubscription { get; }
    }
}
