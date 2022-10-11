using System;

namespace Contracts
{
    public record SubmitOrder
    {
        public Guid OrderId { get; init; }
    }
}
