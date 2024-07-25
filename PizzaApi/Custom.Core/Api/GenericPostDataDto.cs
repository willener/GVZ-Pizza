using System.Globalization;

namespace Custom.Core.Api;

/// <summary>
/// DTO for generic post data.
/// </summary>
public class GenericPostDataDto {

    /// <summary>
    /// Gets or sets the array of field values.
    /// </summary>
    public required FieldDataDto[] FieldDataArray { get; set; }

}

/// <summary>
/// DTO for field data.
/// </summary>
public class FieldDataDto {

    /// <summary>
    /// Gets or sets the name of the API field.
    /// </summary>
    public required string FieldName { get; set; }

    /// <summary>
    /// Gets or sets the string representation of the value.
    /// </summary>
    public string? ValueString { get; set; }

}

/// <summary>
/// DTO for field data.
/// </summary>
public class FieldDataDto<T> : FieldDataDto where T : notnull, IParsable<T> {

    /// <summary>
    /// The culture used to parse and serialize values.
    /// </summary>
    public static readonly CultureInfo CULTURE_PARSE = new("de-CH");

    /// <summary>
    /// Gets the parsed value.
    /// </summary>
    public T? Value {
        get {
            if (!m_valueSpecified) {
                m_value = ValueString == null ? default : T.Parse(ValueString, CULTURE_PARSE);
                m_valueSpecified = true;
            }
            return m_value;
        }
    }

    private T? m_value;
    private bool m_valueSpecified;
}
