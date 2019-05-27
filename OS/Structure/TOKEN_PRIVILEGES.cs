using System.Runtime.InteropServices;
namespace NK.OS.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TOKEN_PRIVILEGES
    {
        /// <summary>
        /// Specifies the number of entries in the Privileges array.
        /// 指定了权限数组的容量
        /// </summary>
        public int PrivilegeCount;
        /// <summary>
        /// Specifies an array of LUID_AND_ATTRIBUTES structures. Each structure contains the LUID and attributes of a privilege.
        /// 指定一组的LUID_AND_ATTRIBUTES 结构，每个结构包含了LUID和权限的属性
        /// </summary>
        public LUID_AND_ATTRIBUTES Privileges;
    }
}
