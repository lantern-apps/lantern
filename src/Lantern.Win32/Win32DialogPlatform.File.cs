using Lantern.Platform;
using Lantern.Win32.Interop;
using MicroCom.Runtime;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Lantern.Win32;

internal partial class Win32DialogPlatform : IDialogPlatform
{
    private const uint SIGDN_FILESYSPATH = 0x80058000;

    private const FILEOPENDIALOGOPTIONS DefaultDialogOptions = FILEOPENDIALOGOPTIONS.FOS_FORCEFILESYSTEM | FILEOPENDIALOGOPTIONS.FOS_NOVALIDATE |
        FILEOPENDIALOGOPTIONS.FOS_NOTESTFILECREATE | FILEOPENDIALOGOPTIONS.FOS_DONTADDTORECENT;

    public async Task<string[]> OpenFolderAsync(IWindowImpl? parent, FolderPickerOpenOptions options)
    {
        var files = await ShowFilePicker(parent,
            true, true,
            options.AllowMultiple, false,
            options.Title, null, options.SuggestedStartLocation, null, null);
        return files.ToArray();
    }

    public async Task<string[]> OpenFileAsync(IWindowImpl? parent, FilePickerOpenOptions options)
    {
        var files = await ShowFilePicker(parent,
            true, false,
            options.AllowMultiple, false,
            options.Title, null, options.SuggestedStartLocation,
            null, options.FileTypeFilter);
        return files.ToArray();
    }

    public async Task<string?> SaveFileAsync(IWindowImpl? parent, FilePickerSaveOptions options)
    {
        var files = await ShowFilePicker(parent,
            false, false,
            false, options.ShowOverwritePrompt,
            options.Title, options.SuggestedFileName, options.SuggestedStartLocation,
            options.DefaultExtension, options.FileTypeChoices);
        return files.FirstOrDefault();
    }

    private unsafe Task<IEnumerable<string>> ShowFilePicker(
        IWindowImpl? parent,
        bool isOpenFile,
        bool openFolder,
        bool allowMultiple,
        bool? showOverwritePrompt,
        string? title,
        string? suggestedFileName,
        string? folder,
        string? defaultExtension,
        IReadOnlyList<FilePickerFileType>? filters)
    {
        return Task.Run(() =>
        {
            IEnumerable<string> result = Array.Empty<string>();
            try
            {
                var clsid = isOpenFile ? NativeMethods.ShellIds.OpenFileDialog : NativeMethods.ShellIds.SaveFileDialog;
                var iid = NativeMethods.ShellIds.IFileDialog;
                var frm = NativeUtilities.CreateInstance<IFileDialog>(ref clsid, ref iid);

                var options = frm.Options;
                options |= DefaultDialogOptions;
                if (openFolder)
                {
                    options |= FILEOPENDIALOGOPTIONS.FOS_PICKFOLDERS;
                }
                if (allowMultiple)
                {
                    options |= FILEOPENDIALOGOPTIONS.FOS_ALLOWMULTISELECT;
                }

                if (showOverwritePrompt == false)
                {
                    options &= ~FILEOPENDIALOGOPTIONS.FOS_OVERWRITEPROMPT;
                }
                frm.SetOptions(options);

                if (defaultExtension is not null)
                {
                    fixed (char* pExt = defaultExtension)
                    {
                        frm.SetDefaultExtension(pExt);
                    }
                }

                suggestedFileName ??= "";
                fixed (char* fExt = suggestedFileName)
                {
                    frm.SetFileName(fExt);
                }

                title ??= "";
                fixed (char* tExt = title)
                {
                    frm.SetTitle(tExt);
                }

                if (!openFolder)
                {
                    fixed (void* pFilters = FiltersToPointer(filters, out var count))
                    {
                        frm.SetFileTypes((ushort)count, pFilters);
                        if (count > 0)
                        {
                            frm.SetFileTypeIndex(0);
                        }
                    }
                }

                if (folder != null)
                {
                    var riid = NativeMethods.ShellIds.IShellItem;
                    if (NativeMethods.SHCreateItemFromParsingName(folder, IntPtr.Zero, ref riid, out var directoryShellItem)
                        == (uint)NativeMethods.HRESULT.S_OK)
                    {
                        var proxy = MicroComRuntime.CreateProxyFor<IShellItem>(directoryShellItem, true);
                        frm.SetFolder(proxy);
                        frm.SetDefaultFolder(proxy);
                    }
                }

                var showResult = frm.Show(parent?.NativeHandle ?? 0);
                //var showResult = frm.Show(0);

                if ((uint)showResult == (uint)NativeMethods.HRESULT.E_CANCELLED)
                {
                    return result;
                }
                else if ((uint)showResult != (uint)NativeMethods.HRESULT.S_OK)
                {
                    throw new Win32Exception(showResult);
                }

                if (allowMultiple)
                {
                    using var fileOpenDialog = frm.QueryInterface<IFileOpenDialog>();
                    var shellItemArray = fileOpenDialog.Results;
                    var count = shellItemArray.Count;

                    var results = new List<string>();
                    for (int i = 0; i < count; i++)
                    {
                        var shellItem = shellItemArray.GetItemAt(i);
                        if (GetAbsoluteFilePath(shellItem) is { } selected)
                        {
                            results.Add(selected);
                        }
                    }

                    result = results;
                }
                else if (frm.Result is { } shellItem
                    && GetAbsoluteFilePath(shellItem) is { } singleResult)
                {
                    result = new[] { singleResult };
                }

                return result;
            }
            catch (COMException ex)
            {
                var message = new Win32Exception(ex.HResult).Message;
                throw new COMException(message, ex);
            }
        })!;
    }

    private static unsafe string? GetAbsoluteFilePath(IShellItem shellItem)
    {
        var pszString = new IntPtr(shellItem.GetDisplayName(SIGDN_FILESYSPATH));
        if (pszString != IntPtr.Zero)
        {
            try
            {
                return Marshal.PtrToStringUni(pszString);
            }
            finally
            {
                Marshal.FreeCoTaskMem(pszString);
            }
        }
        return default;
    }

    private static byte[] FiltersToPointer(IReadOnlyList<FilePickerFileType>? filters, out int length)
    {
        if (filters == null || filters.Count == 0)
        {
            filters = new List<FilePickerFileType> { FilePickerFileTypes.All };
        }

        var size = Marshal.SizeOf<NativeMethods.COMDLG_FILTERSPEC>();
        var arr = new byte[size];
        var resultArr = new byte[size * filters.Count];

        for (int i = 0; i < filters.Count; i++)
        {
            var filter = filters[i];
            if (filter.Patterns is null || !filter.Patterns.Any())
            {
                continue;
            }

            var filterPtr = Marshal.AllocHGlobal(size);
            try
            {
                var filterStr = new NativeMethods.COMDLG_FILTERSPEC
                {
                    pszName = filter.Name ?? string.Empty,
                    pszSpec = string.Join(";", filter.Patterns)
                };

                Marshal.StructureToPtr(filterStr, filterPtr, false);
                Marshal.Copy(filterPtr, resultArr, i * size, size);
            }
            finally
            {
                Marshal.FreeHGlobal(filterPtr);
            }
        }

        length = filters.Count;
        return resultArr;
    }
}
