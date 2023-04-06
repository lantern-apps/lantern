namespace Lantern.Win32.Interop;
#pragma warning disable CA2255 // 不应在库中使用 “ModuleInitializer” 属性

[Flags()]
internal enum FILEOPENDIALOGOPTIONS
{
    FOS_OVERWRITEPROMPT = 0x00000002,
    FOS_STRICTFILETYPES = 0x00000004,
    FOS_NOCHANGEDIR = 0x00000008,
    FOS_PICKFOLDERS = 0x00000020,
    FOS_FORCEFILESYSTEM = 0x00000040,
    FOS_ALLNONSTORAGEITEMS = 0x00000080,
    FOS_NOVALIDATE = 0x00000100,
    FOS_ALLOWMULTISELECT = 0x00000200,
    FOS_PATHMUSTEXIST = 0x00000800,
    FOS_FILEMUSTEXIST = 0x00001000,
    FOS_CREATEPROMPT = 0x00002000,
    FOS_SHAREAWARE = 0x00004000,
    FOS_NOREADONLYRETURN = 0x00008000,
    FOS_NOTESTFILECREATE = 0x00010000,
    FOS_HIDEMRUPLACES = 0x00020000,
    FOS_HIDEPINNEDPLACES = 0x00040000,
    FOS_NODEREFERENCELINKS = 0x00100000,
    FOS_DONTADDTORECENT = 0x02000000,
    FOS_FORCESHOWHIDDEN = 0x10000000,
    FOS_DEFAULTNOMINIMODE = 0x20000000
}

internal unsafe partial interface IShellItem : global::MicroCom.Runtime.IUnknown
{
    void* BindToHandler(void* pbc, System.Guid* bhid, System.Guid* riid);
    IShellItem Parent { get; }

    System.Char* GetDisplayName(uint sigdnName);
    uint GetAttributes(uint sfgaoMask);
    int Compare(IShellItem psi, uint hint);
}

internal unsafe partial interface IShellItemArray : global::MicroCom.Runtime.IUnknown
{
    void* BindToHandler(void* pbc, System.Guid* bhid, System.Guid* riid);
    void* GetPropertyStore(ushort flags, System.Guid* riid);
    void* GetPropertyDescriptionList(void* keyType, System.Guid* riid);
    ushort GetAttributes(int AttribFlags, ushort sfgaoMask);
    int Count { get; }

    IShellItem GetItemAt(int dwIndex);
    void* EnumItems();
}

internal unsafe partial interface IModalWindow : global::MicroCom.Runtime.IUnknown
{
    int Show(IntPtr hwndOwner);
}

internal unsafe partial interface IFileDialog : IModalWindow
{
    void SetFileTypes(ushort cFileTypes, void* rgFilterSpec);
    void SetFileTypeIndex(ushort iFileType);
    ushort FileTypeIndex { get; }

    int Advise(void* pfde);
    void Unadvise(int dwCookie);
    void SetOptions(FILEOPENDIALOGOPTIONS fos);
    FILEOPENDIALOGOPTIONS Options { get; }

    void SetDefaultFolder(IShellItem psi);
    void SetFolder(IShellItem psi);
    IShellItem Folder { get; }

    IShellItem CurrentSelection { get; }

    void SetFileName(System.Char* pszName);
    System.Char* FileName { get; }

    void SetTitle(System.Char* pszTitle);
    void SetOkButtonLabel(System.Char* pszText);
    void SetFileNameLabel(System.Char* pszLabel);
    IShellItem Result { get; }

    void AddPlace(IShellItem psi, int fdap);
    void SetDefaultExtension(System.Char* pszDefaultExtension);
    void Close(int hr);
    void SetClientGuid(System.Guid* guid);
    void ClearClientData();
    void SetFilter(void* pFilter);
}

internal unsafe partial class __MicroComIShellItemProxy : global::MicroCom.Runtime.MicroComProxyBase, IShellItem
{
    public void* BindToHandler(void* pbc, System.Guid* bhid, System.Guid* riid)
    {
        int __result;
        void* ppv = default;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, void*, void*, void*, int>)(*PPV)[base.VTableSize + 0])(PPV, pbc, bhid, riid, &ppv);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("BindToHandler failed", __result);
        return ppv;
    }

    public IShellItem Parent
    {
        get
        {
            int __result;
            void* __marshal_ppsi = null;
            __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 1])(PPV, &__marshal_ppsi);
            if (__result != 0)
                throw new System.Runtime.InteropServices.COMException("GetParent failed", __result);
            return global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<IShellItem>(__marshal_ppsi, true);
        }
    }

    public System.Char* GetDisplayName(uint sigdnName)
    {
        int __result;
        System.Char* ppszName = default;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, uint, void*, int>)(*PPV)[base.VTableSize + 2])(PPV, sigdnName, &ppszName);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("GetDisplayName failed", __result);
        return ppszName;
    }

    public uint GetAttributes(uint sfgaoMask)
    {
        int __result;
        uint psfgaoAttribs = default;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, uint, void*, int>)(*PPV)[base.VTableSize + 3])(PPV, sfgaoMask, &psfgaoAttribs);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("GetAttributes failed", __result);
        return psfgaoAttribs;
    }

    public int Compare(IShellItem psi, uint hint)
    {
        int __result;
        int piOrder = default;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, uint, void*, int>)(*PPV)[base.VTableSize + 4])(PPV, global::MicroCom.Runtime.MicroComRuntime.GetNativePointer(psi), hint, &piOrder);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("Compare failed", __result);
        return piOrder;
    }

    [System.Runtime.CompilerServices.ModuleInitializer()]
    internal static void __MicroComModuleInit()
    {
        global::MicroCom.Runtime.MicroComRuntime.Register(typeof(IShellItem), new Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe"), (p, owns) => new __MicroComIShellItemProxy(p, owns));
    }

    protected __MicroComIShellItemProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
    {
    }

    protected override int VTableSize => base.VTableSize + 5;
}

internal unsafe partial interface IFileOpenDialog : IFileDialog
{
    IShellItemArray Results { get; }

    IShellItemArray SelectedItems { get; }
}

internal unsafe partial class __MicroComIModalWindowProxy : global::MicroCom.Runtime.MicroComProxyBase, IModalWindow
{
    public int Show(IntPtr hwndOwner)
    {
        int __result;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, IntPtr, int>)(*PPV)[base.VTableSize + 0])(PPV, hwndOwner);
        return __result;
    }

    [System.Runtime.CompilerServices.ModuleInitializer()]
    internal static void __MicroComModuleInit()
    {
        global::MicroCom.Runtime.MicroComRuntime.Register(typeof(IModalWindow), new Guid("B4DB1657-70D7-485E-8E3E-6FCB5A5C1802"), (p, owns) => new __MicroComIModalWindowProxy(p, owns));
    }

    protected __MicroComIModalWindowProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
    {
    }

    protected override int VTableSize => base.VTableSize + 1;
}

internal unsafe partial class __MicroComIFileDialogProxy : __MicroComIModalWindowProxy, IFileDialog
{
    public void SetFileTypes(ushort cFileTypes, void* rgFilterSpec)
    {
        int __result;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, ushort, void*, int>)(*PPV)[base.VTableSize + 0])(PPV, cFileTypes, rgFilterSpec);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("SetFileTypes failed", __result);
    }

    public void SetFileTypeIndex(ushort iFileType)
    {
        int __result;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, ushort, int>)(*PPV)[base.VTableSize + 1])(PPV, iFileType);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("SetFileTypeIndex failed", __result);
    }

    public ushort FileTypeIndex
    {
        get
        {
            int __result;
            ushort piFileType = default;
            __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 2])(PPV, &piFileType);
            if (__result != 0)
                throw new System.Runtime.InteropServices.COMException("GetFileTypeIndex failed", __result);
            return piFileType;
        }
    }

    public int Advise(void* pfde)
    {
        int __result;
        int pdwCookie = default;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, void*, int>)(*PPV)[base.VTableSize + 3])(PPV, pfde, &pdwCookie);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("Advise failed", __result);
        return pdwCookie;
    }

    public void Unadvise(int dwCookie)
    {
        int __result;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, int, int>)(*PPV)[base.VTableSize + 4])(PPV, dwCookie);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("Unadvise failed", __result);
    }

    public void SetOptions(FILEOPENDIALOGOPTIONS fos)
    {
        int __result;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, FILEOPENDIALOGOPTIONS, int>)(*PPV)[base.VTableSize + 5])(PPV, fos);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("SetOptions failed", __result);
    }

    public FILEOPENDIALOGOPTIONS Options
    {
        get
        {
            int __result;
            FILEOPENDIALOGOPTIONS pfos = default;
            __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 6])(PPV, &pfos);
            if (__result != 0)
                throw new System.Runtime.InteropServices.COMException("GetOptions failed", __result);
            return pfos;
        }
    }

    public void SetDefaultFolder(IShellItem psi)
    {
        int __result;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 7])(PPV, global::MicroCom.Runtime.MicroComRuntime.GetNativePointer(psi));
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("SetDefaultFolder failed", __result);
    }

    public void SetFolder(IShellItem psi)
    {
        int __result;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 8])(PPV, global::MicroCom.Runtime.MicroComRuntime.GetNativePointer(psi));
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("SetFolder failed", __result);
    }

    public IShellItem Folder
    {
        get
        {
            int __result;
            void* __marshal_ppsi = null;
            __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 9])(PPV, &__marshal_ppsi);
            if (__result != 0)
                throw new System.Runtime.InteropServices.COMException("GetFolder failed", __result);
            return global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<IShellItem>(__marshal_ppsi, true);
        }
    }

    public IShellItem CurrentSelection
    {
        get
        {
            int __result;
            void* __marshal_ppsi = null;
            __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 10])(PPV, &__marshal_ppsi);
            if (__result != 0)
                throw new System.Runtime.InteropServices.COMException("GetCurrentSelection failed", __result);
            return global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<IShellItem>(__marshal_ppsi, true);
        }
    }

    public void SetFileName(System.Char* pszName)
    {
        int __result;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 11])(PPV, pszName);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("SetFileName failed", __result);
    }

    public System.Char* FileName
    {
        get
        {
            int __result;
            System.Char* pszName = default;
            __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 12])(PPV, &pszName);
            if (__result != 0)
                throw new System.Runtime.InteropServices.COMException("GetFileName failed", __result);
            return pszName;
        }
    }

    public void SetTitle(System.Char* pszTitle)
    {
        int __result;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 13])(PPV, pszTitle);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("SetTitle failed", __result);
    }

    public void SetOkButtonLabel(System.Char* pszText)
    {
        int __result;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 14])(PPV, pszText);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("SetOkButtonLabel failed", __result);
    }

    public void SetFileNameLabel(System.Char* pszLabel)
    {
        int __result;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 15])(PPV, pszLabel);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("SetFileNameLabel failed", __result);
    }

    public IShellItem Result
    {
        get
        {
            int __result;
            void* __marshal_ppsi = null;
            __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 16])(PPV, &__marshal_ppsi);
            if (__result != 0)
                throw new System.Runtime.InteropServices.COMException("GetResult failed", __result);
            return global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<IShellItem>(__marshal_ppsi, true);
        }
    }

    public void AddPlace(IShellItem psi, int fdap)
    {
        int __result;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int, int>)(*PPV)[base.VTableSize + 17])(PPV, global::MicroCom.Runtime.MicroComRuntime.GetNativePointer(psi), fdap);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("AddPlace failed", __result);
    }

    public void SetDefaultExtension(System.Char* pszDefaultExtension)
    {
        int __result;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 18])(PPV, pszDefaultExtension);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("SetDefaultExtension failed", __result);
    }

    public void Close(int hr)
    {
        int __result;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, int, int>)(*PPV)[base.VTableSize + 19])(PPV, hr);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("Close failed", __result);
    }

    public void SetClientGuid(System.Guid* guid)
    {
        int __result;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 20])(PPV, guid);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("SetClientGuid failed", __result);
    }

    public void ClearClientData()
    {
        int __result;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, int>)(*PPV)[base.VTableSize + 21])(PPV);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("ClearClientData failed", __result);
    }

    public void SetFilter(void* pFilter)
    {
        int __result;
        __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 22])(PPV, pFilter);
        if (__result != 0)
            throw new System.Runtime.InteropServices.COMException("SetFilter failed", __result);
    }

    [System.Runtime.CompilerServices.ModuleInitializer()]
    internal static void __MicroComModuleInit2()
    {
        global::MicroCom.Runtime.MicroComRuntime.Register(typeof(IFileDialog), new Guid("42F85136-DB7E-439C-85F1-E4075D135FC8"), (p, owns) => new __MicroComIFileDialogProxy(p, owns));
    }

    protected __MicroComIFileDialogProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
    {
    }

    protected override int VTableSize => base.VTableSize + 23;
}
