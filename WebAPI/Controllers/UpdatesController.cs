using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UpdatesController : ControllerBase
    {
        private readonly ILogger<UpdatesController> _logger;
        private readonly IUpdateService _updateService;

        public UpdatesController(ILogger<UpdatesController> logger, 
                                 IUpdateService updateService)
        {
            _logger = logger;
            _updateService = updateService;
        }

        [HttpPost]
        public Task PushUpdates([FromBody] Update update)
        {
            _logger.LogInformation($"Update {update.UpdateId} recived\t:{DateTime.UtcNow}");
            return _updateService.HandleUpdate(update);
        }
    }
}
