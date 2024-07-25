namespace Custom.Core.Api;

/// <summary>
/// API Helper class.
/// </summary>
public static class ApiHelper {

    /// <summary>
    /// Gets a field data DTO by field name.
    /// </summary>
    /// <param name="fieldDataArray"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static FieldDataDto? GetFieldData(FieldDataDto[]? fieldDataArray, string? fieldName) {
        if (fieldName == null) return null;
        return fieldDataArray?.FirstOrDefault(f => string.Compare(f.FieldName, fieldName, StringComparison.InvariantCultureIgnoreCase) == 0);
    }

    /// <summary>
    /// Gets a field data DTO by field name.
    /// </summary>
    /// <param name="fieldDataArray"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static FieldDataDto<T>? GetFieldData<T>(FieldDataDto[]? fieldDataArray, string? fieldName) where T : notnull, IParsable<T> {
        if (fieldName == null) return null;
        return fieldDataArray?
            .Where(f => string.Compare(f.FieldName, fieldName, StringComparison.InvariantCultureIgnoreCase) == 0)
            .Select(f => new FieldDataDto<T>() { FieldName = f.FieldName, ValueString = f.ValueString })
            .FirstOrDefault();
    }

    /// <summary>
    /// Gets a string value from field data DTO.
    /// </summary>
    /// <param name="fieldDataArray"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static string? GetStringValue(FieldDataDto[]? fieldDataArray, string? fieldName) {
        return GetFieldData(fieldDataArray, fieldName)?.ValueString;
    }

    /// <summary>
    /// Gets an integer value from field data DTO.
    /// </summary>
    /// <param name="fieldDataArray"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static int? GetIntValue(FieldDataDto[]? fieldDataArray, string? fieldName) {
        var fieldData = GetFieldData(fieldDataArray, fieldName);
        return GetIntValue(fieldData);
    }

    /// <summary>
    /// Gets an integer value from field data DTO.
    /// </summary>
    /// <param name="fieldData"></param>
    /// <returns></returns>
    public static int? GetIntValue(FieldDataDto? fieldData) {
        string? valueString = fieldData?.ValueString;
        if (string.IsNullOrWhiteSpace(valueString)) return null;
        return int.TryParse(valueString, out int result) ? result : null;
    }

    /// <summary>
    /// Gets a double value from field data DTO.
    /// </summary>
    /// <param name="fieldDataArray"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static double? GetDoubleValue(FieldDataDto[]? fieldDataArray, string? fieldName) {
        var fieldData = GetFieldData(fieldDataArray, fieldName);
        return GetDoubleValue(fieldData);
    }

    /// <summary>
    /// Gets a double value from field data DTO.
    /// </summary>
    /// <param name="fieldData"></param>
    /// <returns></returns>
    public static double? GetDoubleValue(FieldDataDto? fieldData) {
        string? valueString = fieldData?.ValueString;
        if (string.IsNullOrWhiteSpace(valueString)) return null;
        return double.TryParse(valueString, out double result) ? result : null;
    }

}