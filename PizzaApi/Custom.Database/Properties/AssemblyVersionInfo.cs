// (c) IMT - Information Management Technology AG, CH-9470 Buchs, www.imt.ch. 

namespace Custom.Database {

    /// <summary>
    /// Defines common version information for Custom assemblies.
    /// </summary>
    /// <remarks>
    /// The following attributes needs to be added to the property group in project file:
    /// <GenerateAssemblyCompanyAttribute>false</GenerateAssemblyCompanyAttribute>
    /// <GenerateAssemblyProductAttribute>false</GenerateAssemblyProductAttribute>
    /// <GenerateAssemblyCopyrightAttribute>false</GenerateAssemblyCopyrightAttribute>
    /// <GenerateAssemblyVersionAttribute>false</GenerateAssemblyVersionAttribute>
    /// <GenerateAssemblyFileVersionAttribute>false</GenerateAssemblyFileVersionAttribute>
    /// <GenerateNeutralResourcesLanguageAttribute>false</GenerateNeutralResourcesLanguageAttribute>
    /// To allow wildcards in AssemblyVersion:
    /// <Deterministic>false</Deterministic>
    /// </remarks>
    public static class AssemblyVersionInfo {

        /// <summary>
        /// Gets the company.
        /// </summary>
        public const string COMPANY = "GVZ AG";

        /// <summary>
        /// Gets the product name.
        /// </summary>
        public const string PRODUCT = "Pizza Shop API";

        /// <summary>
        /// Gets the copy right notice.
        /// 
        /// </summary>
        public const string COPYRIGHT = "Copyright © 2024";

        /// <summary>
        /// Gets the current assembly version containing following parts:
        /// - Major Version
        /// - Minor Version
        /// - Build Number    (if "*" provided: days since 01.01.2000 (automatically set during compilation)
        /// - Revision        (if "*" provided: 2 * Seconds since midnight(automatically set during compilation)
        /// 
        /// Build und Revision are generated automatically when "*" is provided: e. g. "1.0.*"
        /// 
        /// Attention: to support wildcards set <Deterministic>false</Deterministic> in project property group
        /// </summary>
        public const string VERSION = "1.0.*";

        /// <summary>
        /// Gets the current build file version containing following parts:
        /// - Major Version
        /// - Minor Version
        /// - Build Number    (optional)
        /// - Revision        (optional)
        /// </summary>
        public const string FILE_VERSION = "1.0";

        /// <summary>
        /// Gets the neutral resources language.
        /// </summary>
        public const string NEUTRAL_RESOURCES_LANGUAGE = "en-US";
    }
}
