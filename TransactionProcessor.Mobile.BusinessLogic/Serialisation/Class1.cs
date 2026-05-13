using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace TransactionProcessor.Mobile.BusinessLogic.Serialisation
{
    public static class ExpressionHelpers
    {
        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            if (expression.Body is MemberExpression member)
                return member.Member.Name;

            if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
                return unaryMember.Member.Name;

            throw new ArgumentException("Expression must be a property access", nameof(expression));
        }
    }

    public static class Extensions
    {
        public static JsonSerializerOptions AddModifier(
            this JsonSerializerOptions options,
            Action<JsonTypeInfo> modifier)
        {
            if (options.TypeInfoResolver is not DefaultJsonTypeInfoResolver resolver)
                throw new InvalidOperationException("TypeInfoResolver must be DefaultJsonTypeInfoResolver");

            resolver.Modifiers.Add(modifier);
            return options;
        }
    }

    public static class JsonTypeInfoExtensions
    {
        public static void IgnoreProperty<T>(
            this JsonTypeInfo typeInfo,
            Expression<Func<T, object>> selector)
        {
            var name = ExpressionHelpers.GetPropertyName(selector);

            var prop = typeInfo.Properties.FirstOrDefault(p =>
                string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));

            if (prop != null)
            {
                prop.ShouldSerialize = (_, _) => false;
            }
        }

        public static void RenameProperty<T>(
            this JsonTypeInfo typeInfo,
            Expression<Func<T, object>> selector,
            string newName)
        {
            var name = ExpressionHelpers.GetPropertyName(selector);

            var prop = typeInfo.Properties.FirstOrDefault(p =>
                string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));

            if (prop != null)
            {
                prop.Name = newName;
            }
        }
    }

    public static class JsonTypeInfoModifierExtensions
    {
        /// <summary>
        /// Apply a modifier only to a specific type T
        /// </summary>
        public static Action<JsonTypeInfo> ForType<T>(Action<JsonTypeInfo> modifier)
        {
            return typeInfo =>
            {
                if (typeInfo.Type == typeof(T))
                {
                    modifier(typeInfo);
                }
            };
        }

        /// <summary>
        /// Apply a modifier to multiple specific types
        /// </summary>
        public static Action<JsonTypeInfo> ForTypes(IEnumerable<Type> types, Action<JsonTypeInfo> modifier)
        {
            var set = new HashSet<Type>(types);

            return typeInfo =>
            {
                if (set.Contains(typeInfo.Type))
                {
                    modifier(typeInfo);
                }
            };
        }

        /// <summary>
        /// Apply a modifier to a type and all types assignable to it (base class / interface)
        /// </summary>
        public static Action<JsonTypeInfo> ForAssignableTo<TBase>(Action<JsonTypeInfo> modifier)
        {
            return typeInfo =>
            {
                if (typeof(TBase).IsAssignableFrom(typeInfo.Type))
                {
                    modifier(typeInfo);
                }
            };
        }

        /// <summary>
        /// Combine multiple modifiers into one
        /// </summary>
        public static Action<JsonTypeInfo> Combine(params Action<JsonTypeInfo>[] modifiers)
        {
            return typeInfo =>
            {
                foreach (var modifier in modifiers)
                {
                    modifier(typeInfo);
                }
            };
        }
    }

    public enum SerialiserPropertyFormat
    {
        CamelCase,
        SnakeCase,
        CamelCaseUpper,
        KebabCase,
        KeabCaseUpper,
    }
    public record SerialiserOptions(SerialiserPropertyFormat PropertyFormat, Boolean IgnoreNullValues = true, Boolean WriteIndented = false);

    public interface IStringSerialiser
    {
        string Serialize<T>(T obj, SerialiserOptions serialiserOptions = null);
        T Deserialize<T>(string json, SerialiserOptions serialiserOptions = null);

        T DeserializeAnonymousType<T>(string json,
                                      T anonymousTypeObject, SerialiserOptions serialiserOptions = null);

        T DeserializeObject<T>(string json, Type type, SerialiserOptions serialiserOptions = null);

        T? GetValue<T>(string json, string propertyName);
    }

    public class SystemTextJsonSerializer : IStringSerialiser
    {
        private readonly JsonSerializerOptions Options;

        public static JsonSerializerOptions GetDefaultJsonSerializerOptions()
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                WriteIndented = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers = {
                    typeInfo => {
                        String[] names = new[] { "AggregateId", "AggregateVersion", "EventId", "EventNumber", "EventTimestamp", "EventType" };
                        List<JsonPropertyInfo> matches = typeInfo.Properties.Where(p => names.Any(n => string.Equals(p.Name, n, StringComparison.OrdinalIgnoreCase))).ToList();

                        foreach (JsonPropertyInfo match in matches) {
                            match.ShouldSerialize = (_,
                                                     _) => false;
                        }
                    }
                }
                }
            };
            options.Converters.Add(new DateTimeSpaceConverter());

            return options;
        }

        public SystemTextJsonSerializer(JsonSerializerOptions options)
        {
            Options = options;
        }

        private JsonSerializerOptions BuildSerialiserOptions(SerialiserOptions serialiserOptions)
        {
            if (serialiserOptions == null)
            {
                return Options;
            }

            var options = new JsonSerializerOptions(Options);
            if (serialiserOptions.IgnoreNullValues)
            {
                options.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            }
            if (serialiserOptions.WriteIndented)
            {
                options.WriteIndented = true;
            }
            options.PropertyNamingPolicy = serialiserOptions.PropertyFormat switch
            {
                SerialiserPropertyFormat.CamelCase => JsonNamingPolicy.CamelCase,
                SerialiserPropertyFormat.SnakeCase => JsonNamingPolicy.SnakeCaseLower,
                SerialiserPropertyFormat.CamelCaseUpper => JsonNamingPolicy.SnakeCaseUpper,
                SerialiserPropertyFormat.KebabCase => JsonNamingPolicy.KebabCaseLower,
                SerialiserPropertyFormat.KeabCaseUpper => JsonNamingPolicy.KebabCaseUpper,
                _ => options.PropertyNamingPolicy
            };
            return options;
        }

        public string Serialize<T>(T obj, SerialiserOptions serialiserOptions = null)
        {

            var options = BuildSerialiserOptions(serialiserOptions);
            return obj is null
                ? JsonSerializer.Serialize(obj, options)
                : JsonSerializer.Serialize(obj, obj.GetType(), options);
        }

        public T Deserialize<T>(string json, SerialiserOptions serialiserOptions = null)
        {
            var options = BuildSerialiserOptions(serialiserOptions);
            return JsonSerializer.Deserialize<T>(json, options)!;
        }

        public T DeserializeAnonymousType<T>(String json,
                                             T anonymousTypeObject, SerialiserOptions serialiserOptions = null)
        {
            var options = BuildSerialiserOptions(serialiserOptions);
            return JsonSerializer.Deserialize<T>(json, options)!;
        }

        public T DeserializeObject<T>(String json,
                                      Type type, SerialiserOptions serialiserOptions = null)
        {
            var options = BuildSerialiserOptions(serialiserOptions);
            return (T)JsonSerializer.Deserialize(json, type, options)!;
        }

        public T GetValue<T>(String json,
                             String propertyName)
        {
            using var doc = JsonDocument.Parse(json);
            var _root = doc.RootElement.Clone(); // important to avoid disposed doc issues

            if (TryFindProperty(_root, propertyName, out var value))
            {
                try
                {
                    return value.Deserialize<T>();
                }
                catch
                {
                    return default;
                }
            }

            return default;
        }

        private static bool TryFindProperty(
            JsonElement element,
            string propertyName,
            out JsonElement found)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var prop in element.EnumerateObject())
                    {
                        if (string.Equals(prop.Name, propertyName, StringComparison.OrdinalIgnoreCase))
                        {
                            found = prop.Value;
                            return true;
                        }

                        if (TryFindProperty(prop.Value, propertyName, out found))
                            return true;
                    }
                    break;

                case JsonValueKind.Array:
                    foreach (var item in element.EnumerateArray())
                    {
                        if (TryFindProperty(item, propertyName, out found))
                            return true;
                    }
                    break;
                default:
                    found = default;
                    return false;
                    break;
            }

            found = default;
            return false;
        }
    }

    public static class StringSerialiser
    {
        private const String NotInitialisedErrorMessage = "StringSerialiser is not initialised.";
        public static Boolean IsInitialised { get; set; }
        private static IStringSerialiser Serializer;

        public static void Initialise(IStringSerialiser serialiser)
        {
            Serializer = serialiser;
            IsInitialised = true;
        }

        public static string Serialise<T>(T obj, SerialiserOptions serialiserOptions = null)
        {
            if (!IsInitialised) throw new InvalidOperationException(NotInitialisedErrorMessage);
            return Serializer.Serialize(obj, serialiserOptions);
        }

        public static T Deserialise<T>(string json, SerialiserOptions serialiserOptions = null)
        {
            if (!IsInitialised) throw new InvalidOperationException(NotInitialisedErrorMessage);
            return Serializer.Deserialize<T>(json, serialiserOptions);
        }

        public static T DeserialiseAnonymousType<T>(String json, T anonymousTypeObject, SerialiserOptions serialiserOptions = null)
        {
            if (!IsInitialised) throw new InvalidOperationException(NotInitialisedErrorMessage);
            return Serializer.DeserializeAnonymousType(json, anonymousTypeObject, serialiserOptions);
        }

        public static T DeserializeObject<T>(String json, Type type, SerialiserOptions serialiserOptions = null)
        {
            if (!IsInitialised) throw new InvalidOperationException(NotInitialisedErrorMessage);
            return Serializer.DeserializeObject<T>(json, type, serialiserOptions);
        }

        public static T GetValue<T>(String json, String propertyName, SerialiserOptions serialiserOptions = null)
        {
            if (!IsInitialised) throw new InvalidOperationException(NotInitialisedErrorMessage);
            return Serializer.GetValue<T>(json, propertyName);
        }
    }

    public class DateTimeSpaceConverter : JsonConverter<DateTime>
    {
        private static readonly string[] AcceptedFormats = new[] {
                "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd H:mm:ss", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mm:ss.FFFFFFFK", "o" // ISO 8601 round-trip
                                                                                                                                                                                };
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return default;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString();
                if (string.IsNullOrWhiteSpace(s))
                    return default;

                // Try exact known formats first (handles "2026-05-07 06:03:18")
                if (DateTime.TryParseExact(s, AcceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces, out var dtExact))
                    return dtExact;

                // Fall back to general parse
                if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var dt))
                    return dt;

                throw new JsonException($"Unable to parse DateTime: '{s}'.");
            }

            // If JSON contains a number, attempt to treat it as Unix seconds (optional)
            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out long seconds))
            {
                return DateTimeOffset.FromUnixTimeSeconds(seconds).LocalDateTime;
            }

            throw new JsonException($"Unexpected token parsing DateTime. Token: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Write in the same "space" format so round-trip matches your input
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        }
    }
}
