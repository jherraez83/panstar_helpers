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

namespace panstar_helpers
{
    public class OracleHelper
    {
      
        private string connString { get; set; }
            
     
        private int oracleTimeOut
        {
            get {
                return 5;
            }
        }

        public OracleHelper(IConfiguration config) {



            this.connString = config.GetSection("service_conn_string").Value;
        }


        public List<dynamic> ExecuteQuery(string queryText)
        {

            List<dynamic> _lista = new List<object>();
            using (var connection = new OracleConnection(connString))
            {
                             
                connection.Open();
                var cmd = new Oracle.ManagedDataAccess.Client.OracleCommand();

                cmd.CommandText = queryText;
                cmd.Connection = connection;
                cmd.CommandTimeout = oracleTimeOut;
                
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

        public void ExecuteMultipleNonQueryTx(List<string> queries) {

            using (var connection = new OracleConnection(connString))
            {
                connection.Open();
                OracleTransaction transaction = connection.BeginTransaction();

                try {
                    foreach (var query in queries)
                    {
                        var cmd = new OracleCommand(query, connection);
                        var rowsAffected = cmd.ExecuteNonQuery();
                    }
                    //Si hemos llegado aquí, podemos hacer commit;
                    transaction.Commit();
                }
                catch(Exception e){
                    transaction.Rollback(); // dejamos tal cual estaba todo
                }
                
              
                           
            }     
        }


        public int ExecuteNonQuery(string queryText) {
            using (var connection = new OracleConnection(connString))
            {
                connection.Open();
                var cmd = new OracleCommand(queryText, connection);
                return cmd.ExecuteNonQuery();        
            }

        }

    }
}



