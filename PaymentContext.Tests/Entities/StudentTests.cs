using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests {

    [TestClass]
    public class StudentTests {

        private readonly Student _student;
        private readonly Subscription _subscription;
        private readonly Name _name;
        private readonly Document _document;
        private readonly Email _email;
        private readonly Address _address;

        public StudentTests() {
            // ValueObjects
            _name = new Name(firstName: "Arthur", lastName: "Silvestre");
            _document = new Document("08678438452", EDocumentType.CPF);
            _email = new Email("art.brito@outlook.com");
            _address = new Address("Rua 1", "1234", "Bairro Legal", "Gotham", "SP", "BR", "13400000");

            // Entities
            _student = new Student(name: _name, document: _document, email: _email);
            _subscription = new Subscription(null);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenActiveSubscription() {

            Payment payment = new PayPalPayment("12345678", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, "Coorp", _document, _address, _email);            

            _subscription.AddPayment(payment);            
            _student.AddSubscription(_subscription);
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Invalid);

        }

        [TestMethod]
        public void ShouldReturnSucessWhenSubscriptionHasNoPayment() {

            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Invalid);

        }


        [TestMethod]
        public void ShouldReturnSucessWhenAddSubscription() {

            Payment payment = new PayPalPayment("12345678", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, "Coorp", _document, _address, _email);            

            _subscription.AddPayment(payment);            
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Valid);

        }        

    }

}
