﻿namespace Karmr.Domain.Commands
{
    using System;

    public class CreateListingCommand : Command
    {
        public string Description { get; private set; }

        public CreateListingCommand(Guid userId, string description) : base(userId)
        {
            this.Description = description;
        }
    }
}