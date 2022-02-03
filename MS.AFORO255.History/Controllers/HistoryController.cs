using Aforo255.Cross.Cache.Src;
using Microsoft.AspNetCore.Mvc;
using MS.AFORO255.History.DTOs;
using MS.AFORO255.History.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS.AFORO255.History.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;
        private readonly IExtensionCache _extensionCache;

        public HistoryController(IHistoryService historyService, IExtensionCache extensionCache)
        {
            _historyService = historyService;
            _extensionCache = extensionCache;
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> Get(int accountId)
        {
            string keyHistory = $"keyHistory-{accountId}";
            IEnumerable<HistoryResponse> model = null;
            model = _extensionCache.GetData<IEnumerable<HistoryResponse>>(keyHistory);
            
            if (model == null)
            {
                var data = await _historyService.GetAll();
                model = data.Where(x => x.AccountId == accountId).ToList();
                _extensionCache.SetData(model, keyHistory, 1);
            }

            //var result = await _historyService.GetAll();
            //var model = result.Where(x => x.AccountId == accountId).ToList();
            return Ok(model);

        }
    }
}
