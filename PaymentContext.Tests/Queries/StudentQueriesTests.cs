using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.Domain.Queries;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests {

    [TestClass]
    public class StudentQueriesTests {

        private IList<Student> _students;

        public StudentQueriesTests() {
            
            for (int i = 0; i < 10; i++) {
                this._students.Add(new Student(
                    name: new Name("Aluno", i.ToString()),
                    document: new Document(number: "1111111111"+i.ToString(), type: EDocumentType.CPF),
                    email: new Email(address: $"{i}@outlook.com")
                ));
            }

        }

        [TestMethod]
        public void ShouldReturnNullWhenDocumentNotExists() {
            
            var exp = StudentQueries.GetStudentInfo("12345678911");
            var student = _students.AsQueryable().Where(exp).FirstOrDefault();

            Assert.AreEqual(null, student);

        }

        [TestMethod]
        public void ShouldReturnNullWhenDocumentExists() {
            
            var exp = StudentQueries.GetStudentInfo("11111111111");
            var student = _students.AsQueryable().Where(exp).FirstOrDefault();

            Assert.AreNotEqual(null, student);

        }

    }

}
