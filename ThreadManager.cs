
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HashMaker
{
    class ThreadManager
    {

        // Skapa ett objekt för att kunna låsa tillgången till activeTreadCount 
        // så att bara ett thread i taget kan anropa get och set
        private static object SyncLock = new Object();

        private static int activeTreadCount = 0;
        public static int ActiveTreadCount
        {
            get
            {
                lock (SyncLock)
                {
                    return activeTreadCount;
                }
            }
            set
            {
                lock (SyncLock)
                {
                    activeTreadCount = value;
                }
            }
        }
        // Run() kör programmet. 
        public void Run()
        {
            int maxNumberOfRunningThreads = Environment.ProcessorCount - 1;
            int noOfEachThread = 100;

            DatabaseManager dbManager = new DatabaseManager();
            dbManager.init("a");

            // börja på lösenordet där programmet slutade senast (dbManager.NextPassword).
            PasswordGenerator mainPasswordGenerator = 
                new PasswordGenerator(dbManager.NextPassword);


            // deklarera en Dictionary<>,threadPasswordHashData som skall spara 
            // en lista av <PasswordHashData> för varje threadId (int).              
            Dictionary<int, List<PasswordHashData>> threadPasswordHashData = 
                new Dictionary<int, List<PasswordHashData>>();


            while (!Console.KeyAvailable) // så länge är inte en knapp tryckning
            {
                //string password = passwordGenerator.GetCurrentPassword();
                //Console.WriteLine(password);
                //passwordGenerator.NextPassword();

                threadPasswordHashData.Clear();

                // Loop som skapar och kör threads.
                // Varje tread kommer och köra funktionen createHash()
                for (int i = 0; i < maxNumberOfRunningThreads; i++)
                {
                    int treadId = i;
                    // Lägga till en lista av typen <PasswordHashData>
                    //till Dictionary<> threadPasswordHashData.
                    threadPasswordHashData.Add(treadId, new List<PasswordHashData>());
                    String currentPassword = mainPasswordGenerator.GetCurrentPassword();

                    //Skapa new thread() och tala om vilken funktion
                    // den skall köra(createHash())
                    Thread thread = new Thread(() => createHash(treadId, currentPassword, 
                        noOfEachThread, threadPasswordHashData[treadId]));

                    // Öka antal aktiva threads med +1 
                    ActiveTreadCount++;
                    
                    thread.Start();

                    mainPasswordGenerator.fastForward(noOfEachThread);
                }
                // Wait for all threads to finish
                while (ActiveTreadCount > 0)
                {

                    Thread.Sleep(100);
                }

                // alla threads har kört klart och skapat hashar i
                // threadPasswordHashData. Här kommer att slå ihop
                // alla threads listor till en lista.                
                List<PasswordHashData> totalProcessedPasswords = new List<PasswordHashData>();

                // Hämta lista från Dictionary för varje thread och läg till i
                // totala listan(totalProcessedPasswords) 
                for (int i = 0; i < maxNumberOfRunningThreads; i++)
                {
                    List<PasswordHashData> passwordHashData = threadPasswordHashData[i];
                    totalProcessedPasswords.AddRange(passwordHashData);
                }

                // Sparar lösenorden och hasharna till databasen
                dbManager.save(totalProcessedPasswords, 
                    mainPasswordGenerator.GetCurrentPassword());
                // skriver ut hur lång programmet har kommit
                Console.Write("\rPasswords: {0:N0}\tCurrent password: {1}",
                    dbManager.NoOfProcessedPasswords, dbManager.NextPassword);

            }
        }
        // Funktionen som körs av varje thread(createHash()),
        // createHash() Det är en funktion som anropas av alla thread.
        private void createHash(int threadID, // Id för tråden
                                String firstPassword, // Första lösenordet som skall skapas
                                int noOfPasswords, // Antal lösenord som funktionen skall skapa
                                List<PasswordHashData> listOfPasswords) // Lista som skall fyllas med skapade lösenord
        {

             try
            {
                // skapa upp en new PasswordGenerator och ange att första lösenorden 
                // som skall skapas är (firstPassword)
                PasswordGenerator threadPasswordGenerator =
                    new PasswordGenerator(firstPassword);
                // Skapa en new HashGenerator() 
                HashGenerator hashGenerator = new HashGenerator();

                // loopa för varje lösenord som skall skapas och beräknas hash för.
                // spara resultatet i listOfPasswords.
                for (int i = 0; i < noOfPasswords; i++) 
                {
                    // Skall skapa passwordHashData för varje lösenord
                    PasswordHashData passwordHashData =
                        new PasswordHashData();

                    // Hämta nuvarande lösenord från threadPasswordGenerator 
                    // från class PasswordGenerator och sparar i passwordHashData 
                    // i property Password i PasswordHashData class.
                    passwordHashData.Password = 
                        threadPasswordGenerator.GetCurrentPassword();

                    // Anropar calculateHash() i klassen HashGenerator.
                    // calculateHash() kommer att beräkna hashar för lösenordet
                    // som finns sparat i passwordHashData.Password.
                    // calculateHash() kommer att spara hasharna i passwordHashData.HashSha256, 
                    // passwordHashData.HashSha384, passwordHashData.HashSha512 
                    hashGenerator.calculateHash(passwordHashData);

                    // Addar allt som passwordHashData fått i listOfPasswords
                    listOfPasswords.Add(passwordHashData);

                    // Säg till threadPasswordGenerator att skapa nästa lösenord
                    threadPasswordGenerator.NextPassword(); // continue++
                }
             }

                
             finally
             {
                // nät thread är färdigt så räknar ner antalet aktiva threads -1
                ActiveTreadCount--;
             }
        }

    }
}
