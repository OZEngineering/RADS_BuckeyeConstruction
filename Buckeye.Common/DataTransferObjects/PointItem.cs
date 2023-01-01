namespace Buckeye.Common.DataTransferObjects
{
 
        using System;
        using System.Collections.Generic;

        using System.Globalization;
        using Newtonsoft.Json;
        using Newtonsoft.Json.Converters;

    public partial class PointItem
    {
        public partial class PointObject
        {
            [JsonProperty("objectIdFieldName")]
            public string ObjectIdFieldName { get; set; }

            [JsonProperty("uniqueIdField")]
            public UniqueIdField UniqueIdField { get; set; }

            [JsonProperty("globalIdFieldName")]
            public string GlobalIdFieldName { get; set; }

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
            public PointAttributes attribute { get; set; }

            [JsonProperty("geometry")]
            public PointGeometry geometry { get; set; }
        }

        public partial class PointAttributes
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
            public PointLanes? Lanes { get; set; }

            [JsonProperty("REASON")]
            public PointReason Reason { get; set; }

            [JsonProperty("STATUS")]
            public PointStatus? Status { get; set; }

            [JsonProperty("STARTDATE")]
            public long? Startdate { get; set; }

            [JsonProperty("ENDDATE")]
            public long? Enddate { get; set; }

            [JsonProperty("EVENT")]
            public object Event { get; set; }

            [JsonProperty("CONTACTNAME")]
            public PointContact? Contactname { get; set; }

            [JsonProperty("CONTACTEMAIL")]
            public PointContactemail? Contactemail { get; set; }

            [JsonProperty("CONTACTPHONE")]
            public PointContact? Contactphone { get; set; }

            [JsonProperty("GlobalID")]
            public Guid GlobalId { get; set; }
        }

        public partial class PointGeometry
        {
            [JsonProperty("x")]
            public double X { get; set; }

            [JsonProperty("y")]
            public double Y { get; set; }
        }

        public partial class Field
        {
            [JsonProperty("name")]
            public string FieldName { get; set; }

            [JsonProperty("type")]
            public string FieldType { get; set; }

            [JsonProperty("alias")]
            public string FieldAlias { get; set; }

            [JsonProperty("sqlType")]
            public PointSqlType FieldSqlType { get; set; }

            [JsonProperty("domain")]
            public Domain FieldDomain { get; set; }

            [JsonProperty("defaultValue")]
            public object FieldDefault { get; set; }

            [JsonProperty("length", NullValueHandling = NullValueHandling.Ignore)]
            public long? FieldLength { get; set; }
        }

        public partial class Domain
        {
            [JsonProperty("type")]
            public string DomainType { get; set; }

            [JsonProperty("name")]
            public string DomainName { get; set; }

            [JsonProperty("mergePolicy")]
            public string DomainMergePolicy { get; set; }

            [JsonProperty("splitPolicy")]
            public string DomainSplitPolicy { get; set; }

            [JsonProperty("codedValues")]
            public CodedValue[] PomainCodedValues { get; set; }
        }

        public partial class CodedValue
        {
            [JsonProperty("name")]
            public string ValueName { get; set; }

            [JsonProperty("code")]
            public string ValueCode { get; set; }
        }

        public partial class SpatialReference
        {
            [JsonProperty("wkid")]
            public long PointWkid { get; set; }

            [JsonProperty("latestWkid")]
            public long PointLatestWkid { get; set; }
        }

        public partial class UniqueIdField
        {
            [JsonProperty("name")]
            public string UnoqueName { get; set; }

            [JsonProperty("isSystemMaintained")]
            public bool UniqueIsSystemMaintained { get; set; }
        }

        public enum PointContactemail { RlintonBuckeyeazGov, The6232588057 };

        public enum PointContact { Contact6232588057, RobertLinton, The6232588057 };

        public enum PointLanes { AllLanesClosed, LanesPartiallyBlocked, Warning };

        public enum PointReason { Construction, SpecialEvent, Warning };

        public enum PointStatus { Active, Inactive };

        public enum PointSqlType { SqlTypeOther, sqlTypeDouble };

        public static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
                {
                    PointContactemailConverter.Singleton,
                    PointContactConverter.Singleton,
                    PointLanesConverter.Singleton,
                    PointReasonConverter.Singleton,
                    PointStatusConverter.Singleton,
                    PointSqlTypeConverter.Singleton,
                    new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
                },
            };
        }

        internal class PointContactemailConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(PointContactemail) || t == typeof(PointContactemail?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "623-258-8057":
                        return PointContactemail.The6232588057;
                    case "rlinton@buckeyeaz.gov":
                        return PointContactemail.RlintonBuckeyeazGov;
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
                var value = (PointContactemail)untypedValue;
                switch (value)
                {
                    case PointContactemail.The6232588057:
                        serializer.Serialize(writer, "623-258-8057");
                        return;
                    case PointContactemail.RlintonBuckeyeazGov:
                        serializer.Serialize(writer, "rlinton@buckeyeaz.gov");
                        return;
                }
                throw new Exception("Cannot marshal type Contactemail");
            }

            public static readonly PointContactemailConverter Singleton = new PointContactemailConverter();
        }

        internal class PointContactConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(PointContact) || t == typeof(PointContact?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "(623) 258-8057":
                        return PointContact.The6232588057;
                    case "623-258-8057":
                        return PointContact.Contact6232588057;
                    case "Robert Linton":
                        return PointContact.RobertLinton;
                }
                throw new Exception("Cannot unmarshal type Contact");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (PointContact)untypedValue;
                switch (value)
                {
                    case PointContact.The6232588057:
                        serializer.Serialize(writer, "(623) 258-8057");
                        return;
                    case PointContact.Contact6232588057:
                        serializer.Serialize(writer, "623-258-8057");
                        return;
                    case PointContact.RobertLinton:
                        serializer.Serialize(writer, "Robert Linton");
                        return;
                }
                throw new Exception("Cannot marshal type Contact");
            }

            public static readonly PointContactConverter Singleton = new PointContactConverter();
        }

        internal class PointLanesConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(PointLanes) || t == typeof(PointLanes?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "ALL LANES CLOSED":
                        return PointLanes.AllLanesClosed;
                    case "LANES PARTIALLY BLOCKED":
                        return PointLanes.LanesPartiallyBlocked;
                    case "WARNING":
                        return PointLanes.Warning;
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
                var value = (PointLanes)untypedValue;
                switch (value)
                {
                    case PointLanes.AllLanesClosed:
                        serializer.Serialize(writer, "ALL LANES CLOSED");
                        return;
                    case PointLanes.LanesPartiallyBlocked:
                        serializer.Serialize(writer, "LANES PARTIALLY BLOCKED");
                        return;
                    case PointLanes.Warning:
                        serializer.Serialize(writer, "WARNING");
                        return;
                }
                throw new Exception("Cannot marshal type Lanes");
            }

            public static readonly PointLanesConverter Singleton = new PointLanesConverter();
        }

        internal class PointReasonConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(PointReason) || t == typeof(PointReason?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "CONSTRUCTION":
                        return PointReason.Construction;
                    case "SPECIAL EVENT":
                        return PointReason.SpecialEvent;
                    case "WARNING":
                        return PointReason.Warning;
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
                var value = (PointReason)untypedValue;
                switch (value)
                {
                    case PointReason.Construction:
                        serializer.Serialize(writer, "CONSTRUCTION");
                        return;
                    case PointReason.SpecialEvent:
                        serializer.Serialize(writer, "SPECIAL EVENT");
                        return;
                    case PointReason.Warning:
                        serializer.Serialize(writer, "WARNING");
                        return;
                }
                throw new Exception("Cannot marshal type Reason");
            }

            public static readonly PointReasonConverter Singleton = new PointReasonConverter();
        }

        internal class PointStatusConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(PointStatus) || t == typeof(PointStatus?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "ACTIVE":
                        return PointStatus.Active;
                    case "INACTIVE":
                        return PointStatus.Inactive;
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
                var value = (PointStatus)untypedValue;
                switch (value)
                {
                    case PointStatus.Active:
                        serializer.Serialize(writer, "ACTIVE");
                        return;
                    case PointStatus.Inactive:
                        serializer.Serialize(writer, "INACTIVE");
                        return;
                }
                throw new Exception("Cannot marshal type Status");
            }

            public static readonly PointStatusConverter Singleton = new PointStatusConverter();
        }

        internal class PointSqlTypeConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(PointSqlType) || t == typeof(PointSqlType?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                if (value == "sqlTypeOther")
                {
                    return PointSqlType.SqlTypeOther;
                }
                if (value == "sqlTypeDouble")
                {
                    return PointSqlType.sqlTypeDouble;
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
                var value = (PointSqlType)untypedValue;
                if (value == PointSqlType.SqlTypeOther)
                {
                    serializer.Serialize(writer, "sqlTypeOther");
                    return;
                }
                if (value == PointSqlType.sqlTypeDouble)
                {
                    serializer.Serialize(writer, "sqlTypeDouble");
                    return;
                }
                throw new Exception("Cannot marshal type SqlType");
            }

            public static readonly PointSqlTypeConverter Singleton = new PointSqlTypeConverter();
        }

/*
        public class PointItem
        {
            public class FieldAliases
            {
                public string OBJECTID { get; set; }
                public string InspectorName { get; set; }
                public string InspectorPhoneNumber { get; set; }
                public string StartDate { get; set; }
                public string EndDate { get; set; }
                public string EventType { get; set; }
                public string FullClosure { get; set; }
                public string Active { get; set; }
                public string Location { get; set; }
                public string RestrictionBoundaries { get; set; }
                public string RestrictionDetails { get; set; }
                public string Direction { get; set; }
                public string AlternateRoute { get; set; }
                public string ProjectName { get; set; }
                public string AssociatedPermit { get; set; }
                public string BarricadeCompany { get; set; }
                public string BarricadePhoneNumber { get; set; }
                public string Barricade24HourContact { get; set; }
                public string Contractor { get; set; }
                public string ContractorPhoneNumber { get; set; }
                public string Contractor24HourContact { get; set; }
                public string DescriptionOfWork { get; set; }
                public string OfficerRequiredAtIntersection { get; set; }
                public string EmergencyAccessMaintained { get; set; }
                public string PostToAZ511 { get; set; }
                public string GlobalID { get; set; }
                public string DateToPost { get; set; }
                public string DateToClose { get; set; }
            }

            public class SpatialReference
            {
                public int wkid { get; set; }
                public int latestWkid { get; set; }
            }

            public class Field
            {
                public string name { get; set; }
                public string type { get; set; }
                public string alias { get; set; }
                public int? length { get; set; }
            }

            public class Attributes
            {
                public int OBJECTID { get; set; }
                public string InspectorName { get; set; }
                public string InspectorPhoneNumber { get; set; }
                public object StartDate { get; set; }
                public object EndDate { get; set; }
                public string EventType { get; set; }
                public string FullClosure { get; set; }
                public string Active { get; set; }
                public string Location { get; set; }
                public string RestrictionBoundaries { get; set; }
                public string RestrictionDetails { get; set; }
                public string Direction { get; set; }
                public object AlternateRoute { get; set; }
                public string ProjectName { get; set; }
                public string AssociatedPermit { get; set; }
                public string BarricadeCompany { get; set; }
                public object BarricadePhoneNumber { get; set; }
                public object Barricade24HourContact { get; set; }
                public string Contractor { get; set; }
                public object ContractorPhoneNumber { get; set; }
                public object Contractor24HourContact { get; set; }
                public string DescriptionOfWork { get; set; }
                public string OfficerRequiredAtIntersection { get; set; }
                public string EmergencyAccessMaintained { get; set; }
                public string PostToAZ511 { get; set; }
                public string GlobalID { get; set; }
                public object DateToPost { get; set; }
                public object DateToClose { get; set; }
                public long? created_date { get; set; }
            }

            public class Geometry
            {
                public double x { get; set; }
                public double y { get; set; }
            }

            public class Feature
            {
                public Attributes attributes { get; set; }
                public Geometry geometry { get; set; }
            }

            public class PointObject
            {
                public string displayFieldName { get; set; }
                public FieldAliases fieldAliases { get; set; }
                public string geometryType { get; set; }
                public SpatialReference spatialReference { get; set; }
                public List<Field> fields { get; set; }
                public List<Feature> features { get; set; }
            }
        }
*/
    }

}
