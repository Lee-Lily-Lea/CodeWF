using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebSite.Services.Configure
{
    public class SiteOption
    {
        /// <summary>
        ///
        /// </summary>
        public string? LocalAssetsDir { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? RemoteAssetsRepository { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int StartYear { get; set; }
    }
}
