using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests {

    [TestClass]
    public class SubscriptionHandlerTests {

        //Red, Green, Refactor
        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExists() {
            
            var handler = new SubscriptionHandler(repository: new FakeStudentRepository(), emailService: new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand();

            command.FirstName = "Arthur";
            command.LastName = "Silvestre de Brito Viana";        
            command.Document = "99999999999";
            command.Email = "art.brito@outlook.com";
            command.BarCode = "132456789";
            command.BoletoNumber = "1324654987";
            command.PaymentNumber = "123121";
            command.PaidDate = DateTime.Now;
            command.ExpireDate = DateTime.Now.AddMonths(1);
            command.Total = 60;
            command.TotalPaid = 60;
            command.Payer = "Arthur Silvestre";
            command.PayerDocument = "12345678911";
            command.PayerDocumentType = EDocumentType.CPF;
            command.PayerEmail = "arthursilvestre@outlook.com";
            //command.Address = "";
            command.Street = "Rua Anísio de Azevedo Lima";
            command.Number = "232";
            command.Neighborhood = "Mangabeira 2";
            command.City = "João Pessoa";
            command.State = "PB";
            command.Country = "BR";
            command.ZipCode = "58057270";

            handler.Handle(command: command);

            Assert.AreEqual(false, handler.Valid);

        }

    }

}
