using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashMaker.Database;

namespace HashMaker {
    class DatabaseManager {
        public String NextPassword { get; set; }
        public long NoOfProcessedPasswords { get; set; }

        //  Hämta nästa lösenord från databasen. Fins det inget sparat lösenord
        //  så kommer defaultPassword att användas.
        public void init(String defaultPassword) {

            using (var db = new HashingContext()) {
                ProcessingInfo processingInfo = db.ProcessingInfo.FirstOrDefault();
                if (processingInfo == null) {
                    db.Add(new ProcessingInfo { NextPassword = defaultPassword, NoOfProcessedPasswords = 0 });
                    db.SaveChanges();
                    NextPassword = defaultPassword;
                    NoOfProcessedPasswords = 0;
                } else {
                    NextPassword = processingInfo.NextPassword;
                    NoOfProcessedPasswords = processingInfo.NoOfProcessedPasswords;
                }
            }
        }

        // sparar lista av PasswordHashData och nextpassword till databasen 
        public void save(List<PasswordHashData> passwordHashDataList, String nexPassword) 
        {
            bulkSave(passwordHashDataList);
            saveProgress(passwordHashDataList.Count, nexPassword);
        }
        // spara antalet lösenord och nästa lösenord till databasen
        // med hjälp av entityFramwork.
        private void saveProgress(long noOfProcessedPasswords, String nexPassword) {

            // using kommer att rensa i ordning efter db när scopet är klart
            using (var db = new HashingContext()) {
                ProcessingInfo processingInfo = db.ProcessingInfo.FirstOrDefault();
                processingInfo.NextPassword = nexPassword;
                processingInfo.NoOfProcessedPasswords += noOfProcessedPasswords;
                db.SaveChanges();

                NoOfProcessedPasswords = processingInfo.NoOfProcessedPasswords;
                NextPassword = processingInfo.NextPassword;
            }
        }
        // Spara listan med PasswordHashData till databasen med hjälp av SqlBulkCopy() 
        // Det gick för långsamt att använda entityFramwork och därför använts SqlBulkCopy.
        private void bulkSave(List<PasswordHashData> passwordHashDataList) 
        {
            // using kommer att rensa i ordning efter bulkCopy när scopet är klart
            using (var bulkCopy = new SqlBulkCopy(HashingContext.connectionString)) 
            {
                bulkCopy.DestinationTableName = "dbo.PasswordHashes";
                bulkCopy.ColumnMappings.Add(nameof(PasswordHashData.Password), "Password");
                bulkCopy.ColumnMappings.Add(nameof(PasswordHashData.HashSha256), "HashSha256");
                bulkCopy.ColumnMappings.Add(nameof(PasswordHashData.HashSha384), "HashSha384");
                bulkCopy.ColumnMappings.Add(nameof(PasswordHashData.HashSha512), "HashSha512");
                bulkCopy.WriteToServer(toDataTable(passwordHashDataList));
            }
        }

        // gör om lista av List<PasswordHashData> till DataTable, för kunna spara i bulkCopy.
        private DataTable toDataTable(List<PasswordHashData> passwordHashDataList) {
           
            DataTable dt = new DataTable();
            dt.Columns.Add("Password");
            dt.Columns.Add("HashSha256");
            dt.Columns.Add("HashSha384");
            dt.Columns.Add("HashSha512");

            object[] row = new object[4];
            foreach (var data in passwordHashDataList) {
                row[0] = data.Password;
                row[1] = data.HashSha256;
                row[2] = data.HashSha384;
                row[3] = data.HashSha512;
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
