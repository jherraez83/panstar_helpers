using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using panstar_helpers.Extensions;
using System.Configuration.Internal;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Collections;
using Microsoft.Data.SqlClient;

namespace panstar_helpers
{
    public class SQLHelper
    {

        public  string connString { 
            get {
                return "Data Source = 192.168.5.239; Initial Catalog = mapexbp_candina_prod; User Id = sa; Password = DATANUDESPA00;Encrypt = false;";
            
            } 
        }
        public  List<dynamic> ExecuteQuery(string queryText, string dataSource = "mapex")
        {

            var _lista = new List<dynamic>();

            using (var connection = new SqlConnection(connString))
            {
                var cmd = new SqlCommand(queryText, connection);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 600;
              


                connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    IEnumerable<string> _prop;
                    IEnumerable<object> _values;

                    while (reader.Read())
                    {
                        _prop = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName);
                        _values = Enumerable.Range(0, reader.FieldCount).Select(reader.GetValue);
                        ExpandoObject _object = new ExpandoObject();

                        _object.AddProperties(_prop, _values);
                        _lista.Add(_object);
                    }
                }
            }
            return _lista;

        }


        public  void ExecuteNonQuery(string queryText, string dataSource = "mapex")
        {
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryText;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }

            }

        }



    }
}







