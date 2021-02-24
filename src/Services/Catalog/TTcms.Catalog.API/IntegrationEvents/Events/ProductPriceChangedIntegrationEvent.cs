namespace TTcms.Catalog.API.IntegrationEvents.Events
{
    using BuildingBlocks.EventBus.Events;

    //集成事件
    //一个事件是“过去发生的事情”，因此它的名字必须是
    //一个集成事件是一个可能对其他微服务、边界上下文或外部系统产生副作用的事件。
    public class ProductPriceChangedIntegrationEvent : IntegrationEvent
    {        
        public int ProductId { get; private set; }

        public decimal NewPrice { get; private set; }

        public decimal OldPrice { get; private set; }

        public ProductPriceChangedIntegrationEvent(int productId, decimal newPrice, decimal oldPrice)
        {
            ProductId = productId;
            NewPrice = newPrice;
            OldPrice = oldPrice;
        }
    }
}