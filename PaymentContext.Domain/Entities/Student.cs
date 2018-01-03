using System.Collections.Generic;
using System.Linq;
using Flunt.Validations;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Entities;

namespace PaymentContext.Domain.Entities {
    
    public class Student : Entity {

        public Name Name { get; private set; }
        public Document Document { get; private set; }
        public Email Email { get; private set; }
        public Address Address { get; private set; }
        private IList<Subscription> _subscriptions;
        public IReadOnlyCollection<Subscription> Subscriptions {
            get { return this._subscriptions.ToArray(); }
        }

        public Student(Name name, Document document, Email email) {
            this.Name = name;
            this.Document = document;
            this.Email = email;
            this._subscriptions = new List<Subscription>();

            this.AddNotifications(name, document, email);
        }

        public void AddSubscription(Subscription subscription) {
            
            var hasSubscriptionActive = false;

            foreach (Subscription current_subscription in this._subscriptions) {
                if (current_subscription.Active)
                hasSubscriptionActive =  true;
            }

            AddNotifications(new Contract()
                .Requires()
                .IsFalse(hasSubscriptionActive, "Student.Subscriptions", "Você já tem uma assinatura ativa")
                .AreEquals(0, subscription.Payments.Count, "Student.Subscription.Payment", "A assinatura não possui pagamentos")
            );

        }

    }

}