using System;
using System.Collections.Generic;
using CrudOpExternalDb;

namespace CallingApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            CrudOperations db = new CrudOperations("Data Source=D-1;Initial Catalog=TestDb;User ID=MyUser;Password=12345");

            //-------------------------
            // for insert
            //List<InsertUpdateValues> insertValues = new List<InsertUpdateValues>();
            //insertValues.Add(new InsertUpdateValues()
            //{
            //    ParamName = "UserName",
            //    ParamValue = "DHEYYTED"
            //});

            //insertValues.Add(new InsertUpdateValues()
            //{
            //    ParamName = "EmailAddress",
            //    ParamValue = "dhrufhu@gmail.com"
            //});

            //insertValues.Add(new InsertUpdateValues()
            //{
            //    ParamName = "Password",
            //    ParamValue = "123456"
            //});

            //string response = db.Save(insertValues, "Users");

            //----------------------------
            // for update
            //List<InsertUpdateValues> updatedValues = new List<InsertUpdateValues>();
            //updatedValues.Add(new InsertUpdateValues()
            //{
            //    ParamName = "UserName",
            //    ParamValue = "Irfat"
            //});

            //updatedValues.Add(new InsertUpdateValues()
            //{
            //    ParamName = "EmailAddress",
            //    ParamValue = "irfat@gmail.com"
            //});

            //List<WhereParams> whereParams = new List<WhereParams>();
            //WhereParams where = new WhereParams()
            //{
            //    ParamName = "Id",
            //    ParamValue = "16"
            //};

            //string response = db.Update(updatedValues, where, "Users");

            //--------------------------------
            // for selection
            //List<User> users = new List<User>();

            //List<WhereParams> where = new List<WhereParams>();
            //where.Add(new WhereParams()
            //{
            //    ParamName = "Id",
            //    ParamValue = "16"
            //});

            ////where.Add(new WhereParams()
            ////{
            ////    ParamName = "UserName",
            ////    ParamValue = "Irfat",
            ////});

            //List<string> lists = db.SelectByUserDefinedQuery(where, "SELECT * FROM Users");

            // ----------------------
            // for delete
            List<WhereParams> where = new List<WhereParams>();
            where.Add(new WhereParams()
            {
                ParamName = "Id",
                ParamValue = "16",
                LogicalExpression = "AND"
            });

            where.Add(new WhereParams()
            {
                ParamName = "UserName",
                ParamValue = "Irfat",
            });

            string response = db.Delete(where, "Users");

            Console.WriteLine("Done");
        }
    }
}
