using System;
using System.Collections.Generic;
using System.Linq;
using Flunt.Validations;
using PaymentContext.Shared.Entities;

namespace PaymentContext.Domain.Entities {
    
    public class Subscription : Entity {

        public DateTime CreateDate { get; private set; }
        public DateTime LastUpdateDate { get; private set; }
        public DateTime? ExpireDate { get; private set; }
        public bool Active { get; private set; }
        private IList<Payment> _payments;
        public IReadOnlyCollection<Payment> Payments {
            get { return this._payments.ToArray();}
        }

        public Subscription(DateTime? expireDate) {
            this.CreateDate = DateTime.Now;
            this.LastUpdateDate = DateTime.Now;
            this.ExpireDate = expireDate;
            this.Active = true;
            this._payments = new List<Payment>();
        }

        public void AddPayment(Payment payment) {
            
            AddNotifications(new Contract()
                .Requires()
                .IsGreaterThan(DateTime.Now, payment.PaidDate, "Subscription.Payments", "A data do pagamento deve ser no futuro")
            );

            this._payments.Add(payment);
        }

        public void Activete(){
            this.Active = true;
            this.LastUpdateDate = DateTime.Now;
        }

        public void Inactivate(){
            this.Active = false;
            this.LastUpdateDate = DateTime.Now;
        }

    }

}