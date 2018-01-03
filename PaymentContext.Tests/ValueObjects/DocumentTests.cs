using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests {

    [TestClass]
    public class DocumentTests {

        //Red, Green, Refactor
        [TestMethod]
        public void ShouldReturnErrorWhenCNPJIsInvalid() {

            Document document = new Document("123", EDocumentType.CNPJ);
            
            Assert.IsTrue(document.Invalid);

        }

        [TestMethod]
        public void ShouldReturnSucessWhenCNPJIsValid() {

            Document document = new Document("34110468000150", EDocumentType.CNPJ);
            
            Assert.IsTrue(document.Valid);

        }        

        [TestMethod]
        public void ShouldReturnErrorWhenCPFIsInvalid() {

            Document document = new Document("123", EDocumentType.CPF);
            
            Assert.IsTrue(document.Invalid);

        }

        [TestMethod]
        [DataTestMethod]
        [DataRow("08678438452")]
        [DataRow("34225545806")]
        [DataRow("54139739347")]
        [DataRow("01077284608")]
        public void ShouldReturnSucessWhenCPFIsValid(string cpf) {

            Document document = new Document(cpf, EDocumentType.CPF);
            
            Assert.IsTrue(document.Valid);

        }        


    }

}
