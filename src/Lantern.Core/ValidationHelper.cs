using Lantern.Windows;

namespace Lantern;

public static class ValidationHelper
{
    public static string ValidatePathQualified(string path)
    {
        if (Path.IsPathFullyQualified(path))
            return path;

        var fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        if (!Path.IsPathFullyQualified(fullpath))
        {
            ThrowHelper.ThrowInvaidDirectoryPathException(path);
        }

        return fullpath;
    }

    public static string ValidateDirectoryExists(string folderPath)
    {
        if (Path.IsPathFullyQualified(folderPath))
        {
            if (!Directory.Exists(folderPath))
            {
                ThrowHelper.ThrowDirectoryNotFoundException(folderPath);
            }
            return folderPath;
        }
        else
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderPath);

            if (!Path.IsPathFullyQualified(path))
            {
                ThrowHelper.ThrowInvaidDirectoryPathException(folderPath);
            }

            if (!Directory.Exists(folderPath))
            {
                ThrowHelper.ThrowDirectoryNotFoundException(folderPath);
            }

            return path;
        }
    }

    public static string ValidateFileExists(string filePath)
    {
        if (Path.IsPathFullyQualified(filePath))
        {
            if (!File.Exists(filePath))
            {
                ThrowHelper.ThrowFileNotFoundException(filePath);
            }
            return filePath;
        }
        else
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            if (!Path.IsPathFullyQualified(path))
            {
                ThrowHelper.ThrowInvaidFileNameException(filePath);
            }
            return path;
        }
    }

    public static void Validate(WebViewWindowOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Name))
        {
            throw new ArgumentException("Window name cannot be null or empty.");
        }

        if (options.Url != null && !Uri.IsWellFormedUriString(options.Url, UriKind.Absolute))
        {
            throw new ArgumentException($"Invaild Url '{options.Url}'");
        }

        if (options.Width.HasValue && options.Width < 0)
        {
            throw new ArgumentException($"Invaild Width '{options.Width}'");
        }

        if (options.Height.HasValue && options.Width < 0)
        {
            throw new ArgumentException($"Invaild Height '{options.Height}'");
        }

        if (options.MaxWidth.HasValue && options.MaxWidth < 0)
        {
            throw new ArgumentException($"Invaild MaxWidth '{options.MaxWidth}'");
        }

        if (options.MaxHeight.HasValue && options.MaxHeight < 0)
        {
            throw new ArgumentException($"Invaild MaxHeight '{options.MaxHeight}'");
        }

        if (options.MinWidth.HasValue && options.MinWidth < 0)
        {
            throw new ArgumentException($"Invaild MinWidth '{options.MinWidth}'");
        }

        if (options.MinHeight.HasValue && options.MinHeight < 0)
        {
            throw new ArgumentException($"Invaild MinHeight '{options.MinHeight}'");
        }

        if (options.IconPath != null)
        {
            options.IconPath = ValidateFileExists(options.IconPath);
        }

    }

}