using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashMaker
{
    class PasswordGenerator
    {
       
        //private static String validChars = "abcdefghijklmnopqrstuvwxyzåäöABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ0123456789!?";
        private static String validChars = "abcdefg";
        private static int validCharsCount;
        // den är sifror motsvarande lösenordet
        // Lista med vilket tecken som står i vilken position
        private List<int> listOfNumbers = new List<int>();
        private string currentPassword;

        // constructor tar emot lösenordet programmet skall starta på
        public PasswordGenerator(string wordToStartFrom)
        {
            //Hur många tecken är giltiga
            validCharsCount = validChars.Length;
            //sätt nuvarande lösenord till första lösenordet
            currentPassword = wordToStartFrom;

            // uppdatera listOfNumbers så att det motsvarar wordToStartFrom
            if (wordToStartFrom == "")
            {
                listOfNumbers.Add(0);
            }
            else
            {
                convertToNumbers(wordToStartFrom);
            }
        }
        public string GetCurrentPassword()
        {
            return currentPassword;
        }
        // Skapar nästa lösenord genom att räkna upp första positionen listOfNumbers med 1.
        // Med hjälp av checkOverflow() kontrollera och hantera i fall värdet 
        // blivit störe än sista tillåtna tecknet.
        // Sen konverteras listOfNumbers till currentPassword med hjälp av transLateToLetters().
        public void NextPassword()
        {
             
            listOfNumbers[0] += 1;
            checkOverflow();
            transLateToLetters();
        }
        // Det gör exakt som fuktionen NextPassword() fast här räknas listOfNumbers upp med 
        // värdet från inparametern 'count'.
        public void fastForward(int count)
        {
            listOfNumbers[0] += count;
            checkOverflow();
            transLateToLetters();
        }
        // checkOverflow() kontrollerar och hanterar i fall värdet 
        // blivit störe än sista tillåtna tecknet. I så fall kommer 
        // att ändra värdet i nästa position att räknas upp.      
        private void checkOverflow()
        {
            int i = 0;
            do
            {
                if (listOfNumbers[i] > validCharsCount)
                {
                    int carryOver = listOfNumbers[i] / validCharsCount;
                    int rest = listOfNumbers[i] % validCharsCount;
                    listOfNumbers[i] = rest;

                    if (rest == 0)
                    {
                        carryOver -= 1;
                        listOfNumbers[i] = validCharsCount;
                    }

                    if (carryOver > 0)
                    {
                        if (i + 1 < listOfNumbers.Count) 
                        {
                            listOfNumbers[i + 1] += carryOver;
                        }
                        else
                        {
                            listOfNumbers.Add(carryOver);
                        }
                    }
                }
                i++;
            } while (i < listOfNumbers.Count);
        }
        // transLateToLetters() översätter listOfNumbers till tecken som sparas i currentPassword.
        // Det sker för en siffra i taget med hjälp av funktionen NumberToString().
        private void transLateToLetters()
        {
            int[] reversed = new int[listOfNumbers.Count];
            listOfNumbers.CopyTo(reversed);
            Array.Reverse(reversed);

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < reversed.Length; i++)
            {
                builder.Append(NumberToString(reversed[i]));
            }
            currentPassword = Convert.ToString(builder);
        }
        // convertToNumbers() gör om inparametern "password" till siffror 
        // som sparas in i listOfNumbers. Det sker för ett tecken i taget
        // med hjälp av funktionen StringToNumber() 
        private void convertToNumbers(string password)
        {
            char[] entry; 
            entry = password.ToCharArray();//Det gör en string till array av char
            for (int i = 0; i < entry.Length; i++)
            {                
                char bokstav = entry[i];
                listOfNumbers.Add(StringToNumber(bokstav));

            }
            listOfNumbers.Reverse();
        }
        
        // Gör om en siffra som skickas till funktionen till motsvarande tecken i validChars
        private string NumberToString(int i)
        {
            return validChars[i - 1].ToString();
        }

        // Gör om ett tecken som skickas till funktionen förvandlar till motsvarande nummer i validChars.
        private int StringToNumber(char letter)
        {
            
            return validChars.IndexOf(letter) + 1;
        }
    }
}
