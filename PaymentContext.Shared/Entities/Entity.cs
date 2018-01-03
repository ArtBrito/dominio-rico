using System;
using System.Collections.Generic;
using Flunt.Notifications;

namespace PaymentContext.Shared.Entities {
    
    public abstract class Entity : Notifiable {

        public Guid Id { get; private set; }

        public Entity(){
            this.Id = Guid.NewGuid();
        }

    }

}