using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TTcms.BuildingBlocks.EventBus.Abstractions;
using TTcms.BuildingBlocks.EventBus.Events;
using TTcms.BuildingBlocks.IntegrationEventLogEF.Services;
using TTcms.BuildingBlocks.IntegrationEventLogEF.Utilities;
using TTcms.Catalog.API;
using TTcms.Catalog.API.Infrastructure;
using Microsoft.Extensions.Logging;

using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace TTcms.Catalog.API.IntegrationEvents
{
    public class CatalogIntegrationEventService : ICatalogIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly CatalogContext _catalogContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<CatalogIntegrationEventService> _logger;

        public CatalogIntegrationEventService(
            ILogger<CatalogIntegrationEventService> logger,
            IEventBus eventBus,
            CatalogContext catalogContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_catalogContext.Database.GetDbConnection());
        }

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            try
            {
                _logger.LogInformation("----- 发布集成事件:{IntegrationEventId_published} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);

                await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
                _eventBus.Publish(evt);
                await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "错误发布集成事件:{IntegrationEventId}从{AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);
                await _eventLogService.MarkEventAsFailedAsync(evt.Id);
            }
        }

        public async Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- CatalogIntegrationEventService -保存更改和integrationEvent: {IntegrationEventId}", evt.Id);

            //在一个显式的BeginTransaction()中使用多个DbContexts时，EF核心弹性策略的使用:
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
            await ResilientTransaction.New(_catalogContext).ExecuteAsync(async () =>
            {
                //通过本地事务实现原始编目数据库操作和IntegrationEventLog之间的原子性
                await _catalogContext.SaveChangesAsync();
                await _eventLogService.SaveEventAsync(evt, _catalogContext.Database.CurrentTransaction);
            });
        }
    }
}
