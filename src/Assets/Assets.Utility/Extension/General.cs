using Assets.Model;
using Assets.Model.Base;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Assets.Utility.Extension {
    public static class General {
        #region string
        public static bool IsPhoneNumber(this string number) {
            return Regex.Match(number, "^[+][0-9]{12}$").Success;// || Regex.Match(number, "^[0][0-9]{10}$").Success;
        }

        public static AccountProvider ToProvider(this string provider) {
            switch(provider.ToLower()) {
                case "google":
                    return AccountProvider.Google;
                case "microsoft":
                    return AccountProvider.Microsoft;
                case "facebook":
                    return AccountProvider.Facebook;
                case "twitter":
                    return AccountProvider.Twitter;
                default:
                    return AccountProvider.Clipboard;
            }
        }
        #endregion

        #region number
        public static byte ToByte(this Enum val) {
            return Convert.ToByte(val);
        }

        public static int ToInt(this Enum val) {
            return Convert.ToInt32(val);
        }
        #endregion

        #region mapper
        public static IMappingExpression<TSource, TDestination> IgnoreAllVirtual<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression) {
            var desType = typeof(TDestination);
            foreach(var property in desType.GetProperties().Where(p => p.Name.ToLower() != "id" && p.GetGetMethod().IsVirtual)) {
                expression.ForMember(property.Name, opt => opt.Ignore());
            }
            return expression;
        }
        #endregion

        #region linq
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> query, string sortField, bool ascending) {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, sortField);
            var exp = Expression.Lambda(prop, param);
            var method = ascending ? "OrderBy" : "OrderByDescending";
            var types = new[] { query.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, query.Expression, exp);
            return query.Provider.CreateQuery<T>(mce);
        }
        #endregion

        #region date and time
        public static long? UnixTimestampFromDateTime(this DateTime? date) {
            if(!date.HasValue)
                return null;
            long unixTimestamp = date.Value.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp;
        }
        public static string PersianFromDateTime(this DateTime GregorianDate) {
            var pc = new PersianCalendar();
            return string.Format("{0}/{1}/{2} {3:00}:{4:00}", pc.GetYear(GregorianDate), pc.GetMonth(GregorianDate), pc.GetDayOfMonth(GregorianDate), pc.GetHour(GregorianDate), pc.GetMinute(GregorianDate));
        }
        public static DateTime? ToDateTime(this long? unixTime) {
            if(unixTime.HasValue) {
                var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(unixTime.Value).ToLocalTime();
                return dateTime;
            }
            return null;
        }
        public static DateTime? ToDateTime(this long? unixTime, TimeZoneInfo timeZoneInfo) {
            if(unixTime.HasValue) {
                var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(unixTime.Value);
                return TimeZoneInfo.ConvertTime(dateTime, timeZoneInfo ?? TimeZoneInfo.Utc);
            }
            return null;
        }
        public static DateTime ToUnixMiliseconds(this long unixTime) {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(unixTime).ToLocalTime();
            return dateTime;
        }
        public static DateTime ToUnixMiliseconds(this long unixTime, TimeZoneInfo timeZoneInfo) {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(unixTime);
            return TimeZoneInfo.ConvertTime(dateTime, timeZoneInfo ?? TimeZoneInfo.Utc);
        }
        #endregion

        #region data types
        public static DataTable ToDataTable<T>(this IEnumerable<T> list) {
            var dataTable = new DataTable();
            var propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(T));
            for(int i = 0; i < propertyDescriptorCollection.Count; i++) {
                var propertyDescriptor = propertyDescriptorCollection[i];
                var type = propertyDescriptor.PropertyType;
                if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);
                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach(T item in list) {
                for(int i = 0; i < values.Length; i++) {
                    values[i] = propertyDescriptorCollection[i].GetValue(item);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static DbType ToDbType(this Type type) {
            return new Dictionary<Type, DbType> {
                //[typeof(System.Collections.IEnumerable)] = DbType.Object,
                //[typeof(Binary)] = DbType.Binary
                [typeof(byte)] = DbType.Byte,
                [typeof(sbyte)] = DbType.SByte,
                [typeof(short)] = DbType.Int16,
                [typeof(ushort)] = DbType.UInt16,
                [typeof(int)] = DbType.Int32,
                [typeof(uint)] = DbType.UInt32,
                [typeof(long)] = DbType.Int64,
                [typeof(ulong)] = DbType.UInt64,
                [typeof(float)] = DbType.Single,
                [typeof(double)] = DbType.Double,
                [typeof(decimal)] = DbType.Decimal,
                [typeof(bool)] = DbType.Boolean,
                [typeof(string)] = DbType.String,
                [typeof(char)] = DbType.StringFixedLength,
                [typeof(Guid)] = DbType.Guid,
                [typeof(DateTime)] = DbType.DateTime,
                [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
                [typeof(byte[])] = DbType.Binary,
                [typeof(byte?)] = DbType.Byte,
                [typeof(sbyte?)] = DbType.SByte,
                [typeof(short?)] = DbType.Int16,
                [typeof(ushort?)] = DbType.UInt16,
                [typeof(int?)] = DbType.Int32,
                [typeof(uint?)] = DbType.UInt32,
                [typeof(long?)] = DbType.Int64,
                [typeof(ulong?)] = DbType.UInt64,
                [typeof(float?)] = DbType.Single,
                [typeof(double?)] = DbType.Double,
                [typeof(decimal?)] = DbType.Decimal,
                [typeof(bool?)] = DbType.Boolean,
                [typeof(char?)] = DbType.StringFixedLength,
                [typeof(Guid?)] = DbType.Guid,
                [typeof(DateTime?)] = DbType.DateTime,
                [typeof(DateTimeOffset?)] = DbType.DateTimeOffset,
            }[type];
        }

        public static string GetStoredProcedureName(this IStoredProcSchema baseSchema) {
            var schema = "dbo";
            var name = baseSchema.GetType().Name;

            var attr = (StoredProcedureAttribute)baseSchema.GetType()
                .GetCustomAttributes(typeof(StoredProcedureAttribute), true).FirstOrDefault();

            if(!string.IsNullOrWhiteSpace(attr.Schema))
                schema = attr.Schema;

            if(!string.IsNullOrWhiteSpace(attr.Name))
                name = attr.Name;

            return $"{schema}.{name}";
        }

        public static string GetSQLType(this PropertyInfo propertyInfo, Type customAttribute) {
            CustomParameterAttribute attr;
            if(customAttribute == typeof(InputParameterAttribute)) {
                attr = (InputParameterAttribute)propertyInfo.GetType()
                    .GetCustomAttributes(typeof(InputParameterAttribute), true).FirstOrDefault();
            }
            else {
                attr = (OutputParameterAttribute)propertyInfo.GetType()
                    .GetCustomAttributes(typeof(OutputParameterAttribute), true).FirstOrDefault();
            }

            var thetype = attr.Type.ToString();
            switch(attr.Type) {
                case SQLPropType.BIT:
                case SQLPropType.INT:
                    thetype += attr.IsNullable ? " = NULL" : string.Empty;
                    break;
                case SQLPropType.VARCHAR:
                case SQLPropType.NVARCHAR:
                    thetype += (attr.Length > 0 ? $"({attr.Length})" : "(MAX)") + (attr.IsNullable ? " = NULL" : string.Empty);
                    break;
                default:
                    break;
            }

            return thetype;
        }
        #endregion
    }
}
