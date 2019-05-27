using System.Runtime.InteropServices;
namespace NK.API.Struct
{
    /// <summary>
    /// Contains information about the current state of both physical and virtual memory. The GlobalMemoryStatus function stores information in a MEMORYSTATUS structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORYSTATUS
   {
        /// <summary>
        /// The size of the MEMORYSTATUS data structure, in bytes. You do not need to set this member before calling the GlobalMemoryStatus function; 
        /// </summary>
        public int dwLength;
        /// <summary>
        /// A number between 0 and 100 that specifies the approximate percentage of physical memory that is in use (0 indicates no memory use and 100 indicates full memory use).
        /// </summary>
        public int dwMemoryLoad;
        /// <summary>
        /// The amount of actual physical memory, in bytes.
        /// </summary>
        public int dwTotalPhys;
        /// <summary>
        /// The amount of physical memory currently available, in bytes. This is the amount of physical memory that can be immediately reused without having to write its contents to disk first. It is the sum of the size of the standby, free, and zero lists.
        /// </summary>
        public int dwAvailPhys;
        /// <summary>
        /// The current size of the committed memory limit, in bytes. This is physical memory plus the size of the page file, minus a small overhead.
        /// </summary>
        public int dwTotalPageFile;
        /// <summary>
        /// The maximum amount of memory the current process can commit, in bytes. This value should be smaller than the system-wide available commit. To calculate this value, call GetPerformanceInfo and subtract the value of CommitTotal from CommitLimit.
        /// </summary>
        public int dwAvailPageFile;
        /// <summary>
        /// The size of the user-mode portion of the virtual address space of the calling process, in bytes. This value depends on the type of process, the type of processor, and the configuration of the operating system. For example, this value is approximately 2 GB for most 32-bit processes on an x86 processor and approximately 3 GB for 32-bit processes that are large address aware running on a system with 4 GT RAM Tuning enabled.
        /// </summary>
        public int dwTotalVirtual;
        /// <summary>
        /// The amount of unreserved and uncommitted memory currently in the user-mode portion of the virtual address space of the calling process, in bytes.
        /// </summary>
        public int dwAvailVirtual;
    }
}
