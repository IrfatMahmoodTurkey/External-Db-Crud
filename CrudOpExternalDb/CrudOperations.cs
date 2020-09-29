using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Linq;

namespace CrudOpExternalDb
{
    public class CrudOperations
    {
        private string connectionString = String.Empty;
        private SqlConnection connection;

        public CrudOperations(string _connectionString)
        {
            if (!String.IsNullOrEmpty(_connectionString))
            {
                connectionString = _connectionString;
                connection = new SqlConnection(connectionString);
            }
            else
            {
                connectionString = String.Empty;
            }
        }

        // save (returns rows affected in string)
        public string Save(List<InsertUpdateValues> values, string tableName)
        {
            try
            {
                connection.Open();

                string valuesParam = "(";
                string value = "(";
                int paramCount = 0;

                foreach (InsertUpdateValues parameter in values)
                {
                    valuesParam = valuesParam + parameter.ParamName;
                    value = value + "@"+parameter.ParamName;

                    paramCount++;

                    if (paramCount != values.Count)
                    {
                        valuesParam = valuesParam + ",";
                        value = value + ",";
                    }
                    else
                    {
                        valuesParam = valuesParam + ")";
                        value = value + ")";
                    }
                }

                string query = "INSERT INTO " + tableName + " " + valuesParam + " VALUES " + value;
                SqlCommand command = new SqlCommand(query, connection);

                foreach (InsertUpdateValues parameter in values)
                {
                    command.Parameters.AddWithValue("@" + parameter.ParamName, parameter.ParamValue);
                }

                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                return rowsAffected.ToString();
            }
            catch (Exception ep)
            {
                return ep.Message;
            }
        }

        // update record ( returns how many rows updated )
        public string Update(List<InsertUpdateValues> values, WhereParams where, string tableName)
        {
            try
            {
                connection.Open();

                string value = String.Empty;
                int paramCount = 0;

                foreach (InsertUpdateValues parameter in values)
                {
                    value = value + parameter.ParamName + "=@" + parameter.ParamName;

                    paramCount++;

                    if (paramCount != values.Count)
                    {
                        value = value + ",";
                    }
                }

                string query = "UPDATE " + tableName + " SET " + value;

                string whereClause = " WHERE ";
                
                whereClause = whereClause + where.ParamName + "=@" + where.ParamName;

                string finalQuery = query + whereClause;
                SqlCommand command = new SqlCommand(finalQuery, connection);

                foreach (InsertUpdateValues parameter in values)
                {
                    command.Parameters.AddWithValue("@" + parameter.ParamName, parameter.ParamValue);
                }

                command.Parameters.AddWithValue("@" + where.ParamName, where.ParamValue);
                
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();
                return rowsAffected.ToString();
            }
            catch (Exception ep)
            {
                return ep.Message;
            }
        }

        // select data by user defined query
        // user type query must be end before where conditions
        public List<string> SelectByUserDefinedQuery(List<WhereParams> where, string query)
        {
            connection.Open();

            if (where != null)
            {
                if (where.Count > 0)
                {
                    query = query + " WHERE ";
                    int whereCount = 0;

                    foreach (WhereParams parameter in where)
                    {
                        query = query + parameter.ParamName + "=@" + parameter.ParamName.ToLower();
                        whereCount++;

                        if (whereCount != where.Count)
                        {
                            query = query+ " " + parameter.LogicalExpression+" ";
                        }
                    }
                }
            }

            SqlCommand command = new SqlCommand(query, connection);

            if (where != null)
            {
                if (where.Count > 0)
                {
                    foreach (WhereParams parameter in where)
                    {
                        command.Parameters.AddWithValue("@" + parameter.ParamName.ToLower(), parameter.ParamValue);
                    }
                }
            }

            SqlDataReader reader = command.ExecuteReader();
            List<string> columnList = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();

            List<string> objects = new List<string>();

            while (reader.Read())
            {
                string jsonString = "";

                foreach (string columnName in columnList)
                {
                    if (string.IsNullOrEmpty(jsonString))
                    {
                        jsonString = "'" + columnName + "':'" + reader[columnName] + "'";
                    }
                    else
                    {
                        jsonString = jsonString + ",'" + columnName + "':'" + reader[columnName] + "'";
                    }
                }

                objects.Add("{" + jsonString + "}");
            }

            connection.Close();
            return objects;
        }

        // delete record
        public string Delete(List<WhereParams> where, string tableName)
        {
            try
            {
                connection.Open();

                string query = "DELETE FROM " + tableName;

                if (where != null)
                {
                    if (where.Count > 0)
                    {
                        query = query + " WHERE ";
                        int whereCount = 0;

                        foreach (WhereParams parameter in where)
                        {
                            query = query + parameter.ParamName + "=@" + parameter.ParamName.ToLower();
                            whereCount++;

                            if (whereCount != where.Count)
                            {
                                query = query + " " + parameter.LogicalExpression + " ";
                            }
                        }
                    }
                }

                SqlCommand command = new SqlCommand(query, connection);
                if (where != null)
                {
                    if (where.Count > 0)
                    {
                        foreach (WhereParams parameter in where)
                        {
                            command.Parameters.AddWithValue("@" + parameter.ParamName.ToLower(), parameter.ParamValue);
                        }
                    }
                }

                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();
                return rowsAffected.ToString();
            }
            catch (Exception ep)
            {
                return ep.Message;
            }
        }
    }
}
