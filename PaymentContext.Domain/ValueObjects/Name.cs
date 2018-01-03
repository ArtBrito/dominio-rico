using Flunt.Validations;
using PaymentContext.Shared.ValueObjects;

namespace PaymentContext.Domain.ValueObjects {
    
    public class Name : ValueObject {

        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public Name(string firstName, string lastName) {
            this.FirstName = firstName;
            this.LastName = lastName;

            this.AddNotifications(new Contract()
                .Requires()
                .HasMinLen(FirstName, 3, "Name.FirstName", "O nome deve conter pelo menos 3 caracteres.")
                .HasMinLen(LastName, 3, "Name.LastName", "O sobrenome deve conter pelo menos 3 caracteres.")
                .HasMaxLen(FirstName, 40, "Name.FirstName", "O nome deve conter até 40 caracteres.")
            );

        }

        public override string ToString(){
            return $"{this.FirstName} {this.LastName}";
        }

    }

}