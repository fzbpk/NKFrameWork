using System;
using System.Runtime.InteropServices;
using NK.API.Struct;
using Microsoft.Win32.SafeHandles;
namespace NK.API
{
    public static partial class Kernel32
    {
        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const uint FILE_SHARE_READ = 0x00000001;
        public const uint FILE_SHARE_WRITE = 0x00000002;

        /// <summary>
        /// This function creates, opens, or truncates a file, COM port, device, service, or console. 
        /// </summary>
        /// <param name="lpFileName">a null-terminated string that specifies the name of the object</param>
        /// <param name="dwDesiredAccess">Type of access to the object</param>
        /// <param name="dwShareMode">Share mode for object</param>
        /// <param name="lpSecurityAttributes">Ignored; set to NULL</param>
        /// <param name="dwCreationDisposition">Action to take on files that exist, and which action to take when files do not exist</param>
        /// <param name="dwFlagsAndAttributes">File attributes and flags for the file</param>
        /// <param name="hTemplateFile">Ignored</param>
        /// <returns>An open handle to the specified file indicates success</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, uint lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, int hTemplateFile);

        /// <summary>
        /// Formats a message string.
        /// </summary>
        /// <param name="dwFlags">The formatting options, and how to interpret the lpSource parameter. The low-order byte of dwFlags specifies how the function handles line breaks in the output buffer. The low-order byte can also specify the maximum width of a formatted output line.</param>
        /// <param name="lpSource">The location of the message definition. The type of this parameter depends upon the settings in the dwFlags parameter.</param>
        /// <param name="dwMessageId">The message identifier for the requested message. This parameter is ignored if dwFlags includes FORMAT_MESSAGE_FROM_STRING.</param>
        /// <param name="dwLanguageId">The language identifier for the requested message. This parameter is ignored if dwFlags includes FORMAT_MESSAGE_FROM_STRING.</param>
        /// <param name="lpBuffer">A pointer to a buffer that receives the null-terminated string that specifies the formatted message. If dwFlags includes FORMAT_MESSAGE_ALLOCATE_BUFFER, the function allocates a buffer using the LocalAlloc function, and places the pointer to the buffer at the address specified in lpBuffer.</param>
        /// <param name="nSize">If the FORMAT_MESSAGE_ALLOCATE_BUFFER flag is not set, this parameter specifies the size of the output buffer, in TCHARs. If FORMAT_MESSAGE_ALLOCATE_BUFFER is set, this parameter specifies the minimum number of TCHARs to allocate for an output buffer.</param>
        /// <param name="Arguments">An array of values that are used as insert values in the formatted message. A %1 in the format string indicates the first value in the Arguments array; a %2 indicates the second argument; and so on.</param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public extern static int FormatMessage(int dwFlags, ref IntPtr lpSource, int dwMessageId, int dwLanguageId, ref string lpBuffer, int nSize, ref IntPtr Arguments);

        /// <summary>
        /// Cancels all pending input and output (I/O) operations that are issued by the calling thread for the specified file. The function does not cancel I/O operations that other threads issue for a file handle.
        /// </summary>
        /// <param name="hFile">   A handle to the file.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int CancelIo(SafeFileHandle hFile);

        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="hObject">A valid handle to an open object.</param>
        /// <returns></returns>
        [DllImport("kernel32", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObjecth);

        /// <summary>
        /// Creates or opens a named or unnamed event object.
        /// </summary>
        /// <param name="lpEventAttributes">A pointer to a SECURITY_ATTRIBUTES structure. If this parameter is NULL, the handle cannot be inherited by child processes.</param>
        /// <param name="bManualReset">If this parameter is TRUE, the function creates a manual-reset event object, which requires the use of the ResetEvent function to set the event state to nonsignaled. If this parameter is FALSE, the function creates an auto-reset event object, and system automatically resets the event state to nonsignaled after a single waiting thread has been released.</param>
        /// <param name="bInitialState">If this parameter is TRUE, the initial state of the event object is signaled; otherwise, it is nonsignaled.</param>
        /// <param name="lpName">The name of the event object. The name is limited to MAX_PATH characters. Name comparison is case sensitive.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

        /// <summary>
        /// Retrieves the results of an overlapped operation on the specified file, named pipe, or communications device. To specify a timeout interval or wait on an alertable thread, use GetOverlappedResultEx.
        /// </summary>
        /// <param name="hFile">A handle to the file, named pipe, or communications device. This is the same handle that was specified when the overlapped operation was started by a call to the ReadFile, WriteFile, ConnectNamedPipe, TransactNamedPipe, DeviceIoControl, or WaitCommEvent function.</param>
        /// <param name="lpOverlapped">A pointer to an OVERLAPPED structure that was specified when the overlapped operation was started.</param>
        /// <param name="lpNumberOfBytesTransferred">A pointer to a variable that receives the number of bytes that were actually transferred by a read or write operation. For a TransactNamedPipe operation, this is the number of bytes that were read from the pipe. For a DeviceIoControl operation, this is the number of bytes of output data returned by the device driver. For a ConnectNamedPipe or WaitCommEvent operation, this value is undefined.</param>
        /// <param name="bWait">If this parameter is TRUE, and the Internal member of the lpOverlapped structure is STATUS_PENDING, the function does not return until the operation has been completed. If this parameter is FALSE and the operation is still pending, the function returns FALSE and the GetLastError function returns ERROR_IO_INCOMPLETE.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetOverlappedResult(SafeFileHandle hFile, IntPtr lpOverlapped, ref int lpNumberOfBytesTransferred, bool bWait);

        /// <summary>
        /// Reads data from the specified file or input/output (I/O) device. Reads occur at the position specified by the file pointer if supported by the device.
        /// </summary>
        /// <param name="hFile">A handle to the device (for example, a file, file stream, physical disk, volume, console buffer, tape drive, socket, communications resource, mailslot, or pipe).</param>
        /// <param name="lpBuffer">A pointer to the buffer that receives the data read from a file or device.</param>
        /// <param name="nNumberOfBytesToRead">he maximum number of bytes to be read.</param>
        /// <param name="lpNumberOfBytesRead">A pointer to the variable that receives the number of bytes read when using a synchronous hFile parameter. ReadFile sets this value to zero before doing any work or error checking. Use NULL for this parameter if this is an asynchronous operation to avoid potentially erroneous results.</param>
        /// <param name="lpOverlapped">A pointer to an OVERLAPPED structure is required if the hFile parameter was opened with FILE_FLAG_OVERLAPPED, otherwise it can be NULL.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadFile(SafeFileHandle hFile, IntPtr lpBuffer, int nNumberOfBytesToRead, ref int lpNumberOfBytesRead, IntPtr lpOverlapped);

        /// <summary>
        /// This function returns when the specified object is in the signaled state or when the time-out interval elapses.
        /// </summary>
        /// <param name="hHandle">[in] Handle to the object. For a list of the object types whose handles can be specified, see the Remarks section.</param>
        /// <param name="dwMilliseconds">[in] Specifies the time-out interval, in milliseconds. The function returns if the interval elapses, even if the object's state is nonsignaled. If dwMilliseconds is zero, the function tests the object's state and returns immediately. If dwMilliseconds is INFINITE, the function's time-out interval never elapses.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int WaitForSingleObject(IntPtr hHandle, int dwMilliseconds);

        /// <summary>
        /// Writes data to the specified file or input/output (I/O) device.
        /// </summary>
        /// <param name="hFile">A handle to the file or I/O device (for example, a file, file stream, physical disk, volume, console buffer, tape drive, socket, communications resource, mailslot, or pipe).</param>
        /// <param name="lpBuffer">A pointer to the buffer containing the data to be written to the file or device.</param>
        /// <param name="nNumberOfBytesToWrite">The number of bytes to be written to the file or device.</param>
        /// <param name="lpNumberOfBytesWritten">A pointer to the variable that receives the number of bytes written when using a synchronous hFile parameter. WriteFile sets this value to zero before doing any work or error checking. Use NULL for this parameter if this is an asynchronous operation to avoid potentially erroneous results.</param>
        /// <param name="lpOverlapped">A pointer to an OVERLAPPED structure is required if the hFile parameter was opened with FILE_FLAG_OVERLAPPED, otherwise this parameter can be NULL</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteFile(SafeFileHandle hFile, byte[] lpBuffer, int nNumberOfBytesToWrite, ref int lpNumberOfBytesWritten, IntPtr lpOverlapped);

        /// <summary>
        /// [GlobalMemoryStatus can return incorrect information. Use the GlobalMemoryStatusEx function instead.]
        /// </summary>
        /// <param name="lpBuffer">A pointer to a MEMORYSTATUS structure. The GlobalMemoryStatus function stores information about current memory availability into this structure.</param> 
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern void GlobalMemoryStatus(ref MEMORYSTATUS lpBuffer);

        /// <summary>
        /// Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
        /// </summary>
        /// <param name="lpFileName">The name of the module. This can be either a library module (a .dll file) or an executable module (an .exe file). The name specified is the file name of the module and is not related to the name stored in the library module itself, as specified by the LIBRARY keyword in the module-definition (.def) file.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string  lpFileName);

        /// <summary>
        /// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count. When the reference count reaches zero, the module is unloaded from the address space of the calling process and the handle is no longer valid.
        /// </summary>
        /// <param name="hModule">A handle to the loaded library module. The LoadLibrary, LoadLibraryEx, GetModuleHandle, or GetModuleHandleEx function returns this handle.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        /// <summary>
        /// Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
        /// </summary>
        /// <param name="hModule">A handle to the DLL module that contains the function or variable. The LoadLibrary, LoadLibraryEx, LoadPackagedLibrary, or GetModuleHandle function returns this handle.</param>
        /// <param name="lpProcName">The function or variable name, or the function's ordinal value. If this parameter is an ordinal value, it must be in the low-order word; the high-order word must be zero.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule,string lpProcName);

        /// <summary>
        /// Retrieves information about the amount of space that is available on a disk volume, which is the total amount of space, the total amount of free space, and the total amount of free space available to the user that is associated with the calling thread.
        /// </summary>
        /// <param name="lpDirectoryName">A directory on the disk.</param>
        /// <param name="lpFreeBytesAvailable">A pointer to a variable that receives the total number of free bytes on a disk that are available to the user who is associated with the calling thread.</param>
        /// <param name="lpTotalNumberOfBytes">A pointer to a variable that receives the total number of bytes on a disk that are available to the user who is associated with the calling thread.</param>
        /// <param name="lpTotalNumberOfFreeBytes">A pointer to a variable that receives the total number of free bytes on a disk.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetDiskFreeSpaceEx(IntPtr lpDirectoryName, ref long lpFreeBytesAvailable, ref long lpTotalNumberOfBytes, ref long lpTotalNumberOfFreeBytes);

        /// <summary>
        /// Retrieves a pseudo handle for the current process.
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetCurrentProcess();

        /// <summary>
        /// Sets the current local time and date.
        /// </summary>
        /// <param name="lpSystemTime">A pointer to a SYSTEMTIME structure that contains the new local date and time.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(SYSTEMTIME lpSystemTime);

        /// <summary>
        /// Retrieves the current local date and time.
        /// </summary>
        /// <param name="lpSystemTime">A pointer to a SYSTEMTIME structure to receive the current local date and time</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern void SetLocalTime(ref SYSTEMTIME lpSystemTime);

        /// <summary>
        /// Decrements the lock count associated with a memory object that was allocated with GMEM_MOVEABLE.
        /// </summary>
        /// <param name="hMem">A handle to the global memory object. This handle is returned by either the GlobalAlloc or GlobalReAlloc function.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GlobalUnlock(IntPtr hMem);

        /// <summary>
        /// Allocates the specified number of bytes from the heap.
        /// </summary>
        /// <param name="uFlags">The memory allocation attributes. If zero is specified, the default is GMEM_FIXED. This parameter can be one or more of the following values, except for the incompatible combinations that are specifically noted.</param>
        /// <param name="dwBytes">The number of bytes to allocate. If this parameter is zero and the uFlags parameter specifies GMEM_MOVEABLE, the function returns a handle to a memory object that is marked as discarded.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GlobalAlloc(uint uFlags,long dwBytes);

        /// <summary>
        /// Locks a global memory object and returns a pointer to the first byte of the object's memory block.
        /// </summary>
        /// <param name="hMem">A handle to the global memory object. This handle is returned by either the GlobalAlloc or GlobalReAlloc function.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GlobalLock(IntPtr hMem);

        /// <summary>
        /// Frees the specified global memory object and invalidates its handle.
        /// </summary>
        /// <param name="hMem">A handle to the global memory object. This handle is returned by either the GlobalAlloc or GlobalReAlloc function. It is not safe to free memory allocated with LocalAlloc.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GlobalFree(IntPtr hMem);

        /// <summary>
        /// Retrieves the power status of the system. The status indicates whether the system is running on AC or DC power, whether the battery is currently charging, how much battery life remains, and if battery saver is on or off.
        /// </summary>
        /// <param name="lpSystemPowerStatus">A pointer to a SYSTEM_POWER_STATUS structure that receives status information.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetSystemPowerStatus(ref SYSTEM_POWER_STATUS lpSystemPowerStatus);

        /// <summary>
        /// Retrieves a volume GUID path for the volume that is associated with the specified volume mount point ( drive letter, volume GUID path, or mounted folder).
        /// </summary>
        /// <param name="lpszVolumeMountPoint">A pointer to a string that contains the path of a mounted folder (for example, "Y:\MountX\") or a drive letter (for example, "X:\"). The string must end with a trailing backslash ('\').</param>
        /// <param name="lpszVolumeName">A pointer to a string that receives the volume GUID path. This path is of the form "\\?\Volume{GUID}\" where GUID is a GUID that identifies the volume. If there is more than one volume GUID path for the volume, only the first one in the mount manager's cache is returned.</param>
        /// <param name="cchBufferLength">The length of the output buffer, in TCHARs. A reasonable size for the buffer to accommodate the largest possible volume GUID path is 50 characters.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetVolumeNameForVolumeMountPoint(string lpszVolumeMountPoint,ref string lpszVolumeName,int cchBufferLength);

        /// <summary>
        /// Retrieves the calling thread's last-error code value. The last-error code is maintained on a per-thread basis. Multiple threads do not overwrite each other's last-error code.
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GetLastError();


    }
}
