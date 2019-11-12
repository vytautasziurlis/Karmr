using System;

namespace Karmr.WebUI.Models.Listing
{
    public sealed class DiscussionPostViewModel
    {
        public Guid UserId { get; }

        public string Content { get; }

        public DateTime Created { get; }

        public DiscussionPostViewModel(Domain.Queries.Models.DiscussionPost discussionPost)
        {
            UserId = discussionPost.UserId;
            Content = discussionPost.Content;
            Created = discussionPost.Created;
        }
    }
}