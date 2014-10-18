using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _445project2.EDServiceRef;


namespace _445project2
{
    class BankService
    {
        private Boolean iscreditcardValid;
        public Boolean isccValid(int cc)
        {
            EDServiceRef.ServiceClient serviceClient = new EDServiceRef.ServiceClient();    //uses ASU decryption service

            String creditcard = cc.ToString();

            //encrypts credit card number
            String encryptedcc = serviceClient.Encrypt(creditcard);
            //decrypts credit card number
            String decryptedcc = serviceClient.Decrypt(encryptedcc);

            //checks to see if original and decrypted are equal, and then returns true or false
            if(string.Equals(decryptedcc, creditcard))
            {
                iscreditcardValid = true;
            }

            else
            {
                iscreditcardValid = false;
            }

            return iscreditcardValid;


        }
   
                



    }
}
