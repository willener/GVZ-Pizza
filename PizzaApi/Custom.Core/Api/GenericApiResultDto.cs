namespace Custom.Database.Data;

/// <summary>
/// Data transfer objects (from controller to frontend).
/// </summary>
public class GenericApiResultDto {

    /// <summary>
    /// Gets or sets if the Action was successful.
    /// </summary>
    public bool Success { get; set; } = true;

    /// <summary>
    /// Gets or sets the result.
    /// </summary>
    public int Result { get; set; } = 0;

    /// <summary>
    /// Gets or sets the summary info.
    /// </summary>
    public string? SummaryInfo { get; set; }

    /// <summary>
    /// Gets or sets the list of detail infos.
    /// </summary>
    public List<string> DetailInfos { get; set; } = [];
}
