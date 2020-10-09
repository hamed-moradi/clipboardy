using Assets.Model;
using Assets.Model.Base;
using Assets.Utility.Extension;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;

namespace Assets.Utility.Infrastructure {
    public interface IParameterHandler {
        DynamicParameters MakeParameters<T>(T schema) where T : IStoredProcSchema;
        void SetOutputValues<T>(T model, DynamicParameters parameters) where T : IStoredProcSchema;
        void SetReturnValue<T>(T model, DynamicParameters parameters) where T : IStoredProcSchema;
        void SetTotalCount<Schema, Result>(Schema model, IEnumerable<Result> result) where Schema : IStoredProcSchema where Result : IStoredProcResult;
    }

    public class ParameterHandler: IParameterHandler {
        public DynamicParameters MakeParameters<T>(T schema) where T : IStoredProcSchema {
            var parameters = new DynamicParameters();
            // Input parameters (Include IEnumerable as Table type value)
            var inputProperties = schema.GetType().GetProperties().Where(attr => Attribute.IsDefined(attr, typeof(InputParameterAttribute)));
            foreach(var propertyInfo in inputProperties) {
                var key = propertyInfo.Name;
                var value = propertyInfo.GetValue(schema, null);
                if(value != null) {
                    var atrb = (InputParameterAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(InputParameterAttribute));
                    if(atrb != null && !string.IsNullOrWhiteSpace(atrb.Name))
                        key = atrb.Name;

                    if(propertyInfo.PropertyType.Name.Contains("IEnumerable")) {
                        var genericType = propertyInfo.PropertyType.GetGenericArguments().Single();
                        var genericTypeName = genericType.Name;

                        var genericTypeAttrs = (InputParameterAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(InputParameterAttribute));
                        if(atrb != null && !string.IsNullOrWhiteSpace(genericTypeAttrs.Name))
                            genericTypeName = genericTypeAttrs.Name;

                        var dataTable = new DataTable(genericTypeName);
                        foreach(var column in genericType.GetProperties().Where(attr => !Attribute.IsDefined(attr, typeof(NotMappedAttribute))))
                            dataTable.Columns.Add(column.Name);

                        var list = (System.Collections.IEnumerable)propertyInfo.GetValue(schema);
                        foreach(var row in list) {
                            var genericTypeProps = row.GetType().GetProperties();
                            var dataRow = dataTable.NewRow();
                            for(var i = 0; i < dataTable.Columns.Count; i++)
                                dataRow[i] = genericTypeProps[i].GetValue(row);
                            dataTable.Rows.Add(dataRow);
                        }
                        parameters.Add(key, dataTable.AsTableValuedParameter(genericTypeName), DbType.Object, direction: ParameterDirection.Input);
                    }
                    else
                        parameters.Add(key, value, propertyInfo.PropertyType.ToDbType(), direction: ParameterDirection.Input);
                }
            }

            // Output parameters
            var outputProperties = schema.GetType().GetProperties()?.Where(attr => Attribute.IsDefined(attr, typeof(OutputParameterAttribute)));
            foreach(var propertyInfo in outputProperties) {
                var key = propertyInfo.Name;
                var attrs = propertyInfo.GetCustomAttributes(true);
                foreach(var attr in attrs) {
                    if(attr is OutputParameterAttribute output && !string.IsNullOrWhiteSpace(output.Name))
                        key = output.Name;
                }
                parameters.Add(key, dbType: propertyInfo.PropertyType.ToDbType(), direction: ParameterDirection.Output);
            }

            // Return parameter
            var returnPropery = schema.GetType().GetProperties().FirstOrDefault(attr => Attribute.IsDefined(attr, typeof(ReturnParameterAttribute)));
            if(returnPropery != null) {
                parameters.Add(returnPropery.Name, dbType: returnPropery.PropertyType.ToDbType(), direction: ParameterDirection.ReturnValue);
            }
            return parameters;
        }

        public void SetOutputValues<T>(T model, DynamicParameters parameters) where T : IStoredProcSchema {
            var outputProperties = model.GetType().GetProperties().Where(item => Attribute.IsDefined(item, typeof(OutputParameterAttribute)));
            foreach(var propertyInfo in outputProperties) {
                var key = propertyInfo.Name;
                var atrb = (InputParameterAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(InputParameterAttribute));
                if(atrb != null && !string.IsNullOrWhiteSpace(atrb.Name))
                    key = atrb.Name;

                if(propertyInfo.PropertyType == typeof(byte))
                    propertyInfo.SetValue(model, parameters.Get<byte>(key));
                else if(propertyInfo.PropertyType == typeof(byte?))
                    propertyInfo.SetValue(model, parameters.Get<byte?>(key));
                else if(propertyInfo.PropertyType == typeof(byte[]))
                    propertyInfo.SetValue(model, parameters.Get<byte[]>(key));
                else if(propertyInfo.PropertyType == typeof(sbyte))
                    propertyInfo.SetValue(model, parameters.Get<sbyte>(key));
                else if(propertyInfo.PropertyType == typeof(sbyte?))
                    propertyInfo.SetValue(model, parameters.Get<sbyte?>(key));
                else if(propertyInfo.PropertyType == typeof(short))
                    propertyInfo.SetValue(model, parameters.Get<short>(key));
                else if(propertyInfo.PropertyType == typeof(short?))
                    propertyInfo.SetValue(model, parameters.Get<short?>(key));
                else if(propertyInfo.PropertyType == typeof(ushort))
                    propertyInfo.SetValue(model, parameters.Get<ushort>(key));
                else if(propertyInfo.PropertyType == typeof(ushort?))
                    propertyInfo.SetValue(model, parameters.Get<ushort?>(key));
                else if(propertyInfo.PropertyType == typeof(int))
                    propertyInfo.SetValue(model, parameters.Get<int>(key));
                else if(propertyInfo.PropertyType == typeof(int?))
                    propertyInfo.SetValue(model, parameters.Get<int?>(key));
                else if(propertyInfo.PropertyType == typeof(uint))
                    propertyInfo.SetValue(model, parameters.Get<uint>(key));
                else if(propertyInfo.PropertyType == typeof(uint?))
                    propertyInfo.SetValue(model, parameters.Get<uint?>(key));
                else if(propertyInfo.PropertyType == typeof(long))
                    propertyInfo.SetValue(model, parameters.Get<long>(key));
                else if(propertyInfo.PropertyType == typeof(long?))
                    propertyInfo.SetValue(model, parameters.Get<long?>(key));
                else if(propertyInfo.PropertyType == typeof(ulong))
                    propertyInfo.SetValue(model, parameters.Get<ulong>(key));
                else if(propertyInfo.PropertyType == typeof(ulong?))
                    propertyInfo.SetValue(model, parameters.Get<ulong?>(key));
                else if(propertyInfo.PropertyType == typeof(float))
                    propertyInfo.SetValue(model, parameters.Get<float>(key));
                else if(propertyInfo.PropertyType == typeof(float?))
                    propertyInfo.SetValue(model, parameters.Get<float?>(key));
                else if(propertyInfo.PropertyType == typeof(double))
                    propertyInfo.SetValue(model, parameters.Get<double>(key));
                else if(propertyInfo.PropertyType == typeof(double?))
                    propertyInfo.SetValue(model, parameters.Get<double?>(key));
                else if(propertyInfo.PropertyType == typeof(decimal))
                    propertyInfo.SetValue(model, parameters.Get<decimal>(key));
                else if(propertyInfo.PropertyType == typeof(decimal?))
                    propertyInfo.SetValue(model, parameters.Get<decimal?>(key));
                else if(propertyInfo.PropertyType == typeof(bool))
                    propertyInfo.SetValue(model, parameters.Get<bool>(key));
                else if(propertyInfo.PropertyType == typeof(bool?))
                    propertyInfo.SetValue(model, parameters.Get<bool?>(key));
                else if(propertyInfo.PropertyType == typeof(string))
                    propertyInfo.SetValue(model, parameters.Get<string>(key));
                else if(propertyInfo.PropertyType == typeof(char))
                    propertyInfo.SetValue(model, parameters.Get<char>(key));
                else if(propertyInfo.PropertyType == typeof(char?))
                    propertyInfo.SetValue(model, parameters.Get<char?>(key));
                else if(propertyInfo.PropertyType == typeof(Guid))
                    propertyInfo.SetValue(model, parameters.Get<Guid>(key));
                else if(propertyInfo.PropertyType == typeof(Guid?))
                    propertyInfo.SetValue(model, parameters.Get<Guid?>(key));
                else if(propertyInfo.PropertyType == typeof(DateTime))
                    propertyInfo.SetValue(model, parameters.Get<DateTime>(key));
                else if(propertyInfo.PropertyType == typeof(DateTime?))
                    propertyInfo.SetValue(model, parameters.Get<DateTime?>(key));
                else if(propertyInfo.PropertyType == typeof(DateTimeOffset))
                    propertyInfo.SetValue(model, parameters.Get<DateTimeOffset>(key));
                else if(propertyInfo.PropertyType == typeof(DateTimeOffset?))
                    propertyInfo.SetValue(model, parameters.Get<DateTimeOffset?>(key));
            }
        }

        public void SetReturnValue<T>(T model, DynamicParameters parameters) where T : IStoredProcSchema {
            var returnProperty = model.GetType().GetProperties().FirstOrDefault(item => Attribute.IsDefined(item, typeof(ReturnParameterAttribute)));
            if(returnProperty != null) {
                returnProperty.SetValue(model, parameters.Get<int>(returnProperty.Name));
            }
        }

        public void SetTotalCount<Schema, Result>(Schema model, IEnumerable<Result> result)
            where Schema : IStoredProcSchema
            where Result : IStoredProcResult {

            if(result.Any()) {
                var node = result.FirstOrDefault();
                var resulttotalcount = node.GetType().GetProperty(nameof(PagingResult.TotalCount));
                var schematotalcount = model.GetType().GetProperty(nameof(PagingSchema.TotalCount));

                if(schematotalcount != null && resulttotalcount != null) {
                    schematotalcount.SetValue(model, resulttotalcount.GetValue(node, null));
                }
            }
        }
    }
}
