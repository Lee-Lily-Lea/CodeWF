using BlogWebSite.Shared;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BlogWebSite.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SiteController(IOptions<SiteInfo> siteInfoOpt) : ControllerBase
    {
        [HttpGet]
        public SiteInfo GetSiteInfo()
        {
            return siteInfoOpt.Value;
        }


    }
}
