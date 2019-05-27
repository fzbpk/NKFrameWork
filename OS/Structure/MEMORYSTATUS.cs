namespace NK.OS.Struct
{
    /// <summary>
    /// Contains information about the current state of both physical and virtual memory. 
    /// </summary>
    public struct MEMORYSTATUS
    {
        /// <summary>
        /// The size of the MEMORYSTATUS data structure, in bytes.
        /// </summary>
        public uint dwLength;
        /// <summary>
        /// A number between 0 and 100 that specifies the approximate percentage of physical memory that is in use (0 indicates no memory use and 100 indicates full memory use).
        /// </summary>
        public uint dwMemoryLoad;
        /// <summary>
        /// The amount of actual physical memory, in bytes.
        /// </summary>
        public uint dwTotalPhys;
        /// <summary>
        /// The amount of physical memory currently available, in bytes. This is the amount of physical memory that can be immediately reused without having to write its contents to disk first. It is the sum of the size of the standby, free, and zero lists.
        /// </summary>
        public uint dwAvailPhys;
        /// <summary>
        /// The current size of the committed memory limit, in bytes. This is physical memory plus the size of the page file, minus a small overhead.
        /// </summary>
        public uint dwTotalPageFile;
        /// <summary>
        /// The maximum amount of memory the current process can commit, in bytes.
        /// </summary>
        public uint dwAvailPageFile;
        /// <summary>
        /// The size of the user-mode portion of the virtual address space of the calling process, in bytes.
        /// </summary>
        public uint dwTotalVirtual;
        /// <summary>
        /// he amount of unreserved and uncommitted memory currently in the user-mode portion of the virtual address space of the calling process, in bytes.
        /// </summary>
        public uint dwAvailVirtual;
    }
}
