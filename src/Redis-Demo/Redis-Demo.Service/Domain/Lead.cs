using System;

namespace Redis_Demo.Service.Domain
{
    public class Lead
    {
        public Guid Id { get; private set; }
        public string Cnpj { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }
        public string PhoneNumber { get; private set; }

        public Lead(Guid id, string cnpj, string name, int age, string phoneNumber)
        {
            Id = id;
            Cnpj = cnpj;
            Name = name;
            Age = age;
            PhoneNumber = phoneNumber;
        }
    }
}
