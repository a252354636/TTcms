﻿using TTcms.BuildingBlocks.EventBus.Events;
using System.Threading.Tasks;

namespace TTcms.Catalog.API.IntegrationEvents
{
    public interface ICatalogIntegrationEventService
    {
        Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
