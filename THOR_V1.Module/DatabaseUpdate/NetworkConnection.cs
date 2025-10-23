using DevExpress.XtraPrinting.Native;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace THOR_V1.Module.DatabaseUpdate
{
    public class NetworkConnection : IDisposable
    {
        string _networkName;

        const int CONNECT_UPDATE_PROFILE = 0x1;
        const int CONNECT_INTERACTIVE = 0x8;
        const int CONNECT_PROMPT = 0x10;
        const int CONNECT_REDIRECT = 0x80;
        const int CONNECT_COMMANDLINE = 0x800;
        const int CONNECT_CMD_SAVECRED = 0x1000;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int GetLastError();

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NetResource netResource,
            string password, string username, int flags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags,
            bool force);

        public NetworkConnection()
        {
        }

        public NetworkConnection(string localName, string networkName,
            NetworkCredential credentials)
        {
            _networkName = networkName;

            var netResource = new NetResource()
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                LocalName = localName,
                DisplayType = ResourceDisplaytype.Share,
                RemoteName = networkName
            };

            int flags = CONNECT_UPDATE_PROFILE;

            var result = WNetAddConnection2(
                netResource,
                credentials.Password,
                credentials.UserName,
                flags);


            if (result != 0)
            {
                Win32Exception winex = new Win32Exception(result);
                throw new IOException("Error connecting to remote share",
                    winex);
            }
        }

        ~NetworkConnection()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //WNetCancelConnection2(_networkName, 0, true); 
        }


    }



    [StructLayout(LayoutKind.Sequential)]
    public class NetResource
    {
        public ResourceScope Scope;
        public ResourceType ResourceType;
        public ResourceDisplaytype DisplayType;
        public int Usage;
        public string LocalName;
        public string RemoteName;
        public string Comment;
        public string Provider;
    }

}

public enum ResourceScope : int
{
    Connected = 1,
    GlobalNetwork,
    Remembered,
    Recent,
    Context
};

public enum ResourceType : int
{
    Any = 0,
    Disk = 1,
    Print = 2,
    Reserved = 8,
}

public enum ResourceDisplaytype : int
{
    Generic = 0x0,
    Domain = 0x01,
    Server = 0x02,
    Share = 0x03,
    File = 0x04,
    Group = 0x05,
    Network = 0x06,
    Root = 0x07,
    Shareadmin = 0x08,
    Directory = 0x09,
    Tree = 0x0a,
    Ndscontainer = 0x0b
}

/// <summary>
/// MVC 5 dosnt support impersonation via web.config for good reasons microsoft decided one day
/// </summary>
internal static class NativeMethods
{
    [DllImport("kernel32.dll")]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [SuppressUnmanagedCodeSecurity]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool CloseHandle(IntPtr handle);

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
        int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);
}

public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private SafeTokenHandle()
        : base(true)
    {
    }

    protected override bool ReleaseHandle()
    {
        return NativeMethods.CloseHandle(handle);
    }
}

class ClassAccessFolder
{
    public void MoveFolderLockedFolder(string sourceFolder, string distinationFolder, string Username, string DomainName, string Password)
    {
        const int LOGON32_PROVIDER_DEFAULT = 0;
        //This parameter causes LogonUser to create a primary token. 
        const int LOGON32_LOGON_INTERACTIVE = 2;
        SafeTokenHandle safeTokenHandle;

        // Call LogonUser to obtain a handle to an access token. 
        bool returnValue = NativeMethods.LogonUser(
            Username,
            DomainName,
            Password,
            LOGON32_LOGON_INTERACTIVE,
            LOGON32_PROVIDER_DEFAULT,
            out safeTokenHandle);

        if (!returnValue)
        {
            //throw new Exception("unable to login as specifed user :" + Username);
        }
        //using (WindowsIdentity id = new WindowsIdentity(safeTokenHandle.DangerousGetHandle()))
        //using (System.Security.Principal.WindowsImpersonationContext wic = id.Impersonate())
        //{

        //    DirectoryInfo di;
        //    foreach (String subFolders in Directory.GetDirectories(sourceFolder))
        //    {
        //        di = new DirectoryInfo(subFolders);
        //        di.MoveTo(distinationFolder);
        //    }
        //}
    }

    public void CopyFileFromLocalToServer(string sourceFolder, string distinationFolder, string Username, string DomainName, string Password)
    {
        const int LOGON32_PROVIDER_DEFAULT = 0;
        //This parameter causes LogonUser to create a primary token. 
        const int LOGON32_LOGON_INTERACTIVE = 2;
        SafeTokenHandle safeTokenHandle;

        // Call LogonUser to obtain a handle to an access token. 
        bool returnValue = NativeMethods.LogonUser(
            Username,
            DomainName,
            Password,
            LOGON32_LOGON_INTERACTIVE,
            LOGON32_PROVIDER_DEFAULT,
            out safeTokenHandle);
    }
}
