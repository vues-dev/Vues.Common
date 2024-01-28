using Asp.Versioning;

namespace Vues.Common;

public abstract class VuesApiDescription
{
    /// <summary>
    /// Список версий Api приложения
    /// </summary>
    public abstract ApiVersion[] ApiVersions { get;}

    public abstract int DefaultMajorApiVersion {get;}
    public abstract int DefaultMinorApiVersion {get;}


    public virtual ApiVersion Get(int major, int minor)
    {
        var apiVersion = ApiVersions.FirstOrDefault(x => x.MajorVersion == major && x.MinorVersion == minor);

        if (apiVersion is null)
        {
            throw new InvalidOperationException($"API версия {major}.{minor} не существует.");
        }

        return apiVersion;
    }
}
