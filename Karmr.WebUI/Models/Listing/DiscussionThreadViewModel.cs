using System;
using System.Collections.Generic;
using System.Linq;

namespace Karmr.WebUI.Models.Listing
{
    public sealed class DiscussionThreadViewModel
    {
        public Guid ThreadId { get; }

        public Guid UserId { get; }

        public IEnumerable<DiscussionPostViewModel> Posts { get; }

        public DiscussionThreadViewModel(Domain.Queries.Models.DiscussionThread discussionThread)
        {
            ThreadId = discussionThread.ThreadId;
            UserId = discussionThread.UserId;
            Posts = discussionThread.Posts.Select(x => new DiscussionPostViewModel(x));
        }
    }
}