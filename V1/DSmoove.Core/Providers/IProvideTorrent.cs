using DSmoove.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Providers
{
    public interface IProvideTorrent
    {
        Task<Metadata> GetMetadataAsync();
    }
}