using Karmr.Common.Contracts;
using Karmr.Domain.Events;

namespace Karmr.Domain.Denormalizers
{
    internal sealed class ListingOfferDenormalizer : Denormalizer
    {
        internal ListingOfferDenormalizer(IDenormalizerRepository repository) : base(repository)
        {
        }

        private void Apply(ListingOfferCreated @event)
        {
            const string sql = @"INSERT INTO ReadModel.ListingOffer ([ListingId], [UserId], [Accepted], [Created]) VALUES (@ListingId, @UserId, 0, @Created)";
            var @params = new
            {
                @event.EntityKey,
                @event.UserId,
                @event.Timestamp
            };
            this.Repository.Execute(sql, @params);
        }
    }
}