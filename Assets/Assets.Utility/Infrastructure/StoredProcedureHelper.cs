using Assets.Model;
using Assets.Model.Base;
using Assets.Utility.Extension;
using System;
using System.Linq;
using System.Reflection;

namespace Assets.Utility.Infrastructure {
    public class StoredProcedureHelper {

        public void Generate<Schema>(Schema schema, bool rebuild = false) where Schema : IStoredProcSchema {
            var wholequery = string.Empty;

            var proc = schema.GetStoredProcedureName();
            if(rebuild) {
                wholequery = $"IF(OBJECT({proc}, 'SP') <> NULL) " +
                    $"DROP OBJECT({proc});";
            }

            var inprops = schema.GetType().GetProperties().Where(attr => Attribute.IsDefined(attr, typeof(InputParameterAttribute)));

            var outprops = schema.GetType().GetProperties().Where(attr => Attribute.IsDefined(attr, typeof(OutputParameterAttribute)));

            var retprops = schema.GetType().GetProperties().Where(attr => Attribute.IsDefined(attr, typeof(ReturnParameterAttribute)));

            // generating stored procedure query
            wholequery = "SET QUOTED_IDENTIFIER ON|OFF " +
                "SET ANSI_NULLS ON | OFF " +
                "GO " +
                $"CREATE PROCEDURE[{proc}] ";

            // generating input parameters
            foreach(var arg in inprops) {
                wholequery += $"@{arg.Name} {arg.GetSQLType(typeof(InputParameterAttribute))},";
            }

            // generating output parameters
            foreach(var arg in outprops) {
                wholequery += $"@{arg.Name} {arg.GetSQLType(typeof(OutputParameterAttribute))} OUT,";
            }

            wholequery = wholequery.Remove(wholequery.Length - 1);
            wholequery = "@parameter_name AS INT " +
                "-- WITH ENCRYPTION, RECOMPILE, EXECUTE AS CALLER| SELF | OWNER | 'user_name' " +
                "AS " +
                "BEGIN ";

            wholequery = "END " +
            "GO " +
            "SET QUOTED_IDENTIFIER ON| OFF " +
            "SET ANSI_NULLS ON | OFF " +
            "GO";
        }
    }
}
