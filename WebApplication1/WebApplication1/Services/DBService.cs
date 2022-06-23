using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class DBService : IDBService
    {
        private readonly string stringConnection = @"Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True";
        public void DeletePatient(int idPatient)
        {

            using var connection = new SqlConnection(stringConnection);
            using var command = new SqlCommand();
            command.Connection = connection;

            List<int> prescriptionList = new List<int>();

            command.CommandText = $"SELECT IdPrescpription FROM Prescription WHERE IdPatient = @idPatient";
            command.Parameters.AddWithValue("@idPatient", idPatient);
            connection.Open();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
                prescriptionList.Add(Int32.Parse(reader["IdPrescription"].ToString()));
            reader.Close();

            foreach(var idPrescription in prescriptionList)
            {
                command.CommandText = $"DELETE FROM PrescriptionMedicament WHERE IdPrescription = @idPrescription";
            }

            command.CommandText = $"DELETE FROM Prescription WHERE IdPatient = @idPatient";
            command.Parameters.AddWithValue("@idPatient", idPatient);
            command.CommandText = $"DELETE FROM Patient WHERE IdPatient = @idPatient";
            command.Parameters.AddWithValue("@idPatient", idPatient);
            

            int affectedRows = command.ExecuteNonQuery();
            if (affectedRows < 0) throw new Exception();

            connection.Close();
        }

        public IEnumerable<Medicament> GetMedicament(int IdMedicament)
        {
            var meds = new List<Medicament>();
            var connection = new SqlConnection(stringConnection);
            {
                var command = new SqlCommand($"SELECT IdMedicament, Name, Description, Type, Prescription.Date FROM Medicament"
                    + "INNER JOIN PrescriptionMedicament ON Medicament.IdMedicament = PrescriptionMedicament.IdMedicament"
                    + "INNER JOIN Prescription ON PrscriptionMedicament.IdPrescription = Prescription.IdPrescription"
                    + "ORDER BY [Date] DESC", connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    meds.Add(new Medicament
                    {
                        IdMedicament = int.Parse(reader["IdMedicament"].ToString()),
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Type = reader["Type"].ToString(),
                        PrescriptionMedicament = reader["PrescriptionMedicament.Idprescription"].ToString()
                    });

            }

            return meds;
        }
    }
}
