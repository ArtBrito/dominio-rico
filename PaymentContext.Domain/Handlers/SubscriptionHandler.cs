using System;
using Flunt.Notifications;
using Flunt.Validations;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers {

    public class SubscriptionHandler : Notifiable, IHandler<CreateBoletoSubscriptionCommand>, IHandler<CreatePayPalSubscriptionCommand> {

        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService) {
            this._repository = repository;
            this._emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command) {
            //Fail Fast Falidations
            command.Validate();

            if (command.Invalid){
               this.AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar sua assinatura");
            }

            //Verificar se o Documento já está cadastrado
            if (_repository.DocumentExists(command.Document)){
               this.AddNotification("Document", "Este CPF já está em uso");
            }

            //Verificar se o E-mail já está cadastrado
            if (_repository.EmailExists(command.Email)){
               this.AddNotification("Email", "Este Email já está em uso");
            }

            //Gerar os VOs
            var name = new Name(firstName: command.FirstName, lastName: command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            //Gerar as entidades
            var student = new Student(name: name, document: document, email: email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(barCode: command.BarCode, boletoNumber: command.BoletoNumber, paidDate: command.PaidDate, expireDate: command.ExpireDate, total: command.Total, totalPaid: command.TotalPaid, payer: command.Payer, document: new Document(number: command.PayerDocument, type: command.PayerDocumentType), address: address, email: email);

            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Agrupar as validações
            this.AddNotifications(name, document, email, address, student, subscription, payment);

            //Checar as notificações
            if (this.Invalid)
                return new CommandResult(false, "Não foi possível realizar sua assinatura");

            //Salvar as informações
            this._repository.CreateSubscription(student);

            //Enviar E-mail de boas vindas
            this._emailService.send(to: student.Name.ToString(), email: student.Email.Address, subject: "Bem vindo", body: "Sua assinatura foi realizada com sucesso.");

            //Retornar informações
            return new CommandResult(true, "Assinatura realizada com sucesso.");
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command) {
            //Verificar se o Documento já está cadastrado
            if (_repository.DocumentExists(command.Document)){
               this.AddNotification("Document", "Este CPF já está em uso");
            }

            //Verificar se o E-mail já está cadastrado
            if (_repository.EmailExists(command.Email)){
               this.AddNotification("Email", "Este Email já está em uso");
            }

            //Gerar os VOs
            var name = new Name(firstName: command.FirstName, lastName: command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            //Gerar as entidades
            var student = new Student(name: name, document: document, email: email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new PayPalPayment(transactionCode: command.TransactionCode, paidDate: command.PaidDate, expireDate: command.ExpireDate, total: command.Total, totalPaid: command.TotalPaid, payer: command.Payer, document: new Document(number: command.PayerDocument, type: command.PayerDocumentType), address: address, email: email);

            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Agrupar as validações
            this.AddNotifications(name, document, email, address, student, subscription, payment);

            //Checar as notificações
            if (this.Invalid)
                return new CommandResult(false, "Não foi possível realizar sua assinatura");

            //Salvar as informações
            this._repository.CreateSubscription(student);

            //Enviar E-mail de boas vindas
            this._emailService.send(to: student.Name.ToString(), email: student.Email.Address, subject: "Bem vindo", body: "Sua assinatura foi realizada com sucesso.");

            //Retornar informações
            return new CommandResult(true, "Assinatura realizada com sucesso.");
        }
    }

}