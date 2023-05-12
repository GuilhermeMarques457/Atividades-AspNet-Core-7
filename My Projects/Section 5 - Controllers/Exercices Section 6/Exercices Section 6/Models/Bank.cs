using Microsoft.AspNetCore.Mvc;

namespace Exercices_Section_6.Models
{
    public class Bank
    {
        //[FromQuery] we can specify if the object will be filled by route or query akakakak
        //[FromRoute]

        public int accountNumber { get; set; }

        //[FromRoute]
        public string accountHolderName { get; set; }

        //[FromRoute]
        public int currentBalance { get; set; }

        public override string ToString()
        {
            return $"Bank object - " +
                $"Account Number:{accountNumber}" +
                $"Acoount Name:{accountHolderName}" +
                $"Account Currenty Balance: {currentBalance}";
        }
    }
}
