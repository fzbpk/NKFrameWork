namespace NK.OS.Enum
{
    /// <summary>
    /// 磁盘格式
    /// </summary>
    public enum DiskFormatType:byte
    {
        FAT = 1,
        FAT32 = 2,
        NTFS = 3,
        ExFat=4,
        Ext3=5,
        Ext4=6,
        Journaled=7,
        None = 0,
    }
}
