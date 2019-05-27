using System; 
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using NK.API.Struct;
namespace NK.API.API
{  
    /// <summary>
    /// HID
    /// </summary>
    public  static partial class Hid
    {

        /// <summary>
        /// The HidD_GetAttributes routine returns the attributes of a specified top-level collection.
        /// </summary>
        /// <param name="HidDeviceObject">Specifies an open handle to a top-level collection</param>
        /// <param name="Attributes">a caller-allocated HIDD_ATTRIBUTES structure that returns the attributes of the collection specified by HidDeviceObject</param>
        /// <returns></returns>
        [DllImport("hid.dll")]
        public static extern bool HidD_GetAttributes(IntPtr hidDeviceObject, out HIDD_ATTRIBUTES attributes);

        /// <summary>
        /// The HidD_GetPreparsedData routine returns a top-level collection's preparsed data.
        /// </summary>
        /// <param name="hidDeviceObject">Specifies an open handle to a top-level collection. </param>
        /// <param name="PreparsedData">Pointer to the address of a routine-allocated buffer that contains a collection's preparsed data in a _HIDP_PREPARSED_DATA structure.</param>
        /// <returns>HidD_GetPreparsedData returns TRUE if it succeeds; otherwise, it returns FALSE.</returns>
        [DllImport("hid.dll")]
        public static extern Boolean HidD_GetPreparsedData(IntPtr hidDeviceObject, out IntPtr PreparsedData);

        /// <summary>
        /// This function returns a top-level collection's HIDP_CAPS structure.
        /// </summary>
        /// <param name="PreparsedData">[in] Pointer to a top-level collection's preparsed data.</param>
        /// <param name="Capabilities">[out] Pointer to a caller-allocated buffer that the function uses to return a collection's HIDP_CAPS structure.</param>
        /// <returns></returns>
        [DllImport("hid.dll")]
        public static extern uint HidP_GetCaps(IntPtr PreparsedData, out HIDP_CAPS Capabilities);
 
        /// <summary>
        /// The HidD_GetHidGuid routine returns the device interface GUID for HIDClass devices.
        /// </summary>
        /// <param name="HidGuid">a caller-allocated GUID buffer that the routine uses to return the device interface GUID for HIDClass devices.</param>
        [DllImport("hid.dll")]
        public static extern void HidD_GetHidGuid(ref Guid HidGuid);

        /// <summary>
        /// The HidD_FlushQueue routine deletes all pending input reports in a top-level collection's input queue.
        /// </summary>
        /// <param name="HidDeviceObject">Specifies an open handle to the top-level collection whose input queue is flushed.</param>
        /// <returns></returns>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_FlushQueue(SafeFileHandle HidDeviceObject);

        /// <summary>
        /// The HidD_FreePreparsedData routine releases the resources that the HID class driver allocated to hold a top-level collection's preparsed data.
        /// </summary>
        /// <param name="PreparsedData">Pointer to the buffer, returned by HidD_GetPreparsedData, that is freed.</param>
        /// <returns></returns>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_FreePreparsedData(IntPtr PreparsedData);

        /// <summary>
        /// The HidD_GetFeature routine returns a feature report from a specified top-level collection.
        /// </summary>
        /// <param name="HidDeviceObject">Specifies an open handle to a top-level collection.</param>
        /// <param name="ReportBuffer">Pointer to a caller-allocated HID report buffer that the caller uses to specify a report ID. HidD_GetFeature uses ReportBuffer to return the specified feature report.</param>
        /// <param name="reportBufferLength">Specifies the size, in bytes, of the report buffer. The report buffer must be large enough to hold the feature report -- excluding its report ID, if report IDs are used -- plus one additional byte that specifies a nonzero report ID or zero.</param>
        /// <returns></returns>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetFeature(SafeFileHandle HidDeviceObject, byte[] ReportBuffer, int reportBufferLength);

        /// <summary>
        /// The HidD_GetInputReport routine returns an input reports from a top-level collection.
        /// </summary>
        /// <param name="HidDeviceObject">Specifies an open handle to a top-level collection.</param>
        /// <param name="ReportBuffer">Pointer to a caller-allocated input report buffer that the caller uses to specify a HID report ID and HidD_GetInputReport uses to return the specified input report.</param>
        /// <param name="ReportBufferLength">Specifies the size, in bytes, of the report buffer. The report buffer must be large enough to hold the input report -- excluding its report ID, if report IDs are used -- plus one additional byte that specifies a nonzero report ID or zero.</param>
        /// <returns></returns>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetInputReport(SafeFileHandle HidDeviceObject, byte[] ReportBuffer, int ReportBufferLength);

        /// <summary>
        /// The HidD_GetNumInputBuffers routine returns the current size, in number of reports, of the ring buffer that the HID class driver uses to queue input reports from a specified top-level collection.
        /// </summary>
        /// <param name="HidDeviceObject">Specifies an open handle to a top-level collection.</param>
        /// <param name="NumberBuffers">Pointer to a caller-allocated variable that the routine uses to return the maximum number of input reports the ring buffer can hold.</param>
        /// <returns></returns>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetNumInputBuffers(SafeFileHandle HidDeviceObject, ref int NumberBuffers);

        /// <summary>
        /// The HidD_GetPreparsedData routine returns a top-level collection's preparsed data.
        /// </summary>
        /// <param name="HidDeviceObject">Specifies an open handle to a top-level collection.</param>
        /// <param name="PreparsedData">Pointer to the address of a routine-allocated buffer that contains a collection's preparsed data in a _HIDP_PREPARSED_DATA structure</param>
        /// <returns></returns>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetPreparsedData(SafeFileHandle HidDeviceObject, ref IntPtr PreparsedData);

        /// <summary>
        /// The HidD_SetFeature routine sends a feature report to a top-level collection.
        /// </summary>
        /// <param name="HidDeviceObject">Specifies an open handle to a top-level collection.</param>
        /// <param name="ReportBuffer">Pointer to a caller-allocated feature report buffer that the caller uses to specify a HID report ID.</param>
        /// <param name="ReportBufferLength">Specifies the size, in bytes, of the report buffer. The report buffer must be large enough to hold the feature report -- excluding its report ID, if report IDs are used -- plus one additional byte that specifies a nonzero report ID or zero.</param>
        /// <returns></returns>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_SetFeature(SafeFileHandle HidDeviceObject, byte[] ReportBuffer, int ReportBufferLength);

        /// <summary>
        /// The HidD_SetNumInputBuffers routine sets the maximum number of input reports that the HID class driver ring buffer can hold for a specified top-level collection.
        /// </summary>
        /// <param name="HidDeviceObject">Specifies an open handle to a top-level collection.</param>
        /// <param name="NumberBuffers">Specifies the maximum number of buffers that the HID class driver should maintain for the input reports generated by the HidDeviceObject collection.</param>
        /// <returns></returns>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_SetNumInputBuffers(SafeFileHandle HidDeviceObject, int NumberBuffers);

        /// <summary>
        /// The HidD_SetOutputReport routine sends an output report to a top-level collection.
        /// </summary>
        /// <param name="HidDeviceObject">Specifies an open handle to a top-level collection.</param>
        /// <param name="ReportBuffer">Pointer to a caller-allocated output report buffer that the caller uses to specify a report ID.</param>
        /// <param name="ReportBufferLength">Specifies the size, in bytes, of the report buffer. The report buffer must be large enough to hold the output report -- excluding its report ID, if report IDs are used </param>
        /// <returns></returns>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_SetOutputReport(SafeFileHandle HidDeviceObject, byte[] ReportBuffer, int ReportBufferLength);

        /// <summary>
        /// The HidP_GetValueCaps routine returns a value capability array that describes all the HID control values in a top-level collection for a specified type of HID report.
        /// </summary>
        /// <param name="ReportType">Specifies a HIDP_REPORT_TYPE enumerator value that identifies the report type.</param>
        /// <param name="ValueCaps">Pointer to a caller-allocated buffer in which the routine returns a value capability array for the specified report type.</param>
        /// <param name="ValueCapsLength">Specifies the length, on input, in array elements, of the ValueCaps buffer. On output, the routine sets ValueCapsLength to the number of elements that the it actually returns.</param>
        /// <param name="PreparsedData">Pointer to a top-level collection's preparsed data.</param>
        /// <returns></returns>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern int HidP_GetValueCaps(int ReportType, byte[] ValueCaps, ref int ValueCapsLength, IntPtr PreparsedData);
    }
}
