//using Assets.Model.Base;
//using Dapper;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;

//namespace Assets.Utility.Infrastructure {
//    public class QueryHandler<TEntity> where TEntity: IEntity {
//        #region ctor
//        private readonly string table = nameof(TEntity);
//        #endregion

//        public void All(Expression<Func<TEntity, bool>> predicate) {
//            //predicate.to
//        }

//        public void All(TEntity entity) {
//            var query = $"SELECT * FROM {table} ";
//            var where = "WHERE ";
//            var properties = entity.GetType().GetProperties().Where(item
//                => !Attribute.IsDefined(item, typeof(NotMappedAttribute))
//                && !Attribute.IsDefined(item, typeof(ForeignKeyAttribute)));
//            foreach(var prp in properties) {
//                var key = prp.Name;
//                var value = prp.GetValue(entity, null);
//                if(value != null) {
//                    where += $"{key} = {value}";
//                }
//            }
//        }

//        public static (string Query, DynamicParameters Parameters) Select(TEntity model, bool additionalInfo = true) {
//            var index = 0;
//            var columns = string.Empty;
//            var joins = string.Empty;
//            var where = string.Empty;
//            var tableName = typeof(TEntity).Name;
//            var hasStatus = false;
//            var statusHasValue = false;
//            var properties = model.GetType().GetProperties();
//            var entityField = properties.Where(prop => !Attribute.IsDefined(prop, typeof(HelperField)));
//            var relatedFields = properties.Where(prop => Attribute.IsDefined(prop, typeof(RelatedField))).ToList();
//            var parameters = new DynamicParameters();
//            foreach(var item in entityField) {
//                index++;
//                var entity = string.Empty;
//                var alies = string.Empty;
//                var primaryTitle = string.Empty;
//                var forignKey = string.Empty;
//                var mandatory = string.Empty;
//                var key = item.Name;
//                var value = item.GetValue(model, null);
//                var propertyType = item.PropertyType;
//                if(relatedFields.Contains(item)) {
//                    var attrs = item.GetCustomAttributes(true);
//                    foreach(var attr in attrs) {
//                        var related = attr as RelatedField;
//                        if(related == null) continue;
//                        entity = related.EntityTitle;
//                        alies = $"{entity}{index}";
//                        primaryTitle = related.PrimaryTitle;
//                        forignKey = related.ForignKey;
//                        mandatory = related.Mandatory ? "INNER" : "LEFT";
//                    }
//                    columns += $",[{alies}].[{primaryTitle}] AS {key}";

//                    joins += forignKey.Contains("]")
//                        ? $" {mandatory} JOIN [{entity}] AS [{alies}] WITH(NOLOCK) ON [{forignKey}] = [{alies}].[{nameof(Entity.Id)}]"
//                        : $" {mandatory} JOIN [{entity}] AS [{alies}] WITH(NOLOCK) ON [{tableName}].[{forignKey}] = [{alies}].[{nameof(Entity.Id)}]";
//                    //var theEntity = Type.GetType($"Domain.Application.Entities.{entity}, Domain.Application");
//                    //if (theEntity.GetProperties().SingleOrDefault(prop => prop.Name.Equals(nameof(Entity.Status))) != null)
//                    //{
//                    //    where += $" AND [{alies}].[{nameof(Entity.Status)}]!={(byte)GeneralEnums.Status.Deleted}";
//                    //}
//                }
//                if(key == nameof(Entity.Status)) hasStatus = true;
//                if(value == null) continue;
//                //if (propertyType == typeof(string))
//                //{
//                //    if (relatedFields.Contains(item))
//                //        where += $" AND [{alies}].[{primaryTitle}] LIKE @{key}";
//                //    else
//                //        where += $" AND [{tableName}].[{key}] LIKE @{key}";
//                //    parameters.Add(key, $"%{value}%");
//                //}
//                if(propertyType == typeof(string)) {
//                    var v1 = value.ToString();
//                    var key2 = key + "_2";
//                    string value2 = CommonHelper.ConvertArabicToPersianAndViceVersa(v1);
//                    if(relatedFields.Contains(item))
//                        where += $" AND ([{alies}].[{primaryTitle}] LIKE @{key} OR [{alies}].[{primaryTitle}] LIKE @{key2})";
//                    else
//                        where += $" AND ([{tableName}].[{key}] LIKE @{key} OR [{tableName}].[{key}] LIKE @{key2})";
//                    parameters.Add(key, $"%{value}%");
//                    parameters.Add(key2, $"%{value2}%");
//                }
//                else if(propertyType == typeof(DateTime) || propertyType == typeof(DateTime?)) {
//                    if(relatedFields.Contains(item))
//                        where += $" AND [{alies}].[{primaryTitle}] >= @{key}_MIN AND [{alies}].[{primaryTitle}] < @{key}_MAX";
//                    //where += $" AND Cast([{alies}].[{primaryTitle}] AS Date) = @{key}";
//                    else
//                        where += $" AND [{tableName}].[{key}] >= @{key}_MIN AND [{tableName}].[{key}] < @{key}_MAX";
//                    //where += $" AND Cast([{tableName}].[{key}] AS Date) = @{key}";
//                    parameters.Add($"{key}_MIN", ((DateTime)value).Date);
//                    parameters.Add($"{key}_MAX", ((DateTime)value).Date.AddDays(1));
//                }
//                else {
//                    if(relatedFields.Contains(item))
//                        where += $" AND [{alies}].[{primaryTitle}] = @{key}";
//                    else {
//                        if(key == nameof(Entity.Status)) statusHasValue = true;
//                        where += $" AND [{tableName}].[{key}]=@{key}";
//                    }
//                    parameters.Add(key, value);
//                }
//            }
//            if(hasStatus && !statusHasValue)
//                where += $" AND [{tableName}].[{nameof(Entity.Status)}]!={(byte)GeneralEnums.Status.Deleted}";


//            where = AdditionalCondition(tableName, where);

//            var query = $"SELECT COUNT(1) OVER() AS RowsCount, [{tableName}].* {columns} FROM [{tableName}] {joins} WHERE 1=1 {where}";
//            return (query, parameters);
//        }

//        public static (string Query, DynamicParameters Parameters) Single(TEntity model, bool additionalInfo = true) {
//            var index = 0;
//            var columns = string.Empty;
//            var joins = string.Empty;
//            var where = string.Empty;
//            var tableName = typeof(TEntity).Name;
//            var hasStatus = false;
//            var statusHasValue = false;
//            var properties = model.GetType().GetProperties();
//            var entityField = properties.Where(prop => !Attribute.IsDefined(prop, typeof(HelperField)));
//            var relatedFields = properties.Where(prop => Attribute.IsDefined(prop, typeof(RelatedField))).ToList();
//            var parameters = new DynamicParameters();
//            foreach(var item in entityField) {
//                index++;
//                var entity = string.Empty;
//                var alies = string.Empty;
//                var primaryTitle = string.Empty;
//                var forignKey = string.Empty;
//                var mandatory = string.Empty;
//                var key = item.Name;
//                var value = item.GetValue(model, null);
//                if(additionalInfo) {
//                    if(relatedFields.Contains(item)) {
//                        var attrs = item.GetCustomAttributes(true);
//                        foreach(var attr in attrs) {
//                            var related = attr as RelatedField;
//                            if(related == null) continue;
//                            entity = related.EntityTitle;
//                            alies = $"{entity}{index}";
//                            primaryTitle = related.PrimaryTitle;
//                            forignKey = related.ForignKey;
//                            mandatory = related.Mandatory ? "INNER" : "LEFT";
//                        }
//                        columns += $",[{alies}].[{primaryTitle}] AS {key}";
//                        joins += $" {mandatory} JOIN [{entity}] AS [{alies}] WITH(NOLOCK) ON [{tableName}].[{forignKey}] = [{alies}].[{nameof(Entity.Id)}]";
//                    }
//                }
//                if(key == nameof(Entity.Status)) hasStatus = true;
//                if(value == null) continue;
//                var propertyType = item.PropertyType;
//                if(propertyType == typeof(string)) {
//                    if(relatedFields.Contains(item))
//                        where += $" AND [{alies}].[{primaryTitle}] LIKE @{key}";
//                    else
//                        where += $" AND [{tableName}].[{key}] LIKE @{key}";
//                    parameters.Add(key, $"%{value}%");
//                }
//                else if(propertyType == typeof(DateTime) || propertyType == typeof(DateTime?)) {
//                    if(relatedFields.Contains(item))
//                        where += $" AND [{alies}].[{primaryTitle}] >= @{key}_MIN AND [{alies}].[{primaryTitle}] < @{key}_MAX";
//                    else
//                        where += $" AND [{tableName}].[{key}] >= @{key}_MIN AND [{tableName}].[{key}] < @{key}_MAX";
//                    parameters.Add($"{key}_MIN", ((DateTime)value).Date);
//                    parameters.Add($"{key}_MAX", ((DateTime)value).Date.AddDays(1));
//                }
//                else {
//                    if(relatedFields.Contains(item))
//                        where += $" AND [{alies}].[{primaryTitle}] = @{key}";
//                    else {
//                        if(key == nameof(Entity.Status)) statusHasValue = true;
//                        where += $" AND [{tableName}].[{key}]=@{key}";
//                    }
//                    parameters.Add(key, value);
//                }
//            }
//            if(hasStatus && !statusHasValue)
//                where += $" AND [{tableName}].[{nameof(Entity.Status)}] = {(byte)GeneralEnums.Status.Active}";

//            where = AdditionalCondition(tableName, where);

//            var query = $"SELECT TOP(1) [{tableName}].* {columns} FROM [{tableName}] {joins} WHERE 1=1 {where}";
//            return (query, parameters);
//        }

//        public static (string Query, DynamicParameters Parameters) GetById(int id, bool additionalInfo = true) {
//            var index = 0;
//            var columns = string.Empty;
//            var joins = string.Empty;
//            var where = string.Empty;
//            var tableName = typeof(TEntity).Name;
//            var hasStatus = false;
//            var entityField = typeof(TEntity).GetProperties().Where(prop => !Attribute.IsDefined(prop, typeof(HelperField)));
//            var relatedFields = typeof(TEntity).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(RelatedField))).ToList();
//            foreach(var item in entityField) {
//                index++;
//                var entity = string.Empty;
//                var alies = string.Empty;
//                var primaryTitle = string.Empty;
//                var forignKey = string.Empty;
//                var mandatory = string.Empty;
//                var key = item.Name;
//                if(additionalInfo) {
//                    if(relatedFields.Contains(item)) {
//                        var attrs = item.GetCustomAttributes(true);
//                        foreach(var attr in attrs) {
//                            var related = attr as RelatedField;
//                            if(related == null) continue;
//                            entity = related.EntityTitle;
//                            alies = $"{entity}{index}";
//                            primaryTitle = related.PrimaryTitle;
//                            forignKey = related.ForignKey;
//                            mandatory = related.Mandatory ? "INNER" : "LEFT";
//                        }
//                        columns += $",[{alies}].[{primaryTitle}] AS {key}";
//                        joins += $" {mandatory} JOIN [{entity}] AS [{alies}] WITH(NOLOCK) ON [{forignKey}] = [{alies}].[{nameof(Entity.Id)}]";
//                        //var theEntity = Type.GetType($"Domain.Application.Entities.{entity}, Domain.Application");
//                        //if (theEntity.GetProperties().SingleOrDefault(prop => prop.Name.Equals(nameof(Entity.Status))) != null)
//                        //{
//                        //    where += $" AND [{alies}].[{nameof(Entity.Status)}]!={(byte)GeneralEnums.Status.Deleted}";
//                        //}
//                    }
//                }
//                if(key == nameof(Entity.Status)) hasStatus = true;
//            }

//            where = AdditionalCondition(tableName, where);

//            var query = $"SELECT [{tableName}].* {columns} FROM [{tableName}] {joins} WHERE [{tableName}].[{nameof(Entity.Id)}] = @{nameof(Entity.Id)} {where}";
//            if(hasStatus)
//                query += $" AND [{tableName}].[{nameof(Entity.Status)}]!={(byte)GeneralEnums.Status.Deleted}";
//            var parameters = new DynamicParameters();
//            parameters.Add(nameof(Entity.Id), id);
//            return (query, parameters);
//        }

//        public static (string Query, DynamicParameters Parameters) GetPaging(TEntity model, string orderBy, string order, int skip, int take) {
//            var tableName = typeof(TEntity).Name;
//            var select = Select(model);
//            var query = select.Query + $" ORDER BY [{tableName}].[{orderBy}] {order} OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
//            return (query, select.Parameters);
//        }

//        //public static (string Query, DynamicParameters Parameters) GetPaging(T model, string orderBy, string order, int skip, int take)
//        //{
//        //    var tableName = typeof(T).Name;
//        //    var select = SELECT(model);
//        //    var query = select.Query.Replace($"[{tableName}].*", $"Count(1) Over() AS RowsCount,Row_Number() Over(Order By [{tableName}].[{orderBy}] {order}) AS RowNum,[{tableName}].*");
//        //    query = $"With Temp AS ({query}) SELECT [{tableName}].* FROM Temp[{tableName}] WHERE RowNum > {skip} AND RowNum <= {take}";
//        //    return (query, select.Parameters);
//        //}

//        public static (string Query, DynamicParameters Parameters) Insert(TEntity model) {
//            var query = $"INSERT INTO[{typeof(TEntity).Name}] (";
//            var myProviderInfo = model.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(InsertField)) || Attribute.IsDefined(prop, typeof(InsertMandatoryField)));
//            var parameters = new DynamicParameters();
//            foreach(var item in myProviderInfo) {
//                var key = item.Name;
//                var value = item.GetValue(model, null);
//                parameters.Add(key, key.Equals(nameof(Entity.CreatedAt)) ? DateTime.Now : value);
//            }
//            query = myProviderInfo.Select(item => item.Name).Aggregate(query, (current, propertyName) => current + $"[{propertyName}],");
//            query = query.Remove(query.Length - 1);
//            query += ") VALUES(";
//            query = myProviderInfo.Select(item => item.Name).Aggregate(query, (current, propertyName) => current + $"@{propertyName},");
//            query = query.Remove(query.Length - 1);
//            query += ") SELECT CAST(SCOPE_IDENTITY() AS INT)";
//            return (query, parameters);
//        }

//        public static (string Query, DynamicParameters Parameters) BulkInsert(IList<TEntity> model) {
//            if(!model.Any()) {
//                throw new Exception("داده برای درج داده ها بصورت چنتایی دریافت نشد.", new Exception { Source = GeneralMessages.ExceptionSource });
//            }
//            var query = $"INSERT INTO[{typeof(TEntity).Name}] (";
//            var columns = model[0].GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(InsertField)) || Attribute.IsDefined(prop, typeof(InsertMandatoryField)));
//            query = columns.Select(item => item.Name).Aggregate(query, (current, propertyName) => current + $"[{propertyName}],");
//            query = query.Remove(query.Length - 1);
//            query += ") Values ";
//            var parameters = new DynamicParameters();
//            for(var i = 0; i < model.Count; i++) {
//                query += "(";
//                var properties = model[i].GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(InsertField)) || Attribute.IsDefined(prop, typeof(InsertMandatoryField)));
//                foreach(var item in properties) {
//                    var key = item.Name;
//                    var value = item.GetValue(model[i], null);
//                    query += $"@{key}{i},";
//                    parameters.Add($"{key}{i}", key.Equals(nameof(Entity.CreatedAt)) ? DateTime.Now : value);
//                }
//                query = query.Remove(query.Length - 1);
//                query += "),";
//            }
//            query = query.Remove(query.Length - 1);
//            return (query, parameters);
//        }

//        public static (string Query, DynamicParameters Parameters) Update(TEntity model) {
//            var query = $"Update [{typeof(TEntity).Name}] Set ";
//            var myProviderInfo = model.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(UpdateField)) || Attribute.IsDefined(prop, typeof(UpdateMandatoryField)));
//            var parameters = new DynamicParameters();
//            foreach(var item in myProviderInfo) {
//                var key = item.Name;
//                var value = item.GetValue(model, null);
//                query += $"[{key}]=@{key},";
//                parameters.Add(key, key.Equals(nameof(Entity.UpdatedAt)) ? DateTime.Now : value);
//            }
//            query = query.Remove(query.Length - 1);
//            query += $" WHERE [{nameof(Entity.Id)}]=@{nameof(Entity.Id)}";

//            query = AdditionalCondition(typeof(TEntity).Name, query);

//            parameters.Add(nameof(Entity.Id), model.GetType().GetProperties().SingleOrDefault(prop => prop.Name.Equals(nameof(Entity.Id))).GetValue(model, null));
//            return (query, parameters);
//        }

//        public static (string Query, DynamicParameters Parameters) UpdateByKey(TEntity model) {
//            var query = $"Update [{typeof(TEntity).Name}] Set ";
//            var myProviderInfo = model.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(UpdateField)) || Attribute.IsDefined(prop, typeof(UpdateMandatoryField)));
//            var parameters = new DynamicParameters();
//            foreach(var item in myProviderInfo) {
//                var key = item.Name;
//                var value = item.GetValue(model, null);
//                query += $"[{key}]=@{key},";
//                parameters.Add(key, key.Equals(nameof(Entity.UpdatedAt)) ? DateTime.Now : value);
//            }
//            query = query.Remove(query.Length - 1);
//            query += $" WHERE [Key]=@Key";

//            query = AdditionalCondition(typeof(TEntity).Name, query);

//            parameters.Add(nameof(KeyEntity.Key), model.GetType().GetProperties().SingleOrDefault(prop => prop.Name.Equals(nameof(KeyEntity.Key))).GetValue(model, null));
//            return (query, parameters);
//        }

//        public static (string Query, DynamicParameters Parameters) LogicalDelete(long[] ids) {
//            var model = typeof(TEntity);
//            var columns = string.Empty;
//            var properties = model.GetProperties().Where(prop => !Attribute.IsDefined(prop, typeof(HelperField)));
//            foreach(var item in properties) {
//                var key = item.Name;
//                if(key.Equals(nameof(Entity.Status)))
//                    columns += $"[{model.Name}].[{nameof(Entity.Status)}]={(byte)GeneralEnums.Status.Deleted},";
//                if(key.Equals(nameof(Entity.UpdatedAt)))
//                    columns += $"[{model.Name}].[{nameof(Entity.UpdatedAt)}]='{DateTime.Now}',";
//            }
//            columns = columns.Length > 0 ? columns.Remove(columns.Length - 1) : columns;

//            var parameters = new DynamicParameters();
//            var query = $"Update [{typeof(TEntity).Name}] Set {columns} WHERE [{nameof(Entity.Id)}] In (";
//            for(int i = 1; i <= ids.Length; i++) {
//                query += i == 1 ? $"@ids{i}" : $", @ids{i}";
//                parameters.Add($"@ids{i}", ids[i - 1]);
//            }
//            query += " )";
//            query = AdditionalCondition(typeof(TEntity).Name, query);
//            return (query, parameters);
//        }

//        public static (string Query, DynamicParameters Parameters) PhysicalDelete(object[] ids) {
//            string flags = string.Join(",", ids.Select((s, i) => "@flag" + i));
//            var query = $"DELETE FROM [{typeof(TEntity).Name}] WHERE [{nameof(Entity.Id)}] In ({flags})";

//            query = AdditionalCondition(typeof(TEntity).Name, query);

//            var parameters = new DynamicParameters();
//            for(int i = 0; i < ids.Length; i++) {
//                parameters.Add("@flag" + i, ids[i]);
//            }
//            return (query, parameters);
//        }

//        public static string AdditionalCondition(string model, string where) {
//            if(model == "Admin" || model == "Role") {
//                where += $" AND [{model}].[Id] <> 1";
//            }

//            return where;
//        }
//    }
//}
