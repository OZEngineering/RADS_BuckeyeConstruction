namespace Buckeye.Common.DataTransferObjects
{
 
        using System;
        using System.Collections.Generic;

        using System.Globalization;
        using Newtonsoft.Json;
        using Newtonsoft.Json.Converters;

    public class LineItem
    {
        public partial class LineObject
        {
            [JsonProperty("objectIdFieldName")]
            public string ObjectIdFieldName { get; set; }

            [JsonProperty("uniqueIdField")]
            public UniqueIdField UniqueIdField { get; set; }

            [JsonProperty("globalIdFieldName")]
            public string GlobalIdFieldName { get; set; }

            [JsonProperty("geometryProperties")]
            public GeometryProperties GeometryProperties { get; set; }

            [JsonProperty("geometryType")]
            public string GeometryType { get; set; }

            [JsonProperty("spatialReference")]
            public SpatialReference SpatialReference { get; set; }

            [JsonProperty("fields")]
            public Field[] Fields { get; set; }

            [JsonProperty("features")]
            public Feature[] Features { get; set; }
        }

        public partial class Feature
        {
            [JsonProperty("attributes")]
            public Attributes Attributes { get; set; }

            [JsonProperty("geometry")]
            public Geometry Geometry { get; set; }
        }

        public partial class Attributes
        {
            [JsonProperty("OBJECTID")]
            public long Objectid { get; set; }

            [JsonProperty("STREET")]
            public string Street { get; set; }

            [JsonProperty("DESCRIPTION")]
            public string Description { get; set; }

            [JsonProperty("LOCATION")]
            public string Location { get; set; }

            [JsonProperty("LANES")]
            public Lanes Lanes { get; set; }

            [JsonProperty("REASON")]
            public Reason? Reason { get; set; }

            [JsonProperty("STATUS")]
            public Status? Status { get; set; }

            [JsonProperty("STARTDATE")]
            public long? Startdate { get; set; }

            [JsonProperty("ENDDATE")]
            public long? Enddate { get; set; }

            [JsonProperty("EVENT")]
            public object Event { get; set; }

            [JsonProperty("CONTACTNAME")]
            public Contactname? Contactname { get; set; }

            [JsonProperty("CONTACTEMAIL")]
            public Contactemail? Contactemail { get; set; }

            [JsonProperty("CONTACTPHONE")]
            public Contactphone? Contactphone { get; set; }

            [JsonProperty("GlobalID")]
            public Guid GlobalId { get; set; }

            [JsonProperty("Shape__Length")]
            public double ShapeLength { get; set; }
        }

        public partial class Geometry
        {
            [JsonProperty("paths")]
            public double[][][] Paths { get; set; }
        }

        public partial class Field
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("alias")]
            public string Alias { get; set; }

            [JsonProperty("sqlType")]
            public SqlType SqlType { get; set; }

            [JsonProperty("domain")]
            public Domain Domain { get; set; }

            [JsonProperty("defaultValue")]
            public object DefaultValue { get; set; }

            [JsonProperty("length", NullValueHandling = NullValueHandling.Ignore)]
            public long? Length { get; set; }
        }

        public partial class Domain
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("mergePolicy")]
            public string MergePolicy { get; set; }

            [JsonProperty("splitPolicy")]
            public string SplitPolicy { get; set; }

            [JsonProperty("codedValues")]
            public CodedValue[] CodedValues { get; set; }
        }

        public partial class CodedValue
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }
        }

        public partial class GeometryProperties
        {
            [JsonProperty("shapeLengthFieldName")]
            public string ShapeLengthFieldName { get; set; }

            [JsonProperty("units")]
            public string Units { get; set; }
        }

        public partial class SpatialReference
        {
            [JsonProperty("wkid")]
            public long Wkid { get; set; }

            [JsonProperty("latestWkid")]
            public long LatestWkid { get; set; }
        }

        public partial class UniqueIdField
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("isSystemMaintained")]
            public bool IsSystemMaintained { get; set; }
        }

        public enum Contactemail { RlintonBuckeyeazGov };

        public enum Contactname { RobertLinton };

        public enum Contactphone { Contactphone6232588057, The6232588057 };

        public enum Lanes { AllLanesClosed, LanesPartiallyBlocked, Warning };

        public enum Reason { Construction, SpecialEvent, Warning };

        public enum Status { Active, Inactive };

        public enum SqlType { SqlTypeDouble, SqlTypeOther };

        public static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
            {
                ContactemailConverter.Singleton,
                ContactnameConverter.Singleton,
                ContactphoneConverter.Singleton,
                LanesConverter.Singleton,
                ReasonConverter.Singleton,
                StatusConverter.Singleton,
                SqlTypeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            };
        }

        internal class ContactemailConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(Contactemail) || t == typeof(Contactemail?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                if (value == "rlinton@buckeyeaz.gov")
                {
                    return Contactemail.RlintonBuckeyeazGov;
                }
                throw new Exception("Cannot unmarshal type Contactemail");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (Contactemail)untypedValue;
                if (value == Contactemail.RlintonBuckeyeazGov)
                {
                    serializer.Serialize(writer, "rlinton@buckeyeaz.gov");
                    return;
                }
                throw new Exception("Cannot marshal type Contactemail");
            }

            public static readonly ContactemailConverter Singleton = new ContactemailConverter();
        }

        internal class ContactnameConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(Contactname) || t == typeof(Contactname?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                if (value == "Robert Linton")
                {
                    return Contactname.RobertLinton;
                }
                throw new Exception("Cannot unmarshal type Contactname");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (Contactname)untypedValue;
                if (value == Contactname.RobertLinton)
                {
                    serializer.Serialize(writer, "Robert Linton");
                    return;
                }
                throw new Exception("Cannot marshal type Contactname");
            }

            public static readonly ContactnameConverter Singleton = new ContactnameConverter();
        }

        internal class ContactphoneConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(Contactphone) || t == typeof(Contactphone?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "(623) 258-8057":
                        return Contactphone.The6232588057;
                    case "623-258-8057":
                        return Contactphone.Contactphone6232588057;
                }
                throw new Exception("Cannot unmarshal type Contactphone");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (Contactphone)untypedValue;
                switch (value)
                {
                    case Contactphone.The6232588057:
                        serializer.Serialize(writer, "(623) 258-8057");
                        return;
                    case Contactphone.Contactphone6232588057:
                        serializer.Serialize(writer, "623-258-8057");
                        return;
                }
                throw new Exception("Cannot marshal type Contactphone");
            }

            public static readonly ContactphoneConverter Singleton = new ContactphoneConverter();
        }

        internal class LanesConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(Lanes) || t == typeof(Lanes?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "ALL LANES CLOSED":
                        return Lanes.AllLanesClosed;
                    case "LANES PARTIALLY BLOCKED":
                        return Lanes.LanesPartiallyBlocked;
                    case "WARNING":
                        return Lanes.Warning;
                }
                throw new Exception("Cannot unmarshal type Lanes");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (Lanes)untypedValue;
                switch (value)
                {
                    case Lanes.AllLanesClosed:
                        serializer.Serialize(writer, "ALL LANES CLOSED");
                        return;
                    case Lanes.LanesPartiallyBlocked:
                        serializer.Serialize(writer, "LANES PARTIALLY BLOCKED");
                        return;
                    case Lanes.Warning:
                        serializer.Serialize(writer, "WARNING");
                        return;
                }
                throw new Exception("Cannot marshal type Lanes");
            }

            public static readonly LanesConverter Singleton = new LanesConverter();
        }

        internal class ReasonConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(Reason) || t == typeof(Reason?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "CONSTRUCTION":
                        return Reason.Construction;
                    case "SPECIAL EVENT":
                        return Reason.SpecialEvent;
                    case "WARNING":
                        return Reason.Warning;
                }
                throw new Exception("Cannot unmarshal type Reason");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (Reason)untypedValue;
                switch (value)
                {
                    case Reason.Construction:
                        serializer.Serialize(writer, "CONSTRUCTION");
                        return;
                    case Reason.SpecialEvent:
                        serializer.Serialize(writer, "SPECIAL EVENT");
                        return;
                    case Reason.Warning:
                        serializer.Serialize(writer, "WARNING");
                        return;
                }
                throw new Exception("Cannot marshal type Reason");
            }

            public static readonly ReasonConverter Singleton = new ReasonConverter();
        }

        internal class StatusConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(Status) || t == typeof(Status?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "ACTIVE":
                        return Status.Active;
                    case "INACTIVE":
                        return Status.Inactive;
                }
                throw new Exception("Cannot unmarshal type Status");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (Status)untypedValue;
                switch (value)
                {
                    case Status.Active:
                        serializer.Serialize(writer, "ACTIVE");
                        return;
                    case Status.Inactive:
                        serializer.Serialize(writer, "INACTIVE");
                        return;
                }
                throw new Exception("Cannot marshal type Status");
            }

            public static readonly StatusConverter Singleton = new StatusConverter();
        }

        internal class SqlTypeConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(SqlType) || t == typeof(SqlType?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "sqlTypeDouble":
                        return SqlType.SqlTypeDouble;
                    case "sqlTypeOther":
                        return SqlType.SqlTypeOther;
                }
                throw new Exception("Cannot unmarshal type SqlType");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (SqlType)untypedValue;
                switch (value)
                {
                    case SqlType.SqlTypeDouble:
                        serializer.Serialize(writer, "sqlTypeDouble");
                        return;
                    case SqlType.SqlTypeOther:
                        serializer.Serialize(writer, "sqlTypeOther");
                        return;
                }
                throw new Exception("Cannot marshal type SqlType");
            }

            public static readonly SqlTypeConverter Singleton = new SqlTypeConverter();
        }
    }
}
