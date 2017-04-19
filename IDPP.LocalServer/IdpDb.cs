using MySql.Data.MySqlClient;
using SourceAFIS.Simple;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDPP.LocalServer
{
    public class IdpDb
    {
        string connectionString;
        MySqlConnection mySqlConnection;
        static AfisEngine Afis = new AfisEngine();

        public IdpDb()
        {
            connectionString = "server=localhost;database=idpp;uid=root;pwd=root;";
            mySqlConnection = new MySqlConnection(connectionString);
        }

        public bool Open()
        {
            try
            {
                mySqlConnection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeletePerson(string id)
        {
            MySqlCommand selectCommand = new MySqlCommand();
            selectCommand.Connection = mySqlConnection;
            selectCommand.CommandText =
                "DELETE from idp where id=\"" + id + "\"";
            //TODO: remove history, delete photo and fingers

            bool noErrorOccured = true;
            try
            {
                this.Open();
                selectCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                noErrorOccured = false;
            }
            finally
            {
                mySqlConnection.Close();
            }
            return noErrorOccured;
        }

        public IdpPerson GetPerson(string id)
        {
            MySqlCommand selectCommand = new MySqlCommand();
            selectCommand.Connection = mySqlConnection;
            selectCommand.CommandText =
                "SELECT id, date_registered, dob, first_name, gender, last_name, lga, marital_status, other_names, photo, state, yob, finger_1, finger_2, finger_3, finger_4, finger_5, finger_6, finger_7, finger_8, finger_9, finger_10 " +
                "from idp where id=\"" + id + "\"";

            IdpPerson person = null;

            try
            {
                this.Open();
                MySqlDataReader dr = selectCommand.ExecuteReader(System.Data.CommandBehavior.SequentialAccess);
                while (dr.Read())
                {
                    person = new IdpPerson();

                    if (!dr.IsDBNull(dr.GetOrdinal("id")))
                        person.ID = dr.GetString("id");
                    if (!dr.IsDBNull(dr.GetOrdinal("date_registered")))
                    {
                        //test this to ensure the person supplied the right format
                        person.DateRegisteredString = dr.GetString("date_registered");
                        string[] parts = person.DateRegisteredString.Split('/');
                        DateTime dt = new DateTime(int.Parse(parts[2]), int.Parse(parts[1]), int.Parse(parts[0]));
                        person.DateRegistered = dt;
                    }

                    if (!dr.IsDBNull(dr.GetOrdinal("dob")))
                    {
                        //test this to ensure the person supplied the right format
                        person.DoBString = dr.GetString("dob");
                        string[] parts = person.DoBString.Split('/');
                        DateTime dt = new DateTime(int.Parse(parts[2]), int.Parse(parts[1]), int.Parse(parts[0]));
                        person.DoB = dt;
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("first_name")))
                        person.FirstName = dr.GetString("first_name");
                    if (!dr.IsDBNull(dr.GetOrdinal("gender")))
                        person.Gender = dr.GetString("gender");
                    if (!dr.IsDBNull(dr.GetOrdinal("last_name")))
                        person.LastName = dr.GetString("last_name");
                    if (!dr.IsDBNull(dr.GetOrdinal("lga")))
                        person.LGA = dr.GetString("lga");
                    if (!dr.IsDBNull(dr.GetOrdinal("marital_status")))
                        person.MaritalStatus = dr.GetString("marital_status");
                    if (!dr.IsDBNull(dr.GetOrdinal("other_names")))
                        person.OtherNames = dr.GetString("other_names");
                    if (!dr.IsDBNull(dr.GetOrdinal("photo")))
                    {
                        person.PhotoLocation = dr.GetString("photo");
                        person.Photo = Image.FromFile(person.PhotoLocation);
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("state")))
                        person.State = dr.GetString("state");
                    if (!dr.IsDBNull(dr.GetOrdinal("yob")))
                    {
                        person.YoBString = dr.GetString("yob");
                        person.YoB = int.Parse(person.YoBString);
                    }

                    break;
                }
                dr.Close();
            }
            catch (Exception ex) { }
            finally
            {
                mySqlConnection.Close();
            }
            return person;
        }

        public IEnumerable<IdpPerson> GetPersons()
        {
            MySqlCommand selectCommand = new MySqlCommand();
            selectCommand.Connection = mySqlConnection;
            selectCommand.CommandText =
                "SELECT id, date_registered, dob, first_name, gender, last_name, lga, marital_status, other_names, photo, state, yob, finger_1, finger_2, finger_3, finger_4, finger_5, finger_6, finger_7, finger_8, finger_9, finger_10  " +
                "from idp";

            List<IdpPerson> persons = new List<IdpPerson>();
            try
            {
                this.Open();
                MySqlDataReader dr = selectCommand.ExecuteReader(System.Data.CommandBehavior.SequentialAccess);
                while (dr.Read())
                {
                    IdpPerson person = new IdpPerson();

                    if (!dr.IsDBNull(dr.GetOrdinal("id")))
                        person.ID = dr.GetString("id");
                    if (!dr.IsDBNull(dr.GetOrdinal("date_registered")))
                    {
                        //test this to ensure the person supplied the right format
                        person.DateRegisteredString = dr.GetString("date_registered");
                        string[] parts = person.DateRegisteredString.Split('/');
                        DateTime dt = new DateTime(int.Parse(parts[2]), int.Parse(parts[1]), int.Parse(parts[0]));
                        person.DateRegistered = dt;
                    }

                    if (!dr.IsDBNull(dr.GetOrdinal("dob")))
                    {
                        //test this to ensure the person supplied the right format
                        person.DoBString = dr.GetString("dob");
                        string[] parts = person.DoBString.Split('/');
                        DateTime dt = new DateTime(int.Parse(parts[2]), int.Parse(parts[1]), int.Parse(parts[0]));
                        person.DoB = dt;
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("first_name")))
                        person.FirstName = dr.GetString("first_name");
                    if (!dr.IsDBNull(dr.GetOrdinal("gender")))
                        person.Gender = dr.GetString("gender");
                    if (!dr.IsDBNull(dr.GetOrdinal("last_name")))
                        person.LastName = dr.GetString("last_name");
                    if (!dr.IsDBNull(dr.GetOrdinal("lga")))
                        person.LGA = dr.GetString("lga");
                    if (!dr.IsDBNull(dr.GetOrdinal("marital_status")))
                        person.MaritalStatus = dr.GetString("marital_status");
                    if (!dr.IsDBNull(dr.GetOrdinal("other_names")))
                        person.OtherNames = dr.GetString("other_names");
                    if (!dr.IsDBNull(dr.GetOrdinal("photo")))
                    {
                        person.PhotoLocation = dr.GetString("photo");
                        person.Photo = Image.FromFile(person.PhotoLocation);
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("state")))
                        person.State = dr.GetString("state");
                    if (!dr.IsDBNull(dr.GetOrdinal("yob")))
                    {
                        person.YoBString = dr.GetString("yob");
                        person.YoB = int.Parse(person.YoBString);
                    }

                    for (int index = 0; index < person.FingerprintLocations.Length; index++)
                    {
                        int realIndex = index + 1;
                        int ordinal = dr.GetOrdinal("finger_" + realIndex);
                        if (!(dr.IsDBNull(ordinal)))
                        {
                            person.FingerprintLocations[index] = dr.GetString("finger_" + realIndex);
                            Fingerprint fp = new Fingerprint();
                            fp.AsBitmap = new Bitmap(Bitmap.FromFile(person.FingerprintLocations[index]));
                            person.Fingerprints.Add(fp);
                        }
                    }
                    Afis.Extract(person);

                    persons.Add(person);
                }
                dr.Close();
            }
            catch (Exception ex) { }
            finally
            {
                mySqlConnection.Close();
            }
            return persons;
        }
    }
}
